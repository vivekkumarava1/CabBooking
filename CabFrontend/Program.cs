using CabFrontend.IServices;
using CabFrontend.Models;
using CabFrontend.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.ResponseCompression;
using Stripe;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*builder.Services.AddControllersWithViews();*/
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		.AddCookie(options =>
		{
			options.LoginPath = "/User/Index";
			options.AccessDeniedPath = "/User/AccessDenied";
            options.AccessDeniedPath = "/Reservation/AccessDenied";
			options.AccessDeniedPath = "/Rating/AccessDenied";
			options.AccessDeniedPath = "/Stripe/AccessDenied";
		});
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(AuthFilter));
	
});
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddScoped<IStripeService, StripeService>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddHttpClient<UserServices>();
builder.Services.AddScoped<AuthFilter>();



builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddLogging();

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

app.UseSession();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

	endpoints.MapControllerRoute(
		name: "reservation",
		pattern: "{controller=Reservation}/{action=Booking}/{id?}");
});


app.Run();
