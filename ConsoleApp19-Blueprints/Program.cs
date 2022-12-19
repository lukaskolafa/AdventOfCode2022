// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;
using ConsoleApp19_Blueprints;

var parserRegex = new Regex(@"^Blueprint (\d+): Each ore robot costs (\d+) ore. Each clay robot costs (\d+) ore. Each obsidian robot costs (\d+) ore and (\d+) clay. Each geode robot costs (\d+) ore and (\d+) obsidian.");
string[] allLines = File.ReadAllLines(@"c:\temp\input.txt");

int maxTime = 24;

IList<Blueprint> blueprints = new List<Blueprint>(allLines.Length);

foreach (var line in allLines)
{
    var match = parserRegex.Match(line);

    blueprints.Add(new Blueprint
    {
        Index = int.Parse(match.Groups[1].Value),
        OreRobotOreCost = int.Parse(match.Groups[2].Value),
        ClayRobotOreCost = int.Parse(match.Groups[3].Value),
        ObsidianRobotOreCost = int.Parse(match.Groups[4].Value),
        ObsidianRobotClayCost = int.Parse(match.Groups[5].Value),
        GeodeRobotOreCost = int.Parse(match.Groups[6].Value),
        GeodeRobotObsidianCost = int.Parse(match.Groups[7].Value),
    });
}

var sum = 0;

IList<Task> tasks = new List<Task>();

foreach (var blueprint in blueprints)
{
    var task = Task.Run(() =>
    {
        ProcessState processState = new ProcessState { OreRobotCount = 1, Blueprint = blueprint, ElapsedTime = 0 };
        ProcessMonitor processMonitor = new ProcessMonitor();

        MaximizeGeodes(processState, processMonitor);

        Console.WriteLine($"Blueprint {blueprint.Index}: max geodes collected: {processMonitor.MaxCollectedGeodes}.");

        sum += blueprint.Index * processMonitor.MaxCollectedGeodes;
    });

    tasks.Add(task);
}

await Task.WhenAll(tasks);

Console.WriteLine(sum);

void MaximizeGeodes(ProcessState processState, ProcessMonitor processMonitor)
{
    if (processState.ElapsedTime == maxTime)
    {
        if (processState.GeodesCount > processMonitor.MaxCollectedGeodes)
        {
            processMonitor.MaxCollectedGeodes = processState.GeodesCount;
        }

        return;
    }

    // skip if this branch has no potential to be best (not enough time to build more geodes robots)
    int timeLeft = maxTime - processState.ElapsedTime;
    int ensuredGeodesGain = timeLeft * processState.GeodeRobotCount;
    int potentialGeodesGainIfWeCreateRobotEachTurn = ((timeLeft - 1) * timeLeft) / 2;

    if (processState.GeodesCount + ensuredGeodesGain + potentialGeodesGainIfWeCreateRobotEachTurn <= processMonitor.MaxCollectedGeodes)
    {
        return;
    }

    var nextRoundBranches = GetNextRoundVariants(processState).ToArray();

    foreach (var nextRoundBranch in nextRoundBranches)
    {
        MaximizeGeodes(nextRoundBranch, processMonitor);
    }
}


IEnumerable<ProcessState> GetNextRoundVariants(ProcessState processState)
{
    processState.ElapsedTime++;

    bool canBuildOreRobot = processState.OreCount >= processState.Blueprint.OreRobotOreCost;
    bool canBuildClayRobot = processState.OreCount >= processState.Blueprint.ClayRobotOreCost;
    bool canBuildObsidianRobot = processState.OreCount >= processState.Blueprint.ObsidianRobotOreCost && processState.ClayCount >= processState.Blueprint.ObsidianRobotClayCost;
    bool canBuildGeodeRobot = processState.OreCount >= processState.Blueprint.GeodeRobotOreCost && processState.ObsidianCount >= processState.Blueprint.GeodeRobotObsidianCost;

    bool shouldBuildOreRobot = processState.OreRobotCount <= Math.Max(processState.Blueprint.ClayRobotOreCost, Math.Max(processState.Blueprint.GeodeRobotOreCost, Math.Max(processState.Blueprint.ObsidianRobotOreCost, processState.Blueprint.OreRobotOreCost)));
    bool shouldBuildClayRobot = processState.ClayRobotCount <= processState.Blueprint.ObsidianRobotClayCost;
    bool shouldBuildObsidianRobot = processState.ObsidianRobotCount <= processState.Blueprint.GeodeRobotObsidianCost;

    processState.OreCount += processState.OreRobotCount;
    processState.ClayCount += processState.ClayRobotCount;
    processState.GeodesCount += processState.GeodeRobotCount;
    processState.ObsidianCount += processState.ObsidianRobotCount;

    if (canBuildGeodeRobot)
    {
        var result = processState;
        result.GeodeRobotCount++;
        result.OreCount -= processState.Blueprint.GeodeRobotOreCost;
        result.ObsidianCount -= processState.Blueprint.GeodeRobotObsidianCost;
        yield return result;
        yield break; // Do not create anything else if we can create a geode robot, nothing else could be faster
    }

    if (canBuildObsidianRobot && shouldBuildObsidianRobot)
    {
        var result = processState;
        result.ObsidianRobotCount++;
        result.OreCount -= processState.Blueprint.ObsidianRobotOreCost;
        result.ClayCount -= processState.Blueprint.ObsidianRobotClayCost;
        yield return result;
    }

    if (canBuildOreRobot && shouldBuildOreRobot)
    {
        var result = processState;
        result.OreRobotCount++;
        result.OreCount -= processState.Blueprint.OreRobotOreCost;
        yield return result;
    }

    if (canBuildClayRobot && shouldBuildClayRobot)
    {
        var result = processState;
        result.ClayRobotCount++;
        result.OreCount -= processState.Blueprint.ClayRobotOreCost;
        yield return result;
    }

    yield return processState; // This is the variant, if we did not build anything, await a better robot in next round
}
