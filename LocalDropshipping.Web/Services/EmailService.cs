using System.IO;
using System.Net;
using System.Text;
using LocalDropshipping.Web.Models;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace LocalDropshipping.Web.Services
{
	public class EmailService : IEmailService
	{
		private const string templatepath = @"EmailTemplate/{0}.html";
		private readonly SMTPConfigModel _smtpconfig;

		public EmailService(IOptions<SMTPConfigModel> smtpconfig)
		{
			_smtpconfig = smtpconfig.Value;
		}

		public async Task SendTestEmail(UserEmailOptions userEmailOptions)
		{
			userEmailOptions.Subject = UpdatePlaceHolders( "Hello {{UserName}},This is test email",userEmailOptions.Placeholders);
			userEmailOptions.TemplatePath = UpdatePlaceHolders( GetTemplate("TestEmail"),userEmailOptions.Placeholders);
			await SendEmail(userEmailOptions);
		}

		public async Task SendEmail(UserEmailOptions userEmailOptions)
		{
			var email = new MimeMessage();
			email.From.Add(MailboxAddress.Parse(_smtpconfig.From));
			email.To.Add(MailboxAddress.Parse(userEmailOptions.ToEmail));
			email.Subject = userEmailOptions.Subject;


			email.Body = new TextPart(TextFormat.Html) { Text = userEmailOptions.TemplatePath };

			using var smtp = new SmtpClient();
			smtp.Connect(_smtpconfig.SmtpServer, _smtpconfig.Port, SecureSocketOptions.StartTls);
			
			smtp.Authenticate(_smtpconfig.From, _smtpconfig.Password);
			await smtp.SendAsync(email)
;
			smtp.Disconnect(true);
		}
		// GetTemplate

		private string GetTemplate(string templateName)
		
		{
			var template = File.ReadAllText(string.Format(templatepath, templateName));
			return template;
		}
		// UpdatePlaceHolder

		private string UpdatePlaceHolders(string template ,List<KeyValuePair<String,String>> keyValuePairs)
		{
			if (!string.IsNullOrEmpty(template) && keyValuePairs != null)
			{
				foreach (var placeholder in keyValuePairs) 
				{
					if (template.Contains(placeholder.Key))
					{
						template = template.Replace(placeholder.Key, placeholder.Value);

					}
				}
			}
			return template;
		}  

	}
}
