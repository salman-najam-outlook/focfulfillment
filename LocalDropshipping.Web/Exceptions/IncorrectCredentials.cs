namespace LocalDropshipping.Web.Exceptions
{
    public class InvalidCredentialException : Exception
    {
        public InvalidCredentialException() : base ("Incorrect email or password.")
        {
            
        }
    }
}
