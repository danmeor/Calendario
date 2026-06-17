using CalendarAds.Api.Domain;

namespace CalendarAds.Api.Contracts;

public sealed record CreateCalendarEventRequest(
    string Title,
    string? Description,
    DateOnly StartDate,
    DateOnly? EndDate,
    string ColorHex,
    CalendarEventType Type,
    bool IsPublic);
