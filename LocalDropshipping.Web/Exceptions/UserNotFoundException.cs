namespace LocalDropshipping.Web.Exceptions
{
    public class UserNotFoundException : IdentityException
    {
        public UserNotFoundException() : base("User Not Found")
        {
            
        }
        public UserNotFoundException(string message): base(message)
        { }
    }
}
