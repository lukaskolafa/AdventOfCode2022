// See https://aka.ms/new-console-template for more information

using System.Text;

var allLines = File.ReadAllLines(@"c:\temp\input.txt");

var alph = "abcdefghijklmnopqrstuvwxyz";

int startX = 0;
int startY = 0;
int targetX = 0;
int targetY = 0;
int width = allLines[0].Length;
int height = allLines.Length;

int getIndex(char c)
{
    return alph.IndexOf(c);
}

int[,] heightMap = new int[width, height];
int?[,] walkMap = new int?[width, height];

for (int y = 0; y < height; y++)
{
    for (int x = 0; x < width; x++)
    {
        if (allLines[y][x] == 'S')
        {
            startX = x;
            startY = y;
            heightMap[x, y] = 0;
        }
        else if (allLines[y][x] == 'E')
        {
            targetX = x;
            targetY = y;
            heightMap[x, y] = getIndex('z');
        }
        else
        {
            heightMap[x, y] = getIndex(allLines[y][x]);
        }

        if (heightMap[x, y] == 0)
        {
            walkMap[x, y] = 0;
        }
    }
}

walkMap[startX, startY] = 0;

bool needOneMoreRound = true;

while (needOneMoreRound)
{
    needOneMoreRound = false;

    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            var fromVal = walkMap[x, y];

            if (fromVal.HasValue)
            {
                foreach(var to in getWalkPossibilities((x, y)))
                {
                    var toVal = walkMap[to.X, to.Y];

                    if (!toVal.HasValue || toVal - 1 > fromVal)
                    {
                        walkMap[to.X, to.Y] = fromVal + 1;
                        needOneMoreRound = true;
                    }
                }
            }
        }
    }
}


IEnumerable<(int X, int Y)> getWalkPossibilities((int X, int Y) start)
{
    // up
    if (canWalk(start.X, start.Y, start.X, start.Y - 1))
    {
        yield return new(start.X, start.Y - 1);
    }

    // down
    if (canWalk(start.X, start.Y, start.X, start.Y + 1))
    {
        yield return new(start.X, start.Y + 1);
    }

    // left
    if (canWalk(start.X, start.Y, start.X - 1, start.Y))
    {
        yield return new(start.X- 1, start.Y );
    }

    // right
    if (canWalk(start.X, start.Y, start.X + 1, start.Y))
    {
        yield return new(start.X + 1, start.Y);
    }
}


bool canWalk(int fromX, int fromY, int toX, int toY)
{
    if (toX < 0 || toX >= width || toY < 0 || toY >= height)
    {
        return false;
    }

    int fromHeight = heightMap[fromX, fromY];
    int toHeight = heightMap[toX, toY];

    return (toHeight - 1) <= fromHeight;
}

printHeightMap();
printWalkMap();


Console.WriteLine(walkMap[targetX, targetY]);

void printHeightMap()
{
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            Console.Write(heightMap[x,y]);
        }

        Console.WriteLine();
    }

    Console.WriteLine();
}

void printWalkMap()
{
    StringBuilder sb = new StringBuilder();

    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            if (walkMap[x, y].HasValue)
            {
                sb.Append(walkMap[x, y].Value.ToString("0000"));
                sb.Append(" ");
            }
            else
            {
                sb.Append("----");
                sb.Append(" ");
            }
        }

        sb.AppendLine();
    }

    sb.AppendLine();

    Console.Write(sb.ToString());

    File.WriteAllText("C:\\temp\\walk.txt",sb.ToString());
}
