using System.Collections.Generic;

namespace CineReserva.Models;
public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public int DurationMinutes { get; set; }
    public string Rating { get; set; } = ""; // e.g., PG-13
    public string? Description { get; set; }

    public ICollection<Show> Shows { get; set; } = new List<Show>();
}

