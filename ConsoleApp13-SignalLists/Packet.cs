using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp13_SignalLists;

public class Packet : IComparable
{
    private Regex bracketParser = new(@"^(?<init>\[*)(?<number>\d*)(?<close>\]*)$");

    public Packet(string input)
    {
        RawInput = input;
        Items = this.Parse(input);
    }

    public IList<PacketItem> Items { get; set; }

    public string RawInput { get; set; }

    public override string ToString()
     {
         StringBuilder sb = new StringBuilder();

         sb.Append(this.RawInput);

         foreach (var packetItem in this.Items)
         {
             sb.Append(packetItem);
         }

         return sb.ToString();
     }

    private IList<PacketItem> Parse(string input)
    {
        PacketItem resultWrapper = new PacketItem(null);
        PacketItem pointer = resultWrapper;

        string[] parts = input.Split(',');

        foreach (var part in parts)
        {
            var match = bracketParser.Match(part);

            for (int i = 0; i < match.Groups["init"].Value.Length; i++)
            {
                var deeper = new PacketItem(pointer);
                pointer.Items.Add(deeper);
                pointer = deeper;
            }

            var numberString = match.Groups["number"].Value;
            if (!string.IsNullOrWhiteSpace(numberString))
            {
                var number = new PacketItem(pointer)  { Number = int.Parse(numberString) };
                pointer.Items.Add(number);
            }

            for (int i = 0; i < match.Groups["close"].Value.Length; i++)
            {
                pointer = pointer.Parent;
            }
        }

        if (pointer != resultWrapper)
        {
            throw new Exception();
        }

        return resultWrapper.Items.First().Items;
    }

    public int CompareTo(object? obj)
    {
        var b = obj as Packet;

        if (b == null)
        {
            throw new Exception("null");
        }

        bool? correct = this.IsCorrectOrder(this.Items, b.Items);

        if (!correct.HasValue)
        {
            throw new Exception("not comparable");
        }

        return correct.Value ? -1 : 1;
    }

    private bool? IsCorrectOrder(IList<PacketItem> left, IList<PacketItem> right)
    {
        int i = -1;

        while (true)
        {
            i++;

            if (left.Count == i && right.Count == i)
            {
                // ran out of items on both sides, no resolution
                return null;
            }

            if (left.Count == i)
            {
                // left side out => correct order, no need to check further
                return true;
            }

            if (right.Count == i)
            {
                // right side => not correct
                return false;
            }

            var leftItem = left[i];
            var rightItem = right[i];

            if (leftItem.Number.HasValue && rightItem.Number.HasValue)
            {
                if (leftItem.Number < rightItem.Number)
                {
                    // left smaller => correct order
                    return true;
                }
                else if (leftItem.Number == rightItem.Number)
                {
                    // same => continue
                    continue;
                }
                else
                {
                    // bigger left => not correct
                    return false;
                }
            }
            else if (leftItem.Number.HasValue && !rightItem.Number.HasValue)
            {
                bool? result = IsCorrectOrder(PacketItem.CreateListFromNumber(leftItem.Number.Value), rightItem.Items);
                if (result.HasValue)
                {
                    return result;
                }
                else
                {
                    continue;
                }
            }
            else if (!leftItem.Number.HasValue && rightItem.Number.HasValue)
            {
                bool? result = IsCorrectOrder(leftItem.Items, PacketItem.CreateListFromNumber(rightItem.Number.Value));
                if (result.HasValue)
                {
                    return result;
                }
                else
                {
                    continue;
                }
            }
            else
            {
                bool? result = IsCorrectOrder(leftItem.Items, rightItem.Items);
                if (result.HasValue)
                {
                    return result;
                }
                else
                {
                    continue;
                }
            }
        }
    }
}