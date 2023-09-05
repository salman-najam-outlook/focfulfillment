using LocalDropshipping.Web.Models;

namespace LocalDropshipping.Web.Services
{
	public interface IEmailService
	{
		Task SendTestEmail(UserEmailOptions userEmailOptions);
	}
}