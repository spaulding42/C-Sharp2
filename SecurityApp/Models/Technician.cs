namespace SecurityApp.Models;
#pragma warning disable CS8618

public class Technician : Person
{
    public int TechID {get; set; }
    public int InstallCount {get; set; } = 0;
    public List<Account> Installs = new List<Account>();

    public Technician(string firstName, string lastName, string email ) : base(firstName,lastName,email)
    {
        
    }
}