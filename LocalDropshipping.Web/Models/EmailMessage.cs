namespace LocalDropshipping.Web.Models
{
	public class EmailMessage
	{
		public string ToEmail { get; set; }
		public string Subject { get; set; }
		public string TemplatePath { get; set; }

		public List<KeyValuePair<string,string>> Placeholders { get; set; }

	}
}
