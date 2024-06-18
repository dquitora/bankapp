using BankApp.Views;

namespace BankApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("itemEdit", typeof(EditItemPage));
            Routing.RegisterRoute("transferEdit", typeof(EditTransferPage));
            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("paymentEdit", typeof(EditPaymentPage));
            Routing.RegisterRoute("debitpaymentEdit", typeof(EditDebitPaymentPage));
        }
    }
}
