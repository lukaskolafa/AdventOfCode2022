public class Line
{
    public Line()
    {
        Rock = 0;
        Falling = 0;
    }

    public byte Rock { get; set; }

    public byte Falling { get; set; }

    public bool IsEmpty => (Rock | Falling) == 0;

    public bool IsAllFinal => (Rock & 127) == 127;
}