public class Samurai : Human
{

    public Samurai(string name) : base(name)
    {
        Health = 200;
    }
    public override int Attack(Human target)
    {
        target.Health = base.Attack(target);
        if(target.Health < 50)
        {
            target.Health = 0;
            Console.WriteLine($"{Name} executes {target.Name} reducing their hp to 0!");
        }
        return target.Health;
    }

    public void Meditate()
    {
        Console.WriteLine($"{Name} meditates and fully heals!");
        Health = 200;
    }
    
}