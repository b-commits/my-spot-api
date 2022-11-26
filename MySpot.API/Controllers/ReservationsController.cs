using Microsoft.AspNetCore.Mvc;
using MySpot.API.Commands;
using MySpot.API.Entities;
using MySpot.API.Services;

namespace MySpot.API.Controllers;

[Route("[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly ReservationService _reservationService;

    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> Get() => Ok(_reservationService.GetAllWeekly());

    [HttpGet("{id:int}")]
    public ActionResult<Reservation> GetById(Guid id)
    {
        var reservation = _reservationService.Get(id);
        if (reservation is null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult Post(CreateReservation command)
    {
        var id = _reservationService.Create(command with { ReservationId = Guid.NewGuid() });

        if (id is null)
        {
            return BadRequest();
        }

        return Ok(id);
    }

    [HttpPut("{id:guid}")]
    public ActionResult Put(Guid id, ChangeReservationLicensePlate command)
    {
        if (_reservationService.Update(command with { ReservationId = id }))
        {
            return NoContent();
        }

        return NotFound();
    }

    [HttpDelete("{id:guid}")]
    public ActionResult Delete(Guid id)
    {
        if (_reservationService.Delete(new DeleteReservation(id)))
        {
            return NoContent();
        }

        return NotFound();
    }

}