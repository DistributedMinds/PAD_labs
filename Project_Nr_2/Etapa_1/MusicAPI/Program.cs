using Common.Models;
using Microsoft.Extensions.Options;
using MusicAPI.Repositories;
using MusicAPI.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<IMongoDbSettings>(provider =>
    provider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

builder.Services.AddScoped<IMongoRepository<Song>, MongoRepository<Song>>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

