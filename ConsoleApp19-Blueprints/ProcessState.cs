using ConsoleApp19_Blueprints;

public struct ProcessState
{
    public int ElapsedTime { get; set; }

    public int OreCount { get; set; }

    public int ClayCount { get; set; }

    public int ObsidianCount { get; set; }

    public int GeodesCount { get; set; }

    public int OreRobotCount { get; set; }

    public int ClayRobotCount { get; set; }

    public int ObsidianRobotCount { get; set; }

    public int GeodeRobotCount { get; set; }

    public Blueprint Blueprint { get; set; }

    public bool BlockOreRobot { get; set; }

    public bool BlockClayRobot { get; set; }

    public bool BlockObsidianRobot { get; set; }
}