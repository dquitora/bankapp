using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BankApp.Models;
using BankApp.Services;
using Realms;

namespace BankApp.ViewModels
{
    public partial class DebitPaymentViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string connectionStatusIcon = "wifi_on.png";

        [ObservableProperty]
        private bool isShowAllTasks;

        [ObservableProperty]
        private IQueryable<DebitPayment> debitPayments;

        [ObservableProperty]
        public string dataExplorerLink = RealmService.DataExplorerLink;

        private Realm realm;
        private string currentUserId;
        private bool isOnline = true;

        [RelayCommand]
        public void OnAppearing()
        {
            realm = RealmService.GetMainThreadRealm();
            currentUserId = RealmService.CurrentUser.Id;
            DebitPayments = realm.All<DebitPayment>().OrderBy(i => i.Id_Debit_Payment);

            var currentSubscriptionType = RealmService.GetCurrentSubscriptionType(realm);
            IsShowAllTasks = currentSubscriptionType == SubscriptionType.All;
        }

        [RelayCommand]
        public async Task Logout()
        {
            IsBusy = true;
            await RealmService.LogoutAsync();
            IsBusy = false;

            await Shell.Current.GoToAsync($"//login");
        }

        [RelayCommand]
        public async Task AddItem()
        {
            await Shell.Current.GoToAsync($"debitpaymentEdit");
        }

        [RelayCommand]
        public async Task EditItem(DebitPayment debitPayment)
        {
            if (!await CheckItemOwnership(debitPayment))
            {
                return;
            }
            var debitPaymentParameter = new Dictionary<string, object>() { { "debitPayment", debitPayment } };
            await Shell.Current.GoToAsync($"itemEdit", debitPaymentParameter);
        }

        [RelayCommand]
        public async Task DeleteItem(DebitPayment debitPayment)
        {
            if (!await CheckItemOwnership(debitPayment))
            {
                return;
            }

            await realm.WriteAsync(() =>
            {
                realm.Remove(debitPayment);
            });
        }

        [RelayCommand]
        public void ChangeConnectionStatus()
        {
            isOnline = !isOnline;

            if (isOnline)
            {
                realm.SyncSession.Start();
            }
            else
            {
                realm.SyncSession.Stop();
            }

            ConnectionStatusIcon = isOnline ? "wifi_on.png" : "wifi_off.png";
        }

        [RelayCommand]
        public async Task UrlTap(string url)
        {
            await Launcher.OpenAsync(DataExplorerLink);
        }

        private async Task<bool> CheckItemOwnership(DebitPayment debitPayment)
        {
            if (!debitPayment.IsMine)
            {
                await DialogService.ShowAlertAsync("Error", "You cannot modify items not belonging to you", "OK");
                return false;
            }

            return true;
        }

        async partial void OnIsShowAllTasksChanged(bool value)
        {
            await RealmService.SetSubscription(realm, value ? SubscriptionType.All : SubscriptionType.Mine);

            if (!isOnline)
            {
                await DialogService.ShowToast("Switching subscriptions does not affect Realm data when the sync is offline.");
            }
        }
    }
}
