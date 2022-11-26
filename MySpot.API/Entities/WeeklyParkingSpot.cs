using MySpot.API.Exceptions;

namespace MySpot.API.Entities;

public class WeeklyParkingSpot
{
    public Guid Id { get; }
    public DateTime From { get; }
    public DateTime To { get; }
    public string Name { get; }
    public IEnumerable<Reservation> Reservations => _reservations;

    private readonly HashSet<Reservation> _reservations = new();

    public WeeklyParkingSpot(Guid id, DateTime from, DateTime to, string name)
    {
        Id = id;
        From = from;
        To = to;
        Name = name;
    }

    public void AddReservation(Reservation reservation)
    {
        var isInvalidDate = reservation.Date.Date < From || reservation.Date.Date > To ||
                            reservation.Date.Date < DateTime.UtcNow.Date;
        
        if (isInvalidDate)
        {
            throw new InvalidReservationDateException(reservation.Date);
        }

        var reservationDateAlreadyExists = Reservations.Any(x =>
            x.Date.Date == reservation.Date.Date);

        if (reservationDateAlreadyExists)
        {
            throw new SpotAlreadyTaken(reservation.EmployeeName);
        }

        _reservations.Add(reservation);
    }
    
}