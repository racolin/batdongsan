using Infrastructure;
using Application;
using WebUI;
using Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationServices();
builder.Services.AddConfig(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebUIServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Initialize and seed database
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
    await initializer.InitializeAsync();
    await initializer.SeedAsync();
}

//Handle 404 errors
app.Use(async (ctx, next) =>
{
    await next();
    if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
    {
        //Re-execute the request so the user gets the error page
        if (ctx.Request.Path != null 
        && ctx.Request.Path.Value != null 
        && ctx.Request.Path.Value!.Split("/")[1].ToLower().Equals("admin"))
        {
            ctx.Request.Path = new PathString("/Admin/PageNotFound");
        } else
        {
            ctx.Request.Path = new PathString("/PageNotFound");
        }
        await next();
    }
});

app.UseStaticFiles();

app.UseRouting();

app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "admin/{controller}/{action}/{id?}",
    defaults: new { controller = "auth", action = "index" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}",
    defaults: new { controller = "Home", action = "Index" }
);

app.UseAuthentication();

app.UseHttpsRedirection();
//app.UseHsts();

app.UseAuthorization();

app.MapControllers();

app.UseSession();

app.Run();
