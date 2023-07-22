class Buffet
{
    public List<Food> Menu;
     
    //constructor
    public Buffet()
    {
        Menu = new List<Food>()
        {
            new Food("Tacos", 400, false, false),
            new Food("Fajitas", 500, true, false),
            new Food("Spaghetti", 600, false, false),
            new Food("Cake", 800, false, true),
            new Food("Grilled Pineapple", 400, false, true),
            new Food("Wings", 500, true, false),
            new Food("Steak", 700, false, false)
        };
    }
     
    public Food Serve()
    {
        Random rand = new Random();
        int foodIndex = rand.Next(Menu.Count);
        return Menu[foodIndex];
    }
}

