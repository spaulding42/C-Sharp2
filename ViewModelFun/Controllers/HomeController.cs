using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ViewModelFun.Models;

namespace ViewModelFun.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        string message = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Consequatur amet ea voluptatibus reprehenderit deserunt, fuga beatae libero quo nam deleniti culpa laudantium aliquam dolore sequi earum atque quam id fugiat.";
        return View("Index", message);
    }
    [HttpGet("/numbers")]
    public IActionResult Numbers()
    {
        int[] Numbers = {1,2,3,4,5,6,8};

        return View("Numbers", Numbers);
    }
    [HttpGet("/users")]
    public IActionResult Users()
    {
        List<User> Users = new List<User>();
        Users.Add(new User("Moose", "Phillips"));
        Users.Add(new User("Sarah", ""));
        Users.Add(new User("Jerry", ""));
        Users.Add(new User("Rene", "Ricky"));
        Users.Add(new User("Barbarah", ""));
        
        
        return View("Users", Users);
    }
    [HttpGet("/user")]
    public IActionResult OneUser()
    {
        User user1 = new User("Moose", "Phillips");
        return View("OneUser", user1);
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
