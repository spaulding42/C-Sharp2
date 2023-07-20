// See https://aka.ms/new-console-template for more information

for(int i = 1; i <=255; i++)
{
    Console.WriteLine(i);
}

int i2 = 1;

while(i2<=100)
{
    if(i2%3 == 0 && i2%5 != 0)
    {
        Console.WriteLine("Fizz");
    }
    if(i2%3 !=0 && i2%5 ==0)
    {
        Console.WriteLine("Buzz");
    }
    i2++;

}