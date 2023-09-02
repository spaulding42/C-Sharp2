using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecurityApp.Models;

namespace SecurityApp.Controllers;

public class SalesmanController : Controller
{
    private MyContext db;
    public SalesmanController (MyContext DB)
    {
        db = DB;
    }

    [HttpGet("/dashboard/salesman")]
    public IActionResult SalesmanDashboard()
    {
        if (HttpContext.Session.GetInt32("UUID") == null) 
        {
            return RedirectToAction("Index", "Users");
        }
        User? loggedUser = db.Users.FirstOrDefault(x => x.RoleId == HttpContext.Session.GetInt32("UUID"));
      
        if(loggedUser != null)
        {
            
            List<Account> SoldAccounts = db.Accounts.Include(c => c.customer).Where(x => x.SalesId == HttpContext.Session.GetInt32("UUID")).ToList();
            
            if(SoldAccounts != null)
            {
                loggedUser.accounts = SoldAccounts;
            }
        }
        return View("SalesmanDashboard", loggedUser);
    }

    //Customer routes-----------------------------------------
    [HttpGet("/customer/new")]
    public IActionResult NewCustomer()
    {
        if (HttpContext.Session.GetInt32("UUID") == null) 
        {
            return RedirectToAction("Index", "Users");
        }
        
        User? loggedUser = db.Users.FirstOrDefault(x => x.RoleId == HttpContext.Session.GetInt32("UUID"));
        if (loggedUser != null && loggedUser.Role != "Salesman")
        {
            return RedirectToAction("Logout", "Users");
        }

        return View("CreateCustomer");
    }

    [HttpPost("/customer/create")]
    public IActionResult SubmitCustomer(Customer NewCustomer)
    {
        if(ModelState.IsValid)
        {
            NewCustomer.CreatedAt = DateTime.Now;
            NewCustomer.UpdatedAt = DateTime.Now;
            db.Customers.Add(NewCustomer);
            db.SaveChanges();

            HttpContext.Session.SetInt32("CID", NewCustomer.CustomerId);
            HttpContext.Session.SetString("CXName", NewCustomer.FirstName + " " + NewCustomer.LastName);
            return View("CreateAccount");
        }
        Console.WriteLine("Not Valid!");
        return RedirectToAction("SubmitCustomer", "Salesman");
    }

    //edit customer routes 

    [HttpGet("/customer/{customerId}/edit")]
    public IActionResult EditCustomer(int customerId)
    {
        Customer? dbCustomer = db.Customers.FirstOrDefault(i => i.CustomerId == customerId);
        if (dbCustomer != null )
        {
            return View("EditCustomer", dbCustomer);
        }
        return RedirectToAction("SalesmanDashboard");
    }

    [HttpPost("/customer/{CustomerId}/update")]
    public IActionResult UpdateCustomer(Customer CustomerToUpdate)
    {
        if (ModelState.IsValid)
        {
            CustomerToUpdate.UpdatedAt = DateTime.Now;
            db.Customers.Update(CustomerToUpdate);
            db.SaveChanges();
            return RedirectToAction("SalesmanDashboard");
        }
        return View("EditCustomer", CustomerToUpdate);
    }

    //Account Routes ----------------------------------------
    //Edit Account  will allow for editing both accounts and customers

    [HttpGet("/account/{accountId}/view")]
    public IActionResult ViewAccount(int accountId)
    {
        Account? dbAccount = db.Accounts.Include(c => c.customer).FirstOrDefault(x => x.AccountId == accountId);
        if (dbAccount != null)
        {
            if (dbAccount.SalesId == HttpContext.Session.GetInt32("UUID") && dbAccount.customer != null)
            {
                HttpContext.Session.SetInt32("CID", dbAccount.customer.CustomerId);
                return View("ViewOneAccount", dbAccount);
            }
        }
        //if no account or not created by the logged in salesman then redirecct to dashboard
        return RedirectToAction("SalesmanDashboard", "Salesman");
    }

    [HttpGet("/account/{accountId}/edit")]
    public IActionResult EditAccount(int accountId)
    {
        Account? dbAccount = db.Accounts.FirstOrDefault(i => i.AccountId == accountId);
        if (dbAccount != null)
        {
            return View("EditAccount", dbAccount);
        }
        return RedirectToAction("SalesmanDashboard");
    }

    [HttpPost("/account/{AccountId}/update")]
    public IActionResult UpdateAccount(Account AccountToUpdate)
    {
        if (ModelState.IsValid)
        {
            AccountToUpdate.UpdatedAt = DateTime.Now;
            db.Accounts.Update(AccountToUpdate);
            db.SaveChanges();
            return RedirectToAction("SalesmanDashboard");
        }
        return View("EditAccount", AccountToUpdate);
    }
    
    [HttpPost("/account/create")]
    public IActionResult RegisterAccount(Account NewAccount)
    {
        if(ModelState.IsValid)
        {
            db.Accounts.Add(NewAccount);
            db.SaveChanges();
            return RedirectToAction("SalesmanDashboard");
        }

        return View("CreateAccount");
    }

    [HttpPost("/account/{accountId}/delete")]
    public IActionResult DeleteAccount(int accountId)
    {
        Account? AccountToDelete = db.Accounts.FirstOrDefault(i => i.AccountId == accountId);
        // make sure the account was created by the logged in salesman and that that Account is in the db

        if (AccountToDelete != null && AccountToDelete.SalesId == HttpContext.Session.GetInt32("UUID"))
        {
            db.Accounts.Remove(AccountToDelete);
            db.SaveChanges();
        }

        return RedirectToAction("Dashboard", "Home");
    }
}