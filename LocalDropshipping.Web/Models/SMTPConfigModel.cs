namespace LocalDropshipping.Web.Models
{
	public class SMTPConfigModel
	{
		public bool IsBodyHTML { get; set; }
		public string From { get; set; }
		public string SmtpServer { get; set; }
		public int Port { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
	}
}
