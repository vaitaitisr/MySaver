using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MySaver.Models;

public class Product : INotifyPropertyChanged //Should probably implement IEquatable for better comparison functionality
{
    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] String name = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public string StoreName { get; set; }

    public string Name { get; set; }

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

    public float Price
    {
        get
        {
            return UnitPrice * Amount;
        }
    }
}
