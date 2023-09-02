using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecurityApp.Models;

namespace SecurityApp.Controllers;

public class HomeController : Controller
{
    private MyContext db;
    public HomeController (MyContext DB)
    {
        db = DB;
    }

    [HttpGet("/dashboard")]
    public IActionResult Dashboard()
    {
        if (HttpContext.Session.GetInt32("UUID") == null) 
        {
            return RedirectToAction("Index", "Users");
        }
        User? loggedUser = db.Users.FirstOrDefault(x => x.RoleId == HttpContext.Session.GetInt32("UUID"));

        //Re-Route to the proper dashboard depending on the user Role logged in        
        if(loggedUser != null)
        {
            if (loggedUser.Role == "Salesman")
            {
                Console.WriteLine("Made it here ---------------------------------------------------------------------");
                return RedirectToAction("SalesmanDashboard", "Salesman");
            }
            if (loggedUser.Role == "Technician")
            {
                return RedirectToAction("TechnicianDashboard", "Technician");
            }
            if (loggedUser.Role == "Inventory")
            {
                return RedirectToAction("InventoryDashboard", "Inventory");
            }
        }

        // if the managed to log in but don't match the 3 role options then return to login screen
        return RedirectToAction("Logout", "Users");
    }
    

    
    // pre programmed stuff
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
