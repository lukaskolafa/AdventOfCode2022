﻿// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text;

string[] allLines = File.ReadAllLines(@"c:\temp\input.txt");

long rocksLimit = 2022;

Line[] space = new Line[300];

IList<Direction> winds = allLines.Single().Select(x => x == '<' ? Direction.Left : Direction.Right).ToArray();
IList<Shape> shapes = Shape.GetShapes();

int moveCounter = 0;
int shapesCount = shapes.Count;

int activeAreaIndex = 0;
int activeAreaHeight = 0;

int removedLines = 0;

int currentHeight = 0;

InitLines();
PlaceBottom();

Stopwatch sw = Stopwatch.StartNew();

for (long rocksCounter = 0; rocksCounter < rocksLimit; rocksCounter++)
{
    int rockIndex = (int)(rocksCounter % shapesCount);

    PlaceNewShape(rockIndex);

    if (rocksCounter % 1_000_000 == 0)
    {
        Console.WriteLine(sw.Elapsed.TotalSeconds + " Counter: " + rocksCounter);
    }

    while (true)
    {
        Direction windDirection = winds[moveCounter % winds.Count];

        Line[] activeArea = GetActiveArea();

        if (PossibleToMove(windDirection, activeArea))
        {
            MoveFallingObject(windDirection, activeArea);
        }

        moveCounter++;

        if (PossibleToFall(activeArea))
        {
            FallFallingObject(activeArea);
        }
        else
        {
            HardenFallingObject(activeArea);

            OptimizeHeight(activeArea);

            break;
        }
    }
}

Console.WriteLine(currentHeight - 1 + removedLines);

Line[] GetActiveArea()
{
    return space.Skip(activeAreaIndex).Take(activeAreaHeight).ToArray();
}

void OptimizeHeight(Line[] activeArea)
{
    Line tester = new Line();

    foreach (var line in activeArea)
    {
        tester.Rock = (byte)(tester.Rock | line.Rock);
    }

    if (tester.IsAllFinal)
    {
        var linesToRemove = activeAreaIndex;

        removedLines += linesToRemove;
        currentHeight -= linesToRemove;
        activeAreaIndex -= linesToRemove;

        // Reuse same instances, roll over = speed optimize
        var reusal = space.Take(linesToRemove).ToArray();

        foreach (var line in reusal)
        {
            line.Rock = 0;
        }

        space = space.Skip(linesToRemove).Concat(space.Take(linesToRemove)).ToArray();
    }
}

void HardenFallingObject(Line[] activeArea)
{
    foreach (var line in activeArea)
    {
        line.Rock = (byte)(line.Rock | line.Falling);
        line.Falling = 0;
    }

    while (true)
    {
        if (space[currentHeight].Rock > 0)
        {
            currentHeight++;
        }
        else
        {
            break;
        }

    }
}

void MoveFallingObject(Direction direction, Line[] activeArea)
{
    if (direction == Direction.Right)
    {
        foreach (Line movingLine in activeArea)
        {
            movingLine.Falling = (byte)(movingLine.Falling >> 1);
        }
    }
    else
    {
        foreach (Line movingLine in activeArea)
        {
            movingLine.Falling = (byte)(movingLine.Falling << 1);
        }
    }
}

void FallFallingObject(Line[] activeArea)
{
    for (int y = 0; y < activeArea.Length - 1; y++)
    {
        activeArea[y].Falling = activeArea[y + 1].Falling;
        activeArea[y + 1].Falling = 0;
    }

    activeAreaIndex -= 1;
}

bool PossibleToFall(Line[] activeArea)
{
    for (int y = 1; y < activeArea.Length; y++)
    {
        if ((activeArea[y].Falling & activeArea[y - 1].Rock) > 0)
        {
            return false;
        }
    }

    return true;
}

bool PossibleToMove(Direction direction, Line[] activeArea)
{
    if (direction == Direction.Right)
    {
        foreach (Line movingLine in activeArea)
        {
            if ((movingLine.Falling & 1) > 0)
            {
                return false;
            }

            if (((movingLine.Falling >> 1) & movingLine.Rock) > 0)
            {
                return false;
            }
        }
    }
    else
    {
        foreach (Line movingLine in activeArea)
        {
            if ((movingLine.Falling & 64) > 0)
            {
                return false;
            }

            if (((movingLine.Falling << 1) & movingLine.Rock) > 0)
            {
                return false;
            }
        }
    }

    return true;
}

void PlaceNewShape(int shapeIndex)
{
    var shape = shapes[shapeIndex];

    byte aboveTopPlacement = 3;
    foreach (var shapeLine in shape.Rock)
    {
        space[currentHeight + aboveTopPlacement].Falling = shapeLine;
        aboveTopPlacement++;
    }

    activeAreaIndex = currentHeight + 2; // we need to see one line in front of us, so take 3 - 1
    activeAreaHeight = shape.Rock.Length + 1; // we need to see one line in front of us => + 1
}

void InitLines()
{
    for (int i = 0; i < space.Length; i++)
    {
        space[i] = new Line();
    }
}

void PlaceBottom()
{
    space[currentHeight].Rock = 127;
    space[currentHeight].Falling = 0;

    currentHeight = 1;
}

string PrintLine(Line line)
{
    StringBuilder sb = new StringBuilder();

    for (int x = 64; x > 0; x >>= 1)
    {
        if ((line.Falling & x) > 0)
        {
            sb.Append("@");
        }
        else if ((line.Rock & x) > 0)
        {
            sb.Append("#");
        }
        else
        {
            sb.Append(".");
        }
    }

    return sb.ToString();
}

string PrintSpace()
{
    StringBuilder result = new StringBuilder();

    foreach (Line lineToWrite in space.Reverse())
    {
        result.AppendLine(PrintLine(lineToWrite));
    }

    result.AppendLine();
    result.AppendLine("=========================");
    result.AppendLine();

    var activeArea = GetActiveArea();

    foreach (var line in activeArea.Reverse())
    {
        result.AppendLine(PrintLine(line));
    }

    return result.ToString();
}

void PrintOutput()
{
    string output = PrintSpace();

    Console.Clear();
    Console.WriteLine(output);
    File.WriteAllText("c:\\temp\\output.txt", output);
}