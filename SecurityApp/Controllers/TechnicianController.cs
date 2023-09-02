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
        return View("NewInstallLookup");
    }
}