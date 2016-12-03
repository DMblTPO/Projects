namespace Qalsql.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HwAnswers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HwAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User = c.String(),
                        Query = c.String(),
                        Exercise_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HwExercises", t => t.Exercise_Id)
                .Index(t => t.Exercise_Id);
            
            CreateTable(
                "dbo.HwExercises",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LessonId = c.Int(nullable: false),
                        ExerciseNum = c.Int(nullable: false),
                        Conditions = c.String(),
                        QueryCheck = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HwAnswers", "Exercise_Id", "dbo.HwExercises");
            DropIndex("dbo.HwAnswers", new[] { "Exercise_Id" });
            DropTable("dbo.HwExercises");
            DropTable("dbo.HwAnswers");
        }
    }
}
