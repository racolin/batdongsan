using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using WebUI.Services;
using WebUI.Filters;
using Application.Common.Responses;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;

namespace WebUI;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddHttpContextAccessor();

        services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

        services.AddFluentValidationAutoValidation();

        services.AddRouting(options => options.LowercaseUrls = true);

        services.AddControllers(options =>
        {
            options.Filters.Add(new ProducesAttribute("application/json"));
        });

        services.AddControllersWithViews();
        //services.AddRazorPages();
        //services.AddMvc();

        services.AddSession();

        //services.AddScoped<FluentValidationSchemaProcessor>(provider =>
        //{
        //    var validationRules = provider.GetService<IEnumerable<FluentValidationRule>>();
        //    var loggerFactory = provider.GetService<ILoggerFactory>();

        //    return new FluentValidationSchemaProcessor(provider, validationRules, loggerFactory);
        //});

        //Customize default API behavior
        services.Configure<ApiBehaviorOptions>(o =>
        {
            o.InvalidModelStateResponseFactory = InvalidModelStateResponseFactory;
        });

        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                 {
                     Version = "v1",
                     Title = "batdongsan API",
                     Description = "An ASP.NET Core Web API for managing batdongsan items",
                     //TermsOfService = new Uri("https://example.com/terms"),
                     //Contact = new OpenApiContact
                     //{
                     //    Name = "Example Contact",
                     //    Url = new Uri("https://example.com/contact")
                     //},
                     //License = new OpenApiLicense
                     //{
                     //    Name = "Example License",
                     //    Url = new Uri("https://example.com/license")
                     //},
                 });
                options.DocInclusionPredicate((_, api) => !string.IsNullOrWhiteSpace(api.GroupName));
                options.TagActionsBy(api => [api.GroupName]);
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Description = "Please, type your JWT before try out the api!",
                    Scheme = "bearer",
                });
            }
        );

        //services.AddSwaggerDocument((configure, serviceProvider) =>
        //{
        //    var fluentValidationSchemaProcessor = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<FluentValidationSchemaProcessor>();

        //    // Add the fluent validations schema processor
        //    configure.SchemaProcessors.Add(fluentValidationSchemaProcessor);

        //});

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IFileService, FileService>();

        services.AddSingleton<IAuthorizationMiddlewareResultHandler, ApiAuthorizationMiddlewareResultHandler>();

        return services;
    }

    private static IActionResult InvalidModelStateResponseFactory(ActionContext context)
    {
        var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
            .Select(v => v.Errors.Last())
            .Select(v => v.ErrorMessage)
            .ToList();

        var responseObj = new DataResponse<Message>()
        {
            Message = new Message()
            {
                Type = MessageType.Error,
                Name = "Dữ liệu đầu vào không đúng định dạng!!",
                Detail = errors,
            }
        };

        return new OkObjectResult(responseObj);
    }
}
