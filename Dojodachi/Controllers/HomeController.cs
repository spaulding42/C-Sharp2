using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dojodachi.Models;

namespace Dojodachi.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet("/")]
    public IActionResult Index()
    {
        // will only set to the default values if happiness is null meaning its not in session
        HttpContext.Session.SetInt32("Fullness", 20);
        HttpContext.Session.SetInt32("Happiness", 20);
        HttpContext.Session.SetInt32("Meals", 3);
        HttpContext.Session.SetInt32("Energy", 50);
        HttpContext.Session.SetString("Message", "Click an action to affect your Dojodachi!");
        Console.WriteLine("session set!");
        return View();
    }

    [HttpGet("/dojodachi/new")]
    public IActionResult NewGame()
    {
        HttpContext.Session.SetString("Action", "new");
        return RedirectToAction("DojoDachi");
    }

    [HttpGet("/dojodachi")]
    public IActionResult DojoDachi()
    {
        string? action = HttpContext.Session.GetString("Message");
        if (action == null)
        {
            return RedirectToAction("Index");
        }
        if (action == "Click an action to affect your Dojodachi!")
        {

        }

        return View("Dojodachi");
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
