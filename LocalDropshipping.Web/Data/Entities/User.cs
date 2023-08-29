using System.ComponentModel.DataAnnotations;

namespace LocalDropshipping.Web.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
    }
}
