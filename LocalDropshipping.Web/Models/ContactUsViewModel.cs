using System.ComponentModel.DataAnnotations;

namespace LocalDropshipping.Web.Models
{
    public class ContactUsViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Email address")]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Message")]
        [StringLength(500)]
        public string Message { get; set; }

    }
}
