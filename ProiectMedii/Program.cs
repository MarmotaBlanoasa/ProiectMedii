using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProiectMedii.Data;
using Microsoft.AspNetCore.Identity;
using ProiectMedii.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ProiectMediiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProiectMediiContext") ?? throw new InvalidOperationException("Connection string 'ProiectMediiContext' not found.")));
builder.Services.AddDbContext<LibraryIdentityContext>(options =>

options.UseSqlServer(builder.Configuration.GetConnectionString("LibraryIdentityContext") ?? throw new InvalidOperationException("Connectionstring 'LibraryIdentityContext' not found.")));
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<LibraryIdentityContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
