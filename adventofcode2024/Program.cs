using adventofcode2024;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets("1ebb9f98-e28d-4f79-830f-a906291da08b");

var configurationRoot = builder.Build();

var token = configurationRoot.GetSection("ApiKey").Value;

HttpClient client = new()
{
    BaseAddress = new Uri("https://adventofcode.com")
};

client.DefaultRequestHeaders.Add("Cookie", token);

await Utils.GenerateDayInputs(client);

await Run.RunDays(false);