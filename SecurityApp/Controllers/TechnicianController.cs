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
            foreach(Account InstalledAccount in InstalledAccounts){
                InstalledAccount.AccountId = InstalledAccount.AccountId + 1000;
            }
            
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
    public IActionResult LookupInstall(int accountId)
    {        
        if(ModelState.IsValid)
        {
            Account? dbAccount = db.Accounts.Include(c=>c.customer).FirstOrDefault(x => x.AccountId == accountId - 1000);
                        
            if(dbAccount == null)
            {
                ModelState.AddModelError("ARNum", "account not found");
                return Json(new { success = false , error = "account not found" });
            }
            if(dbAccount != null && dbAccount.TechId != null && HttpContext.Session.GetInt32("UUID") != dbAccount.TechId)
            {
                ModelState.AddModelError("ARNum", "is assigned to a different tech");
                return Json(new { success = false , error = "is assigned to a different tech" });

            }

            // if no tech is assigned to the job --OR-- the tech that is assigned is the tech that is logged in then we can proceed
            if (dbAccount != null && dbAccount.TechId == null || dbAccount != null && dbAccount.TechId == HttpContext.Session.GetInt32("UUID"))
            {
                HttpContext.Session.SetInt32("Account", dbAccount.AccountId);
                dbAccount.TechId = HttpContext.Session.GetInt32("UUID");
                dbAccount.UpdatedAt = DateTime.Now;
                db.Update(dbAccount);
                db.SaveChanges();
                List<Item> dbItems = db.Items.Where(i => i.AccountId == dbAccount.AccountId).OrderBy(i=>i.Zone).ToList();
                dbAccount.ItemList = dbItems;           
                return Json(new { success = true });
            }
            
        }
        return Json(new { success = false, error = "Hello World" });
    }

    [HttpGet("/install/{arNum}/lookup")]
    public IActionResult LookupWithAr(int arNum)
    {
        Account? dbAccount = db.Accounts.Include(c=>c.customer).FirstOrDefault(x=>x.AccountId == arNum-1000);
        if(dbAccount != null && dbAccount.TechId == HttpContext.Session.GetInt32("UUID"))
        {
            HttpContext.Session.SetInt32("Account", dbAccount.AccountId);
            List<Item> dbItems = db.Items.Where(i => i.AccountId == dbAccount.AccountId).OrderBy(i=>i.Zone).ToList();
            dbAccount.ItemList = dbItems;
            return View("NewInstall", dbAccount);
        }
        return RedirectToAction("Dashboard", "Home");
    }

    //adding equipment routes
    [HttpGet("/equipment/getcategory")]
    public IActionResult GetCategory()
    {
        //get full unassigned list from db
        List<Item> dbItems = db.Items.Where(i => i.AccountId == 0).ToList();
        List<string> uniqueCategories = new List<string>();

        //select 1 from list and ignore duplicates
        for(int i = 0; i < dbItems.Count; i++)
        {
            //check if the item we're looking at has a category that is already in the uniqueCategores list
            // if not then add to list. if it is then ignore
            if(!uniqueCategories.Contains(dbItems[i].Category))
            {
                uniqueCategories.Add(dbItems[i].Category);
            }
            
        }
        //use ViewBag to make string array of unique Categories
        // ViewBag.cats = uniqueCategories;
        //render page
        return View("SelectCategory", uniqueCategories);
    }

    [HttpGet("/equipment/{category}/getitem")]
    public IActionResult GetItem(string category)
    {
        // get all items in unassigned db where Category = category
        List<Item> sortedItems = db.Items.Where(x => x.AccountId == 0 && x.Category == category).ToList();
        List<string> ItemNameList = new List<string>();
        for(int i = 0; i < sortedItems.Count; i++)
        {
            if(!ItemNameList.Contains(sortedItems[i].ItemName))
            {
                ItemNameList.Add(sortedItems[i].ItemName);
            }
        }
        ViewBag.Category = category;
        ViewBag.ItemList = ItemNameList;
        return View("SelectItem");
    }

    [HttpPost("/equipment/add")]
    public IActionResult AddItem(Item newItem)
    {
        Item? dbItem = db.Items.Where(x => x.Category == newItem.Category && x.ItemName == newItem.ItemName).FirstOrDefault();
        Account? accountNum = db.Accounts.Include(c=> c.customer).FirstOrDefault(a=>a.AccountId ==HttpContext.Session.GetInt32("Account"));
        int? UUID = HttpContext.Session.GetInt32("UUID");
        // add price to item
        
        if(dbItem == null || accountNum == null || UUID == null)
        {
            //this is temporary-----------
            //would rather go back to add equipment page if any are null
            return RedirectToAction("Logout", "Users");
        }
        else{
            newItem.Price = dbItem.Price;
            newItem.AccountId = accountNum.AccountId;
            newItem.RoleId = (int)UUID;
            newItem.CreatedAt = DateTime.Now;
            newItem.UpdatedAt = DateTime.Now;
            if(newItem.Zone== 0) newItem.SignalVerified = true;
        }

        // validate 
        if(ModelState.IsValid)
        {
            db.Items.Add(newItem);
            db.SaveChanges();
            List<Item> dbItems = db.Items.Where(x => x.AccountId == accountNum.AccountId).OrderBy(i=>i.Zone).ToList();
            accountNum.ItemList = dbItems;
            return View("NewInstall", accountNum);  
        }
        else
        {
            List<Item> sortedItems = db.Items.Where(x => x.AccountId == 0 && x.Category == newItem.Category).ToList();
            List<string> ItemNameList = new List<string>();
            for(int i = 0; i < sortedItems.Count; i++)
            {
                if(!ItemNameList.Contains(sortedItems[i].ItemName))
                {
                    ItemNameList.Add(sortedItems[i].ItemName);
                }
            }
            ViewBag.Category = newItem.Category;
            ViewBag.ItemList = ItemNameList;
            return View("SelectItem");
        }
    }

    // edit item
    [HttpGet("/technician/{itemToEdit}/edit")]
    public IActionResult EditItem(int itemToEdit)
    {
        Item? ItemToEdit = db.Items.FirstOrDefault(i => i.ItemId == itemToEdit);
        
        if (ItemToEdit == null || HttpContext.Session.GetInt32("UUID") != ItemToEdit.RoleId)
        {
            return RedirectToAction("Dashboard", "Home");
        }

        return View("EditItem", ItemToEdit);
    }

    [HttpPost("/technician/update")]
    public IActionResult UpdateItem(Item itemToUpdate)
    {
        Item? dbItem = db.Items.FirstOrDefault(i => i.ItemId == itemToUpdate.ItemId);
        Account? dbAccount = db.Accounts.Include(c =>c.customer).FirstOrDefault(a=>a.AccountId == HttpContext.Session.GetInt32("Account"));
        int? UUID = HttpContext.Session.GetInt32("UUID");

        if (dbItem == null || dbAccount == null || UUID == null)
        {
            return RedirectToAction("Dashboard", "Home");
        }
        if(ModelState.IsValid)
        {
            if(dbItem.Zone != itemToUpdate.Zone) dbItem.SignalVerified = false;
            dbItem.Zone = itemToUpdate.Zone;
            if(dbItem.Zone== 0) dbItem.SignalVerified = true;
            dbItem.Location = itemToUpdate.Location;
            dbItem.UpdatedAt = DateTime.Now;
            db.Update(dbItem);
            db.SaveChanges();
            List<Item> dbItems = db.Items.Where(i => i.AccountId == dbAccount.AccountId).OrderBy(i=>i.Zone).ToList();
            dbAccount.ItemList = dbItems;
            return View("NewInstall", dbAccount);
        }

        return View("EditItem", itemToUpdate);
        
    }

    //delete item
    [HttpPost("/technician/{itemToDelete}/delete")]
    public IActionResult DeleteItem(int itemToDelete)
    {
        int? UUID = HttpContext.Session.GetInt32("UUID");
        Item? dbItemToDelete = db.Items.FirstOrDefault(i => i.ItemId==itemToDelete);
        Account? dbAccount = db.Accounts.Include(c=>c.customer).FirstOrDefault(a=>a.AccountId == HttpContext.Session.GetInt32("Account"));
        if(dbAccount == null || UUID == null || dbItemToDelete == null) return RedirectToAction("Dashboard", "Home");

        if(dbItemToDelete.RoleId == UUID)
        {
            db.Items.Remove(dbItemToDelete);
            db.SaveChanges();
        }

        List<Item> dbItems = db.Items.Where(i => i.AccountId == dbAccount.AccountId).OrderBy(i=>i.Zone).ToList();
        dbAccount.ItemList = dbItems;
        return View("NewInstall", dbAccount);
    }

    //finish install
    [HttpPost("/technician/finishinstall")]
    public IActionResult FinishInstall()
    {
        int? UUID = HttpContext.Session.GetInt32("UUID");
        User? loggedUser = db.Users.FirstOrDefault(u => u.RoleId == UUID);
        Account? dbAccount = db.Accounts.FirstOrDefault(a=>a.AccountId == HttpContext.Session.GetInt32("Account"));
        List<Item>? dbItems = db.Items.Where(i => i.AccountId == HttpContext.Session.GetInt32("Account")).ToList();

        if(UUID == null || dbAccount == null || dbItems == null || loggedUser != null && loggedUser.Role != "Technician")
        {
            return RedirectToAction("Dashboard", "Home");
        }

        bool verified = true;
        foreach (Item item in dbItems)
        {
            if(item.SignalVerified == false) verified = false;
        }
        if(verified)
        {
            dbAccount.Installed = true;
            dbAccount.UpdatedAt = DateTime.Now;
            db.Update(dbAccount);
            db.SaveChanges();
        }
        return RedirectToAction("Dashboard", "Home");
    }

    [HttpPost("/technician/{itemToVerify}/verify")]
    public IActionResult VerifyItem(int itemToVerify)
    {
        int? UUID = HttpContext.Session.GetInt32("UUID");
        User? loggedUser = db.Users.FirstOrDefault(u => u.RoleId == UUID);
        Account? dbAccount = db.Accounts.Include(c=>c.customer).FirstOrDefault(a=>a.AccountId == HttpContext.Session.GetInt32("Account"));
        Item? dbItem = db.Items.FirstOrDefault(i => i.ItemId == itemToVerify);
        if(dbItem != null && loggedUser != null && loggedUser.Role == "Technician")
        {
            dbItem.SignalVerified = true;
            db.Update(dbItem);
            db.SaveChanges();
        }
        if(dbAccount == null) return RedirectToAction("Logout", "Users");
        
        List<Item> dbItems = db.Items.Where(i => i.AccountId == dbAccount.AccountId).OrderBy(i=>i.Zone).ToList();
        dbAccount.ItemList = dbItems;
        return View("NewInstall", dbAccount);
    }
}

