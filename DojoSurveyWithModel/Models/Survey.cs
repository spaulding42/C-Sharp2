#pragma warning disable CS8618
namespace DojoSurveyWithModel;
using System.ComponentModel.DataAnnotations;

public class Survey
{
    [Required(ErrorMessage ="Name is required!")]
    [MinLength(2, ErrorMessage ="Name must be 2 or more characters")]
    public string Name {get; set; }
    [Required]
    public string Location {get; set; }
    [Required]
    public string Language {get; set; }

    [MinLength(20, ErrorMessage ="If you leave a comment it must be 20 chars or more")]
    public string? Comment {get; set;}
}