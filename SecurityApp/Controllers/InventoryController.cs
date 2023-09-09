using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SecurityApp.Models;

namespace SecurityApp.Controllers;

public class InventoryController : Controller
{
    private MyContext db;
    public InventoryController (MyContext DB)
    {
        db = DB;
    }

    [HttpGet("/inventory")]
    public IActionResult InventoryDashboard()
    {
        int? UUID = HttpContext.Session.GetInt32("UUID");
        User? loggedUser = db.Users.FirstOrDefault(u=>u.RoleId == UUID);
        if(!Authorized())
        {
            return RedirectToAction("Logout", "Users");
        }
        return View("InventoryDashboard", loggedUser);
    }
    
    [HttpGet("/inventory/create")]
    public IActionResult CreateItem()
    {
        int? UUID = HttpContext.Session.GetInt32("UUID");
        User? loggedUser = db.Users.FirstOrDefault(u=>u.RoleId == UUID);
        if(!Authorized())
        {
            return RedirectToAction("Logout", "Users");
        }

        List<Item> selfCreated = db.Items.Where(i => i.RoleId == UUID).ToList();

        return View("CreateItem", selfCreated);
    }

    [HttpPost("/inventory/new/submit")]
    public IActionResult SubmitItem(Item newItem)
    {
        int? UUID = HttpContext.Session.GetInt32("UUID");
        
        User? loggedUser = db.Users.FirstOrDefault(u=>u.RoleId == UUID);
        if(!Authorized())
        {
            return RedirectToAction("Logout", "Users");
        }       

        if(ModelState.IsValid)
        {
            newItem.RoleId = UUID;
            newItem.CreatedAt = DateTime.Now;
            newItem.UpdatedAt = DateTime.Now;
            db.Add(newItem);
            db.SaveChanges();
            return RedirectToAction("CreateItem");
        }

        return View("CreateItem");
    }

    [HttpGet("/audit/account")]
    public IActionResult AuditAccount()
    {
        if(!Authorized())
        {
            return RedirectToAction("Logout", "Users");
        }

        return View("AuditAccount");
    }

    [HttpGet("/inventory/unassigned/all")]
    public IActionResult AllUnassigned()
    {
        if(!Authorized())
        {
            return RedirectToAction("Logout", "Users");
        }

        return View("AllUnassigned");
    }

    [HttpGet("/inventory/assigned/all")]
    public IActionResult AllAssigned()
    {
        if(!Authorized())
        {
            return RedirectToAction("Logout", "Users");
        }

        return View("AllAssigned");
    }

    private bool Authorized()
    {
        bool authorized = true;
        int? UUID = HttpContext.Session.GetInt32("UUID");
        User? loggedUser = db.Users.FirstOrDefault(u=>u.RoleId == UUID);
        if (UUID == null)
        {
            authorized = false;
        }
        if(loggedUser == null || loggedUser.Role != "Inventory")
        {
            authorized = false;
        }
        return authorized;
    }
}
