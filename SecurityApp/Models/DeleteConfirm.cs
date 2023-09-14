#pragma warning disable CS8618
namespace SecurityApp;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[NotMapped]
public class DeleteConfirm
{
    [Required(ErrorMessage = "must type 'delete' to confirm")]
    public string Confirmation {get; set; }
}