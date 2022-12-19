using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using MySaver.Models;

namespace MySaver.Services;

[ExcludeFromCodeCoverage]
public class ProductListService : IProductListService
{
    private string appDirectory;
    public ProductListService()
    {
        appDirectory = FileSystem.Current.AppDataDirectory;
    }

    public List<Product> OpenList(string listName)
    {
        var path = FilePath(listName);

        using Stream inputFileStream = File.OpenRead(path);
        using StreamReader reader = new StreamReader(inputFileStream);

        var data = reader.ReadToEnd();

        if (data == "")
        {
            return null;
        }

        return JsonSerializer.Deserialize<List<Product>>(data);
    }

    public void SaveList(string listName, IEnumerable<Product> list)
    {
        var path = FilePath(listName);
        var contents = JsonSerializer.Serialize(list);
        File.WriteAllText(path, contents);
    }

    public void RenameList(string oldName, string newName)
    {
        var oldPath = FilePath(oldName);
        var newPath = FilePath(newName);
        File.Move(oldPath, newPath, true);
    }

    public void DeleteList(string listName)
    {
        var path = FilePath(listName);
        File.Delete(path);
    }

    public bool ListExists(string listName)
    {
        var path = FilePath(listName);
        return File.Exists(path);
    }

    public IEnumerable<string> GetListNames()
    {
        var paths = Directory.GetFiles(appDirectory);

        var names =
            from name in paths
            select Path.GetFileNameWithoutExtension(name);

        return names;
    }

    private string FilePath(string listName)
    {
        return Path.Combine(appDirectory, listName + ".json");
    }
}
