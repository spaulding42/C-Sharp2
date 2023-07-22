Card card1 = new Card("Spades",11);
Deck deck1 = new Deck();
Player Devin = new Player("Devin");

deck1.Shuffle();
// deck1.PrintDeck();
Devin.Draw(deck1);


Devin.PrintHand();

Devin.Discard(2);
Devin.PrintHand();
