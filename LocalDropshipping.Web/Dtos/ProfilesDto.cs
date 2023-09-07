using LocalDropshipping.Web.Data.Entities;
using Newtonsoft.Json;

namespace LocalDropshipping.Web.Dtos
{
    public class ProfilesDto
    {
        public string StoreName { get; set; }
        public string StoreURL { get; set; }
        public string BankName { get; set; } 
        public string BankAccountTitle { get; set;}
        public string BankAccountNumberOrIBAN { get; set; }
        public string BankBranch { get; set; }
        public string Address { get; set; }


        public string Userid { get; set; }
        internal Profiles ToEntity()
        {
            return JsonConvert.DeserializeObject<Profiles>(JsonConvert.SerializeObject(this))!;
        }
    }
}
