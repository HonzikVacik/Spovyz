using Microsoft.EntityFrameworkCore;
using Spovyz.Models;

namespace Spovyz
{
    public class ApplicationDbContext : DbContext
    {
        //Add-Migration InitialMigration -c ApplicationDbContext -o Data/Migrations
        //Update-Database

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Enums> Enums {  get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Finance> Finances { get; set; }
        public DbSet<Accounting> Accountings { get; set; }
        public DbSet<Statement> Statements { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Spovyz.Models.Task> Tasks { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Project_tag> Project_tags { get; set; }
        public DbSet<Task_tag> Task_tags { get; set; }
        public DbSet<Project_employee> Project_employees { get; set; }
        public DbSet<Task_employee> Task_employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
