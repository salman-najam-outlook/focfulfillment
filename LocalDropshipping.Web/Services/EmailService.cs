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
		private const string _templateBasePath = @"EmailTemplate/{0}.html";
		private readonly SMTPConfigModel _smtpconfig;

		public EmailService(IOptions<SMTPConfigModel> smtpconfig)
		{
			_smtpconfig = smtpconfig.Value;
		}

		public async Task SendEmail(EmailMessage userEmailOptions)
		{
			var mailMessage = new MimeMessage();
			mailMessage.From.Add(MailboxAddress.Parse(_smtpconfig.From));
			
			mailMessage.To.Add(MailboxAddress.Parse(userEmailOptions.ToEmail));
			mailMessage.Subject =  userEmailOptions.Subject;

			var template = GetTemplate(userEmailOptions.TemplatePath);
			mailMessage.Body = new TextPart(TextFormat.Html) { Text = UpdatePlaceHolders(template, userEmailOptions.Placeholders) };


			using var smtp = new SmtpClient();
            smtp.CheckCertificateRevocation = false;

            smtp.Connect(_smtpconfig.SmtpServer, _smtpconfig.Port, SecureSocketOptions.Auto);
			
			smtp.Authenticate(_smtpconfig.From, _smtpconfig.Password);
			await smtp.SendAsync(mailMessage)
;
			smtp.Disconnect(true);
		}

		// GetTemplate
		private string GetTemplate(string templateName)
		
		{
			var template = File.ReadAllText(string.Format(_templateBasePath, templateName));
			return template;
		}

		// UpdatePlaceHolder
		private string UpdatePlaceHolders(string template, List<KeyValuePair<String,String>> keyValuePairs)
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
