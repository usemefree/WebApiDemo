using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;

namespace WebApiDemo.Models;

public class Comment
{
    public int Id { get; set; }=0;
    public string Title { get; set; }=string.Empty;
    public string Content { get; set; }=string.Empty;
    public DateTime CreatedOn { get; set; }= DateTime.Now;

    //Navigation property
    public int? StockId { get; set; }=null;
    public Stock? Stock { get; set; }=null;

    
    public string AppUserId { get; set; }=string.Empty;
    public AppUser AppUser { get; set; }=null;
}
