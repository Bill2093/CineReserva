using System;
using System.Collections.Generic;

namespace CineReserva.Models;
public class Booking
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = "";
    public string CustomerEmail { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int ShowId { get; set; }
    public Show Show { get; set; } = default!;

    public ICollection<SeatReservation> Reservations { get; set; } = new List<SeatReservation>();
}
