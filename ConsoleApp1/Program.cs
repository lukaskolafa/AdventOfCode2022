// See https://aka.ms/new-console-template for more information

var allLines = File.ReadAllLines(@"c:\temp\input.txt");

var alph = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

var sum = 0;

for (int i = 0; i < allLines.Length / 3; i++)
{
    var f = allLines[i * 3];
    var s = allLines[i * 3 + 1];
    var t = allLines[i * 3 + 2];
    
    char[] interSect = f.Intersect(s).Intersect(t).ToArray();

    sum += getIndex(interSect.First());
    
    Console.WriteLine(f + " " + s + " " + t + " " + interSect.Length + " " + interSect.First());
}

Console.WriteLine(sum);

int getIndex(char c)
{
    return alph.IndexOf(c) + 1;
}