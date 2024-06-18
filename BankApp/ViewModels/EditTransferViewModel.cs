using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BankApp.Models;
using BankApp.Services;

namespace BankApp.ViewModels
{
    public partial class EditTransferViewModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        private Transfer initialItem;

        [ObservableProperty]
        private string transfer_to;

        [ObservableProperty]
        private System.Int32 ammount = 0;

        [ObservableProperty]
        private string pageHeader;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count > 0 && query["transfer"] != null) // we're editing an Item
            {
                InitialItem = query["transfer"] as Transfer;
                Transfer_to = InitialItem.Transfer_to;
                PageHeader = $"Modify Transfer {InitialItem.Id_Transfer}";
            }
            else // we're creating a new transfer
            {
                transfer_to = "";
                PageHeader = "Create a New Transfer";
            }
        }

        [RelayCommand]
        public async Task SaveItem()
        {
            var realm = RealmService.GetMainThreadRealm();
            await realm.WriteAsync(() =>
            {
                if (InitialItem != null) // editing an item
                {
                    InitialItem.Transfer_to = transfer_to;
                }
                else // creating a new item
                {
                    realm.Add(new Transfer()
                    {
                        OwnerId = RealmService.CurrentUser.Id,
                        Transfer_to = Transfer_to,
                        Ammount = ammount
                    });
                }
            });           
            
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
