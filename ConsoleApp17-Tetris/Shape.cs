public class Shape
{
    private Shape()
    {
    }

    private static Shape Get1()
    {
        return new Shape
        {
            Rock = new byte[]
            {
                30
            }
        };
    }

    private static Shape Get2()
    {
        return new Shape
        {
            Rock = new byte[]
            {
                8,
                28,
                8
            }
        };
    }

    private static Shape Get3()
    {
        return new Shape
        {
            Rock = new byte[]
            {
                // The order is reversed to speed up placement upside down
                28,
                4,
                4
            }
        };
    }

    private static Shape Get4()
    {
        return new Shape
        {
            Rock = new byte[]
            {
               16,
               16,
               16,
               16
            }
        };
    }

    private static Shape Get5()
    {
        return new Shape
        {
            Rock = new byte[]
            {
                24,
                24
            }
        };
    }

    public static IList<Shape> GetShapes()
    {
        return new[] { Get1(), Get2(), Get3(), Get4(), Get5() };
    }

    public byte[] Rock { get; private init; }
}