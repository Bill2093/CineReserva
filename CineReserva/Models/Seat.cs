using System.Collections.Generic;

namespace CineReserva.Models;
public class Seat
{
    public int Id { get; set; }
    public int AuditoriumId { get; set; }
    public Auditorium Auditorium { get; set; } = default!;
    public int Row { get; set; }    // 1..Rows
    public int Number { get; set; } // 1..SeatsPerRow

    public ICollection<SeatReservation> Reservations { get; set; } = new List<SeatReservation>();
}
