// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.RegularExpressions;

var parserRegex = new Regex(@"^Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)$");
string[] allLines = File.ReadAllLines(@"c:\temp\input.txt");

int width = 200;
int heigth = 200;
int shift = 0;
bool store = false;

Entity[,] map = new Entity[width,heigth];

List<Sensor> sensors = new();

foreach (var line in allLines)
{
    var match = parserRegex.Match(line);

    (int X, int Y) sensor = (int.Parse(match.Groups[1].Value) + shift, int.Parse(match.Groups[2].Value) + shift);
    (int X, int Y) beacon = (int.Parse(match.Groups[3].Value) + shift, int.Parse(match.Groups[4].Value) + shift);

    if (store)
    {
        map[sensor.X, sensor.Y] = Entity.Sensor;
        map[beacon.X, beacon.Y] = Entity.Beacon;
    }

    sensors.Add(new Sensor { Coords = sensor, ClosestBeacon = beacon, AllowedDist = getManhattanDist(sensor, beacon) });
}

var result = sensors
    .Select(getFreeBorderAroundSensor)
    .SelectMany(x => x)
    .Where(x => !isTooClose(x))
    .Where(x => x.X > 0 && x.Y > 0 && x.Y <= 4000000 && x.X <= 4000000)
    .ToArray();



var a = sensors.Select(getAreaToClosesBeacon).ToArray();
var b = a.SelectMany(s => s).ToArray();
var c1 = b.Where(nb => !sensors.Any(s => s.Coords.X == nb.X && s.Coords.Y == nb.Y));
var c2 = c1.Where(nb => !sensors.Any(s => s.ClosestBeacon.X == nb.X && s.ClosestBeacon.Y == nb.Y));

if (store)
{
    foreach (var coord in c2)
    {
        map[coord.X, coord.Y] = Entity.NoBeacon;
    }

    foreach (var border in sensors.Select(getFreeBorderAroundSensor).SelectMany(x => x))
    {
        map[border.X, border.Y] = Entity.Border;
    }
}

var d = c2.Distinct().Count(nb => nb.Y == 2000000);
Console.WriteLine(d);

// too high: 11215725
// too high: 11215719
//           4861076



File.WriteAllText("c:\\temp\\output.txt", printMap());

bool isTooClose((int X, int Y) coord)
{
    foreach (var sensor in sensors)
    {
        var dist = getManhattanDist(sensor.Coords, coord);
        if (dist <= sensor.AllowedDist)
        {
            return true;
        }
    }

    return false;
}

IEnumerable<(int X, int Y)> getAreaToClosesBeacon(Sensor sensor)
{
    var dist = getManhattanDist(sensor.Coords, sensor.ClosestBeacon);

    for (int x = sensor.Coords.X - dist; x <= sensor.Coords.X + dist; x++)
    {
        for (int y = sensor.Coords.Y - dist; y <= sensor.Coords.Y + dist; y++)
        {
            if (getManhattanDist(sensor.Coords, (x, y)) <= dist)
            {
                yield return (x, y);
            }
        }
    }
}

IEnumerable<(int X, int Y)> getFreeBorderAroundSensor(Sensor sensor)
{
    var dist = getManhattanDist(sensor.Coords, sensor.ClosestBeacon);
    var expectedDist = dist + 1;

    for (int i = 0; i < expectedDist; i++)
    {
        yield return (sensor.Coords.X - expectedDist + i, sensor.Coords.Y - i);
        yield return (sensor.Coords.X + i, sensor.Coords.Y - expectedDist + i);
        yield return (sensor.Coords.X + expectedDist - i, sensor.Coords.Y + i);
        yield return (sensor.Coords.X - i, sensor.Coords.Y + expectedDist - i);
    }
}

int getManhattanDist((int X, int Y) from, (int X, int Y) to)
{
    return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
}

string printMap()
{
    StringBuilder sb = new StringBuilder();

    for (int y = 0; y < heigth; y++)
    {
        for (int x = 0; x < width; x++)
        {
            switch (map[x, y])
            {
                case Entity.Empty:
                    sb.Append(".");
                    break;
                case Entity.Beacon:
                    sb.Append("B");
                    break;
                case Entity.Sensor:
                    sb.Append("S");
                    break;
                case Entity.NoBeacon:
                    sb.Append("#");
                    break;
                case Entity.Border:
                    sb.Append("x");
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
