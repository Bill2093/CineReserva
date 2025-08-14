using CineReserva.Models;
using Microsoft.EntityFrameworkCore;

namespace CineReserva.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Auditorium> Auditoriums => Set<Auditorium>();
    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<Show> Shows => Set<Show>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<SeatReservation> SeatReservations => Set<SeatReservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

     
        modelBuilder.Entity<SeatReservation>()
            .HasIndex(r => new { r.ShowId, r.SeatId })
            .IsUnique();

        modelBuilder.Entity<SeatReservation>()
            .HasOne(r => r.Booking)
            .WithMany(b => b.Reservations)
            .HasForeignKey(r => r.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SeatReservation>()
            .HasOne(r => r.Show)
            .WithMany(s => s.Reservations)
            .HasForeignKey(r => r.ShowId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<SeatReservation>()
            .HasOne(r => r.Seat)
            .WithMany(s => s.Reservations)
            .HasForeignKey(r => r.SeatId)
            .OnDelete(DeleteBehavior.NoAction);

        
        modelBuilder.Entity<Seat>()
            .HasIndex(s => new { s.AuditoriumId, s.Row, s.Number })
            .IsUnique();

        
        modelBuilder.Entity<Show>()
            .HasIndex(sh => new { sh.AuditoriumId, sh.StartTime })
            .IsUnique();
    }
}
