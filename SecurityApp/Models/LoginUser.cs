#pragma warning disable CS8618
namespace SecurityApp;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[NotMapped]
public class LoginUser
{
    [Required(ErrorMessage = "is required.")]
    public string LoginEmail { get; set; }

    [Required(ErrorMessage = "is required.")]
    [DataType(DataType.Password)]
    public string LoginPassword { get; set; }
}