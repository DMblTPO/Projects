using System.Data.Entity;

namespace Qalsql.Models.Db
{
    public class SqlHwCheckerContext : DbContext
    {
        public SqlHwCheckerContext() : base("SqlHwCheckerContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SqlHwCheckerContext, Migrations.Configuration>("SqlHwCheckerContext"));
        }

        public DbSet<HwExercise> HwExercises { get; set; }
        public DbSet<HwAnswer> HwAnswers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}