// Challenge 1
// bool amProgrammer = true; //not used in program
// int Age = 27; //not used in program
List<string> Names = new List<string>();
Names.Add("Monica");
Dictionary<string, string> MyDictionary = new Dictionary<string, string>();
MyDictionary.Add("Hello", "0");
MyDictionary.Add("Hi there", "0");
// This is a tricky one! Hint: look up what a char is in C#
// string MyName = "MyName"; //not used in program
// Challenge 2
List<int> Numbers = new List<int>() {2,3,6,7,1,5};
for(int i = Numbers.Count-1; i >= 0; i--)
{
    Console.WriteLine(Numbers[i]);
}
// Challenge 3
Console.WriteLine("Challenge 3:");
List<int> MoreNumbers = new List<int>() {12,7,10,-3,9};
foreach(int i in MoreNumbers)
{
    Console.WriteLine(i);
}
// Challenge 4
Console.WriteLine("Challenge 4:");
// values in a list can't be changed so it needed to be an array
int[] EvenMoreNumbers = {3,6,9,12,14};
int idx = 0;
foreach(int num in EvenMoreNumbers)
{
    if(num % 3 == 0)
    {
        EvenMoreNumbers[idx] = 0;
    }
    idx++;
}

// Challenge 5
// What can we learn from this error message?
Console.WriteLine("Challenge 5:");
string MyString = "superduberawesome";
static string UpdateString(int i, char value, string oldString)
{
    char[] chars = oldString.ToCharArray();
    chars[i] = value;
    return string.Join("", chars);
}
MyString = UpdateString(7, 'p' ,MyString);
Console.WriteLine(MyString);
// Challenge 6
// Hint: some bugs don't come with error messages
Console.WriteLine("Challenge 6:");
Random rand = new Random();
int randomNum = rand.Next(13);
if(randomNum == 12)
{
    Console.WriteLine("Hello");
}
else
{
    Console.WriteLine(randomNum);
}

