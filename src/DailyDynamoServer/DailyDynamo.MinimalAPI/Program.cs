using System.Text;
using DailyDynamo.DAL.Context;
using DailyDynamo.DAL.UnitOfWork;
using DailyDynamo.MinimalAPI.API;
using DailyDynamo.MinimalAPI.MinimalAPIRoutes;
using DailyDynamo.Services.Interfaces;
using DailyDynamo.Services.Services;
using DailyDynamo.Shared.Common;
using DailyDynamo.Shared.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
AppSettingsUtility.SetSettings(builder.Configuration.Get<AppSettingsModel>());

// Add services to the container
builder.Services.AddDbContext<DailyDynamoDatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DailyDynamo")));
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<AppSettingsModel>();
builder.Services.AddAutoMapper(
    AppDomain.CurrentDomain.GetAssemblies()
    .Where(x => x.FullName.Contains(nameof(DailyDynamo.Services))));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISharedService, SharedService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWorkDiaryService, WorkDiaryService>();

builder.Services.AddSingleton(new FileManagerService(builder.Environment.WebRootPath));

builder.Services.AddCors();

// Adding Authentication  
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer  
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = AppSettingsUtility.Settings.JWT.ValidAudience,
        ValidIssuer = AppSettingsUtility.Settings.JWT.ValidIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettingsUtility.Settings.JWT.Secret))
    };
});


builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Enter 'Bearer {token}' to authorize"
    });

    //authorization header for all endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Example);
        options.DefaultModelExpandDepth(0);
        options.DefaultModelsExpandDepth(-1);
        options.EnableDeepLinking();
        options.DisplayOperationId();
        options.DisplayRequestDuration();
        options.ShowExtensions();

        options.ConfigObject.AdditionalItems["token"] = "Bearer ";
        options.ConfigObject.AdditionalItems["AuthorizationKeyName"] = "Authorization";
    });
}

app.UseCors(options =>
{
    options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Map all the API End points
new AuthenticationRoute(
    app.Services.CreateScope().ServiceProvider.GetRequiredService<IUserService>()
    ).AddRoutes(app);

app.AddWorkDiaryRoutes();

app.Run();

