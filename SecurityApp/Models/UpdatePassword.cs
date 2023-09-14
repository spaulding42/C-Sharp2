#pragma warning disable CS8618
namespace SecurityApp;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[NotMapped]
public class UpdatePassword
{
    [Required(ErrorMessage = "is required.")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; }

    [Required(ErrorMessage = "is required.")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
    
    [Required(ErrorMessage = "is required.")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "doesn't match new password")]
    public string ConfirmNewPassword { get; set; }
}