using CalendarAds.Api.Contracts;
using CalendarAds.Api.Data;
using CalendarAds.Api.Domain;
using CalendarAds.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace CalendarAds.Api.Endpoints;

public static class CalendarEndpoints
{
    public static RouteGroupBuilder MapCalendarEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/calendar-events").WithTags("Calendar Events");

        group.MapGet("/", async (AppDbContext db, int? year) =>
        {
            var query = db.CalendarEvents.AsNoTracking();

            if (year is not null)
            {
                var from = new DateOnly(year.Value, 1, 1);
                var to = new DateOnly(year.Value, 12, 31);
                query = query.Where(x => x.StartDate <= to && (x.EndDate ?? x.StartDate) >= from);
            }

            var events = await query
                .OrderBy(x => x.StartDate)
                .Select(x => new CalendarEventDto(
                    x.Id,
                    x.Title,
                    x.Description,
                    x.StartDate,
                    x.EndDate,
                    x.ColorHex,
                    x.Type,
                    x.IsPublic))
                .ToListAsync();

            if (year is not null)
            {
                events.AddRange(ColombiaHolidayProvider.GetHolidays(year.Value));
            }

            return Results.Ok(events.OrderBy(calendarEvent => calendarEvent.StartDate));
        }).CacheOutput("PublicCalendar");

        group.MapPost("/", async (AppDbContext db, CreateCalendarEventRequest request) =>
        {
            if (request.EndDate is not null && request.EndDate < request.StartDate)
            {
                return Results.BadRequest("EndDate cannot be before StartDate.");
            }

            var calendarEvent = new CalendarEvent
            {
                Id = Guid.NewGuid(),
                Title = request.Title.Trim(),
                Description = request.Description?.Trim(),
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ColorHex = string.IsNullOrWhiteSpace(request.ColorHex) ? "#14b8a6" : request.ColorHex,
                Type = request.Type,
                IsPublic = request.IsPublic
            };

            db.CalendarEvents.Add(calendarEvent);
            await db.SaveChangesAsync();

            return Results.Created($"/api/calendar-events/{calendarEvent.Id}", calendarEvent.Id);
        });

        group.MapDelete("/{id:guid}", async (AppDbContext db, Guid id) =>
        {
            var deleted = await db.CalendarEvents.Where(x => x.Id == id).ExecuteDeleteAsync();
            return deleted == 0 ? Results.NotFound() : Results.NoContent();
        });

        return group;
    }
}
