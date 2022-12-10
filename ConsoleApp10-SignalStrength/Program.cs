// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var allLines = File.ReadAllLines(@"c:\temp\input.txt");

int cycle = 0;

int register = 1;

int result = 0;

bool[] screen = new bool [241];

Regex cmdRegex = new Regex(@"addx\ (-?\d+)");

foreach (string cmd in allLines)
{
   if (cmd == "noop")
    {
        handleCycle();
        handleSprite();

        cycle++;

        continue;
    }

    if (cmdRegex.IsMatch(cmd))
    {
        Match match = cmdRegex.Match(cmd);

        handleCycle();
        handleSprite();

        cycle++;

        handleCycle();
        handleSprite();

        cycle++;

        register += int.Parse(match.Groups[1].Value);

        continue;
    }

    throw new Exception();
}

// 12340 too low

Console.WriteLine(result);

PrintScreen();

void PrintScreen()
{
    for (int i = 0; i < 240; i++)
    {
        if (i % 40 == 0)
        {
            Console.WriteLine();
        }

        Console.Write(screen[i] ? "#" : ".");
    }
}

void handleSprite()
{
    var drawnIndex = cycle % 40;

    if (drawnIndex >= register - 1 && drawnIndex <= register + 1)
    {
        screen[cycle] = true;
    }
}

void handleCycle()
{
    if (cycle % 40 == 20)
    {
        var strength = cycle * register;

        result += strength;
    }
}