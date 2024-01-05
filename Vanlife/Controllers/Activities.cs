using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Vanlife.Models;

namespace Vanlife.Controllers;

public class ActivitiesController : Controller
{
    [HttpGet("/activities")]
    public IActionResult Activities()
    {
        return View("Activities");
    }
}