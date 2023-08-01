namespace SecurityApp.Models;
#pragma warning disable CS8618

public class Salesman : Person
{
    public int SalesID {get; set; }
    public int SaleCount {get; set; } = 0;
    public List<Account> Sales = new List<Account>();

    public Salesman(string firstName, string lastName, string email ) : base(firstName,lastName,email)
    {
        
    }
}