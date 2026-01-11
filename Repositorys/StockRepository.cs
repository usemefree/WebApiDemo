using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiDemo.Data;
using WebApiDemo.Dtos.Stock;
using WebApiDemo.HelperFilter;
using WebApiDemo.Interfaces;
using WebApiDemo.Models;
using WebApiDemo.Repositorys;

namespace WebApiDemo.Repositorys;

public class StockRepository : IStockRepository
{
    public ApplicationDbContext Context { get; }
    public StockRepository(ApplicationDbContext context)
    {
        this.Context = context;
    }
    public async Task<List<Stock>> GetAllAsync()
    {
        return await Context.Stocks
            .Include(c => c.Comments)
            .ThenInclude(u => u.AppUser)
            .ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int Id)
    {
        return await Context.Stocks
            .Include(c => c.Comments)
            .ThenInclude(u => u.AppUser)
            .FirstOrDefaultAsync(x => x.Id == Id);
    }

    public async Task<Stock> CreateAsync(Stock StockModel)
    {
        await Context.Stocks.AddAsync(StockModel);
        await Context.SaveChangesAsync();
        return StockModel;
    }

    public async Task<Stock?> UpdateAsync(int Id, UpdateStockRequestDto updateStockRequestDto)
    {
        var stockModel = await this.GetByIdAsync(Id);
        if (stockModel is null)
            return null;
        stockModel.Symbol = updateStockRequestDto.Symbol;
        stockModel.CompanyName = updateStockRequestDto.CompanyName;
        stockModel.Purchase = updateStockRequestDto.Purchase;
        stockModel.LastDiv = updateStockRequestDto.LastDiv;
        stockModel.Industry = updateStockRequestDto.Industry;
        stockModel.MarketCap = updateStockRequestDto.MarketCap;

        await Context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<Stock?> DeleteAsync(int Id)
    {
        var stockModel = await this.GetByIdAsync(Id);
        if (stockModel is null)
            return null;
        Context.Stocks.Remove(stockModel);
        await Context.SaveChangesAsync();
        return stockModel;
    }

    public Task<bool> StockExists(int Id)
    {
        return Context.Stocks.AnyAsync(x => x.Id == Id);
    }

    public async Task<List<Stock>> GetFilter(StockQueryObject query)
    {
        var stock = Context.Stocks.Include(c => c.Comments).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Symbol))
            stock = stock.Where(x => x.Symbol.Contains(query.Symbol));
        if (!string.IsNullOrWhiteSpace(query.CompanyName))
            stock = stock.Where(x => x.CompanyName.Contains(query.CompanyName));
        if (!string.IsNullOrWhiteSpace(query.Sortby))
        {
            if(query.Sortby.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            {
                stock = query.IsDescending ? stock.OrderByDescending(x => x.Symbol) : stock.OrderBy(x=>x.Symbol);
            }
        }

        return await stock.ToListAsync();

    }

    public async Task<Stock?> GetBySymbolAsync(string symbol)
    {
        return await Context.Stocks.FirstOrDefaultAsync(x=>x.Symbol.ToLower()== symbol.ToLower());
    }
}
