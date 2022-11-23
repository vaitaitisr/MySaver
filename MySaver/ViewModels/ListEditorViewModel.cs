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

    private string _listName = "Titulas";
    public string ListName
    {
        get { return _listName; }
        set { _listName = value; OnPropertyChanged(); }
    }

    public string CurrentListName { get; set; }

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
            var resultQuery =
               (from product in result
                where product.Name.ToLower().Contains(input)
                select product).ToList();

            return resultQuery;
        }

        return null;
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
        CurrentListName = ListName;

        if (listService.ListExists(ListName))
        {
            var tempList = listService.OpenList(ListName);
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
            listService.SaveList(ListName, new List<Product>());
        }
    }
}
