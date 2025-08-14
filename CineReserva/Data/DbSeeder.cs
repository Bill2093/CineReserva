using CineReserva.Models;
using Microsoft.EntityFrameworkCore;

namespace CineReserva.Data;
public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync();

        if (!await db.Auditoriums.AnyAsync())
        {
            var sala1 = new Auditorium { Name = "Sala 1", Rows = 5, SeatsPerRow = 8 };
            db.Auditoriums.Add(sala1);
            await db.SaveChangesAsync();

            // Generar asientos
            for (int r = 1; r <= sala1.Rows; r++)
                for (int n = 1; n <= sala1.SeatsPerRow; n++)
                    db.Seats.Add(new Seat { AuditoriumId = sala1.Id, Row = r, Number = n });

            var m1 = new Movie { Title = "Interstellar", DurationMinutes = 169, Rating = "PG-13", Description = "Sci-fi" };
            var m2 = new Movie { Title = "Inception", DurationMinutes = 148, Rating = "PG-13", Description = "Mind-bender" };
            db.Movies.AddRange(m1, m2);
            await db.SaveChangesAsync();

            db.Shows.Add(new Show { MovieId = m1.Id, AuditoriumId = sala1.Id, StartTime = DateTime.Today.AddHours(18), Price = 5.00m });
            db.Shows.Add(new Show { MovieId = m2.Id, AuditoriumId = sala1.Id, StartTime = DateTime.Today.AddHours(21), Price = 5.50m });

            await db.SaveChangesAsync();
        }
    }
}

