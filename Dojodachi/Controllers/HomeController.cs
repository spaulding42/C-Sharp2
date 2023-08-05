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

        return View();
    }

    [HttpGet("/dojodachi/new")]
    public IActionResult NewGame()
    {
        //on load or returning to the home screen will start a new game
        HttpContext.Session.SetString("Action", "alive");
        HttpContext.Session.SetInt32("Fullness", 20);
        HttpContext.Session.SetInt32("Happiness", 20);
        HttpContext.Session.SetInt32("Meals", 3);
        HttpContext.Session.SetInt32("Energy", 50);
        HttpContext.Session.SetString("Message", "Click an action to affect your Dojodachi!");
        return RedirectToAction("DojoDachi");
    }

    // DONE
    // Feeding your Dojodachi costs 1 meal and gains a random amount of fullness between 5 and 10 (you cannot feed your Dojodachi if you do not have meals)
    // Every time you play with or feed your dojodachi there should be a 25% chance that it won't like it. Energy or meals will still decrease, but happiness and fullness won't change.
     [HttpGet("/dojodachi/feed")]
    public IActionResult Feed()
    {
        HttpContext.Session.SetString("Action", "alive");
        int? meals = HttpContext.Session.GetInt32("Meals");
        int? happiness = HttpContext.Session.GetInt32("Happiness");

        Random rand = new Random();
        int noLikey = rand.Next(4);
        if(meals > 0)
        {
            meals -=1;
            HttpContext.Session.SetInt32("Meals", (int)meals);
            if(noLikey < 3)
            {
                int fullnessBonus = rand.Next(5,11);
                Console.WriteLine(fullnessBonus);
                int? oldFullness = HttpContext.Session.GetInt32("Fullness");
                int? newFullness = fullnessBonus + oldFullness;
                HttpContext.Session.SetInt32("Fullness", (int)newFullness);
                if (newFullness >= 100 && happiness >= 100)
                {
                    HttpContext.Session.SetString("Message", "Congratulations! You Won!");
                    HttpContext.Session.SetString("Action", "game over");
                }
                else {
                    HttpContext.Session.SetString("Message", $"Your Dojodachi ate and gained +{fullnessBonus} more Fullness!");
                }
            }
            else{
                    HttpContext.Session.SetString("Message", "Your Dojodachi didn't like that food!");
            }

        }
        else{
            HttpContext.Session.SetString("Message", $"You don't have any more meals! Work for more meals!");
        }
        
        return RedirectToAction("DojoDachi");
    }

    // DONE
    // Playing with your Dojodachi costs 5 energy and gains a random amount of happiness between 5 and 10
    // Every time you play with or feed your dojodachi there should be a 25% chance that it won't like it. Energy or meals will still decrease, but happiness and fullness won't change.
     [HttpGet("/dojodachi/play")]
    public IActionResult Play()
    {
        HttpContext.Session.SetString("Action", "alive");
        Random rand = new Random();
        int? energy = HttpContext.Session.GetInt32("Energy");
        int? fullness = HttpContext.Session.GetInt32("Fullness");
        

        if(energy >= 5)
        {
            energy -=5;
            int noLikey = rand.Next(4);
            HttpContext.Session.SetInt32("Energy", (int)energy);

            if(noLikey <3)
            {
                int happinessGain = rand.Next(5,11);
                int? oldHappiness = HttpContext.Session.GetInt32("Happiness");
                int? newHappiness = oldHappiness + happinessGain;
                HttpContext.Session.SetInt32("Happiness",(int)newHappiness);
                if (fullness >= 100 && newHappiness >= 100)
                {
                    HttpContext.Session.SetString("Message", "Congratulations! You Won!");
                    HttpContext.Session.SetString("Action", "game over");
                }
                else{
                    HttpContext.Session.SetString("Message", $"You played with your Dojodachi and gained +{happinessGain} happiness!");
                }

            }
            else{
                HttpContext.Session.SetString("Message", "Your Dojodachi didn't want to play that game!");
            }
        }
        else{
            HttpContext.Session.SetString("Message", "Your Dojodachi doesn't have enough energy to play! They need to sleep");
        }
        return RedirectToAction("DojoDachi");
    }

    
    // Working costs 5 energy and earns between 1 and 3 meals   
     [HttpGet("/dojodachi/work")]
    public IActionResult Work()
    {
        Random rand = new Random();
        int? energy = HttpContext.Session.GetInt32("Energy");
        if(energy >= 5)
        {
            int mealGain = rand.Next(1,4);
            energy -=5;
            HttpContext.Session.SetInt32("Energy", (int)energy);
            int? meals = HttpContext.Session.GetInt32("Meals");
            HttpContext.Session.SetInt32("Meals", (int)meals + mealGain);
            HttpContext.Session.SetString("Message", $"Your Dojodachi does some work and gained +{mealGain} meals!");
        }
        else{
            HttpContext.Session.SetString("Message", $"Your Dojodachi is too tired to work and needs to sleep!");
        }
        HttpContext.Session.SetString("Action", "alive");
        return RedirectToAction("DojoDachi");
    }

    // Sleeping earns 15 energy and decreases fullness and happiness each by 5
     [HttpGet("/dojodachi/sleep")]
    public IActionResult Sleep()
    {
        HttpContext.Session.SetString("Action", "alive");
        int? energy = HttpContext.Session.GetInt32("Energy");
        int? fullness = HttpContext.Session.GetInt32("Fullness");
        int? happiness = HttpContext.Session.GetInt32("Happiness");
        fullness -= 5;
        happiness -=5;
        HttpContext.Session.SetInt32("Fullness", (int)fullness);
        HttpContext.Session.SetInt32("Happiness", (int)happiness);

        if(fullness > 0 && happiness > 0)
        {
            energy += 15;
            HttpContext.Session.SetInt32("Energy", (int)energy);
            HttpContext.Session.SetString("Message", "Your Dojodachi sleeps, gaining +15 energy but losing -5 fullness and happiness");
            return RedirectToAction("DojoDachi");
        }

        // check to see if a death condition is met
        if(fullness <= 0 && happiness <= 0)
        {
            HttpContext.Session.SetString("Message", "You're a bad trainer! Your Dojodachi died of hunger AND sadness!");
        }
        else if (fullness <= 0)
        {
            HttpContext.Session.SetString("Message", "Your Dojodachi died of hunger!");
        }
        else if (happiness <= 0)
        {
            HttpContext.Session.SetString("Message", "Your Dojodachi died of sadness!");
        }
        HttpContext.Session.SetString("Action", "game over");
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
