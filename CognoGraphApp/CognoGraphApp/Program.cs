using Neo4j.Driver;
using CognoGraphApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<GraphService>();


var uri = builder.Configuration["CognoDb:Uri"];
var username = builder.Configuration["CognoDb:Username"];
var password = builder.Configuration["CognoDb:Password"];

if (string.IsNullOrWhiteSpace(uri) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
{
    throw new InvalidOperationException("CognoDB settings are missing. Check your User Secrets.");
}

builder.Services.AddSingleton<IDriver>(_ =>
    GraphDatabase.Driver(uri, AuthTokens.Basic(username, password)));



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
