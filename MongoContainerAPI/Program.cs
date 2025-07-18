using MongoDB.Driver;
using MongoContainerAPI.Services;
using MongoContainerAPI;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// bind configuration (not strictly required for simple config reads)
builder.Configuration
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddEnvironmentVariables();

// register Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// register Mongo client & your Mongo-backed IDB
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration["MongoDB:ConnectionString"]));
builder.Services.AddSingleton<IDB, MongoDBStore>();

// config Kestrel in prod
if (builder.Environment.IsProduction())
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        var certPath = builder.Configuration["Kestrel__Certificates__Default__Path"];
        var certPassword = builder.Configuration["Kestrel__Certificates__Default__Password"];
        if (!string.IsNullOrEmpty(certPath) && !string.IsNullOrEmpty(certPassword))
        {
            options.ConfigureHttpsDefaults(httpsOptions =>
            {
                httpsOptions.ServerCertificate = new X509Certificate2(certPath, certPassword);
            });
        }
    });
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/savevalue", (string parameter, IDB db) =>
{
    db.SaveValue(parameter);
    return Results.Created($"/getvalue", parameter);
});

app.MapGet("/getvalue", (IDB db) =>
{
    var all = db.GetValue();
    return Results.Ok(all);
});

app.Run();