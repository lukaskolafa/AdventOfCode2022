// using System.Text;
//
// namespace ConsoleApp13_SignalLists;
//
// public class PacketPair
// {
//     public Packet Left { get; set; }
//     public Packet Right { get; set; }
//
//     public override string ToString()
//     {
//         StringBuilder sb = new StringBuilder();
//
//         sb.Append(this.Left.RawInput);
//         sb.Append(" vs. ");
//         sb.AppendLine(this.Right.RawInput);
//
//         foreach (var packetItem in this.Left.Items)
//         {
//             sb.Append(packetItem);
//         }
//
//         sb.Append(" vs. ");
//
//         foreach (var packetItem in this.Right.Items)
//         {
//             sb.Append(packetItem);
//         }
//
//         return sb.ToString();
//     }
// }