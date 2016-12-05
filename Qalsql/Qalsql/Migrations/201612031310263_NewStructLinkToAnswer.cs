namespace Qalsql.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewStructLinkToAnswer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HwAnswers", "Exercise_Id", "dbo.HwExercises");
            DropIndex("dbo.HwAnswers", new[] { "Exercise_Id" });
            RenameColumn(table: "dbo.HwAnswers", name: "Exercise_Id", newName: "ExeId");
            AddColumn("dbo.HwAnswers", "Passed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.HwAnswers", "ExeId", c => c.Int(nullable: false));
            CreateIndex("dbo.HwAnswers", "ExeId");
            AddForeignKey("dbo.HwAnswers", "ExeId", "dbo.HwExercises", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HwAnswers", "ExeId", "dbo.HwExercises");
            DropIndex("dbo.HwAnswers", new[] { "ExeId" });
            AlterColumn("dbo.HwAnswers", "ExeId", c => c.Int());
            DropColumn("dbo.HwAnswers", "Passed");
            RenameColumn(table: "dbo.HwAnswers", name: "ExeId", newName: "Exercise_Id");
            CreateIndex("dbo.HwAnswers", "Exercise_Id");
            AddForeignKey("dbo.HwAnswers", "Exercise_Id", "dbo.HwExercises", "Id");
        }
    }
}
