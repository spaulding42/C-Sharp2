namespace SecurityApp.Models;
#pragma warning disable CS8618

public class Customer : Person
{
    public int CustomerID {get; set; }
    public Address FullAddress {get; set; }
    public DateTime DoB {get; set;}

    public Customer(string firstName, string lastName, string email, Address fullAddress, DateTime doB) : base(firstName,lastName,email)
    {
        FullAddress = fullAddress;
        DoB = doB;
    }
}