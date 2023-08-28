#pragma warning disable CS8618
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityApp.Models;


public class Customer 
{

    [Key]
    public int CustomerId {get; set; }

    [Required(ErrorMessage ="is required")]
    [MinLength(2,ErrorMessage ="must be 2 or more characters")]
    public string FirstName { get; set; }

    [Required(ErrorMessage ="is required")]
    [MinLength(2,ErrorMessage ="must be 2 or more characters")]
    public string LastName { get; set; }

    [Required]
    public DateTime BirthDate {get; set; }

    [Required(ErrorMessage ="Street Address is required")]
    [MinLength(3,ErrorMessage ="must be 3 or more characters")]
    public string Line1 {get; set; }

    public string? Line2 {get; set; }
    [Required]
    public string City {get; set; }
    [Required]
    public string State {get; set; }
    [Required]
    public int Zip {get; set; }
    
    public bool Assigned {get; set;} = false;
    
    public DateTime CreatedAt {get; set; } = DateTime.Now;
    public DateTime UpdatedAt {get; set; } 
}