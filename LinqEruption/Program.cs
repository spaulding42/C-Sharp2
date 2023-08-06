using LinqEruption;

List<Eruption> eruptions = new List<Eruption>()
{
    new Eruption("La Palma", 2021, "Canary Is", 2426, "Stratovolcano"),
    new Eruption("Villarrica", 1963, "Chile", 2847, "Stratovolcano"),
    new Eruption("Chaiten", 2008, "Chile", 1122, "Caldera"),
    new Eruption("Kilauea", 2018, "Hawaiian Is", 1122, "Shield Volcano"),
    new Eruption("Hekla", 1206, "Iceland", 1490, "Stratovolcano"),
    new Eruption("Taupo", 1910, "New Zealand", 760, "Caldera"),
    new Eruption("Lengai, Ol Doinyo", 1927, "Tanzania", 2962, "Stratovolcano"),
    new Eruption("Santorini", 46,"Greece", 367, "Shield Volcano"),
    new Eruption("Katla", 950, "Iceland", 1490, "Subglacial Volcano"),
    new Eruption("Aira", 766, "Japan", 1117, "Stratovolcano"),
    new Eruption("Ceboruco", 930, "Mexico", 2280, "Stratovolcano"),
    new Eruption("Etna", 1329, "Italy", 3320, "Stratovolcano"),
    new Eruption("Bardarbunga", 1477, "Iceland", 2000, "Stratovolcano")
};
// Example Query - Prints all Stratovolcano eruptions
IEnumerable<Eruption> stratovolcanoEruptions = eruptions.Where(c => c.Type == "Stratovolcano");
PrintEach(stratovolcanoEruptions, "Stratovolcano eruptions.");
// Execute Assignment Tasks here!
 
Console.WriteLine("Full list above ----------------");


Console.WriteLine("Part 1)");
//Use LINQ to find the first eruption that is in Chile and print the result.
Eruption? firstE = eruptions.FirstOrDefault(e => e.Location == "Chile");
if (firstE != null)
{
    Console.WriteLine(firstE.ToString());
}


Console.WriteLine("Part 2)");
//Find the first eruption from the "Hawaiian Is" location and print it. If none is found, print "No Hawaiian Is Eruption found."
string search = "Hawaiian Is";
firstE = eruptions.FirstOrDefault(e => e.Location == search);

if (firstE == null)
{
    Console.WriteLine($"No {search} found.");
}
else
{
    Console.WriteLine(firstE.ToString());
}

Console.WriteLine("Part 3)");
//Find the first eruption that is after the year 1900 AND in "New Zealand", then print it.
search = "year 1900 AND in New Zealand";
firstE = eruptions.FirstOrDefault(e => e.Year > 1900 && e.Location == "New Zealand");
if (firstE == null)
{
    Console.WriteLine($"No {search} found.");
}
else
{
    Console.WriteLine(firstE.ToString());
}

Console.WriteLine("Part 4)");
//Find all eruptions where the volcano's elevation is over 2000m and print them.
List<Eruption> eruptions1 = eruptions.Where(e => e.ElevationInMeters > 2000).ToList();
PrintEach(eruptions1);

Console.WriteLine("Part 5)");
// Find all eruptions where the volcano's name starts with "L" and print them. Also print the number of eruptions found.
eruptions1 = eruptions.Where(e => e.Volcano[0] == 'L').ToList();
PrintEach(eruptions1);

Console.WriteLine("Part 6)");
//Find the highest elevation, and print only that integer (Hint: Look up how to use LINQ to find the max!)
int highest = eruptions.Max(e => e.ElevationInMeters);
Console.WriteLine("Highest elevation: " + highest);

Console.WriteLine("Part 7)");
//Use the highest elevation variable to find a print the name of the Volcano with that elevation.
firstE = eruptions.FirstOrDefault(e => e.ElevationInMeters == highest);
if(firstE != null)
{
    Console.WriteLine($"Highest Volcano Name: {firstE.Volcano}");
}
else {
    Console.WriteLine("Not found");
}

Console.WriteLine("Part 8)");
//Print all Volcano names alphabetically.
eruptions1 = eruptions.OrderBy(e => e.Volcano).ToList();
PrintEach(eruptions1);

Console.WriteLine("Part 9)");
//Print all the eruptions that happened before the year 1000 CE alphabetically according to Volcano name.
eruptions1 = eruptions.Where(e => e.Year < 1000)
    .OrderBy(e=>e.Volcano).ToList();
PrintEach(eruptions1);

Console.WriteLine("Bonus)");
//BONUS: Redo the last query, but this time use LINQ to only select the volcano's name so that only the names are printed.
List<string> eruptions2 = eruptions1.Select(e => e.Volcano).ToList();
PrintEach(eruptions2);

// Helper method to print each item in a List or IEnumerable.This should remain at the bottom of your class!
static void PrintEach(IEnumerable<dynamic> items, string msg = "")
{
    Console.WriteLine("\n" + msg);
    foreach (var item in items)
    {
        Console.WriteLine(item.ToString());
    }
}
