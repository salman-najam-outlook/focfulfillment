using LocalDropshipping.Web.Data.Entities;
using Newtonsoft.Json;

namespace LocalDropshipping.Web.Dtos
{
    public class WithdrawlsDto
    {
        public int AmountInPkr { get; set; }
        public string AccountTitle { get; set; }
        public string AccountNumber { get; set; }

        internal Withdrawals ToEntity()
        {
            return JsonConvert.DeserializeObject<Withdrawals>(JsonConvert.SerializeObject(this))!;
        }
    }
}
