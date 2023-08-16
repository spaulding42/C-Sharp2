#pragma warning disable CS8618
namespace LoginReg;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    public int UserId { get; set; }
    
    [Required(ErrorMessage ="is required!")]
    [MinLength(2, ErrorMessage ="must be 2 or more characters")]
    public string FirstName {get; set; }

    [Required(ErrorMessage ="is required!")]
    [MinLength(2, ErrorMessage ="must be 2 or more characters")]
    public string LastName {get; set; }

    [Required(ErrorMessage = "is required!")]
    [DataType(DataType.EmailAddress)]
    public string Email {get; set; }
    
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "is required!")]
    public string Password {get; set; }
    
    [NotMapped]
    [Compare("Password", ErrorMessage = "doesn't match password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword {get; set; }

    [Required]
    public string Role {get; set;}

    public int? RoleId {get; set;}

    public DateTime CreatedAt {get; set;} = DateTime.Now;
    public DateTime UpdatedAt {get; set;} = DateTime.Now;
}