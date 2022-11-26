namespace MySpot.API.Commands;

public record CreateReservation(
    Guid ParkingSpotId, 
    Guid ReservationId, 
    string EmployeeName, 
    string LicensePlate,
    DateTime Date
    );