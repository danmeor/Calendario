namespace CalendarAds.Api.Domain;

public sealed class CalendarEvent
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string ColorHex { get; set; } = "#14b8a6";
    public CalendarEventType Type { get; set; } = CalendarEventType.General;
    public bool IsPublic { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
