using MongoDB.Bson;
using Realms;
using BankApp.Services;

namespace BankApp.Models
{
    public partial class DebitPayment : IRealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id_Debit_Payment { get; set; } = ObjectId.GenerateNewId();

        [MapTo("owner_id")]
        public string OwnerId { get; set; }

        [MapTo("debit_payment_to")]
        public string Debit_Payment_to { get; set; }

        [MapTo("ammount_debit")]
        public long Ammount_Debit { get; set; }

        [MapTo("isComplete")]
        public bool IsComplete { get; set; }

        public bool IsMine => OwnerId == RealmService.CurrentUser.Id;
    }
}
