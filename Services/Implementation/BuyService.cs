using MazErpBack.DTOs.Movements;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;

namespace MazErpBack.Services.Implementation;

public class BuyService : IBuyService
{
    public Task<BuyDto> CreateBuyAsync(CreateBuyDto buyDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteBuyAsync(int buyId)
    {
        throw new NotImplementedException();
    }

    public Task<Buy> GetBuyByIdAsync(int buyId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Buy>> GetBuysAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<BuyDto>> GetBuysByWarehouseAsync(int warehouseId)
    {
        throw new NotImplementedException();
    }

    public Task<List<BuyDto>> GetBuysByWorkflowAsync(int workflowId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SoftDeleteBuyAsync(int buyID)
    {
        throw new NotImplementedException();
    }

    public Task<BuyDto> UpdateBuyAsync(int buyId, CreateBuyDto buyDto)
    {
        throw new NotImplementedException();
    }
}
