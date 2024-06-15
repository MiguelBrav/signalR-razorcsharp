using DemoSignalR.Interfaces;
using DemoSignalR.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using PruebaSignalR.Pages.Hub;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";

    });

builder.Services.AddSignalR();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".signalTest";
    options.IdleTimeout = TimeSpan.FromMinutes(30);

});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapHub<SignalHub>("login-hub");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.UseSession();

app.MapRazorPages();

app.Run();
