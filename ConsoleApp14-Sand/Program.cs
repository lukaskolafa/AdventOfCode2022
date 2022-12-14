// See https://aka.ms/new-console-template for more information

using System.Text;

var allLines = new Queue<string>(File.ReadAllLines(@"c:\temp\input.txt"));

int width = 2001;
int heigth = 180;
int shift = 500;

Entity[,] map = new Entity[width,heigth];

foreach (var line in allLines)
{
    string[] parts = line.Split(" -> ");

    for(int i = 0; i < parts.Length - 1; i++)
    {
        var start = parts[i].Split(",").Select(int.Parse).ToArray();
        var fin = parts[i+1].Split(",").Select(int.Parse).ToArray();

        foreach ((int X, int Y) coord in GetLine((start[0] + shift, start[1]), (fin[0] + shift, fin[1])))
        {
            map[coord.X, coord.Y] = Entity.Rock;
        }

    }
}

int count = 0;

while (true)
{
    (int X, int Y)? target = findSandPath((500 + shift, 0));

    if (target == null)
    {
        throw new Exception();
    }
    else
    {
        count++;
        map[target.Value.X, target.Value.Y] = Entity.Sand;
    }

    if (map[500 + shift, 0] == Entity.Sand)
    {
        break;
    }
}

File.WriteAllText("c:\\temp\\output.txt", printMap());
Console.WriteLine(count);


(int X, int Y)? findSandPath((int X, int Y) start)
{
    var current = start;

    while (true)
    {
        if (current.Y >= heigth - 2)
        {
            return null;
        }
        else if (map[current.X, current.Y + 1] == Entity.Free)
        {
            current = (current.X, current.Y + 1);
        }
        else if (map[current.X - 1, current.Y + 1] == Entity.Free)
        {
            current = (current.X - 1, current.Y + 1);
        }
        else if (map[current.X + 1, current.Y + 1] == Entity.Free)
        {
            current = (current.X + 1, current.Y + 1);
        }
        else
        {
            return current;
        }
    }
}

string printMap()
{
    StringBuilder sb = new StringBuilder();

    for (int y = 0; y < heigth; y++)
    {
        for (int x = 400; x < width; x++)
        {
            switch (map[x, y])
            {
                case Entity.Free:
                    sb.Append(".");
                    break;
                case Entity.Rock:
                    sb.Append("#");
                    break;
                case Entity.Sand:
                    sb.Append("o");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        sb.AppendLine();
    }

    sb.AppendLine();

    return sb.ToString();
}

IEnumerable<(int X, int Y)> GetLine((int X, int Y) start, (int X, int Y) fin)
{
    if (start.X == fin.X)
    {
        for (int y = Math.Min(start.Y, fin.Y); y <= Math.Max(start.Y, fin.Y); y++)
        {
            yield return (start.X, y);
        }
    }
    else if (start.Y == fin.Y)
    {
        for (int x = Math.Min(start.X, fin.X); x <= Math.Max(start.X, fin.X); x++)
        {
            yield return (x, start.Y);
        }
    }
    else
    {
        throw new Exception("corrupted line");
    }
}

// 29068 too high