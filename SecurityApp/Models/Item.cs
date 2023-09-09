using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityApp.Models;
#pragma warning disable CS8618


public class Item
{
    [Key]
    public int ItemId {get; set; }
    
    [Required(ErrorMessage ="is required")]
    [MinLength(3, ErrorMessage ="must be 3 or more characters")]
    public string Category {get; set; }

    [Range(0, 100, ErrorMessage ="must be between 0 and 100")]
    public int Zone {get; set; } = 0;

    [Required(ErrorMessage ="is required")]
    [MinLength(3, ErrorMessage ="must be 3 or more characters")]
    public string ItemName {get; set; }

    [Required(ErrorMessage ="is required")]
    [MinLength(3, ErrorMessage ="must be 3 or more characters")]
    public string Location {get; set; } = "Unassigned";
    
    [Required(ErrorMessage ="is required")]
    [Range(0,10000,ErrorMessage ="must be in range 0,10000")]
    public float Price {get; set; }

    public int AccountId {get; set; } = 0;

    public bool SignalVerified {get; set; } = false;

    public int? RoleId {get; set; }

    public DateTime CreatedAt {get; set; }

    public DateTime UpdatedAt {get; set;}
}