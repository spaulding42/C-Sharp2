using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SecurityApp.Models;

namespace SecurityApp.Controllers;

public class InventoryController : Controller
{
    [HttpGet("/inventory")]
    public IActionResult Inventory()
    {
        return View("Inventory");
    }
    
}
