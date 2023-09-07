using System.ComponentModel.DataAnnotations;

namespace LocalDropshipping.Web.Models
{
    public class NewPasswordViewModel
    {
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
