using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DigitalProject.DB;

public class Faculty : IdentityUser
{
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    [Required]
    public int OfficeId { get; set; }
    [Key]
    [Required]
    public string? OrediggerId { get; set; }
}

