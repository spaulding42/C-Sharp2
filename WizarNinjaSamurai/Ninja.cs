public class Ninja : Human
{

    public Ninja(string name) : base(name)
    {
        Dexterity = 75;
    }
    public override int Attack(Human target)
    {
        Random rand = new Random();
        int critChance = rand.Next(5);
        int critBonus = 0;
        if(critChance > 3)
        {
            Console.WriteLine("Critical Hit!");
            critBonus = 10;
        } 

        int dmg = Dexterity + critBonus;
        target.Health -= dmg;
        Console.WriteLine($"{Name} attacked {target.Name} for {dmg} damage!");
        return target.Health;
    }

    public int Steal(Human target)
    {
        int dmg = 5;
        target.Health -= dmg;
        Health += dmg;
        return dmg;
    }
}