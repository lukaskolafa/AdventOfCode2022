public class Shape
{
    public static int Width = 4;

    private Shape()
    {
    }

    private static Shape Get1()
    {
        return new Shape
        {
            Rock = new[]
            {
                new[] { true, true, true, true }
            }
        };
    }

    private static Shape Get2()
    {
        return new Shape
        {
            Rock = new[]
            {
                new[] { false, true, false, false },
                new[] { true, true, true, false },
                new[] { false, true, false, false }
            }
        };
    }

    private static Shape Get3()
    {
        return new Shape
        {
            Rock = new[]
            {
                new[] { false, false, true, false },
                new[] { false, false, true, false },
                new[] { true, true, true, false }
            }
        };
    }

    private static Shape Get4()
    {
        return new Shape
        {
            Rock = new[]
            {
               new[] { true, false, false, false },
               new[] { true, false, false, false },
               new[] { true, false, false, false },
               new[] { true, false, false, false }
            }
        };
    }

    private static Shape Get5()
    {
        return new Shape
        {
            Rock = new[]
            {
                 new[] { true, true, false, false },
                 new[] { true, true, false, false }
            }
        };
    }

    public static IList<Shape> GetShapes()
    {
        return new[] { Get1(), Get2(), Get3(), Get4(), Get5() };
    }

    public bool[][] Rock { get; private init; }
}