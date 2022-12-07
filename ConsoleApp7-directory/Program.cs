// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Directory = ConsoleApp7_directory.Directory;
using File = System.IO.File;

var allLines = File.ReadAllLines(@"c:\temp\input.txt");

var rootDirectory = new Directory { Name = "/"};

Directory pointer = rootDirectory;

Regex fileRegex = new Regex(@"(\d*)\ (.*)");

for (int i = 0; i < allLines.Length; i++)
{
    var line = allLines[i];

    if (line.StartsWith("$"))
    {
        if (line == "$ ls")
        {
            // not interesting, the next iteration will include files to the current root
            continue;
        }

        if (line == "$ cd ..")
        {
            pointer = pointer.Parent;
            continue;
        }
        
        if (line.StartsWith("$ cd"))
        {
            pointer = pointer.Directories.Single(d => d.Name == line.Substring(5));
            continue;
        }

        throw new Exception("bad command");
    }

    if (line.StartsWith("dir"))
    {
        pointer.Directories.Add(new Directory { Parent = pointer, Name = line.Substring(4)});
        continue;
    }

    var match = fileRegex.Match(line);
    
    pointer.Files.Add(new ConsoleApp7_directory.File { Name = match.Groups[2].Value, Parent = pointer, Size = int.Parse(match.Groups[1].Value)});
}

Console.Write(rootDirectory);

Console.WriteLine("------------------");

var fllist = rootDirectory.GetFlatten();

foreach (var dir in rootDirectory.GetFlatten().OrderBy(d => d.GetSize()))
{
    Console.WriteLine($"{dir.Name} ({dir.GetSize()})");
}

var sumUnder100000 = fllist.Where(d => d.GetSize() <= 100000).Sum(d => d.GetSize());

Console.WriteLine(sumUnder100000);
