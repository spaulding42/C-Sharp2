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
        List<Dish> AllDishs = db.Dishes.ToList();
        return View("Index", AllDishs);
    }
    // VIEW ONE 
    [HttpGet("dishes/{DishId}/View")]
    public IActionResult ViewOne(int DishId)
    {
        Dish? DishToEdit = db.Dishes.FirstOrDefault(i => i.DishId == DishId);
        // check here to ensure what you are grabbing will not return a null item
        if(DishToEdit == null){
            return View("Index");
        }
        return View("ViewOne", DishToEdit);
    }

    // CREATE ONE
    [HttpGet("/dishes/create")]
    public IActionResult Create()
    {
        return View("Create");
    }
    [HttpPost("dishes/submit")]
    public IActionResult SubmitDish(Dish Dish)
    {
        if(ModelState.IsValid){
            db.Dishes.Add(Dish);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View("Create");
    }

    // EDIT ONE
    [HttpGet("dishes/{DishId}/edit")]
    public IActionResult EditDish(int DishId)
    {
        Dish? DishToEdit = db.Dishes.FirstOrDefault(i => i.DishId == DishId);
        // check here to ensure what you are grabbing will not return a null item
        if(DishToEdit == null){
            return View("Index");
        }
        
        return View("Edit", DishToEdit);
    }

    // UPDATE ONE
    [HttpPost("/dishes/{DishId}/update")]
    public IActionResult UpdateDish(Dish dish)
    {
        if(ModelState.IsValid){
            dish.UpdatedAt = DateTime.Now;
            db.Dishes.Update(dish);
            db.SaveChanges();
            return RedirectToAction("Index");  
        }
        return View("Edit", dish);
    }

    // DELETE ONE
    [HttpPost("/dishes/{DishId}/destroy")]
    public IActionResult Destroy(int DishId)
    {
            Console.WriteLine("DELETING BEEP BEEP BOOP!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        Dish? DishToDelete = db.Dishes.FirstOrDefault(i => i.DishId == DishId);
        if(DishToDelete != null){
            db.Dishes.Remove(DishToDelete);
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
