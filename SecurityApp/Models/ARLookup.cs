#pragma warning disable CS8618
namespace SecurityApp;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[NotMapped]
public class ARLookup
{
    [Required(ErrorMessage = "is required.")]
    [Range(0,9999,ErrorMessage = "out of range")]
    public int ARNum {get; set;}
}