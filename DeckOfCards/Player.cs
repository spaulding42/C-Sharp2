public class Player
{
    public string Name {get; set;} = "";

    public List<Card> Hand = new List<Card>();

    public Player(string name)
    {
        Name = name;
    }

    public void Draw(Deck deck)
    {
        if (deck.cards.Count > 0)
        {
            Hand.Add(deck.Deal());
        }
        else
        {
            Console.WriteLine("The deck is empty!");
        }
    }

    public void PrintHand()
    {
        if(Hand.Count == 0)
        {
            Console.WriteLine("Hand is empty");
        }
        else
        {
            Console.WriteLine($"{Name}'s hand:");
        }
        foreach(Card card in Hand)
        {
            card.Print();
            Console.WriteLine("-");
        }
    }

    public Card Discard(int cardIdx)
    {
        if (cardIdx >=0 && cardIdx < Hand.Count)
        {
            Card discard = Hand[cardIdx];
            Hand.RemoveAt(cardIdx);
            Console.WriteLine($"{Name} discards {discard.Name} of {discard.Suit}");
            return discard;
        }
        Console.WriteLine($"{Name} tried to discard but had an Invalid Selection");
        return null;

    }

}