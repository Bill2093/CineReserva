using CineReserva.Data;
using CineReserva.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineReserva.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly AppDbContext _db;
    public BookingsController(AppDbContext db) => _db = db;

    public record CreateBookingDto(int ShowId, List<int> SeatIds, string CustomerName, string CustomerEmail);

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookingDto dto)
    {
        if (dto.SeatIds is null || dto.SeatIds.Count == 0)
            return BadRequest("Debe seleccionar al menos un asiento.");

        // Validar show
        var show = await _db.Shows
            .Include(s => s.Auditorium)
            .FirstOrDefaultAsync(s => s.Id == dto.ShowId);
        if (show == null) return NotFound("Función no encontrada.");

        // Validar que los asientos pertenezcan a la sala de la función
        var seats = await _db.Seats
            .Where(s => dto.SeatIds.Contains(s.Id) && s.AuditoriumId == show.AuditoriumId)
            .Select(s => s.Id)
            .ToListAsync();
        if (seats.Count != dto.SeatIds.Count)
            return BadRequest("Uno o más asientos no pertenecen a la sala.");

        // Transacción sencilla: verificar disponibilidad y crear
        using var tx = await _db.Database.BeginTransactionAsync();

        // ¿Ya reservados?
        var taken = await _db.SeatReservations
            .Where(r => r.ShowId == dto.ShowId && dto.SeatIds.Contains(r.SeatId))
            .Select(r => r.SeatId)
            .ToListAsync();
        if (taken.Any())
            return Conflict($"Asientos ya reservados: {string.Join(",", taken)}");

        var booking = new Booking
        {
            ShowId = dto.ShowId,
            CustomerName = dto.CustomerName,
            CustomerEmail = dto.CustomerEmail
        };
        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();

        foreach (var seatId in dto.SeatIds)
        {
            _db.SeatReservations.Add(new SeatReservation
            {
                BookingId = booking.Id,
                ShowId = dto.ShowId,
                SeatId = seatId
            });
        }

        await _db.SaveChangesAsync();
        await tx.CommitAsync();

        // Devolver la reserva con sus asientos
        var result = await _db.Bookings
            .Include(b => b.Reservations)
            .FirstAsync(b => b.Id == booking.Id);

        return Ok(result);
    }

    [HttpGet("{showId}/availability")]
    public async Task<IActionResult> Availability(int showId)
    {
        var show = await _db.Shows
            .Include(s => s.Auditorium)
            .FirstOrDefaultAsync(s => s.Id == showId);
        if (show == null) return NotFound();

        var reserved = await _db.SeatReservations
            .Where(r => r.ShowId == showId)
            .Select(r => r.SeatId)
            .ToListAsync();

        var seats = await _db.Seats
            .Where(s => s.AuditoriumId == show.AuditoriumId)
            .OrderBy(s => s.Row).ThenBy(s => s.Number)
            .Select(s => new { s.Id, s.Row, s.Number, IsReserved = reserved.Contains(s.Id) })
            .ToListAsync();

        return Ok(seats);
    }
}
