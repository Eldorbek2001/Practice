using Microsoft.EntityFrameworkCore;
using Practical.Controllers;
using Practical.Models;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<StackOverflow2010Context>(options =>
    options.UseSqlServer("\"Server=(localdb)\\\\test;Database=StackOverflow2010;Trusted_Connection=True;"));

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<PostsController>();
builder.Services.AddScoped<UsersController>();



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
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Posts}/{action=Index}/{id?}");

app.Run();



