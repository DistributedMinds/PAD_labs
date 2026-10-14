
using Broker_gRPC.Services;
using Broker_gRPC.Storage;
using Broker;
using Message_Agent.Common;
using Microsoft.Data.Sqlite;
using Grpc.Net.Client;
using GrpcAgent;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Broker_gRPC.Storage;

var settings = Settings.Load();
var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls(settings.BrokerUrl);
builder.Services.AddGrpc(o => o.Interceptors.Add<AuthInterceptor>());
builder.Services.AddSingleton<IPersistentStore, PersistentStore>();
builder.Services.AddSingleton<IConnectionStorage, ConnectionStorage>();
builder.Services.AddSingleton<ISessionStore, SessionStore>();

builder.Services.AddHostedService<SenderWorker>();

var app = builder.Build();

app.MapGrpcService<AuthService>();
app.MapGrpcService<PublisherService>();
app.MapGrpcService<SubscriberService>();

app.Run(); 