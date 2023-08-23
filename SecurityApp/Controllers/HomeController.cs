using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
        User? loggedUser = db.Users.FirstOrDefault(x => x.UserId == HttpContext.Session.GetInt32("UUID"));

        // generate a list of all accounts with the UserID == Account.TechID

        return View("Dashboard", loggedUser);
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
