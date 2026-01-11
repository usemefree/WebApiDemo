using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiDemo.Dtos.Stock;

public class CreateStockRequestDto
{
    [Required]
    [MaxLength(250, ErrorMessage = "Symbol can not be over 250 charager")]
    public string Symbol { get; set; } = string.Empty;

    [Required]
    [MaxLength(250, ErrorMessage = "CompanyName can not be over 250 charager")]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    [Range(1, 10000000)]
    public decimal Purchase { get; set; } = 0.0M;

    [Required]
    [Range(1.001, 100)]
    public decimal LastDiv { get; set; } = 0.0M;

    [Required]
    [MaxLength(50, ErrorMessage = "Industry can not be over 50 charager")]
    public string Industry { get; set; } = string.Empty;

    [Required]
    [Range(1, 90000000)]
    public long MarketCap { get; set; } = 0;

}
