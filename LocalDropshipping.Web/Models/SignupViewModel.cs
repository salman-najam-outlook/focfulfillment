using System.ComponentModel.DataAnnotations;

namespace LocalDropshipping.Web.Models
{
    public class SignupViewModel
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        //[DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
