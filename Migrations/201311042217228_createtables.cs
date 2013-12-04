namespace LeaveRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createtables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HR_LR_RequestForm",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false),
                        LName = c.String(),
                        FName = c.String(),
                        Email = c.String(),
                        Comment = c.String(),
                        TypeOfLeave1 = c.String(),
                        StartDate1 = c.DateTime(nullable: false),
                        EndDate1 = c.DateTime(nullable: false),
                        TotalHours1 = c.Double(nullable: false),
                        TypeOfLeave2 = c.String(),
                        StartDate2 = c.DateTime(),
                        EndDate2 = c.DateTime(),
                        TotalHours2 = c.Double(nullable: false),
                        TypeOfLeave3 = c.String(),
                        StartDate3 = c.DateTime(),
                        EndDate3 = c.DateTime(),
                        TotalHours3 = c.Double(nullable: false),
                        PayPeriod = c.String(),
                        EmployeeSigId = c.Int(nullable: false),
                        NoMoreSigsNeeded = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.HR_LR_Signature", t => t.EmployeeSigId, cascadeDelete: true)
                .Index(t => t.EmployeeSigId);
            
            CreateTable(
                "dbo.HR_LR_Signature",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SignatureStr = c.String(),
                        LName = c.String(),
                        FName = c.String(),
                        SignedDate = c.DateTime(),
                        SignatureGuid = c.Guid(),
                        IsSignature = c.Boolean(nullable: false),
                        SignedByManager = c.Int(nullable: false),
                        RequestForm_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.HR_LR_RequestForm", t => t.RequestForm_ID)
                .Index(t => t.RequestForm_ID);
            
            CreateTable(
                "dbo.HR_LR_Manager",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LName = c.String(),
                        FName = c.String(),
                        Title = c.String(),
                        Email = c.String(),
                        IsNotified = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.HR_LR_LeaveType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.HR_PayPeriod",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Period = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.HR_LR_Signature", new[] { "RequestForm_ID" });
            DropIndex("dbo.HR_LR_RequestForm", new[] { "EmployeeSigId" });
            DropForeignKey("dbo.HR_LR_Signature", "RequestForm_ID", "dbo.HR_LR_RequestForm");
            DropForeignKey("dbo.HR_LR_RequestForm", "EmployeeSigId", "dbo.HR_LR_Signature");
            DropTable("dbo.HR_PayPeriod");
            DropTable("dbo.HR_LR_LeaveType");
            DropTable("dbo.HR_LR_Manager");
            DropTable("dbo.HR_LR_Signature");
            DropTable("dbo.HR_LR_RequestForm");
        }
    }
}
