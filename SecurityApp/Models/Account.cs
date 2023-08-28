using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityApp.Models;
#pragma warning disable CS8618


public class Account
{
    [Key]
    public int AccountId {get; set; }
    
    public string AccountPassword {get; set; }
    
    public DateTime ContractStart {get; set; } = DateTime.Now;
    
    public DateTime ContractEnd {get; set; }
    
    public bool Installed {get; set; } = false;
    
    public DateTime CreatedAt {get; set; } = DateTime.Now;

    public DateTime UpdatedAt {get; set; } = DateTime.Now;
    

    // Relationship props
    
    public List<Item> ItemList = new List<Item>();
    
    
    public int CustomerId {get; set; }
    public Customer? customer{ get; set; }
    
    
    public int? TechId {get; set; }
    [NotMapped]
    public User? tech { get; set; }
    
    
    public int SalesId {get; set; }
    [NotMapped]
    public User? salesman { get; set; }

} 