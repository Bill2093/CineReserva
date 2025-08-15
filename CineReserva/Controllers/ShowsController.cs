using CineReserva.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineReserva.Controllers;
public class ShowsController : Controller
{
    private readonly AppDbContext _db;
    public ShowsController(AppDbContext db) => _db = db;

  
    public async Task<IActionResult> Index()
    {
        var shows = await _db.Shows
            .Include(s => s.Movie)
            .Include(s => s.Auditorium)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
        return View(shows);
    }

    public async Task<IActionResult> Seats(int id)
    {
        var show = await _db.Shows
            .Include(s => s.Auditorium)
            .Include(s => s.Movie)
            .FirstOrDefaultAsync(s => s.Id == id);
        if (show == null) return NotFound();

        var reserved = await _db.SeatReservations
            .Where(r => r.ShowId == id)
            .Select(r => r.SeatId)
            .ToListAsync();

        var seats = await _db.Seats
            .Where(s => s.AuditoriumId == show.AuditoriumId)
            .OrderBy(s => s.Row).ThenBy(s => s.Number)
            .ToListAsync();

        ViewBag.ReservedIds = reserved;
        return View((show, seats));
    }
}

