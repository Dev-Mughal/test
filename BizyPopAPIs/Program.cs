using Application.Utilities.Cache;
using Application.Utilities.UserContext;
using Application.Utilities.TokenManager;
using BizyPopAPIs.Features.Auth;
using BizyPopAPIs.Features.Business;
using BizyPopAPIs.Features.BusinessCategory;
using BizyPopAPIs.Features.Coupon;
using BizyPopAPIs.Features.CustomerAuth;
using BizyPopAPIs.Features.CustomerBusiness;
using BizyPopAPIs.Features.CustomerWallet;
using BizyPopAPIs.Features.Profile;
using BizyPopAPIs.Features.Incentive;
using BizyPopAPIs.Features.Scan;
using BizyPopAPIs.Features.State;
using BizyPopAPIs.Utilities.CustomMiddlewares.Application.Middleware;
using Common.Features.Auth.SignUp.Validators;
using Domain;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Data;
using System.Text;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Application.Services.IncentiveServices.Implementations;
using Application.Services.IncentiveServices.Interfaces;
using Infrastructure.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
ConfigurationManager Configuration = builder.Configuration;

builder.Services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));
var jwtSettings = Configuration.GetSection("Jwt").Get<JwtSettings>()
                  ?? throw new InvalidOperationException("Jwt settings are not configured.");

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Servers =
        [
            new OpenApiServer
            {
                Url = "https://hnh6dcn1-7138.asse.devtunnels.ms",
                Description = "Dev Tunnel (Remote)"
            },
            new OpenApiServer
            {
                Url = "https://localhost:7138",
                Description = "Local Development (HTTPS)"
            },
            new OpenApiServer
            {
                Url = "http://100.106.71.8:5000",
                Description = "Production Server"
            },
            new OpenApiServer
            {
                Url = "http://localhost:5138",
                Description = "Local Development (HTTP)"
            }
        ];
        return Task.CompletedTask;
    });
});

// Add exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Add health checks
builder.Services.AddHealthChecks();
builder.Services.AddLogging();
builder.Services.AddPostgresAppLogging(Configuration);

// Configure form options for file uploads
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

#region SERVICE REGISTRATIONS
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuthorizedUser, AuthorizedUser>();
builder.Services.AddScoped<IAuthorizedCustomer, AuthorizedCustomer>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICustomerAuthService, CustomerAuthService>();
builder.Services.AddScoped<IGeoService, GeoService>();
builder.Services.AddScoped<IStateService, StateService>();
builder.Services.AddScoped<ICustomerWalletService, CustomerWalletService>();
builder.Services.AddScoped<IBusinessCategoryService, BusinessCategoryService>();
builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<IBusinessFeedService, BusinessFeedService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IPromoIncentiveService, PromoIncentiveService>();
builder.Services.AddScoped<IStampIncentiveService, StampIncentiveService>();
builder.Services.AddScoped<IGiftCardIncentiveService, GiftCardIncentiveService>();
builder.Services.AddScoped<IVipIncentiveService, VipIncentiveService>();
builder.Services.AddHttpClient<IImageService, ImageService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddSingleton<ICouponCodeService, CouponCodeService>();
builder.Services.AddSingleton<IQRCodeService, QRCodeService>();
builder.Services.AddScoped<IIncentiveScanService, IncentiveScanService>();
builder.Services.AddScoped<IIncentiveTitleService, IncentiveTitleService>();
builder.Services.AddScoped<IPasswordHasher<Customer>, PasswordHasher<Customer>>();
#endregion
#region CORS CONFIGURATION
builder.Services.AddCors(x =>
{
    x.AddPolicy(EnvironmentType.Development.ToString(), policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
    x.AddPolicy(EnvironmentType.Production.ToString(), policy =>
    {
        policy.WithOrigins("https://bizzypop.com")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
#endregion
#region DATABASE AND IDENTITY CONFIGURATION
builder.Services.AddDbContextFactory<BizyPopDbContext>(x =>
   {
       x.UseNpgsql(connectionString: Configuration.GetConnectionString("DefaultConnection"));
   }
);
builder.Services.AddIdentity<BusinessUser, IdentityRole<long>>(x =>
    {
        x.User.RequireUniqueEmail = true;

        x.Password.RequiredLength = 8;
        x.Password.RequireNonAlphanumeric = false;
        x.Password.RequireUppercase = false;
        x.Password.RequireLowercase = false;
        x.Password.RequireDigit = false;

        x.Lockout.AllowedForNewUsers = false;

        x.SignIn.RequireConfirmedEmail = false;

        x.Tokens.ProviderMap.Add("Default", new TokenProviderDescriptor(typeof(IUserTwoFactorTokenProvider<BusinessUser>)));
        x.Tokens.EmailConfirmationTokenProvider = "Default";
        x.Tokens.PasswordResetTokenProvider = "Default";
        x.Tokens.ChangeEmailTokenProvider = "Default";
        x.Tokens.ChangePhoneNumberTokenProvider = "Default";
        x.Tokens.AuthenticatorTokenProvider = "Default";
        x.Tokens.AuthenticatorIssuer = "BizyPopApp";

    })
    .AddUserManager<UserManager<BusinessUser>>()
    .AddSignInManager<SignInManager<BusinessUser>>()
    .AddEntityFrameworkStores<BizyPopDbContext>()
    .AddDefaultTokenProviders();
#endregion
#region JWT CONFIGURATION
builder.Services.AddScoped<ITokenService, TokenService>();

// JWT Configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
});

//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "BizyPop APIs",
//        Version = "v1"
//    });

//    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Type = SecuritySchemeType.Http,
//        Scheme = "bearer",
//        BearerFormat = "JWT",
//        Description = "Enter your JWT Bearer token"
//    });

//    // New .NET 10 pattern - using delegate
//    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
//    {
//        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
//    });
//});
#endregion
#region FLUENT VALIDATION CONFIGURATION
builder.Services.AddValidatorsFromAssembly(typeof(SignUpDtoValidator).Assembly);
#endregion

builder.Services.AddAuthorization();
var app = builder.Build();
//app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.MapOpenApi();
//app.MapSwagger();
app.MapScalarApiReference(options =>
 {
     options.Title = "BizyPop APIs";
     options.Theme = ScalarTheme.BluePlanet;
     options.Telemetry = false;

     options.AddHttpAuthentication(JwtBearerDefaults.AuthenticationScheme, auth =>
     {
         auth.Description = "Enter your JWT Bearer token to authorize requests.";
     });
 });
//}
//else
//{
//    // Exception handling in production
//    app.UseExceptionHandler("/error");
//}

// Exception handler should work in all environments
app.UseExceptionHandler();

#region MIDDLEWARE PIPELINE
//if (app.Environment.IsDevelopment())
//{
    app.UseCors(EnvironmentType.Development.ToString());
//}
//else if (app.Environment.IsProduction())
//{
//    app.UseCors(EnvironmentType.Production.ToString());
//}

// Security Headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    await next();
});

// HTTPS Redirection
//app.UseHttpsRedirection();

// Static files middleware for serving uploaded images
app.UseStaticFiles();
#endregion

#region AUTHENTICATION AND AUTHORIZATION MIDDLEWARE
app.UseAuthentication();
app.UseAuthorization();
#endregion

#region ENDPOINT MAPPINGS
#region AUTHENTICATION ENDPOINTS
app.MapAuthEndpoints();
#endregion

#region BUSINESS CATEGORY ENDPOINTS
app.MapBusinessCategoryEndpoints();
#endregion

#region BUSINESS ENDPOINTS
app.MapBusinessEndpoints();
#endregion

#region COUPON ENDPOINTS
app.MapCouponEndpoints();
#endregion

#region PROFILE ENDPOINTS
app.MapProfileEndpoints();
#endregion

#region CUSTOMER AUTHENTICATION ENDPOINTS
app.MapCustomerAuthEndpoints();
#endregion

#region CUSTOMER BUSINESS ENDPOINTS
app.MapCustomerBusinessEndpoints();
#endregion

#region CUSTOMER WALLET ENDPOINTS
app.MapCustomerWalletEndpoints();
#endregion

#region STATES LOOKUP ENDPOINTS
app.MapLookupStates();
#endregion

#region SCAN ENDPOINT
app.MapScanEndpoints();
#endregion

#region INCENTIVE ENDPOINTS
app.MapIncentiveEndpoints();
#endregion

#region HEALTH CHECK (optional)
app.MapHealthChecks("/health");
#endregion

#region ERROR HANDLING (optional)
app.MapGet("/error", () =>
{
    return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
})
    .ExcludeFromDescription()
    .ExcludeFromApiReference();
#endregion
#endregion


app.Run();



enum EnvironmentType
{
    Development,
    Production
}