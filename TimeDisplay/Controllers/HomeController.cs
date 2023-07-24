using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TimeDisplay.Models;

namespace TimeDisplay.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        DateTime currentTime = DateTime.Now;
        DateTime endTime = new DateTime(2023, 7, 25);
        var difOfDates = endTime - currentTime;

        ViewBag.CurrentTime = currentTime;
        ViewBag.EndTime = endTime;
        ViewBag.DifOfDates = difOfDates;

        return View();
    }

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
