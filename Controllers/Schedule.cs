using DigitalProject.DB;
using DigitalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DigitalProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ScheduleController : ControllerBase
{

    private ApplicationDbContext DbContext;
    private UserManager<Faculty> userManager;

    public ScheduleController(ApplicationDbContext DbContext, 
        UserManager<Faculty> userManager)
    {
        this.DbContext = DbContext;
        this.userManager = userManager;
    }

    /// <summary>
    /// Creates and associates a schedule with a given class room.
    /// </summary>
    /// <param name="roomNumber">The room number.</param>
    /// <param name="model">The data required for the schedule.</param>
    /// <response code="200">The schedule was created successfully.</response>
    /// <response code="404">The room does not exist or the faculty does not exist.</response>
    /// <response code="400">An unknown error has occurred.</response>
    [HttpPut(Name = "{roomNumber}/create")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(ErrorMessageModel), 404)]
    [ProducesResponseType(typeof(ScheduleModel), 200)]
    public async Task<ActionResult<Schedule>> CreateNewSchedule(int roomNumber, PutScheduleModel model) {
        if (!ModelState.IsValid) {
            return BadRequest();
        }

        Faculty? faculty = await DbContext.Faculties.FindAsync(model.OrediggerId);
        if (faculty == null) {
            return NotFound(new ErrorMessageModel("Faculty with ID " + model.OrediggerId + " does not exist."));
        }

        Room? room = await DbContext.Rooms.FindAsync(roomNumber);
        if (room == null) {
            return NotFound(new ErrorMessageModel("Room with nubmer " + roomNumber + " does not exist."));
        }

        Schedule newSchedule = new Schedule() {
            StartTime = model.StartTime,
            EndTime = model.EndTime,
            Date = model.Date,
            CourseCode = model.CourseCode,
            FacultyId = model.OrediggerId,
            RoomNumber = roomNumber
        };

        await DbContext.Schedules.AddAsync(newSchedule);
        await DbContext.SaveChangesAsync();

        return Ok(new ScheduleModel() {
            FacultyName = faculty.getFacultyName(),
            StartTime = newSchedule.StartTime,
            EndTime = newSchedule.EndTime,
            Date = newSchedule.Date,
            CourseCode = newSchedule.CourseCode,
            RoomNumber = newSchedule.RoomNumber
        });
    }
}