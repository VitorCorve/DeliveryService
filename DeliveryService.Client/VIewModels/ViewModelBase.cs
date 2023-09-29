using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DeliveryService.Client.VIewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected bool Set<T>(ref T param, T value, string? property = null)
        {
            if (Equals(param, value))
                return false;
            else
            {
                param = value;
                OnPropertyChanged(property);
                return true;
            }
        }
    }
}
