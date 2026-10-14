
using Broker_gRPC.Services;
using Broker;
using Message_Agent.Common;
using Microsoft.Data.Sqlite;
using Grpc.Net.Client;
using GrpcAgent;
using Microsoft.AspNetCore.Hosting.Server.Features;

var settings = Settings.Load();
var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls(settings.BrokerUrl);
builder.Services.AddGrpc();
builder.Services.AddSingleton<IPersistentStore, PersistentStore>();
builder.Services.AddHostedService<SenderWorker>();

var app = builder.Build();


app.MapGrpcService<PublisherService>();
app.MapGrpcService<SubscriberService>();

app.Run(); 