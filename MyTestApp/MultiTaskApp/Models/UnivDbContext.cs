using System.Data.Entity;

namespace MultiTaskApp.Models
{
    public class UnivDbContext : DbContext
    {
        public UnivDbContext()
            : base("name=UnivDBConnectionString")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<UnivDbContext>());
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<University> Universities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Configure domain classes using modelBuilder here..
            base.OnModelCreating(modelBuilder);
        }
    }
}