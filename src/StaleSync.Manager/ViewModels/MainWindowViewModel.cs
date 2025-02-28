using CommunityToolkit.Mvvm.ComponentModel;

namespace StaleSync.Manager.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty] private string _greeting;
    }
}