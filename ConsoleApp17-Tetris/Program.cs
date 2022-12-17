// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text;

string[] allLines = File.ReadAllLines(@"c:\temp\input1.txt");

int width = 7;
long rocksLimit = 1000000000000;
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

    if (rocksCounter % 10000 == 0)
    {
        Console.WriteLine(sw.Elapsed.TotalSeconds + " Counter: " + rocksCounter);
    }

    PlaceNewShape(rockIndex);

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
    Line tester = new Line(width);

    foreach (var line in activeArea)
    {
        for (int i = 0; i < line.Fields.Length; i++)
        {
            if (line.Fields[i] == Field.Final)
            {
                tester.Fields[i] = Field.Final;
            }
        }
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
        for (int x = 0; x < line.Fields.Length; x++)
        {
            if (line.Fields[x] == Field.Falling)
            {
                line.Fields[x] = Field.Final;
            }
        }
    }
}

void MoveFallingObject(Direction direction, Line[] activeArea)
{
    if (direction == Direction.Left)
    {
        foreach (var line in activeArea)
        {
            for (int x = 0; x < line.Fields.Length - 1; x++)
            {
                if (line.Fields[x + 1] == Field.Falling)
                {
                    // if (line.Fields[x] != Field.Empty)
                    // {
                    //     throw new InvalidOperationException("Moving to nonempty space, check your logic");
                    // }

                    line.Fields[x] = Field.Falling;
                    line.Fields[x + 1] = Field.Empty;
                }
            }
        }
    }
    else
    {
        foreach (var line in activeArea)
        {
            for (int x = width - 1; x > 0; x--)
            {
                if (line.Fields[x - 1] == Field.Falling)
                {
                    // if (line.Fields[x] != Field.Empty)
                    // {
                    //     throw new InvalidOperationException("Moving to nonempty space, check your logic");
                    // }

                    line.Fields[x] = Field.Falling;
                    line.Fields[x - 1] = Field.Empty;
                }
            }
        }
    }
}

void FallFallingObject(Line[] activeArea)
{
    for (int y = 0; y < activeArea.Length - 1; y++)
    {
        for (int x = 0; x < activeArea[y].Fields.Length; x++)
        {
            if (activeArea[y + 1].Fields[x] == Field.Falling)
            {
                // if (activeArea[y].Fields[x] != Field.Empty)
                // {
                //     throw new InvalidOperationException("Moving to nonempty space, check your logic");
                // }

                activeArea[y].Fields[x] = Field.Falling;
                activeArea[y + 1].Fields[x] = Field.Empty;
            }
        }
    }

    activeAreaIndex -= 1;
}

bool PossibleToFall(Line[] activeArea)
{
    for (int y = 1; y < activeArea.Length; y++)
    {
        for (int x = 0; x < activeArea[y].Fields.Length; x++)
        {
            if (activeArea[y].Fields[x] == Field.Falling)
            {
                if (activeArea[y - 1].Fields[x] == Field.Final)
                {
                    return false;
                }
            }
        }
    }

    return true;
}

bool PossibleToMove(Direction direction, Line[] activeArea)
{
    foreach (Line movingLine in activeArea)
    {
        for (int i = 0; i < width; i++)
        {
            if (movingLine.Fields[i] == Field.Falling)
            {
                if (direction == Direction.Left)
                {
                    if (i == 0)
                    {
                        // cannot move, too far left
                        return false;
                    }

                    if (movingLine.Fields[i - 1] == Field.Final)
                    {
                        // cannot move, rock
                        return false;
                    }
                }
                else
                {
                    if (i == width - 1)
                    {
                        // cannot move, too far right
                        return false;
                    }

                    if (movingLine.Fields[i + 1] == Field.Final)
                    {
                        // cannot move, rock
                        return false;
                    }
                }
            }
        }
    }

    return true;
}

void PlaceNewShape(int shapeIndex)
{
    var shape = shapes[shapeIndex];

    space.Enqueue(new Line(width));
    space.Enqueue(new Line(width));
    space.Enqueue(new Line(width));

    for (int y = 0; y < shape.Rock.Length; y++)
    {
        Line line = new Line(width);

        for (int x = 0; x < Shape.Width; x++)
        {
            // Shape Rock definition matrix is inverted and transposed due to human readable input!!
            line.Fields[x + 2] = shape.Rock[shape.Rock.Length - y - 1][x] ? Field.Falling : Field.Empty;
        }

        space.Enqueue(line);
    }

    activeAreaIndex = space.Count - shape.Rock.Length - 1; // we need to see one line in front of us => - 1
    activeAreaHeight = shape.Rock.Length + 1; // we need to see one line in front of us => + 1
}

void PlaceBottom()
{
    Line bottom = new Line(width);
    for(int i = 0; i < width; i++)
    {
        bottom.Fields[i] = Field.Final;
    }

    space.Enqueue(bottom);
}

string PrintLine(Line line)
{
    StringBuilder sb = new StringBuilder();

    for (int x = 0; x < width; x++)
    {
        switch (line.Fields[x])
        {
            case Field.Empty:
                sb.Append(".");
                break;
            case Field.Falling:
                sb.Append("@");
                break;
            case Field.Final:
                sb.Append("#");
                break;
            default:
                throw new ArgumentOutOfRangeException();
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