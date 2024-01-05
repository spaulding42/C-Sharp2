using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Vanlife.Models;

namespace Vanlife.Controllers;



public class CampingController : Controller
{
    [HttpGet("/camping")]
    public IActionResult Camping()
    {
        return View("CampingMain");
    }
    [HttpGet("/camping/{state}")]
    public IActionResult CampingState(string state)
    {
        List<StatePark> jsonParks = new List<StatePark>();

        using (StreamReader r = new StreamReader($"wwwroot/StateParks/{state}Final.json"))
        {
            string json = r.ReadToEnd();
            jsonParks = JsonSerializer.Deserialize<List<StatePark>>(json);
        }
        if (jsonParks != null && jsonParks.Count > 0){
            foreach(var park in jsonParks){
                if(park.name.Contains("State Park")){
                    Console.WriteLine(park.name);
                    Console.WriteLine(park.geometry.location.lat);
                    Console.WriteLine(park.geometry.location.lng);
                }
            }
        }
        return View("CampingState", jsonParks);
    }
}

