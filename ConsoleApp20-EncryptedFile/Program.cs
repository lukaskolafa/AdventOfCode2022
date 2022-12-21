// See https://aka.ms/new-console-template for more information

string[] allLines = File.ReadAllLines(@"c:\temp\input2.txt");

IDictionary<int, Value> valuesByOriginalPosition = new Dictionary<int, Value>();
IDictionary<int, Value> valuesByCurrentPosition = new Dictionary<int, Value>();


int zeroOriginalPosition = 0;

foreach (var line in allLines.Select((line, i) => new Value { Number = int.Parse(line), CurrentPosition = i, OriginalPosition = i }))
{
    valuesByCurrentPosition[line.CurrentPosition] = line;
    valuesByOriginalPosition[line.OriginalPosition] = line;

    if (line.Number == 0)
    {
        zeroOriginalPosition = line.OriginalPosition;
    }
}

// Console.WriteLine(string.Join(", ", valuesByCurrentPosition.Values.Select(x => x.Number)));

for (int i = 0; i < allLines.Length; i++)
{
    var valueToExchange = valuesByOriginalPosition[i];
    var startSwappingAtPosition = valueToExchange.CurrentPosition;
    var finishAtPosition = startSwappingAtPosition + valueToExchange.Number;
    if (valueToExchange.Number < 0 && finishAtPosition < 0)
    {
        finishAtPosition = (finishAtPosition + allLines.Length + allLines.Length - 1) % allLines.Length;
    }

    if (valueToExchange.Number > 0 && finishAtPosition > allLines.Length - 1)
    {
        finishAtPosition %= allLines.Length - 1;
    }

    var step = startSwappingAtPosition < finishAtPosition ? 1 : -1;
    var steps = Math.Abs(startSwappingAtPosition - finishAtPosition);

    // Console.WriteLine();
    // Console.WriteLine($"{valueToExchange.Number} moves by {steps} to the {(step < 0 ? "left" : "right")}");

    for (int counter = 0; counter < steps; counter++)
    {
        Swap(startSwappingAtPosition, startSwappingAtPosition + step);
        startSwappingAtPosition += step;
    }

    // Console.WriteLine(string.Join(", ", valuesByCurrentPosition.Values.Select(x => x.Number)));
}

var v1000 = valuesByCurrentPosition[(valuesByOriginalPosition[zeroOriginalPosition].CurrentPosition + 1000 - 1) % allLines.Length].Number;
var v2000 = valuesByCurrentPosition[(valuesByOriginalPosition[zeroOriginalPosition].CurrentPosition + 2000 - 1) % allLines.Length].Number;
var v3000 = valuesByCurrentPosition[(valuesByOriginalPosition[zeroOriginalPosition].CurrentPosition + 3000 - 1) % allLines.Length].Number;

Console.WriteLine(v1000);
Console.WriteLine(v2000);
Console.WriteLine(v3000);
Console.WriteLine(v1000 + v2000 + v3000);

void Swap(int currentA, int currentB)
{
    var a = valuesByCurrentPosition[currentA];
    var b = valuesByCurrentPosition[currentB];

    a.CurrentPosition = currentB;
    b.CurrentPosition = currentA;

    valuesByCurrentPosition[currentA] = b;
    valuesByCurrentPosition[currentB] = a;
}