using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Practical.Controllers;
using Practical.Models;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<StackOverflow2010Context>(options =>
options.UseSqlServer("connectionString"));
builder.Services.AddControllersWithViews();



builder.Services.AddSignalR();
builder.Services.AddScoped<BadgesController>();
builder.Services.AddScoped<PostsController>();
builder.Services.AddScoped<UsersController>();

builder.Services.AddEndpointsApiExplorer();

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Posts}/{action=Index}/{id?}");

    endpoints.MapHub<NotificationHub>("/notificationhub");
});


app.Run();
