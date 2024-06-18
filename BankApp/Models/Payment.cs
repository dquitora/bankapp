using MongoDB.Bson;
using Realms;
using BankApp.Services;

namespace BankApp.Models
{
    public partial class Payment : IRealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id_Payment { get; set; } = ObjectId.GenerateNewId();

        [MapTo("owner_id")]        
        public string OwnerId { get; set; }

        [MapTo("payment_to")]        
        public string Payment_to { get; set; }

        [MapTo("ammount_payment")]
        public long AmmountPayment { get; set; }

        [MapTo("isComplete")]
        public bool IsComplete { get; set; }

        public bool IsMine => OwnerId == RealmService.CurrentUser.Id;
    }
}
