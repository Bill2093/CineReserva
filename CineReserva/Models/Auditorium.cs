using System.Collections.Generic;

namespace CineReserva.Models;
public class Auditorium
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Rows { get; set; }
    public int SeatsPerRow { get; set; }

    public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    public ICollection<Show> Shows { get; set; } = new List<Show>();
}
