using CalendarAds.Api.Data;
using CalendarAds.Api.Domain;
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

        if (!await db.CalendarEvents.AnyAsync())
        {
            db.CalendarEvents.AddRange(
                new CalendarEvent
                {
                    Id = Guid.NewGuid(),
                    Title = "Lanzamiento editorial",
                    Description = "Publicacion destacada en el calendario.",
                    StartDate = new DateOnly(2026, 6, 12),
                    ColorHex = "#f97316",
                    Type = CalendarEventType.Editorial
                },
                new CalendarEvent
                {
                    Id = Guid.NewGuid(),
                    Title = "Campana patrocinada",
                    StartDate = new DateOnly(2026, 7, 13),
                    EndDate = new DateOnly(2026, 7, 20),
                    ColorHex = "#14b8a6",
                    Type = CalendarEventType.Promotion
                });
        }

        if (!await db.Advertisements.AnyAsync(x => x.Title == "Banner superior demo A"))
        {
            db.Advertisements.Add(new Advertisement
            {
                Id = Guid.NewGuid(),
                Title = "Banner superior demo A",
                ImageUrl = "/assets/ad-top.svg",
                TargetUrl = "https://example.com",
                Placement = AdPlacement.TopBanner,
                StartsOn = new DateOnly(2026, 1, 1),
                EndsOn = new DateOnly(2026, 12, 31),
                Priority = 10
            });
        }

        if (!await db.Advertisements.AnyAsync(x => x.Title == "Banner superior demo B"))
        {
            db.Advertisements.Add(new Advertisement
            {
                Id = Guid.NewGuid(),
                Title = "Banner superior demo B",
                ImageUrl = "/assets/ad-top.svg",
                TargetUrl = "https://example.com",
                Placement = AdPlacement.TopBanner,
                StartsOn = new DateOnly(2026, 1, 1),
                EndsOn = new DateOnly(2026, 12, 31),
                Priority = 10
            });
        }

        if (!await db.Advertisements.AnyAsync(x => x.Title == "Lateral demo A"))
        {
            db.Advertisements.Add(new Advertisement
            {
                Id = Guid.NewGuid(),
                Title = "Lateral demo A",
                ImageUrl = "/assets/ad-right-a.svg",
                TargetUrl = "https://example.com",
                Placement = AdPlacement.RightRail,
                StartsOn = new DateOnly(2026, 1, 1),
                EndsOn = new DateOnly(2026, 12, 31),
                Priority = 10
            });
        }

        if (!await db.Advertisements.AnyAsync(x => x.Title == "Anuncia aqui - superior"))
        {
            db.Advertisements.Add(new Advertisement
            {
                Id = Guid.NewGuid(),
                Title = "Anuncia aqui - superior",
                ImageUrl = "/assets/ad-top.svg",
                TargetUrl = "mailto:ventas@tu-dominio.com?subject=Quiero%20anunciar%20en%20el%20banner%20superior",
                Placement = AdPlacement.TopBanner,
                StartsOn = new DateOnly(2026, 1, 1),
                EndsOn = new DateOnly(2030, 12, 31),
                Priority = 1
            });
        }

        if (!await db.Advertisements.AnyAsync(x => x.Title == "Anuncia aqui - lateral"))
        {
            db.Advertisements.Add(new Advertisement
            {
                Id = Guid.NewGuid(),
                Title = "Anuncia aqui - lateral",
                ImageUrl = "/assets/ad-right-b.svg",
                TargetUrl = "mailto:ventas@tu-dominio.com?subject=Quiero%20anunciar%20en%20el%20espacio%20lateral",
                Placement = AdPlacement.RightRail,
                StartsOn = new DateOnly(2026, 1, 1),
                EndsOn = new DateOnly(2030, 12, 31),
                Priority = 1
            });
        }

        if (!await db.Advertisements.AnyAsync(x => x.Title == "Lateral izquierdo demo A"))
        {
            db.Advertisements.Add(new Advertisement
            {
                Id = Guid.NewGuid(),
                Title = "Lateral izquierdo demo A",
                ImageUrl = "/assets/ad-left-a.svg",
                TargetUrl = "https://example.com",
                Placement = AdPlacement.LeftRail,
                StartsOn = new DateOnly(2026, 1, 1),
                EndsOn = new DateOnly(2026, 12, 31),
                Priority = 10
            });
        }

        if (!await db.Advertisements.AnyAsync(x => x.Title == "Lateral izquierdo demo B"))
        {
            db.Advertisements.Add(new Advertisement
            {
                Id = Guid.NewGuid(),
                Title = "Lateral izquierdo demo B",
                ImageUrl = "/assets/ad-left-b.svg",
                TargetUrl = "https://example.com",
                Placement = AdPlacement.LeftRail,
                StartsOn = new DateOnly(2026, 1, 1),
                EndsOn = new DateOnly(2026, 12, 31),
                Priority = 10
            });
        }

        if (!await db.Advertisements.AnyAsync(x => x.Title == "Banner inferior demo"))
        {
            db.Advertisements.Add(new Advertisement
            {
                Id = Guid.NewGuid(),
                Title = "Banner inferior demo",
                ImageUrl = "/assets/ad-bottom.svg",
                TargetUrl = "https://example.com",
                Placement = AdPlacement.BottomBanner,
                StartsOn = new DateOnly(2026, 1, 1),
                EndsOn = new DateOnly(2026, 12, 31),
                Priority = 10
            });
        }

        await db.SaveChangesAsync();
    }
}
