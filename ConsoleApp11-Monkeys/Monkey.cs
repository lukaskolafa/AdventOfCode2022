// See https://aka.ms/new-console-template for more information

using System.Numerics;

namespace ConsoleApp11_Monkeys;

public class Monkey
{
    public Queue<Modulos> Items = new();

    public Func<Modulos, Modulos> Operation { get; set; }

    public int DivisibleTest { get; set; }

    public int TrueMonkey { get; set; }

    public int FalseMonkey { get; set; }

    public long Inspections { get; set; }
}