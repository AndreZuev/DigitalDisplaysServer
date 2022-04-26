using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalProject.DB;

public class RoomMessage {
    [Key]
    [Required]
    public string? Id { get; set; }
    [Required]
    public int RoomNumber { get; set; }
    [Required]
    public string? Message { get; set; }
    [Required]
    public string? Author { get; set; }
    [Required]
    public string? Time { get; set; }
}