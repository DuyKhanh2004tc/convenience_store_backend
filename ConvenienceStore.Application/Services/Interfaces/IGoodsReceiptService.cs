using ConvenienceStore.Application.DTOs.GoodsReceipt;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface IGoodsReceiptService
{
    Task<GoodsReceiptDto> ReceiveGoodsAsync(CreateGoodsReceiptRequest request, int userId);
    Task<IEnumerable<GoodsReceiptDto>> GetAllGoodsReceiptsAsync();
    Task<GoodsReceiptDto?> GetGoodsReceiptByIdAsync(int id);
}
