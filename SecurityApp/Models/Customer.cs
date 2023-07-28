namespace SecurityApp.Models;
#pragma warning disable CS8618

public class Customer : Person
{
    
    public Address FullAddress {get; set; }
    public DateTime DoB {get; set;}
    public DateTime ContractStart {get; set; } = DateTime.Now;
    public DateTime ContractEnd {get; set; }

    public Customer(string firstName, string lastName, string email, Address fullAddress, DateTime doB, DateTime contractEnd) : base(firstName,lastName,email)
    {
        FullAddress = fullAddress;
        DoB = doB;
        ContractEnd = contractEnd;
    }
}