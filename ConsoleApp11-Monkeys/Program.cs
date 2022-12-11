// See https://aka.ms/new-console-template for more information

using ConsoleApp11_Monkeys;

IList<Monkey> monkeys = ParseMonkeys();

for (int round = 1; round <= 10000; round++)
{
    if (round % 10 == 0) Console.WriteLine(round);

    foreach (var monkey in monkeys)
    {
        while (true)
        {
            if (!monkey.Items.TryDequeue(out Modulos dequeued))
            {
                break;
            }

            dequeued = monkey.Operation(dequeued);

            monkey.Inspections++;

            if (dequeued.Divisible(monkey.DivisibleTest))
            {
                monkeys[monkey.TrueMonkey].Items.Enqueue(dequeued);
            }
            else
            {
                monkeys[monkey.FalseMonkey].Items.Enqueue(dequeued);
            }
        }
    }
}

foreach (var monkey in monkeys.OrderBy(m => m.Inspections))
{
    Console.WriteLine($"Monkey");
    Console.WriteLine(string.Join(", ", monkey.Items));
    Console.WriteLine($"Inspections {monkey.Inspections}");
    Console.WriteLine();
}

IList<Monkey> ParseMonkeys()
{
    List<Monkey> result = new List<Monkey>();

    result.Add(new Monkey
    {
        Items = new Queue<Modulos>(new [] {63, 57 }.Select(x => new Modulos(x))),
        Operation = n => n.Multiply(11),
        DivisibleTest = 7,
        TrueMonkey = 6,
        FalseMonkey = 2
    });

    result.Add(new Monkey
    {
        Items = new Queue<Modulos>(new [] {82, 66, 87, 78, 77, 92, 83 }.Select(x => new Modulos(x))),
        Operation = n => n.Add(1),
        DivisibleTest = 11,
        TrueMonkey = 5,
        FalseMonkey = 0
    });

    result.Add(new Monkey
    {
        Items = new Queue<Modulos>(new [] {97, 53, 53, 85, 58, 54 }.Select(x => new Modulos(x))),
        Operation = n => n.Multiply(7),
        DivisibleTest = 13,
        TrueMonkey = 4,
        FalseMonkey = 3
    });

    result.Add(new Monkey
    {
        Items = new Queue<Modulos>(new [] {50 }.Select(x => new Modulos(x))),
        Operation = n => n.Add(3),
        DivisibleTest = 3,
        TrueMonkey = 1,
        FalseMonkey = 7
    });

    result.Add(new Monkey
    {
        Items = new Queue<Modulos>(new [] {64, 69, 52, 65, 73 }.Select(x => new Modulos(x))),
        Operation = n => n.Add(6),
        DivisibleTest = 17,
        TrueMonkey = 3,
        FalseMonkey = 7
    });

    result.Add(new Monkey
    {
        Items = new Queue<Modulos>(new [] {57, 91, 65 }.Select(x => new Modulos(x))),
        Operation = n => n.Add(5),
        DivisibleTest = 2,
        TrueMonkey = 0,
        FalseMonkey = 6
    });

    result.Add(new Monkey
    {
        Items = new Queue<Modulos>(new [] {67, 91, 84, 78, 60, 69, 99, 83 }.Select(x => new Modulos(x))),
        Operation = n => n.Power(),
        DivisibleTest = 5,
        TrueMonkey = 2,
        FalseMonkey = 4
    });

    result.Add(new Monkey
    {
        Items = new Queue<Modulos>(new [] { 58, 78, 69, 65 }.Select(x => new Modulos(x))),
        Operation = n => n.Add(7),
        DivisibleTest = 19,
        TrueMonkey = 5,
        FalseMonkey = 1
    });

    return result;
}

// too low 25311442632
// 27267163742
// 31529764188