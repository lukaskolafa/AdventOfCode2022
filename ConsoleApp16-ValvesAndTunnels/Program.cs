// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.RegularExpressions;

var parserRegex = new Regex(@"^Valve (\w{2}) has flow rate=(\d+); tunnels? leads? to valves? ([\w,\ ]+)");
string[] allLines = File.ReadAllLines(@"c:\temp\input.txt");

IDictionary<string, Valve> valves = new Dictionary<string, Valve>();

// Map valves
foreach (var line in allLines)
{
    var match = parserRegex.Match(line);

    var name = match.Groups[1].Value;

    valves[name] = new Valve { FlowRate = int.Parse(match.Groups[2].Value), Name = name };
}

// Map connections
foreach (var line in allLines)
{
    var match = parserRegex.Match(line);

    var connections = match.Groups[3].Value.Split(",").Select(x => x.Trim());

    foreach (string connection in connections)
    {
        valves[match.Groups[1].Value].DirectConnections.Add(valves[connection]);
    }
}

// Pre-init for each valve all valve distances
foreach (var valve in valves)
{
    foreach (var goal in valves)
    {
        if (goal.Key == valve.Key)
        {
            valve.Value.ShortestDistanceToAllValves[goal.Key] = 0;
        }
        else
        {
            valve.Value.ShortestDistanceToAllValves[goal.Key] = null;
        }
    }
}

// Find fastest connection from each to each - actually we need only the ones with some flow rate, but who cares, let us do it simple..
foreach (var startValve in valves)
{
    bool needOneMoreRound = true;

    while (needOneMoreRound)
    {
        needOneMoreRound = false;

        foreach (var valve in valves)
        {
            int? valveDistance = startValve.Value.ShortestDistanceToAllValves[valve.Key];

            if (valveDistance == null)
            {
                // this valve has no distance yet, cannot calculate
                continue;
            }
            else
            {
                int distanceToPotentialConnection = valveDistance.Value + 1;

                foreach (var potentialDirectConnection in valve.Value.DirectConnections)
                {
                    int? shortestDistanceFound = startValve.Value.ShortestDistanceToAllValves[potentialDirectConnection.Name];

                    if (shortestDistanceFound == null || shortestDistanceFound > distanceToPotentialConnection)
                    {
                        startValve.Value.ShortestDistanceToAllValves[potentialDirectConnection.Name] = distanceToPotentialConnection;
                        needOneMoreRound = true;
                    }
                }
            }
        }
    }
}

IList<string> valvesWithFlow = valves.Values.Where(v => v.FlowRate > 0).OrderByDescending(v => v.FlowRate).Select(x => x.Name).ToArray();

IList<IList<string>> interestingValvesGroup = new List<IList<string>>();

foreach (var flowValve in valvesWithFlow)
{
    IList<string> l = new[] { flowValve }.Concat(valvesWithFlow.Except(new[] { flowValve })).ToArray();
    interestingValvesGroup.Add(l);
}

foreach (var group in interestingValvesGroup)
{
    Console.WriteLine(string.Join(", ", group));
}

int[] maxPressures = new int [interestingValvesGroup.Count * 2];
Task[] tasks = new Task[interestingValvesGroup.Count * 2];

for (int i = 0; i < interestingValvesGroup.Count; i++)
{
    var index = i;

    Task calculationMe = Task.Run(() => recursiveWalker("AA", interestingValvesGroup[index].Take(1).ToArray(), Array.Empty<string>(), true, index, interestingValvesGroup[index]));

    Task calculationEl = Task.Run(() => recursiveWalker("AA", Array.Empty<string>(), interestingValvesGroup[index].Take(1).ToArray(), false, index + interestingValvesGroup.Count, interestingValvesGroup[index]));

    tasks[index] = calculationMe;
    tasks[index + interestingValvesGroup.Count] = calculationEl;
}

bool finished = false;

Task output = Task.Run(async () =>
{
    Console.Clear();

    while (true)
    {
        Console.SetCursorPosition(0, 0);
        foreach (var maxPressure in maxPressures)
        {
            Console.WriteLine(maxPressure);
        }

        Console.WriteLine("Max:");

        Console.WriteLine(maxPressures.OrderDescending().First());

        await Task.Delay(1000);

        if (finished)
        {
            break;
        }
    }
});

await Task.WhenAll(tasks);

finished = true;

await output;

// 2637
// 2342
// 2864
// 2904
// 2562
// 2716
// 2911
// Max:
// 2911
// 2576
// 2676
// 2864
// 2670
// 2318
// 2557
// 2707
// 2843
// 2637
// 2342
// 2864
// 2904
// 2593
// 2716
// 2911
// 2576
// 2676
// 2864
// 2670
// 2318
// 2557
// 2669
// 2843
// 2637
// 2342
// 2864
// 2904
// 2562
// 2716
// 2911
// Max:
// 2911  (correct, but calculation still not finished after 5 hours)


File.WriteAllText(@"c:\temp\output.txt", ValvesStats());


void recursiveWalker(string start, string[] pathMe, string[] pathElephant, bool swap, int index, IList<string> interestingValves)
{
    var remainings = interestingValves.Except(pathMe).Except(pathElephant);

    foreach (var remaining in remainings)
    {
        // try me
        var nextStepPathMe = pathMe.Concat(new[] { remaining }).ToArray();

        int? pressureByMe = calculatePressure(start, nextStepPathMe) + calculatePressure(start, pathElephant);

        if (pressureByMe.HasValue && pressureByMe.Value > maxPressures[index])
        {
            maxPressures[index] = pressureByMe.Value;
        }

        if (pressureByMe.HasValue)
        {
            recursiveWalker(start, swap ? pathElephant : nextStepPathMe, swap ? nextStepPathMe : pathElephant, !swap, index, interestingValves);
        }

        // try elephant
        var nextStepPathElephant = pathElephant.Concat(new[] { remaining }).ToArray();

        int? pressureByElephant = calculatePressure(start, pathMe) + calculatePressure(start, nextStepPathElephant);

        if (pressureByElephant.HasValue && pressureByElephant.Value > maxPressures[index])
        {
            maxPressures[index] = pressureByElephant.Value;
        }

        if (pressureByElephant.HasValue)
        {
            recursiveWalker(start, swap ? nextStepPathElephant : pathMe, swap ? pathMe : nextStepPathElephant, !swap, index, interestingValves);
        }
    }
}

// maxPressure = calculatePressure("AA", new[] { "JJ", "BB", "CC" })!.Value + calculatePressure("AA", new[] { "DD", "HH", "EE" })!.Value;

int? calculatePressure(string start, IList<string> path)
{
    int currentMinutes = 26;
    string currentValve = start;
    int pressureRelease = 0;

    foreach (var step in path)
    {
        var dist = valves[currentValve].ShortestDistanceToAllValves[step]!.Value;
        var distAndOpenValve = dist + 1;

        currentMinutes -= distAndOpenValve;

        if (currentMinutes < 0)
        {
            return null;
        }

        pressureRelease += currentMinutes * valves[step].FlowRate;

        currentValve = step;
    }

    return pressureRelease;
}

string ValvesStats()
{
    StringBuilder sb = new StringBuilder();

    foreach (var valve in valves)
    {
        sb.AppendLine($"Valve {valve.Value.Name} with flow {valve.Value.FlowRate}:");
        sb.Append("Direct Connections: ");

        foreach (var dc in valve.Value.DirectConnections)
        {
            sb.Append(dc.Name);
            sb.Append(" ");
        }

        sb.AppendLine();
        sb.AppendLine("Distance to all valves: ");

        foreach (var distanceToAllValve in valve.Value.ShortestDistanceToAllValves.OrderBy(x => x.Value!.Value))
        {
            sb.AppendLine($"-> {distanceToAllValve.Key} = {distanceToAllValve.Value!.Value}");
        }

        sb.AppendLine();

        sb.AppendLine("=========================");
        sb.AppendLine();
    }

    return sb.ToString();
}