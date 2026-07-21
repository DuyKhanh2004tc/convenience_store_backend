USE [ConvenientStoreChainDB];
GO

-- ==========================================
-- 1. SEED PRODUCTS
-- ==========================================
SET IDENTITY_INSERT [dbo].[Products] ON;
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (16, N'Mì Hảo Hảo Tôm Chua Cay', 5, 2, '8934909375817', 4000, 5000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 30, N'Thùng', N'Gói');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (17, N'Mì Omachi Xốt Vang', 1, 6, '8936959273655', 6500, 8000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 30, N'Thùng', N'Gói');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (18, N'Nước Tăng Lực Sting Dâu', 3, 6, '8933098838549', 8500, 10000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Chai');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (19, N'Nước Tăng Lực Redbull', 5, 1, '8936209138752', 12000, 15000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Lon');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (20, N'Sữa Tươi Tiệt Trùng Vinamilk 180ml', 5, 6, '8937599846772', 6000, 8000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 48, N'Thùng', N'Hộp');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (21, N'Sữa Chua Uống Probi', 3, 6, '8939477794836', 8000, 10000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 5, N'Lốc', N'Chai');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (22, N'Bánh Mì Tươi Kinh Đô', 3, 3, '8936501652075', 10000, 15000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 20, N'Thùng', N'Cái');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (23, N'Bánh Chocopie Orion (Hộp 12 cái)', 3, 6, '8934388662024', 45000, 55000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 8, N'Thùng', N'Hộp');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (24, N'Snack Khoai Tây Lay''s Vị Tự Nhiên', 6, 1, '8937181621336', 9500, 12000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 40, N'Thùng', N'Gói');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (25, N'Snack Oishi Bắp Ngọt', 3, 3, '8939347966502', 4500, 6000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 40, N'Thùng', N'Gói');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (26, N'Kẹo Cao Su Doublemint', 4, 2, '8934014677171', 8000, 10000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 20, N'Hộp', N'Thỏi');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (27, N'Nước Suối Aquafina 500ml', 6, 2, '8933708429062', 4000, 5000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Chai');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (28, N'Nước Suối Dasani 500ml', 1, 2, '8937786280406', 4000, 5000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Chai');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (29, N'Nước Ngọt Coca Cola 330ml', 1, 6, '8932960385579', 8500, 10000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Lon');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (30, N'Nước Ngọt Pepsi 330ml', 5, 1, '8933234991393', 8500, 10000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Lon');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (31, N'Nước Uống Vị Trái Cây Nutriboost', 3, 6, '8937184351154', 11000, 14000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Chai');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (32, N'Trà Ô Long Tea+ Plus 455ml', 1, 6, '8938265698135', 9000, 11000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Chai');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (33, N'Xúc Xích Heo Vissan (Gói 4 cây)', 2, 2, '8935297247207', 15000, 19000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Gói');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (34, N'Xúc Xích CP (Gói 4 cây)', 1, 1, '8936170639501', 16000, 20000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Gói');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (35, N'Trứng Gà Ba Huân (Hộp 6 quả)', 5, 2, '8938406780069', 18000, 22000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 20, N'Thùng', N'Hộp');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (36, N'Sữa Rửa Mặt Cetaphil 125ml', 6, 3, '8933412678970', 110000, 135000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 12, N'Thùng', N'Chai');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (37, N'Kem Đánh Răng P/S 190g', 2, 1, '8931244036742', 32000, 40000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 36, N'Thùng', N'Hộp');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (38, N'Bàn Chải Đánh Răng Colgate', 4, 1, '8935193542785', 15000, 20000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 12, N'Hộp', N'Cái');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (39, N'Dầu Gội Clear Men 370g', 3, 6, '8932354334838', 85000, 105000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 12, N'Thùng', N'Chai');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (40, N'Sữa Tắm Lifebuoy 800g', 6, 2, '8934587516201', 120000, 150000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 12, N'Thùng', N'Chai');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (41, N'Khăn Giấy Ướt Baby Wipes 80 tờ', 4, 1, '8932187883701', 25000, 35000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Gói');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (42, N'Băng Vệ Sinh Diana Siêu Thấm', 3, 2, '8938395979368', 16000, 21000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 36, N'Thùng', N'Gói');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (43, N'Bao Cao Su Durex Invisible (Hộp 3 cái)', 2, 2, '8934986061979', 60000, 80000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Hộp');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (44, N'Dao Cạo Râu Gillette', 5, 2, '8933173452705', 22000, 28000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 12, N'Hộp', N'Cái');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (45, N'Nước Rửa Tay Khô Green Cross 100ml', 1, 3, '8938490212346', 20000, 25000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Chai');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (46, N'Mì Ý Spaghetti Spinosi 500g', 3, 1, '8938441535092', 35000, 45000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Gói');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (47, N'Dầu Ăn Neptune 1L', 5, 2, '8935551993905', 45000, 55000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 12, N'Thùng', N'Chai');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (48, N'Nước Mắm Nam Ngư 500ml', 1, 1, '8934060044581', 30000, 38000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Chai');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (49, N'Tương Ớt Chinsu 250g', 6, 1, '8931520067515', 12000, 15000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Chai');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (50, N'Cà Phê Sữa Đá MacCoffee 240g', 6, 6, '8936802616219', 40000, 50000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 12, N'Thùng', N'Hộp');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (51, N'Sữa Đặc Ông Thọ 380g', 3, 6, '8939293687149', 20000, 25000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Lon');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (52, N'Kẹo Dẻo Chupa Chups', 6, 1, '8938259475021', 8000, 10000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 48, N'Thùng', N'Gói');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (53, N'Kẹo Socola Snickers', 2, 1, '8933167358096', 12000, 15000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Hộp', N'Thanh');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (54, N'Bia Tiger Xanh 330ml', 2, 6, '8938729548040', 16000, 19000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Lon');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (55, N'Bia Heineken 330ml', 4, 1, '8932518982590', 18000, 21000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Lon');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (56, N'Kem Merino Đậu Xanh', 2, 1, '8938679394484', 10000, 12000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 40, N'Thùng', N'Cây');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (57, N'Kem Celano Ốc Quế', 3, 2, '8936467976433', 18000, 22000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Cây');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (58, N'Nước Ép Trái Cây Vfresh 1L', 1, 2, '8931642074962', 35000, 45000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 12, N'Thùng', N'Hộp');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (59, N'Trà Xanh Không Độ 500ml', 4, 6, '8936292715110', 8500, 10000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Chai');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (60, N'Gạo ST25 5kg', 2, 6, '8931301061567', 150000, 180000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 1, N'Bao', N'Bao');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (61, N'Bột Giặt OMO 800g', 4, 1, '8939379029223', 38000, 45000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 12, N'Thùng', N'Gói');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (62, N'Nước Xả Vải Downy 800ml', 5, 1, '8934196934682', 50000, 65000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 12, N'Thùng', N'Túi');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (63, N'Nước Lau Sàn Sunlight 1Kg', 3, 1, '8935359132091', 28000, 35000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 12, N'Thùng', N'Chai');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (64, N'Nước Rửa Chén Sunlight 400g', 6, 1, '8934279686396', 15000, 19000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 24, N'Thùng', N'Chai');
INSERT INTO [dbo].[Products] ([ProductId], [ProductName], [CategoryId], [SupplierId], [Barcode], [CostPrice], [SellingPrice], [IsActive], [CreatedAt], [UpdatedAt], [ImageUrl], [ConversionFactor], [ImportUnit], [Unit])
VALUES (65, N'Bánh Bao Thọ Phát (Gói 4 cái)', 2, 2, '8933375124150', 25000, 32000, 1, '2026-07-20 21:28:23.366599', '2026-07-20 21:28:23.366599', NULL, 12, N'Thùng', N'Gói');
SET IDENTITY_INSERT [dbo].[Products] OFF;
GO

-- ==========================================
-- 2. SEED INVOICES & INVOICE DETAILS
-- ==========================================
SET IDENTITY_INSERT [dbo].[Invoices] ON;
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (8, 1, 2, 1, '2026-07-12 05:39:23', 177000, 0, 177000, 'MoMo', 'Completed', '2026-07-12 05:39:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (8, 36, 1, 135000, 135000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (8, 42, 2, 21000, 42000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (9, 2, 6, NULL, '2026-07-17 08:30:23', 241000, 4000, 237000, 'Credit Card', 'Completed', '2026-07-17 08:30:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (9, 31, 1, 14000, 14000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (9, 61, 1, 45000, 45000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (9, 56, 1, 12000, 12000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (9, 19, 4, 15000, 60000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (9, 23, 2, 55000, 110000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (10, 2, 6, NULL, '2026-06-23 16:59:23', 148000, 0, 148000, 'MoMo', 'Completed', '2026-06-23 16:59:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (10, 31, 4, 14000, 56000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (10, 42, 3, 21000, 63000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (10, 21, 1, 10000, 10000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (10, 54, 1, 19000, 19000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (11, 1, 10, 1, '2026-07-16 05:21:23', 190000, 0, 190000, 'MoMo', 'Completed', '2026-07-16 05:21:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (11, 19, 3, 15000, 45000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (11, 51, 4, 25000, 100000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (11, 61, 1, 45000, 45000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (12, 2, 4, NULL, '2026-06-23 19:59:23', 575000, 0, 575000, 'MoMo', 'Completed', '2026-06-23 19:59:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (12, 41, 1, 35000, 35000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (12, 36, 4, 135000, 540000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (13, 2, 14, 1, '2026-07-18 11:56:23', 70000, 0, 70000, 'MoMo', 'Completed', '2026-07-18 11:56:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (13, 41, 2, 35000, 70000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (14, 2, 5, 1, '2026-06-29 00:20:23', 255000, 0, 255000, 'Cash', 'Completed', '2026-06-29 00:20:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (14, 36, 1, 135000, 135000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (14, 37, 3, 40000, 120000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (15, 1, 2, 1, '2026-06-24 11:59:23', 113000, 0, 113000, 'MoMo', 'Completed', '2026-06-24 11:59:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (15, 25, 4, 6000, 24000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (15, 21, 3, 10000, 30000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (15, 53, 1, 15000, 15000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (15, 17, 3, 8000, 24000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (15, 28, 4, 5000, 20000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (16, 5, 2, NULL, '2026-07-10 13:16:23', 10000, 2000, 8000, 'Cash', 'Completed', '2026-07-10 13:16:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (16, 27, 2, 5000, 10000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (17, 1, 7, 1, '2026-07-11 17:25:23', 626000, 0, 626000, 'MoMo', 'Completed', '2026-07-11 17:25:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (17, 35, 3, 22000, 66000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (17, 34, 1, 20000, 20000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (17, 36, 4, 135000, 540000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (18, 2, 14, NULL, '2026-06-30 11:47:23', 10000, 0, 10000, 'Credit Card', 'Completed', '2026-06-30 11:47:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (18, 29, 1, 10000, 10000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (19, 5, 11, 1, '2026-07-13 13:55:23', 555000, 0, 555000, 'Cash', 'Completed', '2026-07-13 13:55:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (19, 37, 2, 40000, 80000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (19, 31, 2, 14000, 28000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (19, 56, 1, 12000, 12000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (19, 52, 3, 10000, 30000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (19, 36, 3, 135000, 405000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (20, 5, 1, NULL, '2026-07-18 13:57:23', 86000, 0, 86000, 'MoMo', 'Completed', '2026-07-18 13:57:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (20, 32, 4, 11000, 44000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (20, 55, 2, 21000, 42000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (21, 1, 10, NULL, '2026-07-06 08:16:23', 126000, 0, 126000, 'MoMo', 'Completed', '2026-07-06 08:16:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (21, 21, 2, 10000, 20000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (21, 45, 3, 25000, 75000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (21, 28, 3, 5000, 15000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (21, 17, 2, 8000, 16000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (22, 1, 3, NULL, '2026-07-10 17:28:23', 353000, 0, 353000, 'Credit Card', 'Completed', '2026-07-10 17:28:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (22, 64, 3, 19000, 57000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (22, 17, 3, 8000, 24000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (22, 62, 4, 65000, 260000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (22, 25, 2, 6000, 12000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (23, 5, 6, 1, '2026-06-27 00:02:23', 70000, 3000, 67000, 'MoMo', 'Completed', '2026-06-27 00:02:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (23, 63, 2, 35000, 70000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (24, 5, 2, NULL, '2026-06-28 20:18:23', 80000, 2000, 78000, 'Cash', 'Completed', '2026-06-28 20:18:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (24, 61, 1, 45000, 45000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (24, 28, 4, 5000, 20000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (24, 53, 1, 15000, 15000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (25, 2, 12, NULL, '2026-06-23 20:06:23', 356000, 0, 356000, 'MoMo', 'Completed', '2026-06-23 20:06:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (25, 52, 3, 10000, 30000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (25, 26, 2, 10000, 20000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (25, 64, 2, 19000, 38000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (25, 65, 4, 32000, 128000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (25, 63, 4, 35000, 140000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (26, 5, 6, 1, '2026-07-05 07:21:23', 300000, 0, 300000, 'ZaloPay', 'Completed', '2026-07-05 07:21:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (26, 40, 2, 150000, 300000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (27, 5, 14, 1, '2026-07-15 21:41:23', 70000, 0, 70000, 'ZaloPay', 'Completed', '2026-07-15 21:41:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (27, 28, 1, 5000, 5000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (27, 46, 1, 45000, 45000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (27, 21, 2, 10000, 20000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (28, 5, 8, NULL, '2026-07-13 13:16:23', 552000, 0, 552000, 'MoMo', 'Completed', '2026-07-13 13:16:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (28, 40, 3, 150000, 450000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (28, 56, 3, 12000, 36000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (28, 24, 3, 12000, 36000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (28, 21, 3, 10000, 30000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (29, 5, 11, NULL, '2026-07-20 01:16:23', 168000, 5000, 163000, 'MoMo', 'Completed', '2026-07-20 01:16:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (29, 54, 4, 19000, 76000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (29, 38, 3, 20000, 60000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (29, 20, 4, 8000, 32000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (30, 2, 3, NULL, '2026-06-21 02:29:23', 300000, 4000, 296000, 'Credit Card', 'Completed', '2026-06-21 02:29:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (30, 40, 2, 150000, 300000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (31, 5, 12, 1, '2026-07-01 22:15:23', 21000, 0, 21000, 'MoMo', 'Completed', '2026-07-01 22:15:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (31, 42, 1, 21000, 21000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (32, 1, 5, 1, '2026-06-21 12:56:23', 1041000, 0, 1041000, 'ZaloPay', 'Completed', '2026-06-21 12:56:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (32, 58, 4, 45000, 180000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (32, 46, 3, 45000, 135000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (32, 40, 4, 150000, 600000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (32, 54, 4, 19000, 76000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (32, 45, 2, 25000, 50000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (33, 1, 6, NULL, '2026-06-25 19:20:23', 252000, 0, 252000, 'ZaloPay', 'Completed', '2026-06-25 19:20:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (33, 58, 4, 45000, 180000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (33, 38, 2, 20000, 40000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (33, 20, 4, 8000, 32000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (34, 2, 10, NULL, '2026-07-04 01:46:23', 528000, 0, 528000, 'Cash', 'Completed', '2026-07-04 01:46:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (34, 25, 4, 6000, 24000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (34, 55, 4, 21000, 84000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (34, 39, 4, 105000, 420000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (35, 2, 7, 1, '2026-06-28 00:01:23', 196000, 0, 196000, 'ZaloPay', 'Completed', '2026-06-28 00:01:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (35, 41, 2, 35000, 70000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (35, 32, 3, 11000, 33000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (35, 56, 3, 12000, 36000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (35, 64, 3, 19000, 57000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (36, 2, 9, NULL, '2026-07-02 16:32:23', 48000, 0, 48000, 'ZaloPay', 'Completed', '2026-07-02 16:32:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (36, 24, 4, 12000, 48000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (37, 1, 5, NULL, '2026-07-17 07:39:23', 160000, 2000, 158000, 'Credit Card', 'Completed', '2026-07-17 07:39:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (37, 43, 2, 80000, 160000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (38, 1, 3, 1, '2026-07-03 00:51:23', 30000, 0, 30000, 'Credit Card', 'Completed', '2026-07-03 00:51:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (38, 38, 1, 20000, 20000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (38, 30, 1, 10000, 10000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (39, 2, 10, 1, '2026-07-01 06:40:23', 145000, 5000, 140000, 'Cash', 'Completed', '2026-07-01 06:40:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (39, 63, 1, 35000, 35000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (39, 47, 2, 55000, 110000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (40, 5, 9, NULL, '2026-07-14 12:17:23', 141000, 0, 141000, 'Cash', 'Completed', '2026-07-14 12:17:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (40, 21, 2, 10000, 20000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (40, 50, 1, 50000, 50000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (40, 31, 4, 14000, 56000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (40, 16, 3, 5000, 15000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (41, 2, 12, 1, '2026-07-14 22:35:23', 854000, 0, 854000, 'Credit Card', 'Completed', '2026-07-14 22:35:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (41, 60, 4, 180000, 720000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (41, 48, 3, 38000, 114000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (41, 27, 4, 5000, 20000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (42, 1, 4, NULL, '2026-07-05 20:55:23', 330000, 0, 330000, 'Cash', 'Completed', '2026-07-05 20:55:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (42, 59, 1, 10000, 10000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (42, 45, 2, 25000, 50000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (42, 22, 4, 15000, 60000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (42, 39, 2, 105000, 210000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (43, 2, 10, 1, '2026-07-18 03:10:23', 114000, 0, 114000, 'MoMo', 'Completed', '2026-07-18 03:10:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (43, 38, 2, 20000, 40000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (43, 45, 1, 25000, 25000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (43, 54, 1, 19000, 19000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (43, 49, 2, 15000, 30000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (44, 2, 13, 1, '2026-07-07 17:32:23', 76000, 5000, 71000, 'ZaloPay', 'Completed', '2026-07-07 17:32:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (44, 54, 4, 19000, 76000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (45, 2, 4, NULL, '2026-07-10 01:21:23', 108000, 0, 108000, 'Credit Card', 'Completed', '2026-07-10 01:21:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (45, 20, 1, 8000, 8000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (45, 51, 4, 25000, 100000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (46, 2, 3, 1, '2026-06-29 21:38:23', 706000, 0, 706000, 'ZaloPay', 'Completed', '2026-06-29 21:38:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (46, 23, 4, 55000, 220000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (46, 36, 3, 135000, 405000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (46, 21, 4, 10000, 40000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (46, 18, 3, 10000, 30000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (46, 32, 1, 11000, 11000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (47, 5, 2, NULL, '2026-07-14 06:26:23', 504000, 0, 504000, 'Cash', 'Completed', '2026-07-14 06:26:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (47, 17, 1, 8000, 8000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (47, 52, 4, 10000, 40000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (47, 24, 3, 12000, 36000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (47, 39, 4, 105000, 420000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (48, 1, 2, NULL, '2026-07-01 15:39:23', 110000, 0, 110000, 'MoMo', 'Completed', '2026-07-01 15:39:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (48, 28, 2, 5000, 10000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (48, 50, 2, 50000, 100000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (49, 5, 6, 1, '2026-06-24 21:25:23', 312000, 1000, 311000, 'Cash', 'Completed', '2026-06-24 21:25:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (49, 31, 3, 14000, 42000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (49, 18, 3, 10000, 30000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (49, 43, 3, 80000, 240000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (50, 1, 12, 1, '2026-07-12 22:26:23', 149000, 1000, 148000, 'Cash', 'Completed', '2026-07-12 22:26:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (50, 64, 2, 19000, 38000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (50, 65, 3, 32000, 96000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (50, 19, 1, 15000, 15000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (51, 1, 14, 1, '2026-06-24 23:19:23', 200000, 0, 200000, 'MoMo', 'Completed', '2026-06-24 23:19:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (51, 58, 4, 45000, 180000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (51, 21, 2, 10000, 20000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (52, 1, 8, NULL, '2026-06-22 05:51:23', 222000, 0, 222000, 'Credit Card', 'Completed', '2026-06-22 05:51:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (52, 44, 4, 28000, 112000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (52, 47, 2, 55000, 110000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (53, 5, 3, NULL, '2026-06-26 08:36:23', 108000, 0, 108000, 'Cash', 'Completed', '2026-06-26 08:36:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (53, 65, 2, 32000, 64000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (53, 35, 2, 22000, 44000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (54, 1, 3, 1, '2026-06-27 04:32:23', 436000, 0, 436000, 'Cash', 'Completed', '2026-06-27 04:32:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (54, 61, 3, 45000, 135000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (54, 62, 3, 65000, 195000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (54, 34, 1, 20000, 20000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (54, 44, 2, 28000, 56000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (54, 26, 3, 10000, 30000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (55, 2, 12, 1, '2026-07-05 07:20:23', 10000, 0, 10000, 'Cash', 'Completed', '2026-07-05 07:20:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (55, 30, 1, 10000, 10000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (56, 1, 8, 1, '2026-07-06 04:51:23', 45000, 4000, 41000, 'ZaloPay', 'Completed', '2026-07-06 04:51:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (56, 46, 1, 45000, 45000);
INSERT INTO [dbo].[Invoices] ([InvoiceId], [BranchId], [StaffId], [CustomerId], [InvoiceDate], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [Status], [CreatedAt])
VALUES (57, 5, 9, NULL, '2026-07-02 09:57:23', 284000, 0, 284000, 'MoMo', 'Completed', '2026-07-02 09:57:23');
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (57, 33, 4, 19000, 76000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (57, 34, 4, 20000, 80000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (57, 56, 4, 12000, 48000);
INSERT INTO [dbo].[InvoiceDetails] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [SubTotal]) VALUES (57, 37, 2, 40000, 80000);
SET IDENTITY_INSERT [dbo].[Invoices] OFF;
GO

-- ==========================================
-- 3. SEED ATTENDANCES & SHIFT ASSIGNMENTS
-- ==========================================
SET IDENTITY_INSERT [dbo].[ShiftAssignments] ON;
SET IDENTITY_INSERT [dbo].[Attendances] ON;
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (8, 3, 2, '2026-07-13', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (5, 8, '2026-07-13 08:01:00', '2026-07-13 16:04:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (9, 2, 3, '2026-07-13', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (6, 9, '2026-07-13 08:10:00', '2026-07-13 16:13:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (10, 1, 4, '2026-07-13', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (7, 10, '2026-07-13 08:11:00', '2026-07-13 16:26:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (11, 3, 5, '2026-07-13', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (8, 11, '2026-07-13 08:06:00', '2026-07-13 16:24:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (12, 1, 6, '2026-07-13', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (9, 12, '2026-07-13 08:13:00', '2026-07-13 16:31:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (13, 3, 7, '2026-07-13', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (10, 13, '2026-07-13 08:04:00', '2026-07-13 16:26:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (14, 2, 8, '2026-07-13', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (11, 14, '2026-07-13 08:12:00', '2026-07-13 16:18:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (15, 1, 10, '2026-07-13', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (12, 15, '2026-07-13 08:10:00', '2026-07-13 16:15:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (16, 2, 11, '2026-07-13', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (13, 16, '2026-07-13 08:13:00', '2026-07-13 16:25:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (17, 3, 12, '2026-07-13', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (14, 17, '2026-07-13 08:14:00', '2026-07-13 16:22:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (18, 3, 13, '2026-07-13', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (15, 18, '2026-07-13 08:09:00', '2026-07-13 16:31:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (19, 2, 14, '2026-07-13', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (16, 19, '2026-07-13 08:01:00', '2026-07-13 16:05:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (20, 1, 1, '2026-07-14', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (17, 20, '2026-07-14 08:07:00', '2026-07-14 16:18:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (21, 2, 2, '2026-07-14', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (18, 21, '2026-07-14 08:09:00', '2026-07-14 16:24:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (22, 3, 3, '2026-07-14', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (19, 22, '2026-07-14 08:13:00', '2026-07-14 16:26:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (23, 3, 4, '2026-07-14', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (20, 23, '2026-07-14 08:00:00', '2026-07-14 16:09:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (24, 3, 5, '2026-07-14', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (21, 24, '2026-07-14 08:13:00', '2026-07-14 16:31:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (25, 1, 7, '2026-07-14', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (22, 25, '2026-07-14 08:14:00', '2026-07-14 16:31:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (26, 1, 8, '2026-07-14', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (23, 26, '2026-07-14 08:06:00', '2026-07-14 16:27:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (27, 1, 9, '2026-07-14', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (24, 27, '2026-07-14 08:12:00', '2026-07-14 16:19:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (28, 1, 10, '2026-07-14', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (25, 28, '2026-07-14 08:05:00', '2026-07-14 16:11:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (29, 2, 11, '2026-07-14', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (26, 29, '2026-07-14 08:14:00', '2026-07-14 16:39:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (30, 3, 12, '2026-07-14', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (27, 30, '2026-07-14 08:12:00', '2026-07-14 16:41:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (31, 3, 13, '2026-07-14', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (28, 31, '2026-07-14 08:09:00', '2026-07-14 16:09:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (32, 3, 14, '2026-07-14', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (29, 32, '2026-07-14 08:03:00', '2026-07-14 16:18:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (33, 3, 1, '2026-07-15', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (30, 33, '2026-07-15 08:07:00', '2026-07-15 16:21:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (34, 1, 3, '2026-07-15', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (31, 34, '2026-07-15 08:09:00', '2026-07-15 16:19:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (35, 2, 4, '2026-07-15', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (32, 35, '2026-07-15 08:00:00', '2026-07-15 16:23:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (36, 3, 5, '2026-07-15', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (33, 36, '2026-07-15 08:11:00', '2026-07-15 16:24:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (37, 3, 6, '2026-07-15', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (34, 37, '2026-07-15 08:07:00', '2026-07-15 16:17:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (38, 3, 8, '2026-07-15', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (35, 38, '2026-07-15 08:01:00', '2026-07-15 16:08:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (39, 3, 9, '2026-07-15', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (36, 39, '2026-07-15 08:04:00', '2026-07-15 16:29:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (40, 1, 10, '2026-07-15', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (37, 40, '2026-07-15 08:02:00', '2026-07-15 16:23:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (41, 3, 11, '2026-07-15', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (38, 41, '2026-07-15 08:01:00', '2026-07-15 16:10:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (42, 1, 12, '2026-07-15', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (39, 42, '2026-07-15 08:11:00', '2026-07-15 16:17:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (43, 1, 13, '2026-07-15', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (40, 43, '2026-07-15 08:10:00', '2026-07-15 16:30:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (44, 2, 14, '2026-07-15', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (41, 44, '2026-07-15 08:01:00', '2026-07-15 16:13:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (45, 2, 1, '2026-07-16', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (42, 45, '2026-07-16 08:06:00', '2026-07-16 16:25:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (46, 1, 2, '2026-07-16', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (43, 46, '2026-07-16 08:09:00', '2026-07-16 16:11:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (47, 2, 4, '2026-07-16', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (44, 47, '2026-07-16 08:05:00', '2026-07-16 16:22:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (48, 2, 5, '2026-07-16', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (45, 48, '2026-07-16 08:10:00', '2026-07-16 16:35:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (49, 2, 6, '2026-07-16', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (46, 49, '2026-07-16 08:12:00', '2026-07-16 16:26:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (50, 1, 8, '2026-07-16', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (47, 50, '2026-07-16 08:10:00', '2026-07-16 16:20:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (51, 1, 10, '2026-07-16', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (48, 51, '2026-07-16 08:07:00', '2026-07-16 16:13:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (52, 2, 11, '2026-07-16', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (49, 52, '2026-07-16 08:07:00', '2026-07-16 16:13:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (53, 1, 12, '2026-07-16', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (50, 53, '2026-07-16 08:07:00', '2026-07-16 16:17:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (54, 1, 2, '2026-07-17', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (51, 54, '2026-07-17 08:00:00', '2026-07-17 16:13:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (55, 3, 3, '2026-07-17', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (52, 55, '2026-07-17 08:07:00', '2026-07-17 16:30:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (56, 3, 4, '2026-07-17', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (53, 56, '2026-07-17 08:09:00', '2026-07-17 16:15:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (57, 1, 5, '2026-07-17', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (54, 57, '2026-07-17 08:03:00', '2026-07-17 16:07:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (58, 1, 7, '2026-07-17', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (55, 58, '2026-07-17 08:12:00', '2026-07-17 16:18:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (59, 3, 8, '2026-07-17', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (56, 59, '2026-07-17 08:14:00', '2026-07-17 16:40:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (60, 3, 9, '2026-07-17', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (57, 60, '2026-07-17 08:09:00', '2026-07-17 16:24:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (61, 1, 10, '2026-07-17', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (58, 61, '2026-07-17 08:08:00', '2026-07-17 16:10:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (62, 2, 13, '2026-07-17', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (59, 62, '2026-07-17 08:10:00', '2026-07-17 16:20:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (63, 1, 14, '2026-07-17', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (60, 63, '2026-07-17 08:13:00', '2026-07-17 16:41:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (64, 2, 1, '2026-07-18', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (61, 64, '2026-07-18 08:02:00', '2026-07-18 16:03:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (65, 1, 2, '2026-07-18', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (62, 65, '2026-07-18 08:11:00', '2026-07-18 16:14:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (66, 3, 3, '2026-07-18', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (63, 66, '2026-07-18 08:07:00', '2026-07-18 16:13:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (67, 2, 5, '2026-07-18', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (64, 67, '2026-07-18 08:02:00', '2026-07-18 16:26:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (68, 1, 7, '2026-07-18', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (65, 68, '2026-07-18 08:02:00', '2026-07-18 16:26:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (69, 3, 8, '2026-07-18', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (66, 69, '2026-07-18 08:02:00', '2026-07-18 16:05:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (70, 1, 9, '2026-07-18', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (67, 70, '2026-07-18 08:13:00', '2026-07-18 16:33:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (71, 2, 10, '2026-07-18', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (68, 71, '2026-07-18 08:10:00', '2026-07-18 16:17:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (72, 2, 11, '2026-07-18', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (69, 72, '2026-07-18 08:14:00', '2026-07-18 16:40:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (73, 2, 12, '2026-07-18', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (70, 73, '2026-07-18 08:07:00', '2026-07-18 16:11:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (74, 1, 14, '2026-07-18', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (71, 74, '2026-07-18 08:09:00', '2026-07-18 16:10:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (75, 1, 1, '2026-07-19', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (72, 75, '2026-07-19 08:01:00', '2026-07-19 16:07:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (76, 1, 2, '2026-07-19', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (73, 76, '2026-07-19 08:14:00', '2026-07-19 16:37:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (77, 3, 3, '2026-07-19', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (74, 77, '2026-07-19 08:00:00', '2026-07-19 16:23:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (78, 3, 5, '2026-07-19', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (75, 78, '2026-07-19 08:10:00', '2026-07-19 16:20:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (79, 2, 6, '2026-07-19', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (76, 79, '2026-07-19 08:12:00', '2026-07-19 16:37:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (80, 1, 7, '2026-07-19', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (77, 80, '2026-07-19 08:08:00', '2026-07-19 16:19:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (81, 3, 8, '2026-07-19', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (78, 81, '2026-07-19 08:11:00', '2026-07-19 16:31:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (82, 2, 9, '2026-07-19', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (79, 82, '2026-07-19 08:04:00', '2026-07-19 16:17:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (83, 2, 10, '2026-07-19', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (80, 83, '2026-07-19 08:14:00', '2026-07-19 16:25:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (84, 2, 11, '2026-07-19', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (81, 84, '2026-07-19 08:01:00', '2026-07-19 16:20:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (85, 1, 13, '2026-07-19', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (82, 85, '2026-07-19 08:03:00', '2026-07-19 16:09:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'OnTime', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
INSERT INTO [dbo].[ShiftAssignments] ([AssignmentId], [ShiftId], [EmployeeId], [WorkDate], [Status]) VALUES (86, 3, 14, '2026-07-19', 'Completed');
INSERT INTO [dbo].[Attendances] ([AttendanceId], [ShiftAssignmentId], [CheckInTime], [CheckOutTime], [CheckOutStatus], [CheckInIp], [CheckInLatitude], [CheckInLongitude], [CheckInStatus], [CheckInWifiBssid], [CheckOutIp], [CheckOutLatitude], [CheckOutLongitude], [EarlyMinutes], [LateMinutes])
VALUES (83, 86, '2026-07-19 08:07:00', '2026-07-19 16:24:00', 'Completed', '192.168.1.100', 10.762622, 106.660172, 'Late', 'CStore_WiFi', '192.168.1.100', 10.762622, 106.660172, 0, 0);
SET IDENTITY_INSERT [dbo].[ShiftAssignments] OFF;
SET IDENTITY_INSERT [dbo].[Attendances] OFF;
GO

-- ==========================================
-- 4. SEED PURCHASE ORDERS & BRANCH ORDERS
-- ==========================================
SET IDENTITY_INSERT [dbo].[PurchaseOrders] ON;
INSERT INTO [dbo].[PurchaseOrders] ([PoId], [SupplierId], [CreatedBy], [OrderDate], [TotalAmount], [Status], [UpdatedAt])
VALUES (6, 6, 9, '2026-07-08 12:36:23', 6812000, 'Pending', '2026-07-08 12:36:23');
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (6, 37, 26, 32000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (6, 33, 27, 15000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (6, 40, 41, 120000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (6, 31, 37, 11000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (6, 52, 31, 8000);
INSERT INTO [dbo].[PurchaseOrders] ([PoId], [SupplierId], [CreatedBy], [OrderDate], [TotalAmount], [Status], [UpdatedAt])
VALUES (7, 2, 10, '2026-06-21 11:31:23', 6456500, 'Completed', '2026-06-21 11:31:23');
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (7, 63, 36, 28000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (7, 30, 41, 8500);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (7, 60, 34, 150000);
INSERT INTO [dbo].[PurchaseOrders] ([PoId], [SupplierId], [CreatedBy], [OrderDate], [TotalAmount], [Status], [UpdatedAt])
VALUES (8, 3, 12, '2026-07-01 11:07:23', 11194000, 'Completed', '2026-07-01 11:07:23');
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (8, 27, 31, 4000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (8, 36, 34, 110000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (8, 60, 47, 150000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (8, 22, 28, 10000);
INSERT INTO [dbo].[PurchaseOrders] ([PoId], [SupplierId], [CreatedBy], [OrderDate], [TotalAmount], [Status], [UpdatedAt])
VALUES (9, 6, 12, '2026-07-03 13:39:23', 764000, 'Processing', '2026-07-03 13:39:23');
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (9, 44, 26, 22000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (9, 27, 48, 4000);
INSERT INTO [dbo].[PurchaseOrders] ([PoId], [SupplierId], [CreatedBy], [OrderDate], [TotalAmount], [Status], [UpdatedAt])
VALUES (10, 2, 6, '2026-06-22 00:54:23', 1208000, 'Completed', '2026-06-22 00:54:23');
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
VALUES (12, 6, 1, '2026-07-01 15:32:23', 2981000, 'Completed', '2026-07-01 15:32:23');
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (12, 17, 42, 6500);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (12, 50, 44, 40000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (12, 27, 48, 4000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (12, 52, 33, 8000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (12, 32, 28, 9000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (12, 49, 20, 12000);
INSERT INTO [dbo].[PurchaseOrders] ([PoId], [SupplierId], [CreatedBy], [OrderDate], [TotalAmount], [Status], [UpdatedAt])
VALUES (13, 3, 4, '2026-07-04 05:09:23', 1943000, 'Completed', '2026-07-04 05:09:23');
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (13, 44, 22, 22000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (13, 17, 49, 6500);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (13, 18, 35, 8500);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (13, 64, 13, 15000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (13, 57, 36, 18000);
INSERT INTO [dbo].[PurchaseOrders] ([PoId], [SupplierId], [CreatedBy], [OrderDate], [TotalAmount], [Status], [UpdatedAt])
VALUES (14, 3, 4, '2026-07-20 14:49:23', 4536000, 'Processing', '2026-07-20 14:49:23');
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (14, 65, 42, 25000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (14, 20, 47, 6000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (14, 43, 48, 60000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (14, 19, 27, 12000);
INSERT INTO [dbo].[PurchaseOrders] ([PoId], [SupplierId], [CreatedBy], [OrderDate], [TotalAmount], [Status], [UpdatedAt])
VALUES (15, 1, 14, '2026-07-16 22:16:23', 3269500, 'Processing', '2026-07-16 22:16:23');
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (15, 44, 19, 22000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (15, 33, 48, 15000);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (15, 29, 39, 8500);
INSERT INTO [dbo].[PurchaseOrderDetails] ([PoId], [ProductId], [Quantity], [UnitPrice]) VALUES (15, 23, 40, 45000);
SET IDENTITY_INSERT [dbo].[PurchaseOrders] OFF;
GO
SET IDENTITY_INSERT [dbo].[BranchOrders] ON;
INSERT INTO [dbo].[BranchOrders] ([OrderId], [BranchId], [CreatedBy], [OrderDate], [Status], [UpdatedAt])
VALUES (8, 5, 2, '2026-07-13 15:33:23', 'Processing', '2026-07-13 15:33:23');
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (8, 49, 12);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (8, 57, 14);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (8, 50, 15);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (8, 64, 11);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (8, 30, 19);
INSERT INTO [dbo].[BranchOrders] ([OrderId], [BranchId], [CreatedBy], [OrderDate], [Status], [UpdatedAt])
VALUES (9, 1, 13, '2026-07-10 04:05:23', 'Pending', '2026-07-10 04:05:23');
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (9, 50, 15);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (9, 35, 18);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (9, 41, 13);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (9, 39, 10);
INSERT INTO [dbo].[BranchOrders] ([OrderId], [BranchId], [CreatedBy], [OrderDate], [Status], [UpdatedAt])
VALUES (10, 2, 8, '2026-07-15 08:02:23', 'Completed', '2026-07-15 08:02:23');
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (10, 35, 10);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (10, 41, 11);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (10, 51, 10);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (10, 37, 11);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (10, 55, 10);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (10, 21, 17);
INSERT INTO [dbo].[BranchOrders] ([OrderId], [BranchId], [CreatedBy], [OrderDate], [Status], [UpdatedAt])
VALUES (11, 1, 3, '2026-07-20 09:11:23', 'Processing', '2026-07-20 09:11:23');
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (11, 29, 15);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (11, 57, 10);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (11, 58, 14);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (11, 48, 9);
INSERT INTO [dbo].[BranchOrders] ([OrderId], [BranchId], [CreatedBy], [OrderDate], [Status], [UpdatedAt])
VALUES (12, 2, 6, '2026-07-10 17:50:23', 'Pending', '2026-07-10 17:50:23');
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (12, 50, 8);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (12, 28, 11);
INSERT INTO [dbo].[BranchOrders] ([OrderId], [BranchId], [CreatedBy], [OrderDate], [Status], [UpdatedAt])
VALUES (13, 5, 10, '2026-07-09 21:04:23', 'Completed', '2026-07-09 21:04:23');
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (13, 39, 10);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (13, 18, 13);
INSERT INTO [dbo].[BranchOrders] ([OrderId], [BranchId], [CreatedBy], [OrderDate], [Status], [UpdatedAt])
VALUES (14, 2, 14, '2026-06-21 20:36:23', 'Processing', '2026-06-21 20:36:23');
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (14, 42, 11);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (14, 46, 16);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (14, 31, 11);
INSERT INTO [dbo].[BranchOrders] ([OrderId], [BranchId], [CreatedBy], [OrderDate], [Status], [UpdatedAt])
VALUES (15, 1, 14, '2026-07-10 07:51:23', 'Processing', '2026-07-10 07:51:23');
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (15, 49, 16);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (15, 59, 11);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (15, 31, 5);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (15, 25, 14);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (15, 56, 8);
INSERT INTO [dbo].[BranchOrders] ([OrderId], [BranchId], [CreatedBy], [OrderDate], [Status], [UpdatedAt])
VALUES (16, 1, 6, '2026-06-24 15:01:23', 'Completed', '2026-06-24 15:01:23');
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (16, 50, 16);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (16, 51, 13);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (16, 55, 14);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (16, 58, 9);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (16, 19, 16);
INSERT INTO [dbo].[BranchOrders] ([OrderId], [BranchId], [CreatedBy], [OrderDate], [Status], [UpdatedAt])
VALUES (17, 1, 10, '2026-07-16 16:41:23', 'Completed', '2026-07-16 16:41:23');
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (17, 35, 9);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (17, 53, 9);
INSERT INTO [dbo].[BranchOrderDetails] ([OrderId], [ProductId], [Quantity]) VALUES (17, 45, 16);
SET IDENTITY_INSERT [dbo].[BranchOrders] OFF;
GO
