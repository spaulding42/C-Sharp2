public class Wizard : Human
{

    public Wizard(string name) : base(name)
    {
        Intelligence = 25;
        Health = 30;
    }
    public override int Attack(Human target)
    {
        int dmg = Intelligence *3;
        Health += dmg;
        target.Health -= dmg;
        Console.WriteLine($"{Name} attacked {target.Name} for {dmg} damage!");
        return target.Health;
    }

    public int Heal()
    {
        int healAmt = 3*Intelligence;
        Health += healAmt;
        Console.WriteLine($"{Name} healed for {healAmt} HP!");
        return healAmt;
    }
}