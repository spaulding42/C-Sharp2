
// Random Array
// Create a function called RandomArray() that returns an integer array

// Create an empty array that will hold 10 integer values.
// Loop through the array and assign each index a random integer value between 5 and 25.
// Print the min and max values of the array
// Print the sum of all the values

static int[] RandomArray(int size = 10)
{
    int[] randArr = new int[size];

    Random rand = new Random();
    int newNumber = 0;
    for (int i = 0; i < size; i++)
    {
        newNumber = rand.Next(5,25);
        randArr[i] = newNumber;
    }
    return randArr;
}

int[] newRandArr = RandomArray();
int min = 25;
int max = 0;
int sum = 0;
for(int i = 0; i < newRandArr.Length; i++)
{
    if(newRandArr[i] > max) max = newRandArr[i];
    if(newRandArr[i] < min) min = newRandArr[i];
    Console.WriteLine(newRandArr[i]);
    sum += newRandArr[i];
}
Console.WriteLine($"Min: {min}");
Console.WriteLine($"Max: {max}");
Console.WriteLine($"Sum: {sum}");


// Coin Flip -------------------------------------------------------------------------------

// Create a function called TossCoin() that returns a string

// Have the function print "Tossing a Coin!"
// Randomize a coin toss with a result signaling either side of the coin
// Have the function print either "Heads" or "Tails"
// Finally, return the result

static string TossCoin()
{
    Console.WriteLine("Tossing a Coin!");
    Random rand = new Random();
    int result = rand.Next(2);
    if (result == 0)
    {
        Console.WriteLine("Heads");
        return "Heads";  
    } 
    if (result == 1) 
    {
        Console.WriteLine("Tails");
        return "Tails";
    }
    Console.WriteLine("Error");
    return "Error";
}

TossCoin();

static double TossMultipleCoins(int num)
{
    int heads = 0;
    int tails = 0;
    string result = "";
    for(int i = 0; i < num; i++)
    {
        result = TossCoin();
        if (result == "Heads")
        {
            heads++;
        }
        else
        {
            tails++;
        }
    }
    double ratio = 0.0;
    ratio = (double)heads/num;
    return ratio;
}

Console.WriteLine("multi toss");
double finalResulut = TossMultipleCoins(5);
Console.WriteLine($"Final result is: {finalResulut*100}% Heads");

static List<string> Names()
{
    List<string> names = new List<string>() {"Todd", "Tiffany", "Charlie", "Geneva","Sydney"};
    // shuffling the list
    List<string> shuffledNames = new List<string>();
    Random rand = new Random();
    int next = 0;
    while(names.Count>0)
    {
        next = rand.Next(names.Count);
        shuffledNames.Add(names[next]);
        names.RemoveAt(next);
    }

    List<string> longNames = new List<string>();
    for(int i = 0; i < shuffledNames.Count; i++){
        if(shuffledNames[i].Length>5)
        {
            longNames.Add(shuffledNames[i]);
        }
    }
    return longNames;
}

List<string> shortNames = Names();
for(int i = 0; i < shortNames.Count; i++)
{
    Console.WriteLine(shortNames[i]);
}

