using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityApp.Models;
#pragma warning disable CS8618

[NotMapped]
public class Item
{
    public int ItemID {get; set; }
    public string ItemName {get; set; }
    public string Category {get; set; }
    public int Cost {get; set; }
}