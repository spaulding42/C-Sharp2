#pragma warning disable CS8618
namespace SecurityApp;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SecurityApp.Models;

[NotMapped]
public class ForgotUser
{
    public string ForgotEmail {get; set; }
    public DateTime ForgotDoB {get; set; } = DateTime.Now;
}