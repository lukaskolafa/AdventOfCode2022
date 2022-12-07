// See https://aka.ms/new-console-template for more information

var allLines = File.ReadAllLines(@"c:\temp\input.txt");

var t = allLines[0];

char[] buff = new char[14];

for (int i = 0; i < t.Length; i++)
{
    for (int j = 0; j < buff.Length; j++)
    {
        buff[j] = t[j + i];
    }

    for (int j = 0; j < buff.Length; j++)
    {
        Console.Write(buff[j]);
    }
    Console.WriteLine();
    
    bool distinct = true;

    for (int j = 0; j < buff.Length; j++)
    {
        for (int q = 0; q < buff.Length; q++)
        {
            if (q != j && buff[j] == buff[q]) distinct = false;
        }
    }

    if (distinct)
    {
        Console.WriteLine(i);
        Console.WriteLine(t.Substring(i));
        break;
    }
}



