using Albelli.Assessment.WebApi;
using Albelli.Assessment.WebApi.Filters;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "Using the Authorization header with the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", securitySchema);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { "Bearer" } }
    });
});

IConfiguration configuration = builder.Configuration;

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    AutofacConfig.Configure(containerBuilder);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = $"https://{configuration.GetValue<string>("Auth0:Domain")}/";
    options.Audience = configuration.GetValue<string>("Auth0:Audience");
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpResponseExceptionFilter>();
});


var repository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
var fileInfo = new FileInfo(@"log4net.config");
log4net.Config.XmlConfigurator.Configure(repository, fileInfo);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "albelli assessment WebApi V1");
    options.RoutePrefix = string.Empty;
});

app.UseAuthentication();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
