using Microsoft.Extensions.Options;
using SyncNode.Services;
using SyncNode.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.Configure<MusicAPISettings>(builder.Configuration.GetSection("MusicAPISettings"));

builder.Services.AddSingleton<IMusicAPISettings>(provider =>
    provider.GetRequiredService<IOptions<MusicAPISettings>>().Value);

builder.Services.AddSingleton<SyncWorkJobService>();
builder.Services.AddHostedService(provider => provider.GetService<SyncWorkJobService>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
