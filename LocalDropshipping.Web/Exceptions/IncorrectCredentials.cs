namespace LocalDropshipping.Web.Exceptions
{
    public class InvalidCredentialException : IdentityException
    {
        public InvalidCredentialException() : base ("Incorrect email or password.")
        {
            
        }
    }
}
