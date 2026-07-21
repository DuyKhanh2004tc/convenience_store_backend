SET IDENTITY_INSERT [dbo].[PurchaseOrders] ON;
INSERT INTO [dbo].[PurchaseOrders] ([PoId], [SupplierId], [CreatedBy], [OrderDate], [TotalAmount], [Status], [UpdatedAt])
VALUES (6, 6, 9, '2026-07-08 12:36:23', 6812000, 'Pending', '2026-07-08 12:36:23');
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (6, 37, 26, 32000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (6, 33, 27, 15000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (6, 40, 41, 120000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (6, 31, 37, 11000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (6, 52, 31, 8000);
INSERT INTO [dbo].[PurchaseOrders] ([PoId], [SupplierId], [CreatedBy], [OrderDate], [TotalAmount], [Status], [UpdatedAt])
VALUES (7, 2, 10, '2026-06-21 11:31:23', 6456500, 'Received', '2026-06-21 11:31:23');
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (7, 63, 36, 28000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (7, 30, 41, 8500);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (7, 60, 34, 150000);
INSERT INTO [dbo].[PurchaseOrders] ([PoId], [SupplierId], [CreatedBy], [OrderDate], [TotalAmount], [Status], [UpdatedAt])
VALUES (8, 3, 12, '2026-07-01 11:07:23', 11194000, 'Received', '2026-07-01 11:07:23');
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (8, 27, 31, 4000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (8, 36, 34, 110000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (8, 60, 47, 150000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (8, 22, 28, 10000);
INSERT INTO [dbo].[PurchaseOrders] ([PoId], [SupplierId], [CreatedBy], [OrderDate], [TotalAmount], [Status], [UpdatedAt])
VALUES (9, 6, 12, '2026-07-03 13:39:23', 764000, 'Pending', '2026-07-03 13:39:23');
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (9, 44, 26, 22000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (9, 27, 48, 4000);
INSERT INTO [dbo].[PurchaseOrders] ([PoId], [SupplierId], [CreatedBy], [OrderDate], [TotalAmount], [Status], [UpdatedAt])
VALUES (10, 2, 6, '2026-06-22 00:54:23', 1208000, 'Received', '2026-06-22 00:54:23');
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (10, 25, 24, 4500);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (10, 46, 12, 35000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (10, 45, 34, 20000);
INSERT INTO [dbo].[PurchaseOrders] ([PoId], [SupplierId], [CreatedBy], [OrderDate], [TotalAmount], [Status], [UpdatedAt])
VALUES (11, 1, 13, '2026-07-16 07:42:23', 1335000, 'Pending', '2026-07-16 07:42:23');
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (11, 17, 39, 6500);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (11, 21, 44, 8000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (11, 24, 21, 9500);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (11, 52, 31, 8000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (11, 20, 47, 6000);
INSERT INTO [dbo].[PurchaseOrders] ([PoId], [SupplierId], [CreatedBy], [OrderDate], [TotalAmount], [Status], [UpdatedAt])
VALUES (12, 6, 1, '2026-07-01 15:32:23', 2981000, 'Received', '2026-07-01 15:32:23');
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (12, 17, 42, 6500);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (12, 50, 44, 40000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (12, 27, 48, 4000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (12, 52, 33, 8000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (12, 32, 28, 9000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (12, 49, 20, 12000);
INSERT INTO [dbo].[PurchaseOrders] ([PoId], [SupplierId], [CreatedBy], [OrderDate], [TotalAmount], [Status], [UpdatedAt])
VALUES (13, 3, 4, '2026-07-04 05:09:23', 1943000, 'Received', '2026-07-04 05:09:23');
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (13, 44, 22, 22000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (13, 17, 49, 6500);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (13, 18, 35, 8500);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (13, 64, 13, 15000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (13, 57, 36, 18000);
INSERT INTO [dbo].[PurchaseOrders] ([PoId], [SupplierId], [CreatedBy], [OrderDate], [TotalAmount], [Status], [UpdatedAt])
VALUES (14, 3, 4, '2026-07-20 14:49:23', 4536000, 'Pending', '2026-07-20 14:49:23');
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (14, 65, 42, 25000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (14, 20, 47, 6000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (14, 43, 48, 60000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (14, 19, 27, 12000);
INSERT INTO [dbo].[PurchaseOrders] ([PoId], [SupplierId], [CreatedBy], [OrderDate], [TotalAmount], [Status], [UpdatedAt])
VALUES (15, 1, 14, '2026-07-16 22:16:23', 3269500, 'Pending', '2026-07-16 22:16:23');
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (15, 44, 19, 22000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (15, 33, 48, 15000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (15, 29, 39, 8500);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (15, 23, 40, 45000);
SET IDENTITY_INSERT [dbo].[PurchaseOrders] OFF;
GO
