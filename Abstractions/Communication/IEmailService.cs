namespace Wardrobe.Abstractions.Communication
{
	public interface IEmailService
	{
		Task Send(string Name, string Email, string Subject, string HtmlMessage);
	}
}
