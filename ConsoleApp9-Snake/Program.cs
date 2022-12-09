// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var allLines = File.ReadAllLines(@"c:\temp\input.txt");

bool[,] tailPresence = new bool[1000,1000];

int startX = 500;
int startY = 500;

int[,] snake = new int[2, 10];

for (int i = 0; i < 10; i++)
{
    snake[0, i] = 500;
    snake[1, i] = 500;
}

int cnt = 0;

Regex movementRegex = new Regex(@"(\w)\ (\d*)");

foreach (string line in allLines)
{
    MarkTailPosition();

    var commandMatch = movementRegex.Match(line);

    Console.WriteLine(line);

    string dir = commandMatch.Groups[1].Value;
    int stepsCount = int.Parse(commandMatch.Groups[2].Value);

    for (int i = 0; i < stepsCount; i++)
    {
        MoveHead(dir);
        MoveBody();
        MarkTailPosition();
    }
}

Console.WriteLine(cnt);

void PrintPath()
{
    return;

    for (int y = startY - 8; y < startY + 8; y++)
    {
        for (int x = startX - 8; x < startX + 8; x++)
        {
            string symbol = ".";

            if (startX == x && startY == y)
            {
                symbol = "s";
            }

            for (int j = 0; j < 10; j++)
            {
                if (x == snake[0, j] && y == snake[1, j])
                {
                    symbol = j.ToString();
                    break;
                }
            }

            Console.Write(symbol);
        }

        Console.WriteLine();
    }

    Console.WriteLine();
}

void MarkTailPosition()
{
    var val = tailPresence[snake[0, 9], snake[1, 9]];

    if (val)
    {
        return;
    }

    tailPresence[snake[0, 9], snake[1, 9]] = true;

    cnt++;
}

void MoveBody()
{
    for (int i = 0; i <= 8; i++)
    {
        bool m = MoveElement(i, i + 1);

        if (m) PrintPath();
    }
}

bool MoveElement(int leaderIndex, int moverIndex)
{
    // snake[0, moverIndex]  // tailX
    // snake[1, moverIndex]  // tailY
    // snake[0, leaderIndex]  // headX
    // snake[1, leaderIndex]  // headY

    if ((Math.Abs(snake[0, moverIndex] - snake[0, leaderIndex]) <= 1) && (Math.Abs(snake[1, moverIndex] - snake[1, leaderIndex]) <= 1))
    {
        return false; // no need to move
    }

    var difX = snake[0, moverIndex] - snake[0, leaderIndex];
    var difY = snake[1, moverIndex] - snake[1, leaderIndex];

    if ((Math.Abs(difX) == 2) && (Math.Abs(difY) == 2))
    {
        snake[0, moverIndex] -= difX / 2;
        snake[1, moverIndex] -= difY / 2;

        return true;
    }

    if (Math.Abs(difX) == 2)
    {
        snake[0, moverIndex] -= difX / 2;
        snake[1, moverIndex] = snake[1, leaderIndex];

        return true;
    }

    if (Math.Abs(difY) == 2)
    {
        snake[1, moverIndex] -= difY / 2;
        snake[0, moverIndex] = snake[0, leaderIndex];

        return true;
    }

    throw new Exception();
}

void MoveHead(string dir)
{
    switch (dir)
    {
        case "U":
            snake[1, 0]--;
            break;
        case "D":
            snake[1, 0]++;
            break;
        case "L":
            snake[0, 0]--;
            break;
        case "R":
            snake[0, 0]++;
            break;
        default: throw new Exception();
    }
}