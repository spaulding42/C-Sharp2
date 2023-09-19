using System.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        ViewBag.CompletedAccounts = 0;
        ViewBag.IncompleteAccounts = 0;
        
        List<Account> dbAccounts = db.Accounts.Where(a=>a.Installed == true).ToList();
        if(dbAccounts != null) ViewBag.CompletedAccounts = dbAccounts.Count;
        
        dbAccounts = db.Accounts.Where(a=>a.Installed == false).ToList();
        if(dbAccounts != null) ViewBag.IncompleteAccounts = dbAccounts.Count;

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

    [HttpGet("/inventory/{accountid}/view")]
    public IActionResult ViewAccount(int accountid)
    {
        Account? dbAccount = db.Accounts.Include(c=>c.customer).FirstOrDefault(a=>a.AccountId == accountid);
        if(dbAccount == null) return RedirectToAction("Dashboard", "Home");
        List<Item> installedItems = db.Items.Where(i=>i.AccountId == dbAccount.AccountId).ToList();
        if(installedItems != null) dbAccount.ItemList = installedItems;

        return View("ViewAccount", dbAccount);
    }

    [HttpPost("/inventory/lookup")]
    public IActionResult LookupAccount(ARLookup accountId)
    {
        if(ModelState.IsValid)
        {
            Account? dbAccount = db.Accounts.Include(c=>c.customer).FirstOrDefault(x => x.AccountId == accountId.ARNum);
            if(dbAccount == null)
            {
                ModelState.AddModelError("ARNum", "account not found");
            }
            else
            {
                HttpContext.Session.SetInt32("Account", dbAccount.AccountId);
                dbAccount.UpdatedAt = DateTime.Now;
                db.Update(dbAccount);
                db.SaveChanges();
                List<Item> dbItems = db.Items.Where(i => i.AccountId == dbAccount.AccountId).OrderBy(i=>i.Zone).ToList();
                dbAccount.ItemList = dbItems;
                return View("ViewAccount", dbAccount);
            }
        }
        return View("AuditAccount");
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
