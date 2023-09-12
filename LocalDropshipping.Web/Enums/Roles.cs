namespace LocalDropshipping.Web.Enums
{
    [Flags]
    public enum Roles
    {
        SuperAdmin = 0b_0000_0001,
        Admin = 0b_0000_0010,
        Seller = 0b_0000_0100,
    }
}
