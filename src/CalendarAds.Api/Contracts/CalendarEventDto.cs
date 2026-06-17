using CalendarAds.Api.Domain;

namespace CalendarAds.Api.Contracts;

public sealed record CalendarEventDto(
    Guid Id,
    string Title,
    string? Description,
    DateOnly StartDate,
    DateOnly? EndDate,
    string ColorHex,
    CalendarEventType Type,
    bool IsPublic);
