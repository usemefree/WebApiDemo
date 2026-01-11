using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiDemo.HelperFilter;

public class StockQueryObject
{
    public string? Symbol { get; set; } = null;
    public string? CompanyName { get; set; } = null;
    public string? Sortby { get; set; }=null;
    public bool IsDescending{get;set;} = false;

}