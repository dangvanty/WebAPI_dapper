using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.Razor;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;
using WebAPI_dapper.Resources;
using Microsoft.Extensions.Options;
using WebAPI_dapper.Data.Models;
using System.Data.SqlClient;
using WebAPI_dapper.Data;
using System.ComponentModel;
using Swashbuckle.Swagger;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Custom JSON format for interop with an old system that uses PascalCase
    // and enums as numbers
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// tich hop identity

builder.Services.AddTransient<IUserStore<AppUser>,UserStore>();
builder.Services.AddTransient<IRoleStore<AppRole>, RoleStore>();


builder.Services.AddIdentity<AppUser, AppRole>()
        .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequireDigit= true;
    opt.Password.RequireLowercase= false;
    opt.Password.RequireUppercase= false;
    opt.Password.RequireNonAlphanumeric= false;
    opt.Password.RequiredUniqueChars= 1;
    opt.Password.RequiredLength= 6;


    // Cấu hình Lockout -khóa user
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
    opt.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
    opt.Lockout.AllowedForNewUsers = true;

});
// tich hop da ngon ngu

var supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("vi-VN"),
};

var options = new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture(culture: "vi-VN", uiCulture: "vi-VN"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

options.RequestCultureProviders = new[]
{
    new RouteDataRequestCultureProvider(){Options = options }
};

builder.Services.AddSingleton(options);
builder.Services.AddSingleton<LocalService>();

builder.Services.AddLocalization(opt => opt.ResourcesPath = "Resources");

builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(
    opt =>
    {
        opt.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
            return factory.Create("SharedResource", assemblyName.Name);
        };
    }
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


var app = builder.Build();
var loggerFactory = app.Services.GetService<ILoggerFactory>();

    loggerFactory.AddFile(builder.Configuration.GetSection("Logging"));


var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);

app.UseExceptionHandler(opt =>
{
    opt.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (ex == null) return;

        var error = new
        {
            message = ex.Message
        };
        context.Response.ContentType = "application/json";
        context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
        context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { builder.Configuration["AllowedHosts"] });

        //using (var writer = new StreamWriter(context.Response.Body))
        //{
        //    new JsonSerializer().Serialize(writer, error);
        //    await writer.FlushAsync().ConfigureAwait(false);
        //}
        await context.Response.WriteAsJsonAsync(error);

    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json","Learn WebAPI_dapper v1");
    });
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
