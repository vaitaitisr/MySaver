using MySaver.Models;
using MySaver.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace MySaver.ViewModels;

//[QueryProperty(nameof(ListName), "ListName")]
public class ProductViewModel : INotifyPropertyChanged, IQueryAttributable
{
    private WebService webService;
    private Lazy<Task<List<Product>>> ProductList;

    public event PropertyChangedEventHandler PropertyChanged;

    public ObservableCollection<Product> SelectedProducts { get; }
        = new ObservableCollection<Product>();

    private string mainDir;
    private string targetFile;
    private string _listName = "Titulas";
    public string ListName
    {
        get { return _listName; }
        set { _listName = value; OnPropertyChanged(); }
    }


    //public ProductViewModel(ProductService productService)
    public ProductViewModel(WebService webService)
    {
        this.webService = webService;
        ProductList = new Lazy<Task<List<Product>>>(() =>
            webService.GetObjectListAsync<Product>());

        mainDir = FileSystem.Current.AppDataDirectory;
        // BUG: targetFile is always "Titulas" since it only changes after the constructor finishes.
        // so it always reads Titulas.json
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

        var result = await ProductList.Value;

        if(result != null)
        {
             var resultQuery =
                (from product in result
             where product.Name.ToLower().Contains(input)
             select product).ToList();

            return resultQuery;
        }

        return null;
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
        //File.Delete(renamedFile);
        File.Move(targetFile, renamedFile, true);

        targetFile = renamedFile;
    }

    public void RemoveProduct(Product product)
    {
        SelectedProducts.Remove(product);
    }

    public void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        
        if(query.ContainsKey("ListName"))
        {
            ListName = query["ListName"] as string;
        }
        InitStartList();
    }
    private void InitStartList()
    {
        targetFile = Path.Combine(mainDir, ListName + ".json");

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
    }
}
