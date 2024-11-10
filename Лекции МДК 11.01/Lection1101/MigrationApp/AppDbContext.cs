using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrationApp
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Group> Groups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer("Data Source = PRSERVER\\SQLEXPRESS; Initial Catalog = ispp3502; User ID = ispp3502; Password = 3502; Trust Server Certificate=True");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //fluent API
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Group>().HasData(
                new Group { GroupId = 1, Name = "ИСПП-35"},
                new Group { GroupId = 2, Name = "ИСПП-35" }
                );
            modelBuilder.Entity<Student>().HasData(
                new Student { StudentId = 1, GroupId = 1, Name = "Каспер" },
                new Student { StudentId = 2, GroupId = 2, Name = "Афиногонов" },
                new Student { StudentId = 3, GroupId = 3, Name = "Пожидаев" }
                );
        }
    }
}
