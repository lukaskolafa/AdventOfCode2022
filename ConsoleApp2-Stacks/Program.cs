// See https://aka.ms/new-console-template for more information

/*

[T]     [Q]             [S]        
[R]     [M]             [L] [V] [G]
[D] [V] [V]             [Q] [N] [C]
[H] [T] [S] [C]         [V] [D] [Z]
[Q] [J] [D] [M]     [Z] [C] [M] [F]
[N] [B] [H] [N] [B] [W] [N] [J] [M]
[P] [G] [R] [Z] [Z] [C] [Z] [G] [P]
[B] [W] [N] [P] [D] [V] [G] [L] [T]
 1   2   3   4   5   6   7   8   9 

 */


using System.Text.RegularExpressions;

Stack<string>[] data = new Stack<string>[10];

data[0] = new Stack<string>();
data[1] = new Stack<string>(new [] { "B", "P", "N", "Q", "H", "D", "R", "T" });
data[2] = new Stack<string>(new [] { "W", "G", "B", "J", "T", "V" });
data[3] = new Stack<string>(new [] { "N", "R", "H", "D", "S", "V", "M", "Q" });
data[4] = new Stack<string>(new [] { "P", "Z", "N", "M", "C" });
data[5] = new Stack<string>(new [] { "D", "Z", "B" });
data[6] = new Stack<string>(new [] { "V", "C", "W", "Z" });
data[7] = new Stack<string>(new [] { "G", "Z", "N", "C", "V", "Q", "L", "S" });
data[8] = new Stack<string>(new [] { "L", "G", "J", "M", "D", "N", "V" });
data[9] = new Stack<string>(new [] { "T", "P", "M", "F", "Z", "C", "G" });

var allLines = File.ReadAllLines(@"c:\temp\input.txt");

Regex regex = new Regex(@"move\ (\d*)\ from\ (\d*)\ to\ (\d*)");

foreach (var line in allLines)
{
 var match = regex.Match(line);

 var cnt = int.Parse(match.Groups[1].Value);
 var from = int.Parse(match.Groups[2].Value);
 var to = int.Parse(match.Groups[3].Value);

 Stack<string> temp = new Stack<string>();

 for (int i = 0; i < cnt; i++)
 {
  temp.Push(data[from].Pop());
 }

 for (int i = 0; i < cnt; i++)
 {
  data[to].Push(temp.Pop());
 }
}

for (int i = 1; i < data.Length; i++)
{
  Console.Write(data[i].Pop());
}