namespace CineReserva.Models;
public class SeatReservation
{
    public int Id { get; set; }

    public int BookingId { get; set; }
    public Booking Booking { get; set; } = default!;

    public int ShowId { get; set; }
    public Show Show { get; set; } = default!;

    public int SeatId { get; set; }
    public Seat Seat { get; set; } = default!;
}

