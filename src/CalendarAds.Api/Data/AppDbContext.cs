using CalendarAds.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace CalendarAds.Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<CalendarEvent> CalendarEvents => Set<CalendarEvent>();

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
    }
}
