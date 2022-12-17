// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text;

string[] allLines = File.ReadAllLines(@"c:\temp\input.txt");

long rocksLimit = 1000_000_000_000;
Queue<Line> space = new Queue<Line>();

IList<Direction> winds = allLines.Single().Select(x => x == '<' ? Direction.Left : Direction.Right).ToArray();
IList<Shape> shapes = Shape.GetShapes();

int moveCounter = 0;
int shapesCount = shapes.Count;

int activeAreaIndex = 0;
int activeAreaHeight = 0;

int removedLines = 0;

PlaceBottom();

Stopwatch sw = Stopwatch.StartNew();

for (long rocksCounter = 0; rocksCounter < rocksLimit; rocksCounter++)
{
    int rockIndex = (int)(rocksCounter % shapesCount);

    PlaceNewShape(rockIndex);

    if (rocksCounter % 1_000_000 == 0)
    {
        Console.WriteLine(sw.Elapsed.TotalSeconds + " Counter: " + rocksCounter + " Size: " + space.Count);
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

    CleanEmptyLines();
}

int currentHeight = space.Count;
Console.WriteLine(currentHeight - 1 + removedLines);

Line[] GetActiveArea()
{
    return space.ToArray().Skip(activeAreaIndex).Take(activeAreaHeight).ToArray();
}

void CleanEmptyLines()
{
    space = new Queue<Line>(space.Where(l => !l.IsEmpty));
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

        for (int i = 0; i < linesToRemove; i++)
        {
            _ = space.Dequeue();

            removedLines++;
        }
    }
}

void HardenFallingObject(Line[] activeArea)
{
    foreach (var line in activeArea)
    {
        line.Rock = (byte)(line.Rock | line.Falling);
        line.Falling = 0;
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

    space.Enqueue(new Line());
    space.Enqueue(new Line());
    space.Enqueue(new Line());

    foreach (var shapeLine in shape.Rock)
    {
        space.Enqueue(new Line { Falling = shapeLine });
    }

    activeAreaIndex = space.Count - shape.Rock.Length - 1; // we need to see one line in front of us => - 1
    activeAreaHeight = shape.Rock.Length + 1; // we need to see one line in front of us => + 1
}

void PlaceBottom()
{
    Line bottom = new Line { Rock = 127, Falling = 0 };
    space.Enqueue(bottom);
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