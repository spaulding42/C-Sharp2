namespace SecurityApp.Models;
#pragma warning disable CS8618

public class Customer
{
    public int CustomerID {get; set; }
     public string FirstName {get; set; }
    public string LastName {get; set; }
    public string Email {get; set; }

    public DateTime DoB {get; set;}
    public Address? FullAddress {get; set; }

    public Customer (string firstName, string lastName, string email, DateTime doB, Address address)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        DoB = doB;
        FullAddress = address;
    }


}