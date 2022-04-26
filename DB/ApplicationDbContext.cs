using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DigitalProject.DB;

public class ApplicationDbContext : IdentityDbContext<Faculty>
{
    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<RoomMessage> RoomMessages { get; set; }

    public string DbPath { get; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "digger.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

}
