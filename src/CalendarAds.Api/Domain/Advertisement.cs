namespace CalendarAds.Api.Domain;

public sealed class Advertisement
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string TargetUrl { get; set; } = string.Empty;
    public AdPlacement Placement { get; set; } = AdPlacement.TopBanner;
    public DateOnly StartsOn { get; set; }
    public DateOnly EndsOn { get; set; }
    public bool IsActive { get; set; } = true;
    public int Priority { get; set; } = 1;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public ICollection<AdMetric> Metrics { get; set; } = [];
}
