using System.ComponentModel.DataAnnotations;

namespace LocalDropshipping.Web.Models
{
    public class AdminLoginViewModel
    {
        [Required]
        [Display(Name = "Email address")]
        [EmailAddress]

        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
