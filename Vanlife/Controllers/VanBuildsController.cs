
using Microsoft.AspNetCore.Mvc;
using Vanlife.Models;
using Microsoft.AspNetCore.Identity;

namespace Vanlife.Controllers;

public class VanBuildsController : Controller
{
    [HttpGet("/vanbuilds")]
    public IActionResult Index()
    {

        return View("Index");
    }

    [HttpGet("/vanbuilds/mine")]
    public IActionResult MyVan()
    {
        return View("MyVan");
    }
}