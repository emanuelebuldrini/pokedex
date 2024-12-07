using JewelArchitecture.Core.Interface.Extensions;
using Pokedex.Interface.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddJewelArchitecture();
var pokedexApiConfig = builder.Configuration.GetSection("PokedexApi");
var funTranslationsApiConfig = builder.Configuration.GetSection("FunTranslationsApi");
builder.Services.AddPokedex(pokedexApiConfig, funTranslationsApiConfig);

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
