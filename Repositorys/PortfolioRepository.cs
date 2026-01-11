using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiDemo.Data;
using WebApiDemo.Interfaces;
using WebApiDemo.Models;

namespace WebApiDemo.Repositorys;

public class PortfolioRepository : IPortfolioRepository
{
    private readonly ApplicationDbContext context;
    public ILogger<PortfolioRepository> log { get; private set; }

    public PortfolioRepository(ILogger<PortfolioRepository> log, ApplicationDbContext context)
    {
        this.log = log;
        this.context = context;
    }
    public async Task<List<Stock>?> GetUserPortfolio(AppUser user)
    {
        try
        {
            log.LogInformation("Portfolio Repository Start # "+DateTime.Now );
            var result = await context.portfolios.Where(x => x.AppUserId == user.Id)
            .Select(x => new Stock
            {
                Id = x.StockId,
                Symbol = x.Stock.Symbol,
                CompanyName = x.Stock.CompanyName,
                Purchase = x.Stock.Purchase,
                LastDiv = x.Stock.LastDiv,
                Industry = x.Stock.Industry,
                MarketCap = x.Stock.MarketCap
            }).ToListAsync();
            
            string jsonString = JsonSerializer.Serialize<List<Stock>>(result);
            log.LogInformation("Portfolio Repository json string # "+DateTime.Now );
            //log.LogInformation(jsonString );
            log.LogInformation("Portfolio Repository End # "+DateTime.Now );
            return result;
        }
        catch (Exception ex)
        {
            log.LogError(ex.Message);
            return null;
        }
    }

    public async Task<Portfolio> CreateAsyn(Portfolio portfolio)
    {
        await context.portfolios.AddAsync(portfolio);
        await context.SaveChangesAsync();
        return portfolio;
    }

    public async Task<Portfolio> DeletePortfolioAsyn(AppUser appUser, string symbol)
    {
       var PortfolioModel = await context.portfolios.FirstOrDefaultAsync(x=>x.AppUserId == appUser.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());
        if(PortfolioModel is null) return null;
        context.portfolios.Remove(PortfolioModel);
        await context.SaveChangesAsync();
        return PortfolioModel;
    }
}