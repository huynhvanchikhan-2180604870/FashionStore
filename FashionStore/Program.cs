using FashionStore.Data;
using FashionStore.Models;
using FashionStore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using DinkToPdf.Contracts;
using DinkToPdf;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<FashionStoreDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<FashionStoreDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddSingleton<ShoppingCartService>();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 128;
    });

// Register DinkToPdf services
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton(x => new PaypalClient(
    builder.Configuration["Paypal:AppId"],
    builder.Configuration["Paypal:AppSecrect"],
    builder.Configuration["Paypal:Mode"]
));

builder.Services.AddSingleton<IVnPayService, VnPayService>();
builder.Services.AddSingleton<IPDFService, PDFService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    // Map Admin area route
    endpoints.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

    // Map default route
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // Map API controllers
    endpoints.MapControllers();

    // Map Razor Pages
    endpoints.MapRazorPages();
});

app.Run();
