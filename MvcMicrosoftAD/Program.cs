using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MvcMicrosoftAD.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString =
    builder.Configuration.GetConnectionString("SqlServerLocal");
string appId =
    builder.Configuration.GetValue<string>
    ("Authentication:Microsoft:AppId");
string secretKey =
    builder.Configuration.GetValue<string>
    ("Authentication:Microsoft:SecretKey");
builder.Services.AddDbContext<ApplicationDbContext>
    (options => options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext> ();
builder.Services.AddAuthentication().AddMicrosoftAccount(options =>
{
    options.ClientId = appId;
    options.ClientSecret = secretKey;
});

builder.Services.AddControllersWithViews(
    options => options.EnableEndpointRouting = false);

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
app.UseAuthentication();
app.UseAuthorization();


app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template:"{controller=Home}/{action=Index}/{id?}");
});


app.Run();
