using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<LocalDropshippingContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDropshipping"));
});

// Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<LocalDropshippingContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;

    options.SignIn.RequireConfirmedEmail = false;
});



// Services
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IWishListService, WishListService>();
builder.Services.AddScoped<IWithdrawlsService, WithdrawlsService>();
builder.Services.AddScoped<ISubscriptionsService, SubscriptionsService>();
builder.Services.AddScoped<IAccountService, AccountService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
