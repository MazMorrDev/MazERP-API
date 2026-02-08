using MazErpBack.DTOs.Movements;
using MazErpBack.Models;

namespace MazErpBack.Services.Interfaces;

public interface IDevolutionService
{
    // Only avaible for admin pannel or backend operations
    public Task<List<Devolution>> GetDevolutionsAsync();
    public Task<Devolution> GetDevolutionByIdAsync(int devolutionId);
    public Task<bool> DeleteDevolutionAsync(int devolutionId);

    // For common users
    public Task<List<DevolutionDto>> GetDevolutionsByCompanyAsync(int companyId);
    public Task<List<DevolutionDto>> GetDevolutionsBySellPointAsync(int sellPointId);
    public Task<DevolutionDto> CreateDevolutionAsync(CreateDevolutionDto devolutionDto);
    public Task<DevolutionDto> UpdateDevolutionAsync(int devolutionId, CreateDevolutionDto devolutionDto);
    public Task<bool> SoftDeleteDevolutionAsync(int devolutionId);
}
