using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Models;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<LocalDropshippingContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = IdentityConstants.ApplicationScheme;
    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies(o => { });

builder.Services.ConfigureApplicationCookie(configs =>
{
    configs.LoginPath = "/Seller/Login";
});

// Swagger 
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Foc Fulfilment", Version = "v1" });
});



// Services
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IWishListService, WishListService>();
builder.Services.AddScoped<IWithdrawlsService, WithdrawlsService>();
builder.Services.AddScoped<ISubscriptionsService, SubscriptionsService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.Configure<SMTPConfigModel>(builder.Configuration.GetSection("SMTPConfig"));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IImageService, ImageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Public/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Foc Fulfilment");
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Seller}/{action=Shop}/{id?}"
);

app.Run();
