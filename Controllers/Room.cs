using DigitalProject.DB;
using DigitalProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DigitalProject.Controllers;

[ApiController]
[Route("[controller]")]
public class RoomController : ControllerBase
{

    private ApplicationDbContext DbContext;
    private UserManager<Faculty> userManager;

    public RoomController(ApplicationDbContext DbContext, 
        UserManager<Faculty> userManager)
    {
        this.DbContext = DbContext;
        this.userManager = userManager;
    }

    /// <summary>
    /// Returns classroom schedule information.
    /// </summary>
    /// <param name="roomnumber">The room number.</param>
    /// <response code="200">The room exists and the schedules are returned.</response>
    /// <response code="404">The room does not exist.</response>
    /// <response code="400">An unknown error has occurred.</response>
    [HttpGet(Name = "{roomnumber}")]
    [AllowAnonymous]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(RoomNotFound), 404)]
    [ProducesResponseType(typeof(IEnumerable<GetRoomModel>), 200)]
    public ActionResult<IEnumerable<GetRoomModel>> Get(int roomnumber)
    {
        if (!doesRoomExist(roomnumber)) {
            return NotFound(new RoomNotFound() { RoomNumber = roomnumber });
        }

        List<RoomMessageModel> messages = (from rm in DbContext.RoomMessages 
                    join fa in DbContext.Faculties on rm.Author equals fa.OrediggerId
                    where rm.RoomNumber == roomnumber
                    select new RoomMessageModel() {
                        Message = rm.Message,
                        Author = fa.FirstName + " " + fa.LastName,
                        Time = rm.Time
                    }).ToList();

        List<ScheduleModel> schedules = (from r in DbContext.Rooms 
            join s in DbContext.Schedules on r.RoomNumber equals s.RoomNumber
            join f in DbContext.Faculties on s.FacultyId equals f.OrediggerId
            where r.RoomNumber == roomnumber
            select new ScheduleModel() {
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Date = s.Date,
                CourseCode = s.CourseCode,
                RoomNumber = r.RoomNumber,
                FacultyName = f.FirstName + " " + f.LastName,
            }).ToList();

        return Ok(new GetRoomModel() {
            Schedules = schedules,
            Messages = messages
        });
    }

    [HttpPut(Name = "{roomnumber}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(Room), 200)]
    [ProducesResponseType(typeof(ErrorMessageModel), 409)]
    public async Task<IActionResult> CreateRoom(int roomnumber) {
        if (!doesRoomExist(roomnumber)) {
            Room newRoom = new Room{ RoomNumber = roomnumber };
            await DbContext.Rooms.AddAsync(newRoom);
            await DbContext.SaveChangesAsync();
            return Ok(newRoom);
        }
        return Conflict(new ErrorMessageModel("Room with that number already exists!"));
    }

    /// <summary>
    /// Posts a message to a classroom.
    /// </summary>
    /// <param name="roomnumber">The room number.</param>
    /// <param name="model">The model containing the message data.</param>
    /// <response code="204">Successfully posted the message.</response>
    /// <response code="404">The room does not exist.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="400">An unknown error has occurred.</response>
    [HttpPost(Name = "{roomnumber}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(RoomNotFound), 404)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> PostRoomMessage(int roomnumber, PostRoomModel model) {
        if (!ModelState.IsValid) {
            return BadRequest();
        }

        if (!doesRoomExist(roomnumber)) {
            return NotFound(new RoomNotFound() { RoomNumber = roomnumber });
        }

        Faculty faculty = await userManager.GetUserAsync(User);
        RoomMessage messageToInsert = new RoomMessage {
            Id =  Guid.NewGuid().ToString(),
            RoomNumber = roomnumber,
            Message = model.Message,
            Time = model.Time,
            Author = faculty.OrediggerId
        };

        await DbContext.RoomMessages.AddAsync(messageToInsert);
        await DbContext.SaveChangesAsync();

        return NoContent();
    }

    private bool doesRoomExist(int roomNumber) {
        Room? room = DbContext.Rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
        return (room is not null && room.RoomNumber != -1);
    }

}