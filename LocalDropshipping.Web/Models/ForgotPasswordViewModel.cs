using System.ComponentModel.DataAnnotations;

namespace LocalDropshipping.Web.Models
{
    public class ForgotPasswordViewModel
    {

        [Required]
        [Display(Name = "Email address")]
        [EmailAddress]
        public string Email { get; set; }

    }
}
