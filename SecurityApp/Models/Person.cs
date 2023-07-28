#pragma warning disable CS8618
namespace SecurityApp.Models;

public class Person
{
    public string FirstName {get; set; }
    public string LastName {get; set; }
    public string Email {get; set; }

    public Person (string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}