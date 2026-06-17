using CalendarAds.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace CalendarAds.Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<CalendarEvent> CalendarEvents => Set<CalendarEvent>();
    public DbSet<Advertisement> Advertisements => Set<Advertisement>();
    public DbSet<AdMetric> AdMetrics => Set<AdMetric>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CalendarEvent>(entity =>
        {
            entity.ToTable("calendar_events");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Title).HasMaxLength(140).IsRequired();
            entity.Property(x => x.Title).HasColumnName("title");
            entity.Property(x => x.Description).HasMaxLength(800);
            entity.Property(x => x.Description).HasColumnName("description");
            entity.Property(x => x.StartDate).HasColumnName("start_date");
            entity.Property(x => x.EndDate).HasColumnName("end_date");
            entity.Property(x => x.ColorHex).HasMaxLength(7).IsRequired();
            entity.Property(x => x.ColorHex).HasColumnName("color_hex");
            entity.Property(x => x.Type).HasColumnName("type");
            entity.Property(x => x.IsPublic).HasColumnName("is_public");
            entity.Property(x => x.CreatedAt).HasColumnName("created_at");
            entity.HasIndex(x => x.StartDate);
        });

        modelBuilder.Entity<Advertisement>(entity =>
        {
            entity.ToTable("advertisements");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Title).HasMaxLength(160).IsRequired();
            entity.Property(x => x.Title).HasColumnName("title");
            entity.Property(x => x.ImageUrl).HasMaxLength(1000).IsRequired();
            entity.Property(x => x.ImageUrl).HasColumnName("image_url");
            entity.Property(x => x.TargetUrl).HasMaxLength(1000).IsRequired();
            entity.Property(x => x.TargetUrl).HasColumnName("target_url");
            entity.Property(x => x.Placement).HasColumnName("placement");
            entity.Property(x => x.StartsOn).HasColumnName("starts_on");
            entity.Property(x => x.EndsOn).HasColumnName("ends_on");
            entity.Property(x => x.IsActive).HasColumnName("is_active");
            entity.Property(x => x.Priority).HasColumnName("priority");
            entity.Property(x => x.CreatedAt).HasColumnName("created_at");
            entity.HasIndex(x => new { x.Placement, x.IsActive, x.StartsOn, x.EndsOn });
        });

        modelBuilder.Entity<AdMetric>(entity =>
        {
            entity.ToTable("ad_metrics");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.AdvertisementId).HasColumnName("advertisement_id");
            entity.Property(x => x.Type).HasColumnName("type");
            entity.Property(x => x.UserAgent).HasMaxLength(600);
            entity.Property(x => x.UserAgent).HasColumnName("user_agent");
            entity.Property(x => x.IpHash).HasMaxLength(128);
            entity.Property(x => x.IpHash).HasColumnName("ip_hash");
            entity.Property(x => x.OccurredAt).HasColumnName("occurred_at");
            entity.HasIndex(x => new { x.AdvertisementId, x.Type, x.OccurredAt });
            entity.HasOne(x => x.Advertisement)
                .WithMany(x => x.Metrics)
                .HasForeignKey(x => x.AdvertisementId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
