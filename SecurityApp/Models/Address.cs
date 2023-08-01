#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace SecurityApp.Models;

public class Address 
{
    [Required]
    public int AddressID {get; set; }
    [Required(ErrorMessage ="Street Address is required")]
    [MinLength(3,ErrorMessage ="Address must be 3 or more characters")]
    public string Line1 {get; set; }

    public string? Line2 {get; set; }
    [Required]
    public string City {get; set; }
    [Required]
    public string State {get; set; }
    [Required]
    public int Zip {get; set; }
}