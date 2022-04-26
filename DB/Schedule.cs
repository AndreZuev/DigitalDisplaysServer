using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalProject.DB;

public class Schedule 
{
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    [Required]
    public string? Id { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public string? Date { get; set; }
    [Required]
    public string? CourseCode { get; set; }
    [Required]
    public int RoomNumber { get; set; }
    [Required]
    public string? FacultyId { get; set; }
}

