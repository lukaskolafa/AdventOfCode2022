public class Line
{
    public Line(int width)
    {
        Fields = new Field[width];
    }

    public Field[] Fields { get; set; }

    public bool IsEmpty => this.Fields.All(f => f == Field.Empty);

    public bool IsAllFinal => this.Fields.All(f => f == Field.Final);
}