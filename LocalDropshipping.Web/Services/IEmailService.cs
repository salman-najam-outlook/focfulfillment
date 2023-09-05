using LocalDropshipping.Web.Models;

namespace LocalDropshipping.Web.Services
{
	public interface IEmailService
	{
		Task SendEmail(EmailMessage userEmailOptions);
	}
}