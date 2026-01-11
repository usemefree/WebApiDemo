using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiDemo.Dtos.Comment;

namespace WebApiDemo.Dtos.Stock;

public class StockDto
{
    public int Id { get; set; } = 0;
    public string Symbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public decimal Purchase { get; set; } = 0.0M;
    public decimal LastDiv { get; set; } = 0.0M;
    public string Industry { get; set; } = string.Empty;
    public long MarketCap { get; set; } = 0;
    public List<CommentDto?> Comments { get; set; } 
}
