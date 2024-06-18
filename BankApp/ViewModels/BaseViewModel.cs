using CommunityToolkit.Mvvm.ComponentModel;

namespace BankApp.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        protected bool isBusy;

        protected Action currentDismissAction;

        partial void OnIsBusyChanged(bool value)
        {
            if (value)
            {
                currentDismissAction = Services.DialogService.ShowActivityIndicator();
            }
            else
            {
                currentDismissAction?.Invoke();
                currentDismissAction = null;
            }
        }
    }
}
