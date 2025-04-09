using System.Text.Json;
using Microsoft.Extensions.Options;
using Wardrobe.Abstractions.Communication;
using Wardrobe.Configuration.Settings;

namespace Wardrobe.Implemntations.Communication
{
	public class BrevoEmailService : IEmailService
	{
		private readonly BrevoSettings _brevoSettings;

		public BrevoEmailService(IOptions<BrevoSettings> options)
		{
			_brevoSettings = options.Value;
		}

		public async Task Send(string Name, string Email, string Subject, string HtmlMessage)
		{
			try
			{
				var ApiKey = _brevoSettings.ApiKey;
				var client = new HttpClient();
				var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email");
				request.Headers.Add("accept", "application/json");
				request.Headers.Add("api-key", ApiKey);
				// Payload as an object
				var Payload = new
				{
					sender = new { name = "The Killer Muse", email = "thekillermuseapp@gmail.com" },
					to = new[] { new { Email, Name } },
					subject = Subject,
					htmlContent = HtmlMessage,
				};
				// Serialize payload to JSON
				var JsonPayload = JsonSerializer.Serialize(Payload);
				request.Content = new StringContent(JsonPayload, null, "application/json");

				var response = await client.SendAsync(request);
				if (response.IsSuccessStatusCode)
				{
					// TODO: Logs
				}
				else
				{
					// TODO: Logs
				}
			}
			catch (Exception)
			{
				// TODO: Logs
			}
		}
	}
}
