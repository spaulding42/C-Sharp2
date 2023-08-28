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
        return View("Salesman", loggedUser);
    }

    [HttpGet("/customer/new")]
    public IActionResult NewCustomer()
    {
        if (HttpContext.Session.GetInt32("UUID") == null) 
        {
            return RedirectToAction("Index", "Users");
        }
        User? loggedUser = db.Users.FirstOrDefault(x => x.RoleId == HttpContext.Session.GetInt32("UUID"));

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
        return View("CreateCustomer");
    }

    [HttpPost("/account/create")]
    public IActionResult RegisterAccount(Account NewAccount)
    {
        if(ModelState.IsValid)
        {
            db.Accounts.Add(NewAccount);
            db.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        return View("CreateAccount");
    }
}