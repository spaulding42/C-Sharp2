using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityApp.Models;
#pragma warning disable CS8618

[NotMapped]
public class Account
{
    public int AccountID {get; set; }
    
    public string AccountPassword {get; set; }
    
    public DateTime ContractStart {get; set; } = DateTime.Now;
    
    public DateTime ContractEnd {get; set; }
    
    public string CustomerID {get; set; }
    
    public string SalesID {get; set; }
    
    public string? TechID {get; set; }
    
    public List<Item> ItemList = new List<Item>();
}