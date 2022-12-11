namespace ConsoleApp11_Monkeys;

public class Modulos
{
    public Modulos(int init)
    {
        for (int i = 2; i < 20; i++)
        {
            Remainings[i] = init % i;
        }
    }

    public Modulos(int[] remainings)
    {
        for (int i = 2; i < 20; i++)
        {
            Remainings[i] = remainings[i];
        }
    }

    public bool Divisible(int by)
    {
        return Remainings[by] == 0;
    }

    public Modulos Add(int number)
    {
        int[] result = new int [20];

        for (int i = 2; i < 20; i++)
        {
            result[i] = (Remainings[i] + number) % i;
        }

        return new Modulos(result);
    }

    public Modulos Multiply(int by)
    {
        int[] result = new int [20];

        for (int i = 2; i < 20; i++)
        {
            result[i] = (Remainings[i] * by) % i;
        }

        return new Modulos(result);
    }

    public Modulos Power()
    {
        int[] result = new int [20];

        for (int i = 2; i < 20; i++)
        {
            result[i] = (Remainings[i] * Remainings[i]) % i;
        }

        return new Modulos(result);
    }

    private int[] Remainings = new int[20];
}