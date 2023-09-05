using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecurityApp.Models;

namespace SecurityApp.Controllers;

public class TechnicianController : Controller
{
    private MyContext db;
    public TechnicianController (MyContext DB)
    {
        db = DB;
    }

    [HttpGet("/dashboard/technician")]
    public IActionResult TechnicianDashboard()
    {
        if (HttpContext.Session.GetInt32("UUID") == null) 
        {
            return RedirectToAction("Index", "Users");
        }
        User? loggedUser = db.Users.FirstOrDefault(x => x.RoleId == HttpContext.Session.GetInt32("UUID"));
      
        if(loggedUser != null)
        {
            
            List<Account> InstalledAccounts = db.Accounts.Include(c => c.customer).Where(x => x.TechId == HttpContext.Session.GetInt32("UUID")).ToList();
            
            if(InstalledAccounts != null)
            {
                loggedUser.accounts = InstalledAccounts;
            }
        }
        return View("TechnicianDashboard", loggedUser);
    }

    [HttpGet("/install/new")]
    public IActionResult NewInstall()
    {
        int? roleId = HttpContext.Session.GetInt32("UUID");
        if(roleId == null)
        {
            return RedirectToAction("Logout", "Users");
        }
        
        User? loggedUser = db.Users.FirstOrDefault(u => u.RoleId == roleId);
        if(loggedUser != null && loggedUser.Role == "Technician")
        {
                return View("NewInstallLookup");
        }
        return RedirectToAction("Dashboard", "Home");
        
    }

    [HttpPost("/install/lookup")]
    public IActionResult LookupInstall(ARLookup accountId)
    {
        if(ModelState.IsValid)
        {
            Account? dbAccount = db.Accounts.Include(c=>c.customer).FirstOrDefault(x => x.AccountId == accountId.ARNum);
            if(dbAccount == null)
            {
                ModelState.AddModelError("ARNum", "account not found");
            }
            if(dbAccount != null && dbAccount.TechId != null && HttpContext.Session.GetInt32("UUID") != dbAccount.TechId)
            {
                ModelState.AddModelError("ARNum", "is assigned to a different tech");
            }

            // if no tech is assigned to the job --OR-- the tech that is assigned is the tech that is logged in then we can proceed
            if (dbAccount != null && dbAccount.TechId == null || dbAccount != null && dbAccount.TechId == HttpContext.Session.GetInt32("UUID"))
            {
                HttpContext.Session.SetInt32("Account", dbAccount.AccountId);
                dbAccount.TechId = HttpContext.Session.GetInt32("UUID");
                dbAccount.UpdatedAt = DateTime.Now;
                db.Update(dbAccount);
                db.SaveChanges();
                return View("NewInstall", dbAccount);
            }
            
        }
        return View("NewInstallLookup");
    }
}