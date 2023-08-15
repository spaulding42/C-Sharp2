using System.ComponentModel.DataAnnotations;

namespace SecurityApp.Models;
#pragma warning disable CS8618

public class Salesman
{
    [Key]
    public int SalesID {get; set; }
    public string FirstName {get; set; }
    public string LastName {get; set; }
    public string Email {get; set; }

    
    public List<Account> Sales = new List<Account>();
    public int SaleCount()
    {
        return Sales.Count;
    }
}

