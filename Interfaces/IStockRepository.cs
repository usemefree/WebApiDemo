using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiDemo.Dtos.Stock;
using WebApiDemo.HelperFilter;
using WebApiDemo.Models;

namespace WebApiDemo.Interfaces;

public interface IStockRepository
{
    Task<List<Stock>> GetAllAsync();
    Task<Stock?> GetByIdAsync(int Id);
    Task<Stock?> GetBySymbolAsync(string symbol);
    Task<Stock> CreateAsync(Stock StockModel);
    Task<Stock?> UpdateAsync(int Id, UpdateStockRequestDto updateStockRequestDto);
    Task<Stock?> DeleteAsync(int Id);
    Task<bool> StockExists(int Id);
    Task<List<Stock>> GetFilter(StockQueryObject query);
}
