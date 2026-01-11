using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Extension;
using WebApiDemo.Interfaces;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortfolioController : ControllerBase
{
    private readonly UserManager<AppUser> userManager;
    private readonly IStockRepository stockRepository;
    private readonly IPortfolioRepository portfolioRepository;
    public ILogger<PortfolioController> _logger { get; }

    public PortfolioController(ILogger<PortfolioController> logger, UserManager<AppUser> userManager, IStockRepository stockRepository, IPortfolioRepository portfolioRepository)
    {
        this._logger = logger;
        this.portfolioRepository = portfolioRepository;
        this.userManager = userManager;
        this.stockRepository = stockRepository;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserPortfolio()
    {
        var userName = User.GetUserName();
        var appUser = await userManager.FindByNameAsync(userName);
        var userPortfolio = await portfolioRepository.GetUserPortfolio(appUser);
        _logger.LogInformation(JsonSerializer.Serialize<List<Stock>>(userPortfolio));
        return Ok(userPortfolio);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
        var username = User.GetUserName();
        var appUser = await userManager.FindByNameAsync(username);

        var stock = await stockRepository.GetBySymbolAsync(symbol);
        if (stock is null) return BadRequest("Stock not found");

        var userPortfolio = await portfolioRepository.GetUserPortfolio(appUser);

        if (userPortfolio.Any(x => x.Symbol.ToLower() == symbol.ToLower())) return BadRequest("Can not same Stock to portofolio");

        var portfolioModel = new Portfolio
        {
            AppUserId = appUser.Id,
            StockId = stock.Id
        };
        await portfolioRepository.CreateAsyn(portfolioModel);

        if (portfolioModel is null) return StatusCode(500, "Could not create");
        return Created();

    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeletePortfolio(string symbol)
    {
        var username = User.GetUserName();
        var appUser = await userManager.FindByNameAsync(username);
        var userPortfolio = await portfolioRepository.GetUserPortfolio(appUser);
        var filterStock = userPortfolio.Where(x => x.Symbol.ToLower() == symbol.ToLower()); 
        if (filterStock.Count()==1)
        {
            await portfolioRepository.DeletePortfolioAsyn(appUser,symbol);
        }
        else
        {
            return BadRequest("stock not in you portfolio");
        }
        return Ok();
    }

}
