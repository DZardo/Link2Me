using Microsoft.EntityFrameworkCore;

namespace Link2Me.Models
{
    public class Link2MeContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<User> Users { get; set; }

        public Link2MeContext(DbContextOptions<Link2MeContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Link2MeDB;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friend>()
                .HasKey(f => new { f.UserEmployeeId, f.FriendId });
        }
    }
}