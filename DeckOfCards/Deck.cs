public class Deck
{
    public List<Card> cards {get; set;} = new List<Card>();

    public Deck()
    {
        Reset();
        Console.WriteLine($"Deck initialization complete. {cards.Count} cards in total");
    }
    public void Reset()
    {
        // clear anything currently in deck
        cards = new List<Card>();

        // add all 52 cards to the deck
        Card card = new Card("Spades",1); //assigning a temp card to get the card object created in the correct scope

        // Spades
        for(int i = 1; i <= 13; i++)
        {
            card = new Card("Spades", i);
            cards.Add(card);
        }
        // Hearts
        for(int i = 1; i <= 13; i++)
        {
            card = new Card("Hearts", i);
            cards.Add(card);
        }
        // Diamonds
        for(int i = 1; i <= 13; i++)
        {
            card = new Card("Diamonds", i);
            cards.Add(card);
        }
        // Clubs
        for(int i = 1; i <= 13; i++)
        {
            card = new Card("Clubs", i);
            cards.Add(card);
        }
    }

    public Card Deal()
    {
        if(cards.Count >0)
        {
            Card topCard = cards[cards.Count-1];
            cards.RemoveAt(cards.Count-1);
            return topCard;
        }
        else
        {
            Card emptyDeck = new Card("Empty",0);
            return emptyDeck;
        }
    }

    public void Shuffle()
    {
        List<Card> shuffledDeck = new List<Card>();
        Random rand = new Random();
        int next = 0;
        while(cards.Count > 0)
        {
            next = rand.Next(cards.Count);
            shuffledDeck.Add(cards[next]);
            cards.RemoveAt(next);
        }
        cards = shuffledDeck;
    }

    public void PrintDeck()
    {
        for(int i = 0; i < cards.Count; i++)
        {
            cards[i].Print();
        }
    }
}