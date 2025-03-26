using Microsoft.EntityFrameworkCore;
using ParkManagerAPI.Models;

public class ParkManagerContext : DbContext
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<Park> Parks { get; set; }
    public required DbSet<Room> Rooms { get; set; }
    public required DbSet<Device> Devices { get; set; }
    public ParkManagerContext(DbContextOptions<ParkManagerContext> options) : base(options) { }
}