using BOM.Services.Api;
using BOM.Services.Api.Interfaces;
using BOM.Services.Api.Middlewares;
using BOM.Services.Api.Services;
using Calzolari.Grpc.AspNetCore.Validation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    //Grpc
    options.ListenLocalhost(5001, o => o.Protocols =
        HttpProtocols.Http2);

    //Api
    options.ListenLocalhost(5000, o => o.Protocols =
        HttpProtocols.Http1);
});

builder.Services.AddOptions();
builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfig"));
builder.Services.Configure<OpenviduStoreSetting>(builder.Configuration.GetSection("OpenviduStoreDatabase"));
builder.Services.AddHttpClient("Openvidu", c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("AppConfig:OpenviduUrl"));
    c.DefaultRequestHeaders.Clear();

    var token = $"{builder.Configuration.GetValue<string>("AppConfig:OpenviduUsername")}:{builder.Configuration.GetValue<string>("AppConfig:OpenviduSecret")}";
    var basicAuth = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes(token));

    c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("basic", basicAuth);
    c.Timeout = TimeSpan.FromSeconds(builder.Configuration.GetValue<int>("AppConfig:OpenviduTimeout"));
});

builder.Services.AddControllers();

builder.Services.AddGrpc(o =>
{
    o.EnableMessageValidation();
});
builder.Services.AddGrpcValidation();

builder.Services.AddValidators();

builder.Services.AddScoped<IOpenviduApiService, OpenviduApiService>();
builder.Services.AddScoped<IMeetingService, MeetingService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<IJwtService, JwtService>();

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//                .AddJwtBearer(options =>
//                {
//                    var secretkey = Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("AppConfig:JwtSecret"));
//                    //var encryptionkey = Encoding.UTF8.GetBytes(_appConfig.JwtOptions.Encryptkey);

//                    var validationParameters = new TokenValidationParameters
//                    {
//                        ClockSkew = TimeSpan.Zero, // default: 5 min
//                        RequireSignedTokens = true,

//                        ValidateIssuerSigningKey = true,
//                        IssuerSigningKey = new SymmetricSecurityKey(secretkey),

//                        RequireExpirationTime = true,
//                        ValidateLifetime = true,

//                        ValidateAudience = true, //default : false
//                        ValidAudience = _appConfig.JwtOptions.Audience,

//                        ValidateIssuer = true, //default : false
//                        ValidIssuer = _appConfig.JwtOptions.Issuer,

//                        //TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
//                    };

//                    options.RequireHttpsMetadata = false;
//                    options.SaveToken = true;
//                    options.TokenValidationParameters = validationParameters;
//                });

builder.Services.AddHealthChecks()
    .AddMongoDb(builder.Configuration.GetValue<string>("OpenviduStoreDatabase:ConnectionString"), tags: new string[] { "mongodb" });


var app = builder.Build();
app.UseRouting();
app.UseMiddleware<JwtMiddleware>();
app.UseEndpoints(r =>
{
    r.MapControllers();
    r.MapGrpcService<OpenviduGrpcService>();
});

app.UseHealthChecks("/hc", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).UseHealthChecksUI();

app.Run();