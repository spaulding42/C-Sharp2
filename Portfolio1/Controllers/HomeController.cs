using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Portfolio1.Models;

namespace Portfolio1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("projects")]
    public IActionResult Projects()
    {
        List<string> ProjNames = new List<string>()
        {
            "Cool Cat",
            "Firey Fist Bump",
            "Hot smiley"
        };
        List<string> ImgUrls = new List<string>() 
        {
            "https://m.media-amazon.com/images/I/81sMEvzsAxL._SL500_.jpg",
            "https://clipart-library.com/images/yikr9jXET.jpg",
            "https://marketplace.canva.com/EAFJDaBwwC0/1/0/450w/canva-violet-and-yellow-retro-cool-minimalist-trippy-psychedelic-phone-wallpaper-nW8pQK7YAxQ.jpg"

        };
        List<string> Descriptions = new List<string>()
        {
            "This project showcases how cool cats with sunglasses can look",
            "This project shwos that 2 flaming fists can still fist bump",
            "This project is proof that leaving your smile in the sun can cause it to melt"
        };
        ViewBag.projNames = ProjNames;
        ViewBag.imgUrls = ImgUrls;
        ViewBag.descriptions = Descriptions;
        return View();
    }

    [HttpGet("contact")]
    public IActionResult Contact()
    {
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
