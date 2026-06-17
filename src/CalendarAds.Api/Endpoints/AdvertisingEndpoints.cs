using CalendarAds.Api.Contracts;
using CalendarAds.Api.Data;
using CalendarAds.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace CalendarAds.Api.Endpoints;

public static class AdvertisingEndpoints
{
    public static RouteGroupBuilder MapAdvertisingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/advertisements").WithTags("Advertisements");

        group.MapGet("/", async (AppDbContext db, AdPlacement? placement, int take = 10, bool randomize = false) =>
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var query = db.Advertisements.AsNoTracking()
                .Where(x => x.IsActive && x.StartsOn <= today && x.EndsOn >= today);

            if (placement is not null)
            {
                query = query.Where(x => x.Placement == placement);
            }

            take = Math.Clamp(take, 1, 20);

            if (randomize)
            {
                var maxPriority = await query.MaxAsync(x => (int?)x.Priority);
                if (maxPriority is null)
                {
                    return Results.Ok(Array.Empty<AdvertisementDto>());
                }

                query = query.Where(x => x.Priority == maxPriority.Value)
                    .OrderBy(_ => Guid.NewGuid());
            }
            else
            {
                query = query.OrderByDescending(x => x.Priority)
                    .ThenBy(x => x.CreatedAt);
            }

            var ads = await query.Take(take)
                .Select(x => new AdvertisementDto(
                    x.Id,
                    x.Title,
                    x.ImageUrl,
                    x.TargetUrl,
                    x.Placement,
                    x.StartsOn,
                    x.EndsOn,
                    x.IsActive,
                    x.Priority))
                .ToListAsync();

            return Results.Ok(ads);
        });

        group.MapPost("/", async (AppDbContext db, CreateAdvertisementRequest request) =>
        {
            if (request.EndsOn < request.StartsOn)
            {
                return Results.BadRequest("EndsOn cannot be before StartsOn.");
            }

            var advertisement = new Advertisement
            {
                Id = Guid.NewGuid(),
                Title = request.Title.Trim(),
                ImageUrl = request.ImageUrl.Trim(),
                TargetUrl = request.TargetUrl.Trim(),
                Placement = request.Placement,
                StartsOn = request.StartsOn,
                EndsOn = request.EndsOn,
                IsActive = request.IsActive,
                Priority = request.Priority
            };

            db.Advertisements.Add(advertisement);
            await db.SaveChangesAsync();

            return Results.Created($"/api/advertisements/{advertisement.Id}", advertisement.Id);
        });

        group.MapPost("/{id:guid}/metrics/{type}", async (AppDbContext db, Guid id, AdMetricType type, HttpContext http) =>
        {
            var exists = await db.Advertisements.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return Results.NotFound();
            }

            db.AdMetrics.Add(new AdMetric
            {
                Id = Guid.NewGuid(),
                AdvertisementId = id,
                Type = type,
                UserAgent = http.Request.Headers.UserAgent.ToString()
            });

            await db.SaveChangesAsync();
            return Results.Accepted();
        });

        return group;
    }
}
