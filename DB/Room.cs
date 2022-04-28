using System.ComponentModel.DataAnnotations;

namespace DigitalProject.DB;

public class Room 
{
    [Key]
    [Required]
    public int RoomNumber { get; set; } = -1;

}
