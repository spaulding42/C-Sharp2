public class Card
{
    public string? Name;
    public string Suit;
    public int Val;
    public Card(string suit, int val)
    {
        // infer the name based on the value
        if(val == 0) Name = "MT";
        if(val == 1) Name = "Ace";
        if(val>1 && val <11) Name = val.ToString();
        if(val == 11) Name = "Jack";
        if(val == 12) Name = "Queen";
        if(val == 13) Name = "King";

        Suit = suit;
        Val = val;
    }

    public void Print()
    {
        Console.WriteLine($"Name: {Name}\nSuit: {Suit}\nValue: {Val}");
    }
}