using LocalDropshipping.Web.Data.Entities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace LocalDropshipping.Web.Models
{
    public class ProfileVerificationViewModel
    {
        [Required(ErrorMessage = "Store Name is required.")]
        [StringLength(100, ErrorMessage = "Store Name cannot be longer than 100 characters.")]
        public string StoreName { get; set; }

        [StringLength(100, ErrorMessage = "Store URL cannot be longer than 100 characters.")]
        public string StoreURL { get; set; }

        [Required(ErrorMessage = "Bank Name is required.")]
        [StringLength(100, ErrorMessage = "Bank Name cannot be longer than 100 characters.")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "Bank Account Title is required.")]
        [StringLength(100, ErrorMessage = "Bank Account Title cannot be longer than 100 characters.")]
        public string BankAccountTitle { get; set; }

        [Required(ErrorMessage = "Bank Account Number/IBAN is required.")]
        [StringLength(100, ErrorMessage = "Bank Account Number/IBAN cannot be longer than 100 characters.")]
        public string BankAccountNumberOrIBAN { get; set; }

        [StringLength(250, ErrorMessage = "Bank Branch cannot be longer than 250 characters.")]
        public string BankBranch { get; set; }

        [StringLength(250, ErrorMessage = "Address cannot be longer than 250 characters.")]
        public string Address { get; set; }


		internal Profiles ToEntity()
		{
			return JsonConvert.DeserializeObject<Profiles>(JsonConvert.SerializeObject(this))!;
		}
	}
}
