using Microsoft.EntityFrameworkCore;

namespace BData.Model
{
	[Index(nameof(Username), IsUnique = true)]
	public class User() : BaseModel
	{
		public required string Name { get; set; }
		public required string Username { get; set; }
		public required string Email { get; set; }
		public required string Phone { get; set; }
	}
}
