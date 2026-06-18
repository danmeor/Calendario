using CalendarAds.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CalendarAds.Api.Extensions;

public static class DevelopmentSeedExtensions
{
    public static async Task SeedDevelopmentDataAsync(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return;
        }

        await using var scope = app.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await db.Database.EnsureCreatedAsync();

        await db.CalendarEvents.AnyAsync();
    }
}
