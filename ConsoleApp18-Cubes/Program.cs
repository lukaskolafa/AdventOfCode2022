// See https://aka.ms/new-console-template for more information

string[] allLines = File.ReadAllLines(@"c:\temp\input.txt");

int size = 20;
bool[,,] rocksSpace = new bool[size, size, size];

bool[,,] freeAccessSpace = new bool[size, size, size];

bool[,,] trappedCaves = new bool[size, size, size];

var freeSurface = 0;

foreach (var line in allLines)
{
    var split = line.Split(",").Select(int.Parse).ToArray();

    (int X, int Y, int Z) coords = (split[0], split[1], split[2]);

    rocksSpace[coords.X, coords.Y, coords.Z] = true;

    int siblings = CountOfAdjacent(coords, rocksSpace);

    freeSurface += -2 * siblings + 6;
}

// mark all edges with way out
for (int x = 0; x < size; x++)
{
    for (int y = 0; y < size; y++)
    {
        for (int z = 0; z < size; z++)
        {
            if (!rocksSpace[x, y, z] && IsEdge((x, y, z)))
            {
                freeAccessSpace[x, y, z] = true;
            }
        }
    }
}

// bubble = all siblings to free space are free too, bubble as long as we find a new free space
bool somethingNew = true;

while (somethingNew)
{
    somethingNew = false;

    for (int x = 0; x < size; x++)
    {
        for (int y = 0; y < size; y++)
        {
            for (int z = 0; z < size; z++)
            {
                if (!rocksSpace[x, y, z] && !freeAccessSpace[x, y, z])
                {
                    var siblings = GetSiblingCoordinates((x, y, z));
                    if (siblings.Any(s => freeAccessSpace[s.X, s.Y, s.Z]))
                    {
                        freeAccessSpace[x, y, z] = true;
                        somethingNew = true;
                    }
                }
            }
        }
    }
}

// map trapped caves - where no rock is and where no free access is
for (int x = 0; x < size; x++)
{
    for (int y = 0; y < size; y++)
    {
        for (int z = 0; z < size; z++)
        {
            trappedCaves[x, y, z] = !rocksSpace[x, y, z] && !freeAccessSpace[x, y, z];
        }
    }
}


// now we have a map of blocked caves, so we calculate again how much surface is visible by adding the cubes again from scratch
bool[,,] rockSpaceWithCaves = new bool[size, size, size];
int freeSurfaceWithoutCaves = 0;

foreach (var line in allLines)
{
    var split = line.Split(",").Select(int.Parse).ToArray();

    (int X, int Y, int Z) coords = (split[0], split[1], split[2]);

    rockSpaceWithCaves[coords.X, coords.Y, coords.Z] = true;

    int siblingsRock = CountOfAdjacent(coords, rockSpaceWithCaves);
    int siblingsCave = CountOfAdjacent(coords, trappedCaves);

    freeSurfaceWithoutCaves += 6;
    freeSurfaceWithoutCaves -= 2 * siblingsRock;
    freeSurfaceWithoutCaves -= siblingsCave;
}

Console.WriteLine(freeSurface);
Console.WriteLine(freeSurfaceWithoutCaves);

IEnumerable<(int X, int Y, int Z)> GetSiblingCoordinates((int X, int Y, int Z) coords)
{
    if (coords.X > 0) yield return (coords.X - 1, coords.Y, coords.Z);
    if (coords.X < size - 1) yield return (coords.X + 1, coords.Y, coords.Z);
    if (coords.Y > 0) yield return (coords.X, coords.Y - 1, coords.Z);
    if (coords.Y < size - 1) yield return (coords.X, coords.Y + 1, coords.Z);
    if (coords.Z > 0) yield return (coords.X, coords.Y, coords.Z - 1);
    if (coords.Z < size - 1) yield return (coords.X, coords.Y, coords.Z + 1);
}

bool IsEdge((int X, int Y, int Z) coords)
{
    if (coords.X == 0 || coords.Y == 0 || coords.Z == 0) return true;
    if (coords.X == size - 1 || coords.Y == size - 1 || coords.Z == size - 1) return true;
    return false;
}

int CountOfAdjacent((int X, int Y, int Z) coords, bool[,,] rocksMap)
{
    return GetSiblingCoordinates(coords).Sum(x => rocksMap[x.X, x.Y, x.Z] ? 1 : 0);
}