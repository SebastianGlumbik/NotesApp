using Microsoft.EntityFrameworkCore;
using Server.Models;

public class Database : DbContext
    {
    public DbSet<User> User { get; set; }
    public DbSet<Note> Note { get; set; }
    public Database (DbContextOptions<Database> options)
            : base(options)
        {
        }
    }
