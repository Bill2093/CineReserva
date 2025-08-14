using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineReserva.Models;
public class Show
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = default!;
    public int AuditoriumId { get; set; }
    public Auditorium Auditorium { get; set; } = default!;
    public DateTime StartTime { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    public ICollection<SeatReservation> Reservations { get; set; } = new List<SeatReservation>();
}
