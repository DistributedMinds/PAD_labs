
using Broker_gRPC.Services;
using Broker;
using Message_Agent.Common;
using Microsoft.Data.Sqlite;

var settings = Settings.Load();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddSingleton<IPersistentStore, PersistentStore>();

var app = builder.Build();


app.MapGrpcService<PublisherService>();

app.Run(); 