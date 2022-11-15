using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MySaver.Models;

public class Product : INotifyPropertyChanged, IEquatable<Product>
{
    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] String name = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public string StoreName { get; set; } = String.Empty;

    public string Name { get; set; } = String.Empty;

    private float _amount = 1;
    public float Amount
    {
        get { return _amount; }

        set
        {
            _amount = value;
            OnPropertyChanged();
            OnPropertyChanged("Price");
        }
    }

    public float UnitPrice { get; set; }

    public string Price
    {
        get
        {
            string result = (UnitPrice * Amount).ToString("0.00");
            result += '€';
            return result;
        }
    }
    public bool Equals(Product other)
    {
        return (StoreName?.Equals(other.StoreName) ?? false) &&
               (Name?.Equals(other.Name) ?? false);
    }
}
