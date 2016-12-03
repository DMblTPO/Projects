namespace Qalsql.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PassesNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HwAnswers", "Passed", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HwAnswers", "Passed", c => c.Boolean(nullable: false));
        }
    }
}
