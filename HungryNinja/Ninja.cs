class Ninja
{
    private int calorieIntake;
    public List<Food> FoodHistory;
    
    // add a constructor
    public Ninja()
    {
        calorieIntake = 0;
        FoodHistory = new List<Food>();
    }
    // add a public "getter" property called "IsFull"
    public bool IsFull()
    {
        return calorieIntake > 1200;
    }
    // build out the Eat method
    public void Eat(Food item)
    {
        if(IsFull())
        {
            Console.WriteLine("Ninja is Full");
        }
        else
        {
            FoodHistory.Add(item);
            calorieIntake += item.Calories;
            string spice = item.IsSpicy? "":"not";
            Console.WriteLine($"Ninja eats {item.Name} --------\nit was {spice} spicy");
            string sweetness = item.IsSweet? "":"not";
            Console.WriteLine($"and it was {sweetness} sweet");
        }
    }
}

