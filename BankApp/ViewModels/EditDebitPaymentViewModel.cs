using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BankApp.Models;
using BankApp.Services;

namespace BankApp.ViewModels
{
    public partial class EditDebitPaymentViewModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        private DebitPayment? initialDebitPayment;

        [ObservableProperty]
        private string debit_payment_to;

        [ObservableProperty]
        private long ammount_debit;

        [ObservableProperty]
        private string? pageHeader;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count > 0 && query["debitPayment"] != null) // we're editing an Item
            {
                initialDebitPayment = query["debitPayment"] as DebitPayment;
                Debit_payment_to = initialDebitPayment.Debit_Payment_to;
                PageHeader = $"Modify Debit Payment {initialDebitPayment.Id_Debit_Payment}";
            }
            else // we're creating a new item
            {
                Debit_payment_to = "";
                PageHeader = "Create a New Debit Payment";
            }
        }

        [RelayCommand]
        public async Task SaveItem()
        {
            var realm = RealmService.GetMainThreadRealm();
            await realm.WriteAsync(() =>
            {
                if (initialDebitPayment != null) // editing an item
                {
                    initialDebitPayment.Debit_Payment_to = Debit_payment_to;
                }
                else // creating a new item
                {
                    realm.Add(new DebitPayment()
                    {
                        OwnerId = RealmService.CurrentUser.Id,
                        Debit_Payment_to = debit_payment_to,
                        Ammount_Debit = ammount_debit
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
