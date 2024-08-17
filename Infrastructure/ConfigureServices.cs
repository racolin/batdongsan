using Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Infrastructure.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Files;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Domain.Constants;
using Domain;
using System.Text;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitializer>();

        services
            .AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Lockout.MaxFailedAccessAttempts = RoleConstant.MaxLoginFail;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(7);
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddIdentityServer().AddApiAuthorization<User, ApplicationDbContext>();
        services.AddTransient<IRecaptchaService, RecaptchaService>();
        services.AddTransient<IMailService, MailService>();
        services.AddTransient<IDateTimeService, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

        services.AddAuthentication(options =>
            {
                //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = "JWT_OR_COOKIE";
                options.DefaultChallengeScheme = "JWT_OR_COOKIE";
                options.DefaultAuthenticateScheme = "JWT_OR_COOKIE";
            })
            .AddCookie()
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };
            })
            .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
            {
                // runs on each request
                options.ForwardDefaultSelector = context =>
                {
                    // filter by auth type
                    string authorization = context.Request.Headers[HeaderNames.Authorization];
                    if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                        return JwtBearerDefaults.AuthenticationScheme;

                    // otherwise always check for identity cookie auth
                    return IdentityConstants.ApplicationScheme;
                    // otherwise always check for cookie auth
                    //return CookieAuthenticationDefaults.AuthenticationScheme;
                };
            });

        services.Configure<CookieAuthenticationOptions>(
            IdentityConstants.ApplicationScheme, 
            options => {
                options.LoginPath = new PathString("/Admin");
                options.AccessDeniedPath = new PathString("/Admin/PageNotFound");
                // Config Non-Https
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            }
        );

        services.AddAuthorization(options => options.AddPolicy("CanPurge", policy => policy.RequireRole("Admin")));
        //services.AddAuthorization();
       
        return services;
    }

    public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<ConfigOptions>(config.GetSection(ConfigOptions.Name));
        services.Configure<JwtConfigOptions>(config.GetSection(JwtConfigOptions.Name));
        services.Configure<RecaptchaConfigOptions>(config.GetSection(RecaptchaConfigOptions.Name));
        services.Configure<SecurityStampValidatorOptions>(options =>
        {
            //enables immediate logout, after updating the user's stat.
            //options.ValidationInterval = TimeSpan.Zero;
        });

        return services;
    }
}
