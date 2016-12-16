namespace Qalsql.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class message_for_answer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HwAnswers", "Message", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HwAnswers", "Message");
        }
    }
}
