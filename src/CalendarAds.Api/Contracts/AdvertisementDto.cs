using CalendarAds.Api.Domain;

namespace CalendarAds.Api.Contracts;

public sealed record AdvertisementDto(
    Guid Id,
    string Title,
    string ImageUrl,
    string TargetUrl,
    AdPlacement Placement,
    DateOnly StartsOn,
    DateOnly EndsOn,
    bool IsActive,
    int Priority);
