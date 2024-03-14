using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("Ocelot.json").Build();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOcelot(configuration).AddConsul();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAuthentication("Bearer").AddIdentityServerAuthentication("Bearer", options =>
{
    options.Authority = "https://localhost:44395";
});
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
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.UseOcelot().Wait();
app.Run();
