using System.ComponentModel.DataAnnotations;

namespace SecurityApp.Models;
#pragma warning disable CS8618

public class Technician
{
    [Key]
    public int TechID {get; set; }
    public string FirstName {get; set; }
    public string LastName {get; set; }
    public string Email {get; set; }

    public Technician (string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
    public List<Account> Installs = new List<Account>();
    public int InstallCount()
    {
        return Installs.Count;
    }


}