// See https://aka.ms/new-console-template for more information

string[] allLines = File.ReadAllLines(@"c:\temp\input.txt");

IDictionary<int, Value> valuesByOriginalPosition = new Dictionary<int, Value>();

int zeroOriginalPosition = 0;

foreach ((string line, int i) in allLines.Select((line, i) => (line, i)))
{
    var value = new Value
    {
        Number = long.Parse(line) * 811589153,
        OriginalPosition = i
    };

    valuesByOriginalPosition[i] = value;

    if (value.Number == 0)
    {
        zeroOriginalPosition = i;
    }
}

for(int i = 0; i < allLines.Length; i++)
{
    if (i == allLines.Length - 1)
    {
        valuesByOriginalPosition[i].Next = valuesByOriginalPosition[0];
    }
    else
    {
        valuesByOriginalPosition[i].Next = valuesByOriginalPosition[i + 1];
    }

    if (i == 0)
    {
        valuesByOriginalPosition[i].Previous = valuesByOriginalPosition[allLines.Length - 1];
    }
    else
    {
        valuesByOriginalPosition[i].Previous = valuesByOriginalPosition[i - 1];
    }
}

PrintChain();

for (var cnt = 0; cnt < 10; cnt++)
{
    for (int i = 0; i < allLines.Length; i++)
    {
        var valueToExchange = valuesByOriginalPosition[i];
        var targetPlace = valueToExchange;

        long numberToMove = valueToExchange.Number;
        // we have to wrap by -1 because the steps to make a round is one less than the count of items
        long numberOfSteps = (numberToMove + allLines.Length - 1) % (long)(allLines.Length - 1);

        Console.WriteLine($"Moving number {numberToMove} by steps {numberOfSteps}. Result: ");

        if (numberOfSteps != 0)
        {
            if (numberOfSteps > 0)
            {
                for (long j = 0; j < numberOfSteps; j++)
                {
                    targetPlace = targetPlace.Next;

                    if (targetPlace == valueToExchange)
                    {
                        targetPlace = valueToExchange.Next;
                    }
                }
            }
            else
            {
                for (long j = numberOfSteps; j <= 0; j++)
                {
                    targetPlace = targetPlace.Previous;

                    if (targetPlace == valueToExchange)
                    {
                        targetPlace = valueToExchange.Previous;
                    }
                }
            }

            // we press the item in between here
            var targetChainLeftEnd = targetPlace;
            var targetChainRightEnd = targetPlace.Next;

            // we rip the item from here
            var sourceChainLeftEnd = valueToExchange.Previous;
            var sourceChainRightEnd = valueToExchange.Next;

            // seal the gap on the source
            sourceChainLeftEnd.Next = sourceChainRightEnd;
            sourceChainRightEnd.Previous = sourceChainLeftEnd;

            // insert the item on the target place
            targetChainLeftEnd.Next = valueToExchange;
            valueToExchange.Previous = targetChainLeftEnd;
            targetChainRightEnd.Previous = valueToExchange;
            valueToExchange.Next = targetChainRightEnd;
        }
    }

    PrintChain();
}

void PrintChain()
{
    Value current = valuesByOriginalPosition[0];

    for (int i = 0; i < allLines.Length; i++)
    {
        Console.Write($"{current.Number}, ");
        current = current.Next;
    }

    Console.WriteLine();
    Console.WriteLine();
}

var zeroItem = valuesByOriginalPosition[zeroOriginalPosition];
var v1000 = zeroItem;

for (int j = 0; j < 1000; j++)
{
    v1000 = v1000.Next;
}

var v2000 = v1000;

for (int j = 0; j < 1000; j++)
{
    v2000 = v2000.Next;
}

var v3000 = v2000;

for (int j = 0; j < 1000; j++)
{
    v3000 = v3000.Next;
}

Console.WriteLine(v1000.Number);
Console.WriteLine(v2000.Number);
Console.WriteLine(v3000.Number);
Console.WriteLine(v1000.Number + v2000.Number + v3000.Number);