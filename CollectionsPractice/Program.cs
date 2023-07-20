int[] numbers = {0,1,2,3,4,5,6,7,8,9};
string[] names = {"Tim", "Martin", "Nikki", "Sarah"};
// In an effort to avoid typing true/false 10 times i wrote way more lines of code to do the same thing LOL
List<bool> TOrF = new List<bool>();
bool flag = true;
for(int i = 0; i <10; i++)
{
    TOrF.Add(flag);
    flag= !flag;
}
bool[] TOrFArr = TOrF.ToArray();

List<string> flavors = new List<string>() {"Chocolate", "Vanilla", "Rocky Road", "Cookies 'n Cream", "Butter Pecan", "Mint Chocolate Chip"};
Console.WriteLine(flavors.Count);
Console.WriteLine(flavors[2]);
flavors.Remove(flavors[2]);
Console.WriteLine(flavors.Count);


Dictionary<string, string> dict = new Dictionary<string, string>();
Random rand = new Random();
int newRand = (int)rand.Next(flavors.Count);
for(int i = 0; i < names.Length; i++)
{
    dict.Add(names[i], flavors[newRand]);
    newRand = (int)rand.Next(flavors.Count);
}
foreach(KeyValuePair<string, string> entry in dict)
{
    Console.WriteLine(entry.Key + " likes " + entry.Value);
}
