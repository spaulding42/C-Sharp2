using System.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
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
            newItem.Category = newItem.Category.ToUpper();
            newItem.ItemName = newItem.ItemName.ToUpper();
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

        List<Item> unassignedItems = db.Items.Where(i => i.AccountId == 0).ToList();
        return View("AllUnassigned", unassignedItems);
    }

    [HttpGet("/inventory/assigned/all")]
    public IActionResult AllAssigned()
    {
        if(!Authorized())
        {
            return RedirectToAction("Logout", "Users");
        }
        List<Item> dbItems = db.Items.Where(i=>i.AccountId != 0).ToList();

        return View("AllAssigned", dbItems);
    }

    [HttpGet("/inventory/all/{sortby}")]
    public IActionResult SortBy(string sortby)
    {
        List<Item> dbItems = db.Items.Where(i=>i.AccountId !=0).ToList();

        if(sortby == "accountid")
        {
            dbItems = db.Items.Where(i=>i.AccountId != 0).OrderBy(i=>i.AccountId).ToList();
        }
        if(sortby == "category")
        {
            dbItems = db.Items.Where(i=>i.AccountId != 0).OrderBy(i=>i.Category).ToList();
        }
        if(sortby == "name")
        {
            dbItems = db.Items.Where(i=>i.AccountId != 0).OrderBy(i=>i.ItemName).ToList();
        }
        if(sortby == "price")
        {
            dbItems = db.Items.Where(i=>i.AccountId != 0).OrderBy(i=>i.Price).ToList();
        }
        if(sortby == "createdby")
        {
            dbItems = db.Items.Where(i=>i.AccountId != 0).OrderBy(i=>i.RoleId).ToList();
        }
        return View("AllAssigned", dbItems);
    }

    private bool Authorized()
    {
        bool authorized = true;
        int? UUID = HttpContext.Session.GetInt32("UUID");
        User? loggedUser = db.Users.FirstOrDefault(u=>u.RoleId == UUID);
        
        if(UUID == null || loggedUser == null || loggedUser.Role != "Inventory")
        {
            authorized = false;
        }
        return authorized;
    }

    
}
