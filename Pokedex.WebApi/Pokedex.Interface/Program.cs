using JewelArchitecture.Core.Interface.Extensions;
using Pokedex.Interface.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddJewelArchitecture();
builder.Services.AddPokedex();

// Add external API clients to get Pokemon's data and related fun translations.
var pokedexApiConfig = builder.Configuration.GetSection("Pokeapi");
builder.Services.AddPokeapiClient(pokedexApiConfig);

var funTranslationsApiConfig = builder.Configuration.GetSection("FuntranslationsApi");
builder.Services.AddFuntranslationsClient(funTranslationsApiConfig);

var retryPolicyConfig = builder.Configuration.GetSection("RetryPolicy");
builder.Services.AddRetryPolicyOptions(retryPolicyConfig);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
