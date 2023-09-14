using LocalDropshipping.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace LocalDropshipping.Web.Models
{
    public class CheckoutViewModel
    {
        public List<OrderItem> Cart { get; set; }

        [Required]
        //[Range(0, int.MaxValue)]
        public string SellingPrice { get; set; }
        [Required]
        public string? FullName { get; set; }

        [Required]
        public string? PrimaryPhoneNumber { get; set; }

        public string SecondaryPhoneNumber { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public string? City { get; set; }
    }
}
