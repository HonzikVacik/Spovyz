using Microsoft.EntityFrameworkCore;
using Spovyz.Models;

namespace Spovyz
{
    public class ApplicationDbContext : DbContext
    {
        //Add-Migration InitialMigration -c ApplicationDbContext -o Migrations
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
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Enums>().HasNoKey();

            //Project_tag
            modelBuilder.Entity<Project_tag>()
                .HasKey(pt => new { pt.ProjectId, pt.TagId });

            modelBuilder.Entity<Project_tag>()
                .HasOne(pt => pt.Project)
                .WithMany(p => p.Project_tags)
                .HasForeignKey(pt => pt.ProjectId);

            modelBuilder.Entity<Project_tag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.Project_tags)
                .HasForeignKey(pt => pt.TagId);


            //Project_employee
            modelBuilder.Entity<Project_employee>()
                .HasKey(pt => new { pt.ProjectId, pt.EmployeeId });

            modelBuilder.Entity<Project_employee>()
                .HasOne(pt => pt.Project)
                .WithMany(p => p.Project_employees)
                .HasForeignKey(pt => pt.ProjectId);

            modelBuilder.Entity<Project_employee>()
                .HasOne(pt => pt.Employee)
                .WithMany(t => t.Project_employees)
                .HasForeignKey(pt => pt.EmployeeId);


            //Task_tag
            modelBuilder.Entity<Task_tag>()
                .HasKey(pt => new { pt.TaskId, pt.TagId });

            modelBuilder.Entity<Task_tag>()
                .HasOne(pt => pt.Task)
                .WithMany(p => p.Task_tags)
                .HasForeignKey(pt => pt.TaskId);

            modelBuilder.Entity<Task_tag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.Task_tags)
                .HasForeignKey(pt => pt.TagId);


            //Task_employee
            modelBuilder.Entity<Task_employee>()
                .HasKey(pt => new { pt.TaskId, pt.EmployeeId });

            modelBuilder.Entity<Task_employee>()
                .HasOne(pt => pt.Task)
                .WithMany(p => p.Task_employees)
                .HasForeignKey(pt => pt.TaskId);

            modelBuilder.Entity<Task_employee>()
                .HasOne(pt => pt.Employee)
                .WithMany(t => t.Task_employees)
                .HasForeignKey(pt => pt.EmployeeId);
        }
    }
}
