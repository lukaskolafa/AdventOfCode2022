public class Valve
{
    public Valve()
    {
        DirectConnections = new List<Valve>();
        ShortestDistanceToAllValves = new Dictionary<string, int?>();
    }

    public string Name { get; set; }

    public int FlowRate { get; set; }

    public IList<Valve> DirectConnections { get; }

    public IDictionary<string, int?> ShortestDistanceToAllValves { get; }
}