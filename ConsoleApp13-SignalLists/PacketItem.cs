using System.Text;

namespace ConsoleApp13_SignalLists;

public class PacketItem
{
    public PacketItem? Parent { get; set; }

    public PacketItem(PacketItem? parent)
    {
        this.Parent = parent;
        this.Items = new List<PacketItem>();
    }

    public static IList<PacketItem> CreateListFromNumber(int number)
    {
        return new List<PacketItem> { new PacketItem(null) { Number = number } };
    }

    public int? Number { get; set; }

    public IList<PacketItem> Items { get; set; }

    public override string ToString()
    {
        if (this.Number.HasValue)
        {
            return this.Number.ToString()!;
        }
        else
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            foreach (var packetItem in Items)
            {
                sb.Append(packetItem);
            }

            sb.Append("}");

            return sb.ToString();
        }
    }
}