using MongoDB.Bson;
using Realms;
using BankApp.Services;

namespace BankApp.Models
{
    public partial class Transfer : IRealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id_Transfer { get; set; } = ObjectId.GenerateNewId();

        [MapTo("owner_id")]        
        public string OwnerId { get; set; }

        [MapTo("transfer_to")]        
        public string Transfer_to { get; set; }
            
        [MapTo("ammount")]        
        public int Ammount { get; set; }

        [MapTo("isComplete")]
        public bool IsComplete { get; set; }

        public bool IsMine => OwnerId == RealmService.CurrentUser.Id;
    }
}
