// See https://aka.ms/new-console-template for more information

var allLines = File.ReadAllLines(@"c:\temp\input.txt");

int count = 0;

foreach (var line in allLines)
{
    var pairs = line.Split(',');

    var pair1 = pairs[0].Split('-');
    var pair2 = pairs[1].Split('-');

    var arr1 = new List<int>();
    for (int i = int.Parse(pair1[0]); i <= int.Parse(pair1[1]); i++)
    {
        arr1.Add(i);
    }
    
    var arr2 = new List<int>();
    for (int i = int.Parse(pair2[0]); i <= int.Parse(pair2[1]); i++)
    {
        arr2.Add(i);
    }

    bool overlap = arr1.Intersect(arr2).Any();

    if (overlap) count++;
    
    Console.WriteLine($"{line} {pair1[0]} {pair1[1]} {pair2[0]} {pair2[1]} {overlap}");
}

Console.WriteLine(count);