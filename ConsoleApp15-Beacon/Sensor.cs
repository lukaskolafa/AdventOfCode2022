public class Sensor
{
    public int AllowedDist { get; set; }

    public (int X, int Y) Coords { get; set; }

    public (int X, int Y) ClosestBeacon { get; set; }
}