using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LocalDropshipping.Web.Data.Entities
{
    public class User : IdentityUser
    {
        public string Fullname { get; set; }
        public bool IsSeller { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
