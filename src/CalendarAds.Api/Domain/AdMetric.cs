namespace CalendarAds.Api.Domain;

public sealed class AdMetric
{
    public Guid Id { get; set; }
    public Guid AdvertisementId { get; set; }
    public Advertisement? Advertisement { get; set; }
    public AdMetricType Type { get; set; }
    public string? UserAgent { get; set; }
    public string? IpHash { get; set; }
    public DateTimeOffset OccurredAt { get; set; } = DateTimeOffset.UtcNow;
}
