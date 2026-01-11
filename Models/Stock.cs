using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiDemo.Models;

public class Stock
{
    public int Id { get; set; } = 0;
    public string Symbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Purchase { get; set; } = 0.0M;
    [Column(TypeName = "decimal(18,2)")]
    public decimal LastDiv { get; set; } = 0.0M;
    public string Industry { get; set; } = string.Empty;
    public long MarketCap { get; set; } = 0;

    //navigation Property
    public List<Comment> Comments { get; set; } = new List<Comment>();

    public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();

}
