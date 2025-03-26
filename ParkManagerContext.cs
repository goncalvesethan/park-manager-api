using Microsoft.EntityFrameworkCore;
using ParkManagerAPI.Models;
using Action = ParkManagerAPI.Models.Action;

public class ParkManagerContext : DbContext
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<Park> Parks { get; set; }
    public required DbSet<Room> Rooms { get; set; }
    public required DbSet<Device> Devices { get; set; }
    public required DbSet<Action> Actions { get; set; }
    public required DbSet<Incident> Incidents { get; set; }
    public ParkManagerContext(DbContextOptions<ParkManagerContext> options) : base(options) { }
}