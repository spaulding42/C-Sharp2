using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityApp.Models;
#pragma warning disable CS8618


public class Account
{
    [Key]
    public int AccountId {get; set; }
    
    [Required(ErrorMessage = "is required.")]
    [MinLength(3,ErrorMessage = "must be 3 or more characters")]
    [MaxLength(15,ErrorMessage = "must be less than 15 characters")]
    public string AccountPassword {get; set; }
    
    [Required(ErrorMessage = "is required.")]
    public DateTime ContractStart {get; set; } = DateTime.Now;
    
    [Required(ErrorMessage = "is required.")]
    public DateTime ContractEnd {get; set; }
    
    [Required(ErrorMessage = "is required.")]
    [Range(0,100,ErrorMessage = "must be between 0 and 100")]
    public float RMR {get; set; }

    [Required]
    [Range(1,10000, ErrorMessage = "must be in range 1,10000")]
    public float EquipmentCost {get; set; } = 0;

    public string? SalesNotes {get; set;} = "";
        
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