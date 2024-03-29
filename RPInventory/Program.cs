﻿using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RPInventory.Data;
using RPInventory.Helpers;
using RPInventory.Models;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.AddSerilog(logger);


// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Profiles", "Administrators");
    options.Conventions.AuthorizeFolder("/Users", "Administrators");
    options.Conventions.AuthorizeFolder("/Departments", "Administrators");

    options.Conventions.AuthorizeFolder("/Brands", "Employees");

    options.Conventions.AuthorizeFolder("/Products", "Organization");
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrators",
        policy => policy.RequireRole("Administrator"));

    options.AddPolicy("Employees",
        policy => policy.RequireRole("Administrator", "Employee"));

    options.AddPolicy("Organization",
        policy => policy.RequireRole("Administrator", "Employee", "Guest"));
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan=TimeSpan.FromMinutes(5);
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/AccessDenyed";
        options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
        options.SlidingExpiration = true;
    });

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.PageViewLocationFormats.Add("/Pages/Partials/{0}" + RazorViewEngine.ViewExtension);
});

builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
});

builder.Services.AddDbContext<InventoryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InventoryContext") ?? throw new InvalidOperationException("Connection string 'InventoryContext' not found.")));

builder.Services.AddSingleton<UserFactory>();
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    var context = services.GetRequiredService<InventoryContext>();
    //context.Database.EnsureCreated();
    DbInitializer.Initialize(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
