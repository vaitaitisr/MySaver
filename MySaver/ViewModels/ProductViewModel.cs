using MySaver.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace MySaver.ViewModels;

public delegate void GetProductDelegate(Product name);

public class ProductViewModel
{
    private List<Product> ProductList = new List<Product>();
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
        ReadDataFileAsync();

        ListName = inputName;
    }

    private async Task ReadDataFileAsync()
    {
        using Stream inputFileStream = await FileSystem.OpenAppPackageFileAsync("ProductList.json");
        using StreamReader reader = new StreamReader(inputFileStream);

        var data = await reader.ReadToEndAsync();

        try
        {
            var tempList = JsonSerializer.Deserialize<List<Product>>(data);
            foreach (var product in tempList)
            {
                ProductList.Add(product);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<Product>> GetSearchResultsAsync(string input)
    {
        if (input == null) return null;

        input = input.ToLower();

        var resultQuery =
            (from product in ProductList
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
