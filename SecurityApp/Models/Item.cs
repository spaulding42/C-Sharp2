using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityApp.Models;
#pragma warning disable CS8618

[NotMapped]
public class Item
{
    [Key]
    public int ItemId {get; set; }
    
    public int Zone {get; set; } = 0;

    [Required]
    public string ItemName {get; set; }

    [Required]
    public string Location {get; set; }
    
    [Required]
    public string Category {get; set; }

    public float? Price {get; set; }

    public int AccountId {get; set; } = 0;
}