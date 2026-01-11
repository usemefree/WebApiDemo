using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiDemo.Models;

namespace WebApiDemo.Interfaces;

public interface IPortfolioRepository
{
    Task<List<Stock>> GetUserPortfolio(AppUser user);
    Task<Portfolio> CreateAsyn(Portfolio portfolio);
    Task<Portfolio> DeletePortfolioAsyn(AppUser appUser, string symbol);
}