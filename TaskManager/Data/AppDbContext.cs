using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :  base(options){ }
    
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<CategoryItem> Category { get; set; }
    
    public DbSet<User> Users { get; set; }
    public DbSet<AccessRight> AccessRights { get; set; }
    public DbSet<AccessLog> AccessLogs { get; set; }
    public DbSet<Door> Doors { get; set; }
    public DbSet<QrCode> QrCodes { get; set; }
    public DbSet<Struttura> Strutture { get; set; }
    
}