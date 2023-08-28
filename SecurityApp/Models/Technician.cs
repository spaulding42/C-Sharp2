#pragma warning disable CS8618
namespace SecurityApp;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[NotMapped]
public class Technician
{
    [Key]
    public int TechnicianID { get; set; }

    public int TechID { get; set; }


}