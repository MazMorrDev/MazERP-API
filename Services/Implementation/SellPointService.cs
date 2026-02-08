using MazErpBack.Context;
using MazErpBack.DTOs.Inventory;
using MazErpBack.Models;
using MazErpBack.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Services.Implementation;

public class SellPointService(AppDbContext context, SellPointMapper mapper) : ISellPointService
{
    private readonly AppDbContext _context = context;
    private readonly SellPointMapper _mapper = mapper;

    public async Task<SellPointDto> CreateSellPointAsync(CreateSellPointDto sellPointDto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteSellPointAsync(int sellPointId)
    {
        var sellPoint = await GetSellPointByIdAsync(sellPointId);
        if (sellPoint == null)
        {
            // TODO: logging
            return false;
        }
        _context.SellPoints.Remove(sellPoint);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<SellPoint> GetSellPointByIdAsync(int sellPointId)
    {
        var sellPoint = await _context.SellPoints.FindAsync(sellPointId);
        ArgumentNullException.ThrowIfNull(sellPoint);
        return sellPoint;
    }

    public async Task<List<SellPoint>> GetSellPointsAsync()
    {
        return await _context.SellPoints.ToListAsync();
    }

    public async Task<List<SellPointDto>> GetSellPointsByCompanyAsync(int companyId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<SellPointDto>> GetSellPointsByWarehouseAsync(int warehouseId)
    {
        var sellPoint = ;
    }

    public async Task<bool> SoftDeleteSellPointAsync(int sellPointId)
    {
        var sellPoint = await _context.SellPoints.FindAsync(sellPointId);
        if (sellPoint == null)
        {
            //TODO: logging
            return false;
        }

        sellPoint.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<SellPointDto> UpdateSellPointAsync(int sellPointId, CreateSellPointDto sellPointDto)
    {
        throw new NotImplementedException();
    }
}
