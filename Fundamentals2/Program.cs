// See https://aka.ms/new-console-template for more information
Console.WriteLine("Fundamentals 2");

Console.WriteLine("Part 1: ");
static void PrintNumbers()
{
    for(int i = 1; i <= 255; i++)
    {
        Console.WriteLine(i);
    }
}

// PrintNumbers();

Console.WriteLine("Part 2: ");
static void PrintOdds()
{
    for(int i = 1; i<=255; i = i +2)
    {
        Console.WriteLine(i);
    }
}

// PrintOdds();

Console.WriteLine("Part 3: ");
static void PrintSum()
{
    int sum = 0;
    for(int i = 0; i <= 255; i++)
    {
        sum = sum + i;
        Console.WriteLine($"New Number: {i} Sum: {sum}");
    }
}

// PrintSum();

Console.WriteLine("Part 4: ");
static void LoopArray(int[] numbers)
{
    for(int i = 0; i < numbers.Length; i++)
    {
        Console.WriteLine(numbers[i]);
    }
}
int[] intArr = {3,5,2,1,7,9,111};
// LoopArray(intArr);

Console.WriteLine("Part 5: ");

static int FindMax(int[] numbers)
{
    int max = 0;
    for(int i = 0; i < numbers.Length; i++)
    {
        if(numbers[i] > max)
        {
            max = numbers[i];
        }
    }
    return max;
}

// Console.WriteLine(FindMax(intArr));

Console.WriteLine("Part 6: ");

static void GetAverage(int[] numbers)
{
    int sum = 0;
    for(int i = 0; i < numbers.Length; i++)
    {
        sum += numbers[i];
    }
    Console.WriteLine(sum/numbers.Length);
}
// GetAverage(intArr);

Console.WriteLine("Part 7: ");

static List<int> OddList()
{
    List<int> odds = new List<int>();
    for(int i = 1; i<= 255; i += 2)
    {
        odds.Add(i);
    }
    return odds;
}
List<int> Odds = new List<int>();
Odds = OddList();
// for(int i = 0; i < Odds.Count; i++)
// {
//     Console.WriteLine(Odds[i]);
// }
Console.WriteLine("Part 8: ");

List<int> intList = new List<int>();
intList.Add(3);
intList.Add(6);
intList.Add(9);
intList.Add(1);
intList.Add(-2);
intList.Add(8);
static int GreaterThanY(List<int> numbers, int y)
{
    int sum = 0;
    for(int i = 0; i < numbers.Count; i++)
    {
        if(numbers[i]> y)
        {
            sum++;
        }
    }
    return sum;
}
// Console.WriteLine(GreaterThanY(intList, 5));

Console.WriteLine("Part 9: ");

static void SquareArrayValues(List<int> numbers)
{
    List<int> newNumbers = new List<int>();
    for(int i = 0; i < numbers.Count; i++)
    {
        newNumbers.Add(numbers[i]*numbers[i]);
        Console.WriteLine(newNumbers[i]);
    }
}
// SquareArrayValues(intList);

Console.WriteLine("Part 10: ");

static void EliminateNegatives(List<int> numbers)
{
    for(int i = 0; i < numbers.Count; i++)
    {
        if(numbers[i] < 0 )
        {
            numbers[i] = 0;
        }
        Console.WriteLine(numbers[i]);
    }
}

EliminateNegatives(intList);

