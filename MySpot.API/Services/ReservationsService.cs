using MySpot.API.Commands;
using MySpot.API.DTO;
using MySpot.API.Entities;

namespace MySpot.API.Services;

public class ReservationService
{
    private static readonly List<WeeklyParkingSpot> WeeklyParkingSpots = new()
    {
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-00000000001"), DateTime.UtcNow, DateTime.UtcNow.AddDays(10), "P1"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-00000000002"), DateTime.UtcNow, DateTime.UtcNow.AddDays(10), "P2"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-00000000003"), DateTime.UtcNow, DateTime.UtcNow.AddDays(10), "P3"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-00000000004"), DateTime.UtcNow, DateTime.UtcNow.AddDays(10), "P4"),
    };

    public ReservationDTO Get(Guid id) => GetAllWeekly().SingleOrDefault(x => x.Id == id);

    public IEnumerable<ReservationDTO> GetAllWeekly()
        => WeeklyParkingSpots.SelectMany(x => x.Reservations).Select(x => new ReservationDTO
        {
            Id = x.Id,
            Date = x.Date,
            ParkingSpotId = x.ParkingSpotId,
            EmployeeName = x.EmployeeName
        });

    public Guid? Create(CreateReservation command)
    {
        var weeklyParkingSpot = WeeklyParkingSpots.SingleOrDefault(
            x => x.Id == command.ParkingSpotId);

        if (weeklyParkingSpot is null)
        {
            return default;
        }

        var reservation = new Reservation(command.ReservationId, command.ParkingSpotId, command.EmployeeName,
            command.LicensePlate, command.Date);
        
        weeklyParkingSpot.AddReservation(reservation);

        return reservation.Id;
    }

    public bool Delete(DeleteReservation command)
    {
        var reservation = WeeklyParkingSpots.SingleOrDefault(
            x => x.Id == command.ReservationId);
        
        if (reservation is null)
        {
            return false;
        }

        WeeklyParkingSpots.Remove(reservation);
        return true;
    }

    public bool Update(ChangeReservationLicensePlate command)
    {
        var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);

        if (weeklyParkingSpot is null)
        {
            return false;
        }
        
        var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(x => x.Id == command.ReservationId);
        if (existingReservation is null)
        {
            return false;
        }

        if (existingReservation.Date <= DateTime.UtcNow)
        {
            return false;
        }

        existingReservation.ChangeLicensePlate(command.LicensePlate);
        
        return true;
    }

    private static WeeklyParkingSpot GetWeeklyParkingSpotByReservation(Guid commandReservationId)
        => WeeklyParkingSpots.SingleOrDefault(x => x.Reservations.Any(r => r.Id == commandReservationId));
}


