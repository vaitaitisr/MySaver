using MySaver.Models;
using MySaver.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MySaver.ViewModels;

public class ListEditorViewModel : INotifyPropertyChanged, IQueryAttributable
{
    private IWebService webService;
    private IProductListService listService;
    private Lazy<Task<List<Product>>> ProductList;

    public event PropertyChangedEventHandler PropertyChanged;

    public ObservableCollection<Product> SelectedProducts { get; }
        = new ObservableCollection<Product>();

    private string currentListName;
    private string _listName = "Titulas";
    public string ListName
    {
        get { return _listName; }
        set { _listName = value; OnPropertyChanged(); }
    }

    public ListEditorViewModel(IWebService webService, IProductListService listService)
    {
        this.webService = webService;
        this.listService = listService;

        ProductList = new Lazy<Task<List<Product>>>(() =>
            webService.GetObjectListAsync<Product>());
    }

    public async Task<List<Product>> GetSearchResultsAsync(string input)
    {
        input = input.ToLower();

        var result = await ProductList.Value;

        if (result != null)
        {
            var resultQuery = result.Where(product => product.Name.ToLower().Contains(input))
                .GroupBy(product => product.Name, (key, g) => g.OrderBy(e => e.UnitPrice).FirstOrDefault())
                .ToList();
            return resultQuery;
        }

        return null;
    }

    public List<Product> ReadList()
    {
        return listService.OpenList(currentListName);
    }

    public void SaveList()
    {
        listService.SaveList(currentListName, SelectedProducts);
    }

    public void RenameList()
    {
        listService.RenameList(currentListName, ListName);
        currentListName = ListName;
    }

    public void AddProduct(Product product)
    {
        if (product != null)
        {
            if (!SelectedProducts.Contains(product))
            {
                SelectedProducts.Add(product);
            }
        }
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
        if (query.ContainsKey("ListName"))
        {
            ListName = query["ListName"] as string;
        }
        InitStartList();
    }

    private void InitStartList()
    {
        currentListName = ListName;

        if (listService.ListExists(currentListName))
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
            listService.SaveList(currentListName, new List<Product>());
        }
    }

    public float CalculateTotal()
    {
        float totalPrice = 0;
        foreach (var product in SelectedProducts)
        {
            totalPrice += product.Amount * product.UnitPrice;
        }
        return totalPrice;
    }
    public async Task UpdateStorePrices(CollectionView col)
    {
        var allProducts = await ProductList.Value;

        // Finds all relevant store names
        List<string> storeNames = (
            from product in SelectedProducts
            join compareProduct in allProducts on product.Name equals compareProduct.Name
            select compareProduct.StoreName).Distinct().ToList();

        List<string> resultList = new List<string>();
        foreach (var storeName in storeNames)
        {
            // Finds the prices of all products in a given store name
            var productPrices =
                from product in SelectedProducts
                join compareProduct in allProducts on product.Name equals compareProduct.Name
                where compareProduct.StoreName == storeName
                select compareProduct.UnitPrice * product.Amount;

            string result = storeName + ": " + productPrices.Sum().ToString("0.00") + '€';
            if (SelectedProducts.Count() != productPrices.Count())
            {
                result += " (trūksta produktų)";
            }
            resultList.Add(result);
        }
        col.ItemsSource = resultList;
    }
}
