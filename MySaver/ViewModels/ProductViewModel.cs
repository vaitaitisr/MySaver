using MySaver.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace MySaver.ViewModels;

public class ProductViewModel
{
    private Lazy<Task<List<Product>>> ProductList =
        new Lazy<Task<List<Product>>>(() => ReadDataFileAsync());

    public ObservableCollection<Product> SelectedProducts { get; }
        = new ObservableCollection<Product>();

    private string mainDir;
    private string targetFile;
    public string ListName { get; set; }

    public ProductViewModel(string inputName)
    {
        mainDir = FileSystem.Current.AppDataDirectory;
        targetFile = Path.Combine(mainDir, inputName + ".json");

        if (File.Exists(targetFile))
        {
            var tempList = ReadList();
            if (tempList != null)
            {
                foreach (var item in tempList)
                {
                    SelectedProducts.Add(item);
                }
            }
        }
        else
        {
            File.Create(targetFile).Close();
        }

        ListName = inputName;
    }

    private static async Task<List<Product>> ReadDataFileAsync()
    {
        using Stream inputFileStream = await FileSystem.OpenAppPackageFileAsync("ProductList.json");
        using StreamReader reader = new StreamReader(inputFileStream);

        var data = await reader.ReadToEndAsync();

        try
        {
            var tempList = JsonSerializer.Deserialize<List<Product>>(data);
            return tempList;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<Product>> GetSearchResultsAsync(string input)
    {
        input = input.ToLower();

        var resultQuery =
            (from product in await ProductList.Value
             where product.Name.ToLower().Contains(input)
             select product).ToList();

        return resultQuery;
    }

    public async Task<List<Product>> ReadListAsync()
    {
        using Stream inputFileStream = File.OpenRead(targetFile);
        using StreamReader reader = new StreamReader(inputFileStream);

        var data = await reader.ReadToEndAsync();

        return JsonSerializer.Deserialize<List<Product>>(data);
    }

    public List<Product> ReadList()
    {
        using Stream inputFileStream = File.OpenRead(targetFile);
        using StreamReader reader = new StreamReader(inputFileStream);

        var data = reader.ReadToEnd();
        if (data == "")
        {
            return null;
        }
        return JsonSerializer.Deserialize<List<Product>>(data);
    }

    public void AddSelection(Product selectedProduct)
    {
        if (selectedProduct != null)
        {
            if (!SelectedProducts.Contains(selectedProduct))
            {
                SelectedProducts.Add(selectedProduct);
            }
        }
    }

    public async void SaveFile()
    {
        File.WriteAllText(targetFile, JsonSerializer.Serialize
            <ObservableCollection<Product>>(SelectedProducts));
    }

    public async void RenameFile(string renamedFile)
    {
        File.Delete(renamedFile);
        File.Move(targetFile, renamedFile);

        targetFile = renamedFile;
    }

    public void RemoveProduct(Product product)
    {
        SelectedProducts.Remove(product);
    }
}
