// See https://aka.ms/new-console-template for more information

using ConsoleApp13_SignalLists;

var allLines = new Queue<string>(File.ReadAllLines(@"c:\temp\input2.txt"));

var packets = allLines.Where(l => !string.IsNullOrWhiteSpace(l)).Select(l => new Packet(l)).ToArray();


// while (true)
// {
//     bool leftSuccess = allLines.TryDequeue(out string left);
//     bool rightSuccess = allLines.TryDequeue(out string right);
//     _ = allLines.TryDequeue(out _);
//
//     if (leftSuccess && rightSuccess)
//     {
//         pairs.Add(new PacketPair
//         {
//             Left = new Packet(left!),
//             Right = new Packet(right!)
//         });
//     }
//     else
//     {
//         break;
//     }
// }


int index = 1;
int sum = 1;

foreach (var packet in packets.Order())
{
    Console.WriteLine($"{index}: ===================");
    Console.WriteLine(packet.RawInput);

    if (packet.RawInput == "[[2]]" || packet.RawInput == "[[6]]")
    {
        sum *= index;
    }

    index++;

    Console.WriteLine();
}

// 318 - too low

Console.WriteLine(sum);

