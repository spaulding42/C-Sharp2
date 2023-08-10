#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace CRUDelicious.Models;
public class Dish
{
    [Key]
    [Required(ErrorMessage = "is required")]
    public int DishId { get; set; }

    [Required(ErrorMessage = "is required")]
    [MinLength(2, ErrorMessage ="Must be 2 or more characters")]
    public string Name { get; set; } 

    [Required(ErrorMessage = "is required")]
    [MinLength(2, ErrorMessage ="Must be 2 or more characters")]
    public string Chef { get; set; } 

    [Required(ErrorMessage = "is required")]
    [Range(1,6 , ErrorMessage ="Must be between 1 and 5")]
    [Display(Name = "Tastiness 1-5: ")]
    public int Tastiness { get; set; }

    [Required(ErrorMessage = "is required")]
    [Range(1,10000, ErrorMessage ="Must be greater than 0")]
    public int Calories { get; set; }

    [Required(ErrorMessage = "is required")]
    public string Description { get; set; }


    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
                
