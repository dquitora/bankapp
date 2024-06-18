using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BankApp.Models;
using BankApp.Services;

namespace BankApp.ViewModels
{
    public partial class EditPaymentViewModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        private Payment initialPayment;

        [ObservableProperty]
        private string payment_to;

        [ObservableProperty]
        private long ammount_payment;

        [ObservableProperty]
        private string pageHeader;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count > 0 && query["item"] != null) // we're editing an Item
            {
                initialPayment = query["payment"] as Payment;
                Payment_to = initialPayment.Payment_to;
                Ammount_payment = initialPayment.AmmountPayment;
                PageHeader = $"Modify Item {initialPayment.Id_Payment}";
            }
            else // we're creating a new item
            {
                Payment_to = "";
                PageHeader = "Create New Payment";
            }
        }

        [RelayCommand]
        public async Task SavePayment()
        {
            var realm = RealmService.GetMainThreadRealm();
            await realm.WriteAsync(() =>
            {
                if (initialPayment != null) // editing an item
                {
                    initialPayment.Payment_to = Payment_to;
                    initialPayment.AmmountPayment = Ammount_payment;
                }
                else // creating a new item
                {
                    realm.Add(new Payment()
                    {
                        OwnerId = RealmService.CurrentUser.Id,
                        Payment_to = payment_to,
                        AmmountPayment = ammount_payment
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
