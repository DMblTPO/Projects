using System.Data.Entity;

namespace Qalsql.Models.Db
{
    public partial class QalSqlContext
    {
        public QalSqlContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<QalSqlContext, Migrations.Configuration>("DefaultConnection"));
        }

        public DbSet<HwExercise> HwExercises { get; set; }

        public DbSet<HwAnswer> HwAnswers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}