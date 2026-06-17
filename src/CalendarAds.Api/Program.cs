using CalendarAds.Api.Data;
using CalendarAds.Api.Endpoints;
using CalendarAds.Api.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddResponseCompression();
builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("PublicCalendar", policy =>
        policy.Expire(TimeSpan.FromMinutes(10)).SetVaryByQuery("year"));
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularDev", policy =>
        policy.WithOrigins(
                "http://localhost:4200",
                "http://127.0.0.1:4200",
                "https://diasfestivoscol.com",
                "https://www.diasfestivoscol.com")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors("AngularDev");
app.UseResponseCompression();
app.UseOutputCache();

app.MapCalendarEndpoints();
app.MapAdvertisingEndpoints();

await app.SeedDevelopmentDataAsync();

app.Run();
