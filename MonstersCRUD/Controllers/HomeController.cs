using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUDelicious.Models;

namespace CRUDelicious.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext db;

    public HomeController(ILogger<HomeController> logger , MyContext context)
    {
        _logger = logger;
        db = context;
    }

    public IActionResult Index()
    {
        List<Monster> AllMonsters = db.Monsters.ToList();
        return View("Index", AllMonsters);
    }
    // VIEW ONE 
    [HttpGet("monsters/{MonsterId}/View")]
    public IActionResult ViewOne(int MonsterId)
    {
        Monster? MonsterToEdit = db.Monsters.FirstOrDefault(i => i.MonsterId == MonsterId);
        // check here to ensure what you are grabbing will not return a null item
        if(MonsterToEdit == null){
            return View("Index");
        }
        return View("ViewOne", MonsterToEdit);
    }

    // CREATE ONE
    [HttpGet("/monsters/create")]
    public IActionResult Create()
    {
        return View("Create");
    }
    [HttpPost("monsters/submit")]
    public IActionResult SubmitMonster(Monster monster)
    {
        if(ModelState.IsValid){
            db.Monsters.Add(monster);
            db.SaveChanges();
            
        }
        return RedirectToAction("Index");
    }

    // EDIT ONE
    [HttpGet("monsters/{MonsterId}/edit")]
    public IActionResult EditMonster(int MonsterId)
    {
        Monster? MonsterToEdit = db.Monsters.FirstOrDefault(i => i.MonsterId == MonsterId);
        // check here to ensure what you are grabbing will not return a null item
        if(MonsterToEdit == null){
            return View("Index");
        }
        
        return View("Edit", MonsterToEdit);
    }

    // UPDATE ONE
    [HttpPost("/monsters/{MonsterId}/update")]
    public IActionResult UpdateMonster(Monster monster)
    {
        if(ModelState.IsValid){
            db.Monsters.Update(monster);
            db.SaveChanges();
            return RedirectToAction("Index");  
        }
        return View("Edit", monster);
    }

    // DELETE ONE
    [HttpGet("/monsters/{MonsterId}/delete")]
    public IActionResult Delete(int MonsterId)
    {
            Console.WriteLine("DELETING BEEP BEEP BOOP!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        Monster? MonsterToDelete = db.Monsters.FirstOrDefault(i => i.MonsterId == MonsterId);
        if(MonsterToDelete != null){
            db.Monsters.Remove(MonsterToDelete);
            db.SaveChanges();
        }
        return RedirectToAction("Index");
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
