using System.ComponentModel.DataAnnotations;

namespace LocalDropshipping.Web.Models
{
    public class CustomerDetailsViewModel
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int SellingPrice { get; set; }
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
