using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using QuanLyGameConsole;
using QuanLyGameConsole.Abstractions;
using QuanLyGameConsole.Helper;
using QuanLyGameConsole.Models;
using QuanLyGameConsole.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(x => new PaypalClient(
    builder.Configuration["Paypal:AppId"],
    builder.Configuration["Paypal:AppSecret"],
    builder.Configuration["Paypal:Mode"]

));
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<GameConsoleContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{

    options.AccessDeniedPath = "/Home/404";
})
.AddCookie("Admin", options =>
 {
     options.LoginPath = "/Admin/Account/Login";
     options.AccessDeniedPath = "/Home/404";
 })
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.SaveTokens = true;
    options.Events.OnRedirectToAuthorizationEndpoint = context =>
    {
        context.Response.Redirect(context.RedirectUri + "&prompt=select_account");
        return Task.CompletedTask;
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "2").AuthenticationSchemes = new[] { "Admin" });
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();


app.UseStatusCodePagesWithReExecute("/Home/Error", "?statuscode={0}");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();


app.MapStaticAssets();


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}")
    .RequireAuthorization("Admin")
    .WithStaticAssets();


//app.MapControllerRoute(
//    name: "ProductBrand",
//    pattern: "product-category/brand/{brands}",
//    defaults: new { controller = "Product", action = "ProductList" }
//);

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<GameConsoleContext>();
await SeedData.SeedingData(context);

app.Run();
