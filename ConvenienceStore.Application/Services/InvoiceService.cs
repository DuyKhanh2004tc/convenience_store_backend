using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.Invoice;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;
using ConvenienceStore.Domain.Enums;

namespace ConvenienceStore.Application.Services;

public class InvoiceService : IInvoiceService
{
    // Loyalty configuration: every 10,000 VND spent = 1 point earned
    private const decimal AMOUNT_PER_LOYALTY_POINT = 10_000m;
    // Each loyalty point redeemed = 100 VND discount
    private const decimal DISCOUNT_PER_POINT = 100m;
    // Minimum points required to be allowed to redeem
    private const int MIN_POINTS_TO_REDEEM = 100;

    private readonly IGenericRepository<Invoice> _invoiceRepository;
    private readonly IGenericRepository<InvoiceDetail> _invoiceDetailRepository;
    private readonly IGenericRepository<BranchInventory> _branchInventoryRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<Branch> _branchRepository;
    private readonly IGenericRepository<Customer> _customerRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public InvoiceService(
        IGenericRepository<Invoice> invoiceRepository,
        IGenericRepository<InvoiceDetail> invoiceDetailRepository,
        IGenericRepository<BranchInventory> branchInventoryRepository,
        IGenericRepository<Product> productRepository,
        IGenericRepository<Branch> branchRepository,
        IGenericRepository<Customer> customerRepository,
        IGenericRepository<User> userRepository,
        IUnitOfWork unitOfWork)
    {
        _invoiceRepository = invoiceRepository;
        _invoiceDetailRepository = invoiceDetailRepository;
        _branchInventoryRepository = branchInventoryRepository;
        _productRepository = productRepository;
        _branchRepository = branchRepository;
        _customerRepository = customerRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<InvoiceDto> CreateInvoiceAsync(int staffUserId, CreateInvoiceDto request)
    {
        // ===== VALIDATION =====

        // 1. Validate Branch
        var branch = await _branchRepository.GetByIdAsync(request.BranchId);
        if (branch == null)
            throw new Exception("Branch not found.");

        // 2. Validate Customer (if provided)
        Customer? customer = null;
        if (request.CustomerId.HasValue)
        {
            customer = await _customerRepository.GetByIdAsync(request.CustomerId.Value);
            if (customer == null)
                throw new Exception("Customer not found.");
        }

        // 3. Check for duplicate product IDs
        var duplicateProducts = request.Items
            .GroupBy(i => i.ProductId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);
        if (duplicateProducts.Any())
            throw new Exception($"Duplicate product IDs found: {string.Join(", ", duplicateProducts)}");

        // 4. Validate products and calculate totals
        decimal totalAmount = 0;
        var itemDetails = new List<(CreateInvoiceItemDto item, Product product, BranchInventory stock)>();

        foreach (var item in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new Exception($"Product with ID {item.ProductId} not found.");
            if (!product.IsActive)
                throw new Exception($"Product '{product.ProductName}' is currently inactive.");

            // Check branch inventory
            var branchStock = (await _branchInventoryRepository
                .GetAsync(b => b.BranchId == request.BranchId && b.ProductId == item.ProductId))
                .FirstOrDefault();

            if (branchStock == null)
                throw new Exception($"Product '{product.ProductName}' is not available at this branch.");

            if (branchStock.Quantity < item.Quantity)
                throw new Exception(
                    $"Insufficient stock for product '{product.ProductName}'. " +
                    $"Available: {branchStock.Quantity}, Requested: {item.Quantity}");

            totalAmount += product.SellingPrice * item.Quantity;
            itemDetails.Add((item, product, branchStock));
        }

        // 5. Calculate discount from loyalty points
        decimal discountAmount = 0;
        int pointsRedeemed = 0;

        if (request.RedeemPoints > 0 && customer != null)
        {
            if (customer.LoyaltyPoints < MIN_POINTS_TO_REDEEM)
                throw new Exception($"Customer must have at least {MIN_POINTS_TO_REDEEM} points to redeem.");

            if (request.RedeemPoints > customer.LoyaltyPoints)
                throw new Exception(
                    $"Customer does not have enough loyalty points. " +
                    $"Available: {customer.LoyaltyPoints}, Requested: {request.RedeemPoints}");

            discountAmount = request.RedeemPoints * DISCOUNT_PER_POINT;

            // Discount cannot exceed total amount
            if (discountAmount > totalAmount)
            {
                discountAmount = totalAmount;
                pointsRedeemed = (int)(totalAmount / DISCOUNT_PER_POINT);
            }
            else
            {
                pointsRedeemed = request.RedeemPoints;
            }
        }

        decimal finalAmount = totalAmount - discountAmount;

        // ===== BEGIN TRANSACTION =====
        await _unitOfWork.BeginTransactionAsync();

        int loyaltyPointsEarned = 0;

        try
        {
            // 1. Create Invoice
            var invoice = new Invoice
            {
                BranchId = request.BranchId,
                StaffId = staffUserId,
                CustomerId = request.CustomerId,
                InvoiceDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                DiscountAmount = discountAmount,
                FinalAmount = finalAmount,
                PaymentMethod = request.PaymentMethod,
                Status = OrderStatus.Completed,
                CreatedAt = DateTime.UtcNow
            };

            await _invoiceRepository.AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync(); // Get generated InvoiceId

            // 2. Create Invoice Details & Deduct Branch Inventory
            var invoiceDetails = new List<InvoiceDetail>();

            foreach (var (item, product, branchStock) in itemDetails)
            {
                // Create invoice detail
                var detail = new InvoiceDetail
                {
                    InvoiceId = invoice.InvoiceId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.SellingPrice,
                    SubTotal = product.SellingPrice * item.Quantity
                };
                invoiceDetails.Add(detail);

                // Deduct from branch inventory
                branchStock.Quantity -= item.Quantity;
                branchStock.LastUpdated = DateTime.UtcNow;
                _branchInventoryRepository.Update(branchStock);
            }

            await _invoiceDetailRepository.AddRangeAsync(invoiceDetails);

            // 3. Handle Loyalty Points (if customer is provided)
            if (customer != null)
            {
                // Deduct redeemed points
                if (pointsRedeemed > 0)
                {
                    customer.LoyaltyPoints -= pointsRedeemed;
                }

                // Earn new points based on final amount paid
                loyaltyPointsEarned = (int)(finalAmount / AMOUNT_PER_LOYALTY_POINT);
                if (loyaltyPointsEarned > 0)
                {
                    customer.LoyaltyPoints += loyaltyPointsEarned;
                }

                customer.UpdatedAt = DateTime.UtcNow;
                _customerRepository.Update(customer);
            }

            // 4. Commit Transaction
            await _unitOfWork.CommitTransactionAsync();

            // 5. Build response
            return new InvoiceDto
            {
                InvoiceId = invoice.InvoiceId,
                BranchId = invoice.BranchId,
                BranchName = branch.BranchName,
                StaffId = invoice.StaffId,
                StaffName = (await _userRepository.GetByIdAsync(staffUserId))?.FullName ?? "",
                CustomerId = invoice.CustomerId,
                CustomerName = customer?.FullName,
                CustomerPhone = customer?.Phone,
                InvoiceDate = invoice.InvoiceDate,
                TotalAmount = invoice.TotalAmount,
                DiscountAmount = invoice.DiscountAmount,
                FinalAmount = invoice.FinalAmount,
                PaymentMethod = invoice.PaymentMethod,
                Status = invoice.Status.ToString(),
                LoyaltyPointsEarned = customer != null ? loyaltyPointsEarned : null,
                LoyaltyPointsRedeemed = customer != null ? pointsRedeemed : null,
                CustomerLoyaltyBalance = customer?.LoyaltyPoints,
                CreatedAt = invoice.CreatedAt,
                Items = invoiceDetails.Select(d =>
                {
                    var prod = itemDetails.First(x => x.product.ProductId == d.ProductId).product;
                    return new InvoiceDetailDto
                    {
                        ProductId = d.ProductId,
                        ProductName = prod.ProductName,
                        Barcode = prod.Barcode,
                        Quantity = d.Quantity,
                        UnitPrice = d.UnitPrice,
                        SubTotal = d.SubTotal
                    };
                }).ToList()
            };
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
        // ===== END TRANSACTION =====
    }

    public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync(int? branchId = null, DateTime? fromDate = null, DateTime? toDate = null)
    {
        IReadOnlyList<Invoice> invoices;

        if (branchId.HasValue)
        {
            invoices = await _invoiceRepository.GetAsync(i => i.BranchId == branchId.Value);
        }
        else
        {
            invoices = await _invoiceRepository.GetAllAsync();
        }

        // Filter by date range
        var filtered = invoices.AsEnumerable();
        if (fromDate.HasValue)
            filtered = filtered.Where(i => i.InvoiceDate >= fromDate.Value.Date);
        if (toDate.HasValue)
            filtered = filtered.Where(i => i.InvoiceDate < toDate.Value.Date.AddDays(1));

        var result = new List<InvoiceDto>();
        foreach (var invoice in filtered.OrderByDescending(i => i.InvoiceDate))
        {
            result.Add(await BuildInvoiceDto(invoice));
        }

        return result;
    }

    public async Task<InvoiceDto?> GetInvoiceByIdAsync(int invoiceId)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
        if (invoice == null) return null;

        return await BuildInvoiceDto(invoice);
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoicesByBranchAsync(int branchId)
    {
        var invoices = await _invoiceRepository.GetAsync(i => i.BranchId == branchId);

        var result = new List<InvoiceDto>();
        foreach (var invoice in invoices.OrderByDescending(i => i.InvoiceDate))
        {
            result.Add(await BuildInvoiceDto(invoice));
        }

        return result;
    }

    public async Task<DailySalesSummaryDto> GetDailySalesSummaryAsync(int branchId, DateTime date)
    {
        var branch = await _branchRepository.GetByIdAsync(branchId);
        if (branch == null)
            throw new Exception("Branch not found.");

        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1);

        var invoices = await _invoiceRepository.GetAsync(i =>
            i.BranchId == branchId &&
            i.InvoiceDate >= startOfDay &&
            i.InvoiceDate < endOfDay &&
            i.Status == OrderStatus.Completed);

        return new DailySalesSummaryDto
        {
            BranchId = branchId,
            BranchName = branch.BranchName,
            Date = date.Date,
            TotalInvoices = invoices.Count,
            TotalRevenue = invoices.Sum(i => i.TotalAmount),
            TotalDiscount = invoices.Sum(i => i.DiscountAmount),
            NetRevenue = invoices.Sum(i => i.FinalAmount)
        };
    }

    // ===== PRIVATE HELPER =====

    private async Task<InvoiceDto> BuildInvoiceDto(Invoice invoice)
    {
        var branch = await _branchRepository.GetByIdAsync(invoice.BranchId);
        var staff = await _userRepository.GetByIdAsync(invoice.StaffId);
        Customer? customer = invoice.CustomerId.HasValue
            ? await _customerRepository.GetByIdAsync(invoice.CustomerId.Value)
            : null;

        var details = await _invoiceDetailRepository.GetAsync(d => d.InvoiceId == invoice.InvoiceId);

        var itemDtos = new List<InvoiceDetailDto>();
        foreach (var detail in details)
        {
            var product = await _productRepository.GetByIdAsync(detail.ProductId);
            itemDtos.Add(new InvoiceDetailDto
            {
                ProductId = detail.ProductId,
                ProductName = product?.ProductName ?? "",
                Barcode = product?.Barcode,
                Quantity = detail.Quantity,
                UnitPrice = detail.UnitPrice,
                SubTotal = detail.SubTotal
            });
        }

        return new InvoiceDto
        {
            InvoiceId = invoice.InvoiceId,
            BranchId = invoice.BranchId,
            BranchName = branch?.BranchName ?? "",
            StaffId = invoice.StaffId,
            StaffName = staff?.FullName ?? "",
            CustomerId = invoice.CustomerId,
            CustomerName = customer?.FullName,
            CustomerPhone = customer?.Phone,
            InvoiceDate = invoice.InvoiceDate,
            TotalAmount = invoice.TotalAmount,
            DiscountAmount = invoice.DiscountAmount,
            FinalAmount = invoice.FinalAmount,
            PaymentMethod = invoice.PaymentMethod,
            Status = invoice.Status.ToString(),
            CustomerLoyaltyBalance = customer?.LoyaltyPoints,
            CreatedAt = invoice.CreatedAt,
            Items = itemDtos
        };
    }
}
