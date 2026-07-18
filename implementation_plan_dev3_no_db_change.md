# Implementation Plan - Dev 3: Warehouse & Inventory Management

## Constraint

This plan must keep the current database design unchanged.

Hard rules:

- Do not add new Domain entities.
- Do not add new DbSet properties.
- Do not modify existing EF Core entity configurations.
- Do not create EF Core migrations.
- Do not add new database tables.
- Do not use Hangfire SQL storage because it creates Hangfire tables.
- Use existing entities only: `WarehouseInventory`, `BranchInventory`, `PurchaseOrder`, `PurchaseOrderDetail`, `GoodsReceipt`, `GoodsReceiptDetail`, `Notification`, `Product`, `Branch`, `Supplier`, `User`.

Allowed changes:

- Add DTOs.
- Add services and service interfaces.
- Add controllers.
- Add constants.
- Add SignalR hub.
- Add FCM service using topic-based push.
- Add `BackgroundService` / `IHostedService` for periodic low-stock checks.
- Modify `Program.cs` for DI, SignalR, hosted service, and external service registration.
- Modify `appsettings.json` only for non-secret configuration values.

Implementation conventions:

- Follow the existing service style: inject `IGenericRepository<T>` for each entity and `IUnitOfWork` for save/transaction handling.
- Do not use `GetByIdAsync(int)` for composite-key entities.
- Composite-key entities must be queried with `GetAsync(predicate)`.
- Composite-key entities in this scope: `BranchInventory`, `PurchaseOrderDetail`, `GoodsReceiptDetail`.
- `WarehouseInventory` has a single key (`ProductId`), but receiving goods must still use an explicit upsert pattern because the inventory row may not exist yet.
- List pagination can be done in service layer with LINQ for version 1. If the dataset grows large, inject `AppDbContext` for server-side `IQueryable` pagination, but do not change the database schema.
- Use only current `PurchaseOrderStatus` values: `Pending`, `Received`, `Cancelled`.

---

## Current Database Reuse Strategy

### Warehouse Flow

Use existing tables:

- `PurchaseOrders`
- `PurchaseOrderDetails`
- `GoodsReceipts`
- `GoodsReceiptDetails`
- `WarehouseInventory`

No new table is needed because:

- `PurchaseOrder` stores supplier order header.
- `PurchaseOrderDetail` stores ordered product quantities.
- `GoodsReceipt` stores actual receiving event.
- `GoodsReceiptDetail` stores received product quantities.
- `WarehouseInventory` stores current warehouse stock.

Quantity unit decision:

- `WarehouseInventory.Quantity` and `BranchInventory.Quantity` are stored in selling/base unit.
- `Product.Unit` is the selling/base unit, for example `Lon`.
- `Product.ImportUnit` is the supplier/import unit, for example `Thung`.
- `Product.ConversionFactor` converts one import unit into selling/base units.
- Receiving formula: `stockToAdd = QuantityReceived * Product.ConversionFactor`.
- Example: receive 5 cartons of Pepsi with `ConversionFactor = 24`; warehouse stock increases by 120 cans.

### Branch Stock Adjustment Flow

Use existing tables:

- `BranchInventory`
- `Notifications`

No `StockAdjustment` table will be created. A stock adjustment is treated as an operation. The latest stock quantity is stored in `BranchInventory`. The audit trail is stored as text in `Notification.Title` and `Notification.Message`.

Trade-off:

- This keeps the DB unchanged.
- Audit history can be displayed to FE as text.
- Reporting/filtering by `ProductId`, `BranchId`, or `Reason` is weaker than having a dedicated adjustment table.

---

## Notification Audit Log Format

For stock adjustment, create one `Notification` record per adjusted product.

### Title

```text
Stock Adjustment - {ProductName}
```

### Message

```text
Type: StockAdjustment
Branch: {BranchName} (ID: {BranchId})
Product: {ProductName} (ID: {ProductId})
System Quantity: {OldQuantity}
Actual Quantity: {ActualQuantity}
Delta: {Delta}
Reason: {Reason}
Adjusted By: {UserFullName} (ID: {UserId})
Adjusted At: {UtcDateTime}
```

### FE Display

Frontend should render this as an audit log text block:

- Show `Title` as the audit log heading.
- Show `Message` as preformatted readable text.
- Show `CreatedAt` as timestamp.
- Show `IsRead` for read/unread state.

No FE parsing is required for the first version. FE may display the message directly.

---

## Phase 1 - Warehouse Inventory API

### 1.1 DTOs

Create folder:

```text
ConvenienceStore.Application/DTOs/Inventory/
```

Files:

- `WarehouseInventoryDto.cs`

  - `ProductId`
  - `ProductName`
  - `Barcode`
  - `Unit`
  - `ImportUnit`
  - `ConversionFactor`
  - `Quantity`
  - `LastUpdated`
- `WarehouseInventoryListDto.cs`

  - `Items`
  - `Page`
  - `PageSize`
  - `TotalCount`

Create folder:

```text
ConvenienceStore.Application/DTOs/PurchaseOrder/
```

Files:

- `CreatePurchaseOrderRequest.cs`

  - `SupplierId`
  - `Items`
- `CreatePurchaseOrderItemDto.cs`

  - `ProductId`
  - `Quantity`
  - `UnitPrice`
- `PurchaseOrderDto.cs`

  - `PoId`
  - `SupplierId`
  - `SupplierName`
  - `CreatedBy`
  - `CreatorName`
  - `OrderDate`
  - `TotalAmount`
  - `Status`
  - `Items`
- `PurchaseOrderItemDto.cs`

  - `ProductId`
  - `ProductName`
  - `Quantity`
  - `UnitPrice`
  - `LineTotal`

Create folder:

```text
ConvenienceStore.Application/DTOs/GoodsReceipt/
```

Files:

- `CreateGoodsReceiptRequest.cs`

  - `PoId`
  - `Items`
  - `Notes`
- `GoodsReceiptItemDto.cs`

  - `ProductId`
  - `QuantityReceived`
- `GoodsReceiptDto.cs`

  - `ReceiptId`
  - `PoId`
  - `ReceivedBy`
  - `ReceiverName`
  - `ReceivedDate`
  - `Notes`
  - `Items`

### 1.2 Services

Create:

- `IWarehouseInventoryService`
- `WarehouseInventoryService`
- `IPurchaseOrderService`
- `PurchaseOrderService`
- `IGoodsReceiptService`
- `GoodsReceiptService`
- `INotificationService`
- `NotificationService`

`INotificationService` and `NotificationService` should be created in Phase 1 as a minimal DB-backed implementation. `GoodsReceiptService` and `BranchInventoryService` can depend on this contract early, while SignalR and FCM behavior can be added later in Phase 3 without changing the service contract.

### 1.3 Warehouse Inventory Rules

`GetAllWarehouseInventory(search, page, pageSize)`:

- Support search by product name or barcode.
- Return paginated result.
- Include product display fields.

`GetWarehouseInventoryByProduct(productId)`:

- Return stock for one product.
- Return not found if product or inventory record does not exist.

`GetLowStockProducts(threshold)`:

- Return products where `WarehouseInventory.Quantity <= threshold`.
- Must not create notification by itself.
- Alert creation should be done by explicit service/job flow to avoid duplicate notifications.

### 1.4 Purchase Order Rules

`CreatePurchaseOrder(request, userId)`:

- Supplier must exist and be active.
- Items must not be empty.
- Product must exist and be active.
- Quantity must be greater than 0.
- UnitPrice must be greater than or equal to 0.
- Duplicate products in request should be merged or rejected. Recommended: reject duplicate product IDs.
- `TotalAmount = Sum(Quantity * UnitPrice)`.
- Initial status must be `Pending`.

`CancelPurchaseOrder(id)`:

- Only allowed when status is `Pending`.
- Reject if status is `Received` or `Cancelled`.

### 1.5 Goods Receipt Rules

Recommended no-DB-change approach: one PO can be received once only.

`ReceiveGoods(request, userId)`:

1. Start transaction.
2. Load PO and PO details.
3. Reject if PO does not exist.
4. Reject if PO status is not `Pending`.
5. Reject if request items are empty.
6. Reject product not included in PO.
7. Reject duplicate product IDs in request.
8. Reject `QuantityReceived <= 0`.
9. Reject if `QuantityReceived > ordered quantity`.
10. Create `GoodsReceipt`.
11. Create `GoodsReceiptDetails`.
12. Upsert `WarehouseInventory` per product:
    - Query with `GetAsync(wi => wi.ProductId == productId)`.
    - If the row does not exist, create it with `AddAsync`.
    - If the row exists, add to its quantity and call `Update`.
13. Convert import quantity into selling/base unit:
    - Validate `Product.ConversionFactor > 0`.
    - `stockToAdd = QuantityReceived * Product.ConversionFactor`.
    - `WarehouseInventory.Quantity += stockToAdd`.
14. Set `WarehouseInventory.LastUpdated = DateTime.UtcNow`.
15. Set PO status to `Received`.
16. Set PO `UpdatedAt = DateTime.UtcNow`.
17. Save and commit transaction.
18. Create/send notification after successful commit.

Version 1 allows receiving less than or equal to the ordered quantity, but closes the PO as `Received` after the first successful receipt. This is a deliberate no-DB-change trade-off: it accepts supplier short delivery while preventing accidental double counting from receiving the same PO multiple times. If open partial receiving is required later, keep the same schema and calculate total received from existing `GoodsReceiptDetails`, but that is out of scope for this version.

Warehouse inventory upsert example:

```csharp
var warehouseInventory = (await _warehouseInventoryRepository.GetAsync(
    wi => wi.ProductId == productId
)).FirstOrDefault();

var stockToAdd = quantityReceived * product.ConversionFactor;

if (warehouseInventory == null)
{
    await _warehouseInventoryRepository.AddAsync(new WarehouseInventory
    {
        ProductId = productId,
        Quantity = stockToAdd,
        LastUpdated = DateTime.UtcNow
    });
}
else
{
    warehouseInventory.Quantity += stockToAdd;
    warehouseInventory.LastUpdated = DateTime.UtcNow;
    _warehouseInventoryRepository.Update(warehouseInventory);
}
```

### 1.6 Controllers

Create:

- `WarehouseInventoryController`

  - `[Authorize(Roles = "Warehouse Staff,Chain Director,System Admin")]`
  - `GET /api/warehouse-inventory`
  - `GET /api/warehouse-inventory/{productId}`
  - `GET /api/warehouse-inventory/low-stock?threshold=10`
- `PurchaseOrdersController`

  - `[Authorize(Roles = "Warehouse Staff,Chain Director,System Admin")]`
  - `POST /api/purchase-orders`
  - `GET /api/purchase-orders`
  - `GET /api/purchase-orders/{id}`
  - `PUT /api/purchase-orders/{id}/cancel`
- `GoodsReceiptsController`

  - `[Authorize(Roles = "Warehouse Staff,Chain Director,System Admin")]`
  - `POST /api/goods-receipts`
  - `GET /api/goods-receipts`
  - `GET /api/goods-receipts/{id}`

---

## Phase 2 - Branch Inventory And Stock Adjustment API

### 2.1 DTOs

Add to:

```text
ConvenienceStore.Application/DTOs/Inventory/
```

Files:

- `BranchInventoryDto.cs`

  - `BranchId`
  - `BranchName`
  - `ProductId`
  - `ProductName`
  - `Barcode`
  - `Quantity`
  - `LastUpdated`
- `BranchInventoryListDto.cs`

  - `Items`
  - `Page`
  - `PageSize`
  - `TotalCount`
- `StockAdjustmentRequest.cs`

  - `Items`
- `StockAdjustmentItemDto.cs`

  - `ProductId`
  - `ActualQuantity`
  - `Reason`
- `StockAdjustmentResultDto.cs`

  - `BranchId`
  - `BranchName`
  - `AdjustedItems`
  - `TotalDelta`
- `StockAdjustmentResultItemDto.cs`

  - `ProductId`
  - `ProductName`
  - `OldQuantity`
  - `ActualQuantity`
  - `Delta`
  - `Reason`
  - `AuditMessage`

### 2.2 Constants

Create:

```text
ConvenienceStore.Application/Constants/AdjustmentReasons.cs
```

Allowed values:

- `Damaged`
- `Expired`
- `Lost`
- `Found`
- `Other`

### 2.3 Service

Create:

- `IBranchInventoryService`
- `BranchInventoryService`

Methods:

- `GetBranchInventory(branchId, search, page, pageSize, currentUser)`
- `GetBranchInventoryByProduct(branchId, productId, currentUser)`
- `GetLowStockByBranch(branchId, threshold, currentUser)`
- `AdjustStock(branchId, request, currentUser)`

Composite-key convention:

- `BranchInventory` must be queried with `GetAsync(bi => bi.BranchId == branchId && bi.ProductId == productId)`.
- Do not use `GetByIdAsync` for `BranchInventory`.

### 2.4 Branch Authorization Rules

Use JWT claims:

- Role claim: `ClaimTypes.Role`
- User ID claim: `JwtRegisteredClaimNames.Sub`
- Branch claim: `"BranchId"`

Rules:

- `Store Staff` can view inventory only for their own branch.
- `Store Manager` can view and adjust inventory only for their own branch.
- `Chain Director` and `System Admin` can view all branches.
- Only `Store Manager`, `Chain Director`, and `System Admin` can adjust stock.

### 2.5 Stock Adjustment Rules

`AdjustStock(branchId, request, currentUser)`:

1. Validate current user has access to `branchId`.
2. Validate caller role can adjust stock.
3. Validate request contains at least one item.
4. Reject duplicate product IDs.
5. Reject `ActualQuantity < 0`.
6. Validate reason is in `AdjustmentReasons.ValidReasons`.
7. Load existing `BranchInventory` with `GetAsync(bi => bi.BranchId == branchId && bi.ProductId == productId)`.
8. Reject if inventory record does not exist.
9. Calculate `Delta = ActualQuantity - OldQuantity`.
10. If `Delta == 0`, do not update inventory and do not create notification.
11. Update `BranchInventory.Quantity = ActualQuantity`.
12. Update `BranchInventory.LastUpdated = DateTime.UtcNow`.
13. Create a `Notification` audit log record as text.
14. Save all changes in one transaction.
15. After commit, send SignalR/FCM alert only if absolute delta passes configured threshold.

### 2.6 Notification Audit Record

Use existing `Notification` entity.

For each adjusted item:

- `UserId`: current user's ID
- `Title`: `Stock Adjustment - {ProductName}`
- `Message`: text audit block
- `IsRead`: `false`
- `CreatedAt`: `DateTime.UtcNow`

Title length rule:

- `Notification.Title` has max length 255.
- Service must truncate generated titles to 255 characters before saving.
- `Notification.Message` can store the full audit text block.

Example:

```csharp
var title = $"Stock Adjustment - {productName}";
if (title.Length > 255)
{
    title = title[..255];
}
```

Example:

```text
Type: StockAdjustment
Branch: Cua hang Quan 1 (ID: 1)
Product: Pepsi Cola Lon 320ml (ID: 4)
System Quantity: 120
Actual Quantity: 98
Delta: -22
Reason: Damaged
Adjusted By: Tran Quan Ly Q1 (ID: 7)
Adjusted At: 2026-07-14T01:20:00Z
```

### 2.7 Controller

Create:

- `BranchInventoryController`
  - Class-level: `[Authorize(Roles = "Store Manager,Store Staff,Chain Director,System Admin")]`
  - `GET /api/branches/{branchId}/inventory`
  - `GET /api/branches/{branchId}/inventory/{productId}`
  - `GET /api/branches/{branchId}/inventory/low-stock?threshold=10`
  - `POST /api/branches/{branchId}/inventory/adjust`

For adjust endpoint:

```csharp
[Authorize(Roles = "Store Manager,Chain Director,System Admin")]
```

---

## Phase 3 - Notifications, SignalR, And FCM Without DB Changes

### 3.1 Notification DTOs

Create:

```text
ConvenienceStore.Application/DTOs/Notification/
```

Files:

- `NotificationDto.cs`

  - `NotificationId`
  - `UserId`
  - `Title`
  - `Message`
  - `IsRead`
  - `CreatedAt`
- `NotificationListDto.cs`

  - `Items`
  - `Page`
  - `PageSize`
  - `TotalCount`

### 3.2 Notification Service

Create:

- `INotificationService`
- `NotificationService`

If these were already created as the Phase 1 minimal implementation, expand the same files here instead of creating a second version.

Methods:

- `CreateAuditLogAsync(userId, title, message)`
- `CreateAndSendNotificationAsync(userId, title, message)`
- `GetNotificationsAsync(userId, page, pageSize)`
- `GetUnreadCountAsync(userId)`
- `MarkAsReadAsync(notificationId, userId)`
- `MarkAllAsReadAsync(userId)`

Rules:

- Notifications are stored using the existing `Notifications` table.
- Audit logs are normal notification records with text messages.
- Do not add notification type column.
- Do not add device token table.
- Truncate `Notification.Title` to 255 characters before saving.
- `GoodsReceiptService` may call `CreateAndSendNotificationAsync` after successful commit.
- `BranchInventoryService.AdjustStock` must create audit log records inside the transaction, then send realtime/push alerts after commit when needed.

### 3.3 Notifications Controller

Create:

- `NotificationsController`
  - `[Authorize]`
  - `GET /api/notifications`
  - `GET /api/notifications/unread-count`
  - `PUT /api/notifications/{id}/read`
  - `PUT /api/notifications/read-all`

Rules:

- User can only read/update their own notifications.
- `System Admin` may optionally view all later, but not required for this task.

### 3.4 SignalR Hub

Create:

```text
ConvenienceStore.API/Hubs/InventoryHub.cs
```

Hub route:

```text
/hubs/inventory
```

Groups:

- `warehouse`
- `branch-{branchId}`
- `role-store-manager`
- `role-warehouse-staff`

Client events:

- `ReceiveNotification(title, message, createdAt)`
- `ReceiveInventoryUpdate(type, productName, oldQuantity, newQuantity, branchName)`
- `ReceiveLowStockAlert(scope, productName, currentQuantity, threshold)`

No DB change is required for SignalR.

### 3.5 FCM Service Without Device Token Table

Use topic-based push notification.

Flutter app subscribes to topics:

- `warehouse`
- `branch_{branchId}`
- `low_stock`

Backend sends to topics:

- Warehouse alerts -> topic `warehouse`
- Branch alerts -> topic `branch_{branchId}`
- General low stock alerts -> topic `low_stock`

Create:

- `IFcmService`
- `FcmService`

Methods:

- `SendToTopicAsync(string topic, string title, string body)`

Do not store device tokens in database.

Firebase credential handling:

- Do not store private key JSON directly in `appsettings.json`.
- Use an environment variable such as `GOOGLE_APPLICATION_CREDENTIALS`.
- `appsettings.json` may store only non-secret values, for example:

```json
{
  "Firebase": {
    "ProjectId": "your-project-id"
  }
}
```

### 3.6 Background Low-Stock Check Without Hangfire Tables

Do not use Hangfire.

Create:

```text
ConvenienceStore.Infrastructure/BackgroundJobs/InventoryLowStockBackgroundService.cs
```

Use `BackgroundService` / `IHostedService`.

Behavior:

- Runs every configured interval, for example every 60 minutes.
- Checks `WarehouseInventory` and `BranchInventory`.
- Sends SignalR/FCM alerts for low stock.
- Optionally creates `Notification` records for responsible users.

Configuration:

```json
{
  "InventoryAlerts": {
    "LowStockThreshold": 10,
    "LargeAdjustmentThreshold": 20,
    "CheckIntervalMinutes": 60
  }
}
```

No database table is created by `BackgroundService`.

---

## Phase 4 - Frontend Flutter

### 4.1 Inventory Feature

Create module:

```text
features/inventory/
```

Suggested structure:

- `models/warehouse_inventory_model.dart`
- `models/branch_inventory_model.dart`
- `models/stock_adjustment_result_model.dart`
- `services/inventory_api_service.dart`
- `providers/warehouse_inventory_provider.dart`
- `providers/branch_inventory_provider.dart`
- `screens/warehouse_inventory_screen.dart`
- `screens/goods_receiving_screen.dart`
- `screens/branch_inventory_screen.dart`
- `screens/stock_adjustment_screen.dart`
- `widgets/inventory_card.dart`
- `widgets/low_stock_badge.dart`
- `widgets/barcode_scanner.dart`

### 4.2 Audit Log Display

Create:

```text
features/notification/
```

Suggested files:

- `models/notification_model.dart`
- `services/notification_api_service.dart`
- `providers/notification_provider.dart`
- `screens/notification_screen.dart`
- `widgets/audit_log_card.dart`

Display rules:

- Show `Notification.Title` as heading.
- Show `Notification.Message` as plain text/preformatted text.
- Show `Notification.CreatedAt`.
- Show unread state from `IsRead`.
- Do not require a new backend table or new notification type field.

### 4.3 SignalR And FCM

SignalR:

- Use `signalr_core`.
- Connect to `/hubs/inventory`.
- Join branch/role groups after login.

FCM:

- Use `firebase_messaging`.
- Subscribe by topic:
  - Warehouse staff -> `warehouse`
  - Store manager/staff -> `branch_{branchId}`
  - Users who need alerts -> `low_stock`

No device token API is required in this version.

---

## Phase 5 - Files To Create Or Modify

### Create - Application Layer

- `DTOs/Inventory/WarehouseInventoryDto.cs`
- `DTOs/Inventory/WarehouseInventoryListDto.cs`
- `DTOs/Inventory/BranchInventoryDto.cs`
- `DTOs/Inventory/BranchInventoryListDto.cs`
- `DTOs/Inventory/StockAdjustmentRequest.cs`
- `DTOs/Inventory/StockAdjustmentItemDto.cs`
- `DTOs/Inventory/StockAdjustmentResultDto.cs`
- `DTOs/Inventory/StockAdjustmentResultItemDto.cs`
- `DTOs/PurchaseOrder/CreatePurchaseOrderRequest.cs`
- `DTOs/PurchaseOrder/CreatePurchaseOrderItemDto.cs`
- `DTOs/PurchaseOrder/PurchaseOrderDto.cs`
- `DTOs/PurchaseOrder/PurchaseOrderItemDto.cs`
- `DTOs/GoodsReceipt/CreateGoodsReceiptRequest.cs`
- `DTOs/GoodsReceipt/GoodsReceiptItemDto.cs`
- `DTOs/GoodsReceipt/GoodsReceiptDto.cs`
- `DTOs/Notification/NotificationDto.cs`
- `DTOs/Notification/NotificationListDto.cs`
- `Constants/AdjustmentReasons.cs`
- `Services/Interfaces/IWarehouseInventoryService.cs`
- `Services/Interfaces/IPurchaseOrderService.cs`
- `Services/Interfaces/IGoodsReceiptService.cs`
- `Services/Interfaces/IBranchInventoryService.cs`
- `Services/Interfaces/INotificationService.cs`
- `Services/WarehouseInventoryService.cs`
- `Services/PurchaseOrderService.cs`
- `Services/GoodsReceiptService.cs`
- `Services/BranchInventoryService.cs`
- `Services/NotificationService.cs`
- `Contracts/External/IFcmService.cs`

### Create - Infrastructure Layer

- `ExternalServices/Fcm/FcmService.cs`
- `BackgroundJobs/InventoryLowStockBackgroundService.cs`

### Create - API Layer

- `Controllers/WarehouseInventoryController.cs`
- `Controllers/PurchaseOrdersController.cs`
- `Controllers/GoodsReceiptsController.cs`
- `Controllers/BranchInventoryController.cs`
- `Controllers/NotificationsController.cs`
- `Hubs/InventoryHub.cs`

### Modify

- `ConvenienceStore.API/Program.cs`

  - Register new application services.
  - Register FCM service.
  - Add SignalR.
  - Map `InventoryHub`.
  - Register low-stock hosted service.
- `ConvenienceStore.API/appsettings.json`

  - Add non-secret alert config.
  - Add non-secret Firebase project config.
- Project files only if packages are needed:

  - Add `FirebaseAdmin`.
  - Add SignalR package only if not available through ASP.NET Core shared framework.

### Do Not Modify

- `ConvenienceStore.Domain/Entities/*`
- `ConvenienceStore.Infrastructure/Data/AppDbContext.cs`
- `ConvenienceStore.Infrastructure/Data/Configurations/*`
- `ConvenienceStore.Infrastructure/Migrations/*`

Do not run:

```text
dotnet ef migrations add ...
dotnet ef database update
```

Repository rules:

- Do not modify `GenericRepository` only to support composite keys.
- Use `GetAsync(predicate)` for `BranchInventory`, `PurchaseOrderDetail`, and `GoodsReceiptDetail`.
- Use explicit upsert logic in services where a current-state row may not exist yet, especially `WarehouseInventory`.
- Keep service dependencies consistent with the existing code style:
  - repositories for entity access
  - `IUnitOfWork` for transactions and saving
  - `INotificationService` for notification/audit log creation

---

## Verification Checklist

### Build

- Run `dotnet build`.

### Warehouse Receiving

Test:

1. Create PO.
2. Receive goods for that PO.
3. Verify `WarehouseInventory.Quantity` increased by `QuantityReceived * ConversionFactor`.
4. Verify PO status changed to `Received`.
5. Try receiving the same PO again.
6. Expected result: rejected.
7. Try receiving quantity greater than ordered quantity.
8. Expected result: rejected.
9. Try receiving quantity less than ordered quantity.
10. Expected result: accepted once and PO is closed as `Received`.

### Branch Adjustment

Test:

1. Login as Store Manager.
2. Adjust product stock in manager's own branch.
3. Verify `BranchInventory.Quantity` changed.
4. Verify `Notification` record created.
5. Verify FE displays notification message as audit log text.
6. Try adjusting another branch.
7. Expected result: rejected.
8. Try product with very long name.
9. Expected result: `Notification.Title` is truncated safely to 255 characters.

### No Database Design Change

Verify:

- No new migration files.
- No new entity classes.
- No new DbSet.
- No new EF configurations.
- No Hangfire tables.

### SignalR / FCM

Test:

1. Connect Flutter client to SignalR hub.
2. Subscribe to FCM topic.
3. Trigger large stock adjustment.
4. Verify real-time SignalR alert.
5. Verify topic push notification if FCM is configured.
