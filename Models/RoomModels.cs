using System.ComponentModel.DataAnnotations;

namespace DigitalProject.Models;

public class GetRoomModel {
    public List<ScheduleModel> Schedules { get; set; } = new List<ScheduleModel>();
    public List<RoomMessageModel> Messages { get; set; } = new List<RoomMessageModel>();
}

public class ScheduleModel {
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public string? Date { get; set; }
    [Required]
    public string? CourseCode { get; set; }
    [Required]
    public int RoomNumber { get; set; }
    public string? FacultyName { get; set; }
}

public class RoomMessageModel {
    public string? Message { get; set; } = "Do androids dream of electric sheep?";
    public string? Author { get; set; } = "Unknown Author";
    public string? Time { get; set; } = "Unknown Time";
}

public class PostRoomModel {
    [Required]
    public string? Message { get; set; }
    [Required]
    public string? Time { get; set; }
}

public class RoomNotFound {
    public int RoomNumber { get; set; }
}