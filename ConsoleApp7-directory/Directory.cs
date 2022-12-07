using System.Text;

namespace ConsoleApp7_directory;

public class Directory
{
    public Directory()
    {
        this.Directories = new List<Directory>();
        this.Files = new List<File>();
    }
    
    public string Name { get; set; }
    
    public Directory Parent { get; set; }

    public IList<Directory> Directories { get; set; }
    
    public IList<File> Files { get; set; }

    public int GetLevel()
    {
        if (this.Parent == null)
        {
            return 0;
        }
        else
        {
            return this.Parent.GetLevel() + 1;
        }
    }

    public string GetIndent()
    {
        var lng = this.GetLevel();
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < lng; i++)
        {
            sb.Append(" ");
        }

        return sb.ToString();
    }

    public int GetSize()
    {
        return this.Files.Sum(x => x.Size) + this.Directories.Sum(x => x.GetSize());
    }

    public IList<Directory> GetFlatten()
    {
        var result = new List<Directory>();
        foreach (var directory in this.Directories)
        {
            result.AddRange(directory.GetFlatten());
        }

        result.Add(this);
        
        return result;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(this.GetIndent());
        sb.AppendLine($"[{this.Name}] ({this.GetSize()})");
        
        foreach (var directory in this.Directories)
        {
            sb.Append(directory);
        }

        foreach (var file in this.Files)
        {
            sb.Append(this.GetIndent());
            sb.AppendLine($"| {file.Name} ({file.Size})");
        }
        
        return sb.ToString();
    }
}