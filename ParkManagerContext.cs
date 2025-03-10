using Microsoft.EntityFrameworkCore;
using ParkManagerAPI.Models;

public class ParkManagerContext : DbContext
{
    public required DbSet<User> Users { get; set; }
    public ParkManagerContext(DbContextOptions<ParkManagerContext> options) : base(options) { }
}