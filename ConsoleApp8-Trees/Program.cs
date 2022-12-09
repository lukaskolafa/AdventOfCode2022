// See https://aka.ms/new-console-template for more information

var allLines = File.ReadAllLines(@"c:\temp\input.txt");

int width = allLines[0].Length;
int height = allLines.Length;

// IList<IList<int>> matrix = new List<IList<int>>();

int cnt = 0;

for (int i = 0; i < width; i++)
{
    for (int j = 0; j < height; j++)
    {
        var myHeight = GetNum(i, j);

        var visible1 = CountVisible(myHeight, GetColDown(i, j));
        var visible2 = CountVisible(myHeight, GetColUp(i, j));
        var visible3 = CountVisible(myHeight, GetRowLeft(i, j));
        var visible4 = CountVisible(myHeight, GetRowRight(i, j));

        var sc = visible1 * visible2 * visible3 * visible4;

        Console.Write(sc);

        if (sc > cnt) cnt = sc;
    }
    
    Console.WriteLine();
}

Console.WriteLine(cnt);

int CountVisible(int myHeight, IEnumerable<int> trees)
{
    int count = 0;

    foreach (var tree in trees)
    {
        if (tree <= myHeight) count++;

        if (tree >= myHeight) break;
    }

    return count;
}

IEnumerable<int> GetRowRight(int startRow, int startCol)
{
    for (int i = startCol + 1; i < width; i++)
    {
        yield return GetNum(startRow, i);
    }
}

IEnumerable<int> GetRowLeft(int startRow, int startCol)
{
    for (int i = startCol - 1; i >= 0; i--)
    {
        yield return GetNum(startRow, i);
    }
}

IEnumerable<int> GetColDown(int startRow, int startCol)
{
    for (int i = startRow + 1; i < height; i++)
    {
        yield return GetNum(i, startCol);
    }
}

IEnumerable<int> GetColUp(int startRow, int startCol)
{
    for (int i = startRow - 1; i >= 0; i--)
    {
        yield return GetNum(i, startCol);
    }
}


int GetNum(int row, int col)
{
    char c = allLines[row][col];
    var num = int.Parse(c.ToString());

    return num;
}
