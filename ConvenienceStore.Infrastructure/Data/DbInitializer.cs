using ConvenienceStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConvenienceStore.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        var passwordHash = "$2a$11$8cUWEhHufIXZPdVuOWbqdusL9UDxTL5DxaTKdjkulCSGyFPqWJMRG"; // "123"

        // 1. Roles
        if (!await context.Roles.AnyAsync())
        {
            context.Roles.AddRange(
                new Role { RoleName = "System Admin", Description = "System Administrator" },
                new Role { RoleName = "Chain Director", Description = "Chain Director" },
                new Role { RoleName = "Store Manager", Description = "Store Manager" },
                new Role { RoleName = "Store Staff", Description = "Store Staff" },
                new Role { RoleName = "Warehouse Staff", Description = "Warehouse Staff" }
            );
            await context.SaveChangesAsync();
        }

        var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "System Admin");
        var directorRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Chain Director");
        var managerRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Store Manager");
        var staffRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Store Staff");
        var warehouseRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Warehouse Staff");

        // 2. Branches
        if (!await context.Branches.AnyAsync())
        {
            var branchesToInsert = new List<Branch>
            {
                new Branch { BranchName = "Cửa hàng Quận 1", Address = "123 Lê Lợi, Q1, TP.HCM", Phone = "0123456701", IsActive = true },
                new Branch { BranchName = "Cửa hàng Quận 3", Address = "456 Nam Kỳ Khởi Nghĩa, Q3, TP.HCM", Phone = "0123456703", IsActive = true },
                new Branch { BranchName = "Cửa hàng Quận 10", Address = "789 Sư Vạn Hạnh, Q10, TP.HCM", Phone = "0123456710", IsActive = true }
            };
            context.Branches.AddRange(branchesToInsert);
            await context.SaveChangesAsync();
        }
        var branches = await context.Branches.ToListAsync();

        // 3. Users
        var usersToInsert = new List<User>();
        if (!await context.Users.AnyAsync(u => u.Username == "admin") && adminRole != null)
            usersToInsert.Add(new User { Username = "admin", PasswordHash = passwordHash, FullName = "Hệ thống Admin", Email = "admin@cstore.vn", RoleId = adminRole.RoleId, IsActive = true });
        
        if (!await context.Users.AnyAsync(u => u.Username == "director") && directorRole != null)
            usersToInsert.Add(new User { Username = "director", PasswordHash = passwordHash, FullName = "Nguyễn Văn Giám Đốc", Email = "director@cstore.vn", RoleId = directorRole.RoleId, IsActive = true });

        // Store Managers
        if (!await context.Users.AnyAsync(u => u.Username == "manager.q1") && managerRole != null && branches.Count > 0)
            usersToInsert.Add(new User { Username = "manager.q1", PasswordHash = passwordHash, FullName = "Trần Quản Lý Q1", Email = "manager.q1@cstore.vn", RoleId = managerRole.RoleId, BranchId = branches[0].BranchId, IsActive = true });
        if (!await context.Users.AnyAsync(u => u.Username == "manager.q3") && managerRole != null && branches.Count > 1)
            usersToInsert.Add(new User { Username = "manager.q3", PasswordHash = passwordHash, FullName = "Lê Quản Lý Q3", Email = "manager.q3@cstore.vn", RoleId = managerRole.RoleId, BranchId = branches[1].BranchId, IsActive = true });
        if (!await context.Users.AnyAsync(u => u.Username == "manager.q10") && managerRole != null && branches.Count > 2)
            usersToInsert.Add(new User { Username = "manager.q10", PasswordHash = passwordHash, FullName = "Phạm Quản Lý Q10", Email = "manager.q10@cstore.vn", RoleId = managerRole.RoleId, BranchId = branches[2].BranchId, IsActive = true });

        // Store Staffs
        if (!await context.Users.AnyAsync(u => u.Username == "staff1.q1") && staffRole != null && branches.Count > 0)
            usersToInsert.Add(new User { Username = "staff1.q1", PasswordHash = passwordHash, FullName = "Nhân Viên Q1 - 1", RoleId = staffRole.RoleId, BranchId = branches[0].BranchId, IsActive = true });
        if (!await context.Users.AnyAsync(u => u.Username == "staff2.q1") && staffRole != null && branches.Count > 0)
            usersToInsert.Add(new User { Username = "staff2.q1", PasswordHash = passwordHash, FullName = "Nhân Viên Q1 - 2", RoleId = staffRole.RoleId, BranchId = branches[0].BranchId, IsActive = true });
        if (!await context.Users.AnyAsync(u => u.Username == "staff1.q3") && staffRole != null && branches.Count > 1)
            usersToInsert.Add(new User { Username = "staff1.q3", PasswordHash = passwordHash, FullName = "Nhân Viên Q3 - 1", RoleId = staffRole.RoleId, BranchId = branches[1].BranchId, IsActive = true });
        if (!await context.Users.AnyAsync(u => u.Username == "staff2.q3") && staffRole != null && branches.Count > 1)
            usersToInsert.Add(new User { Username = "staff2.q3", PasswordHash = passwordHash, FullName = "Nhân Viên Q3 - 2", RoleId = staffRole.RoleId, BranchId = branches[1].BranchId, IsActive = true });
        if (!await context.Users.AnyAsync(u => u.Username == "staff1.q10") && staffRole != null && branches.Count > 2)
            usersToInsert.Add(new User { Username = "staff1.q10", PasswordHash = passwordHash, FullName = "Nhân Viên Q10 - 1", RoleId = staffRole.RoleId, BranchId = branches[2].BranchId, IsActive = true });
        if (!await context.Users.AnyAsync(u => u.Username == "staff2.q10") && staffRole != null && branches.Count > 2)
            usersToInsert.Add(new User { Username = "staff2.q10", PasswordHash = passwordHash, FullName = "Nhân Viên Q10 - 2", RoleId = staffRole.RoleId, BranchId = branches[2].BranchId, IsActive = true });

        // Warehouse Staffs
        if (!await context.Users.AnyAsync(u => u.Username == "warehouse1") && warehouseRole != null)
            usersToInsert.Add(new User { Username = "warehouse1", PasswordHash = passwordHash, FullName = "Thủ Kho 1", RoleId = warehouseRole.RoleId, IsActive = true });
        if (!await context.Users.AnyAsync(u => u.Username == "warehouse2") && warehouseRole != null)
            usersToInsert.Add(new User { Username = "warehouse2", PasswordHash = passwordHash, FullName = "Thủ Kho 2", RoleId = warehouseRole.RoleId, IsActive = true });

        if (usersToInsert.Any())
        {
            context.Users.AddRange(usersToInsert);
            await context.SaveChangesAsync();
        }

        // 4. Categories
        if (!await context.Categories.AnyAsync())
        {
            var categoriesToInsert = new List<Category>
            {
                new Category { CategoryName = "Đồ ăn vặt", Description = "Bánh kẹo, snack các loại" },
                new Category { CategoryName = "Đồ uống", Description = "Nước giải khát, nước suối, bia" },
                new Category { CategoryName = "Nhu yếu phẩm", Description = "Mì tôm, đồ hộp, gia vị" },
                new Category { CategoryName = "Hóa mỹ phẩm", Description = "Dầu gội, sữa tắm, kem đánh răng" },
                new Category { CategoryName = "Thuốc lá", Description = "Thuốc lá các loại" }
            };
            context.Categories.AddRange(categoriesToInsert);
            await context.SaveChangesAsync();
        }
        var categories = await context.Categories.ToListAsync();

        // 5. Suppliers
        if (!await context.Suppliers.AnyAsync())
        {
            var suppliersToInsert = new List<Supplier>
            {
                new Supplier { SupplierName = "Vinamilk", ContactName = "Đại diện Vinamilk", Phone = "19001568", Address = "Q7, TP.HCM", IsActive = true },
                new Supplier { SupplierName = "Suntory PepsiCo", ContactName = "Đại diện PepsiCo", Phone = "19001222", Address = "Q1, TP.HCM", IsActive = true },
                new Supplier { SupplierName = "Acecook Việt Nam", ContactName = "Đại diện Acecook", Phone = "19009999", Address = "Tân Phú, TP.HCM", IsActive = true }
            };
            context.Suppliers.AddRange(suppliersToInsert);
            await context.SaveChangesAsync();
        }
        var suppliers = await context.Suppliers.ToListAsync();

        // 6. Products
        if (!await context.Products.AnyAsync() && categories.Count >= 5 && suppliers.Count >= 3)
        {
            var productsToInsert = new List<Product>
            {
                new Product { ProductName = "Bánh Snack Oishi Tôm Cay", CategoryId = categories[0].CategoryId, SupplierId = null, Barcode = "8931234567890", CostPrice = 4000, SellingPrice = 6000, IsActive = true },
                new Product { ProductName = "Bánh Chocopie Hộp 12", CategoryId = categories[0].CategoryId, SupplierId = null, Barcode = "8931234567891", CostPrice = 40000, SellingPrice = 55000, IsActive = true },
                
                new Product { ProductName = "Nước suối Aquafina 500ml", CategoryId = categories[1].CategoryId, SupplierId = suppliers[1].SupplierId, Barcode = "8931234567892", CostPrice = 3000, SellingPrice = 5000, IsActive = true },
                new Product { ProductName = "Pepsi Cola Lon 320ml", CategoryId = categories[1].CategoryId, SupplierId = suppliers[1].SupplierId, Barcode = "8931234567893", CostPrice = 7000, SellingPrice = 10000, IsActive = true },
                new Product { ProductName = "Sữa tươi Vinamilk 100% 180ml", CategoryId = categories[1].CategoryId, SupplierId = suppliers[0].SupplierId, Barcode = "8931234567894", CostPrice = 6000, SellingPrice = 8000, IsActive = true },
                
                new Product { ProductName = "Mì Hảo Hảo Chua Cay", CategoryId = categories[2].CategoryId, SupplierId = suppliers[2].SupplierId, Barcode = "8931234567895", CostPrice = 3000, SellingPrice = 4500, IsActive = true },
                new Product { ProductName = "Mì Ly Modern Lẩu Thái", CategoryId = categories[2].CategoryId, SupplierId = suppliers[2].SupplierId, Barcode = "8931234567896", CostPrice = 6500, SellingPrice = 9000, IsActive = true },
                new Product { ProductName = "Xúc xích Vissan 3 Bông Mai", CategoryId = categories[2].CategoryId, SupplierId = null, Barcode = "8931234567897", CostPrice = 15000, SellingPrice = 20000, IsActive = true },
                
                new Product { ProductName = "Dầu gội Clear Men 170g", CategoryId = categories[3].CategoryId, SupplierId = null, Barcode = "8931234567898", CostPrice = 45000, SellingPrice = 62000, IsActive = true },
                new Product { ProductName = "Kem đánh răng P/S Trà Xanh", CategoryId = categories[3].CategoryId, SupplierId = null, Barcode = "8931234567899", CostPrice = 28000, SellingPrice = 38000, IsActive = true },
                
                new Product { ProductName = "Thuốc lá 555 State Express", CategoryId = categories[4].CategoryId, SupplierId = null, Barcode = "8931234567900", CostPrice = 30000, SellingPrice = 35000, IsActive = true },
                new Product { ProductName = "Thuốc lá Marlboro Trắng", CategoryId = categories[4].CategoryId, SupplierId = null, Barcode = "8931234567901", CostPrice = 25000, SellingPrice = 30000, IsActive = true }
            };
            context.Products.AddRange(productsToInsert);
            await context.SaveChangesAsync();
        }
        var products = await context.Products.ToListAsync();

        // 7. Inventories
        if (!await context.WarehouseInventories.AnyAsync() && products.Any())
        {
            var random = new Random();
            var warehouseInventories = new List<WarehouseInventory>();
            var branchInventories = new List<BranchInventory>();

            foreach (var product in products)
            {
                // Warehouse stock: 500 - 2000
                warehouseInventories.Add(new WarehouseInventory
                {
                    ProductId = product.ProductId,
                    Quantity = random.Next(500, 2000),
                    LastUpdated = DateTime.UtcNow
                });

                // Branch stock: 50 - 200 per branch
                foreach (var branch in branches)
                {
                    branchInventories.Add(new BranchInventory
                    {
                        BranchId = branch.BranchId,
                        ProductId = product.ProductId,
                        Quantity = random.Next(50, 200),
                        LastUpdated = DateTime.UtcNow
                    });
                }
            }

            context.WarehouseInventories.AddRange(warehouseInventories);
            context.BranchInventories.AddRange(branchInventories);
            await context.SaveChangesAsync();
        }
    }
}
