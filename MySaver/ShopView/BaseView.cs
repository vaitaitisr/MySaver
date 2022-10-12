using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace MySaver.ShopView
{
    public partial class BaseView : ObservableObject
    {
        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(IsNotBusy))]
        bool isBusy;

        public bool IsNotBusy => !IsBusy;
    }
}
