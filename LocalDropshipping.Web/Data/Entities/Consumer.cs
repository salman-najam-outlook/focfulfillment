namespace LocalDropshipping.Web.Data.Entities
{
    public class Consumer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PrimaryPhoneNumber { get; set; }
        public string SecondaryPhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public bool IsBlocked { get; set; } = false;
        public string SellerEmail { get; set; }
        public int OrderId { get; set; }    
    }
}
