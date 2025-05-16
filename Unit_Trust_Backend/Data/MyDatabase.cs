using Microsoft.EntityFrameworkCore;
using Unit_Trust_Backend.Investor.Models;
using Unit_Trust_Backend.Staff.Models;

namespace Unit_Trust_Backend.Data
{
    public class MyDatabase : DbContext
    {
        public MyDatabase(DbContextOptions<MyDatabase> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Investors> Investors {get; set;}
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Staffs> Staffs {  get; set; }
        public DbSet<Campaign> Campaign { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Project> Project { get; set; }
    }
}
