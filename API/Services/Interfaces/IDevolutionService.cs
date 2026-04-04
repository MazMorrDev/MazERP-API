using MazErpAPI.DTOs.Movements;
using MazErpAPI.Models;
using MazErpBack.Utils;

namespace MazErpAPI.Services.Interfaces;

public interface IDevolutionService
{
    // Only avaible for admin pannel or backend operations
    public Task<Devolution> GetDevolutionByIdAsync(int devolutionId);
    public Task DeleteDevolutionAsync(int devolutionId);

    // For common users
    public Task<PaginatedResult<DevolutionDto>> GetDevolutionsByInventoryAsync(int inventoryId, int pageNumber, int pageSize); // para el cliente sería como buscar por producto
    public Task<PaginatedResult<DevolutionDto>> GetDevolutionsBySellPointAsync(int sellPointId, int pageNumber, int pageSize);
    public Task<DevolutionDto> CreateDevolutionAsync(CreateDevolutionDto devolutionDto);
    public Task<DevolutionDto> UpdateDevolutionAsync(int devolutionId, CreateDevolutionDto devolutionDto);
    public Task<bool> SoftDeleteDevolutionAsync(int devolutionId);
}
