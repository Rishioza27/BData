using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(opt =>
{
	opt.CustomSchemaIds(type => type.ToString());
	opt.AddSecurityDefinition(
		"Bearer",
		new OpenApiSecurityScheme
		{
			In = ParameterLocation.Header,
			Description = "Please enter JWT with Bearer into field",
			Name = "Authorization",
			Type = SecuritySchemeType.ApiKey,
		}
	);
	opt.AddSecurityRequirement(
		new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
				},
				new string[] { }
			},
		}
	);
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
	"Freezing",
	"Bracing",
	"Chilly",
	"Cool",
	"Mild",
	"Warm",
	"Balmy",
	"Hot",
	"Sweltering",
	"Scorching",
};

app.MapGet(
		"/weatherforecast",
		() =>
		{
			var forecast = Enumerable
				.Range(1, 5)
				.Select(index => new WeatherForecast(
					DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
					Random.Shared.Next(-20, 55),
					summaries[Random.Shared.Next(summaries.Length)]
				))
				.ToArray();
			return forecast;
		}
	)
	.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
