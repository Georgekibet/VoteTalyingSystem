namespace vts.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MD_Candidate",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Surname = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        MiddleName = c.String(),
                        IdCardNumber = c.String(nullable: false),
                        PassportNumber = c.String(),
                        Race_Name = c.String(nullable: false),
                        Race_RaceType = c.Int(nullable: false),
                        Race_Id = c.Guid(nullable: false),
                        PoliticalParty_Name = c.String(),
                        PoliticalParty_Id = c.Guid(nullable: false),
                        CandidateType = c.Int(nullable: false),
                        EmailAddress = c.String(),
                        Symbol = c.Binary(),
                        Photo = c.Binary(),
                        RunningMateName = c.String(),
                        RunningMateIdCardNumber = c.String(),
                        CandidateStatus = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastUpdated = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MD_Constituency",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        County_Name = c.String(),
                        County_Id = c.Guid(nullable: false),
                        Code = c.String(),
                        TotalRegisteredVoters = c.Int(nullable: false),
                        PopulationOver18 = c.Int(nullable: false),
                        Area = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NumberOfWards = c.Int(nullable: false),
                        NumberOfPollingStations = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastUpdated = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MD_ConstituencyMps",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Constituency_Name = c.String(),
                        Constituency_Id = c.Guid(nullable: false),
                        Candidate_FullName = c.String(),
                        Candidate_CandidateType = c.Int(nullable: false),
                        Candidate_Id = c.Guid(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastUpdated = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Constituency_Id1 = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MD_Constituency", t => t.Constituency_Id1)
                .Index(t => t.Constituency_Id1);
            
            CreateTable(
                "dbo.MD_County",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Region_Name = c.String(),
                        Region_Id = c.Guid(nullable: false),
                        Code = c.String(nullable: false),
                        TotalRegisteredVoters = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastUpdated = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MD_CountyGovernors",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        County_Name = c.String(),
                        County_Id = c.Guid(nullable: false),
                        Candidate_FullName = c.String(),
                        Candidate_CandidateType = c.Int(nullable: false),
                        Candidate_Id = c.Guid(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastUpdated = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        County_Id1 = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MD_County", t => t.County_Id1)
                .Index(t => t.County_Id1);
            
            CreateTable(
                "dbo.MD_CountySenators",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        County_Name = c.String(),
                        County_Id = c.Guid(nullable: false),
                        Candidate_FullName = c.String(),
                        Candidate_CandidateType = c.Int(nullable: false),
                        Candidate_Id = c.Guid(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastUpdated = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        County_Id1 = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MD_County", t => t.County_Id1)
                .Index(t => t.County_Id1);
            
            CreateTable(
                "dbo.MD_CountyWomenReps",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        County_Name = c.String(),
                        County_Id = c.Guid(nullable: false),
                        Candidate_FullName = c.String(),
                        Candidate_CandidateType = c.Int(nullable: false),
                        Candidate_Id = c.Guid(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastUpdated = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        County_Id1 = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MD_County", t => t.County_Id1)
                .Index(t => t.County_Id1);
            
            CreateTable(
                "dbo.MD_Election",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        NominationStartDate = c.DateTime(nullable: false),
                        NominationEndDate = c.DateTime(nullable: false),
                        ElectionType = c.Int(nullable: false),
                        Location = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastUpdated = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Results_GubernatorialLineItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Candidate_FullName = c.String(),
                        Candidate_CandidateType = c.Int(nullable: false),
                        Candidate_Id = c.Guid(nullable: false),
                        ResultCount = c.Int(nullable: false),
                        ModifiedCount = c.Int(nullable: false),
                        ReceivedTime = c.DateTime(nullable: false),
                        GubernatorialResult_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Results_Gubernatorial", t => t.GubernatorialResult_Id)
                .Index(t => t.GubernatorialResult_Id);
            
            CreateTable(
                "dbo.Results_Gubernatorial",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ResultReference = c.String(),
                        ResultSender_Username = c.String(),
                        ResultSender_UserType = c.Int(nullable: false),
                        ResultSender_Id = c.Guid(nullable: false),
                        PollingCentre_Name = c.String(),
                        PollingCentre_Id = c.Guid(nullable: false),
                        ResultSendDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        LastResultCommandExecutedOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Results_McaLineItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Candidate_FullName = c.String(),
                        Candidate_CandidateType = c.Int(nullable: false),
                        Candidate_Id = c.Guid(nullable: false),
                        ResultCount = c.Int(nullable: false),
                        ModifiedCount = c.Int(nullable: false),
                        ReceivedTime = c.DateTime(nullable: false),
                        McaResult_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Results_Mca", t => t.McaResult_Id)
                .Index(t => t.McaResult_Id);
            
            CreateTable(
                "dbo.Results_Mca",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ResultReference = c.String(),
                        ResultSender_Username = c.String(),
                        ResultSender_UserType = c.Int(nullable: false),
                        ResultSender_Id = c.Guid(nullable: false),
                        PollingCentre_Name = c.String(),
                        PollingCentre_Id = c.Guid(nullable: false),
                        ResultSendDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        LastResultCommandExecutedOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Results_MpLineItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Candidate_FullName = c.String(),
                        Candidate_CandidateType = c.Int(nullable: false),
                        Candidate_Id = c.Guid(nullable: false),
                        ResultCount = c.Int(nullable: false),
                        ModifiedCount = c.Int(nullable: false),
                        ReceivedTime = c.DateTime(nullable: false),
                        MpResult_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Results_Mp", t => t.MpResult_Id)
                .Index(t => t.MpResult_Id);
            
            CreateTable(
                "dbo.Results_Mp",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ResultReference = c.String(),
                        ResultSender_Username = c.String(),
                        ResultSender_UserType = c.Int(nullable: false),
                        ResultSender_Id = c.Guid(nullable: false),
                        PollingCentre_Name = c.String(),
                        PollingCentre_Id = c.Guid(nullable: false),
                        ResultSendDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        LastResultCommandExecutedOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MD_PoliticalParty",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Acronym = c.String(),
                        DateRegistered = c.DateTime(nullable: false),
                        ApprovedSymbol = c.Binary(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastUpdated = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MD_PollingCentre",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Ward_Name = c.String(),
                        Ward_Id = c.Guid(nullable: false),
                        Code = c.String(nullable: false),
                        RegisteredVoters = c.Int(nullable: false),
                        Streams = c.Int(nullable: false),
                        PollingCentreType = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastUpdated = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Results_PresidentialLineItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Candidate_FullName = c.String(),
                        Candidate_CandidateType = c.Int(nullable: false),
                        Candidate_Id = c.Guid(nullable: false),
                        ResultCount = c.Int(nullable: false),
                        ModifiedCount = c.Int(nullable: false),
                        ReceivedTime = c.DateTime(nullable: false),
                        PresidentialResult_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Results_Presidential", t => t.PresidentialResult_Id)
                .Index(t => t.PresidentialResult_Id);
            
            CreateTable(
                "dbo.Results_Presidential",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ResultReference = c.String(),
                        ResultSender_Username = c.String(),
                        ResultSender_UserType = c.Int(nullable: false),
                        ResultSender_Id = c.Guid(nullable: false),
                        PollingCentre_Name = c.String(),
                        PollingCentre_Id = c.Guid(nullable: false),
                        ResultSendDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        LastResultCommandExecutedOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MD_Race",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        RaceType = c.Int(nullable: false),
                        Election_Name = c.String(nullable: false),
                        Election_Id = c.Guid(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastUpdated = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Results_ReferendumLineItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Candidate_FullName = c.String(),
                        Candidate_CandidateType = c.Int(nullable: false),
                        Candidate_Id = c.Guid(nullable: false),
                        ResultCount = c.Int(nullable: false),
                        ModifiedCount = c.Int(nullable: false),
                        ReceivedTime = c.DateTime(nullable: false),
                        ReferendumResult_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Results_Referendum", t => t.ReferendumResult_Id)
                .Index(t => t.ReferendumResult_Id);
            
            CreateTable(
                "dbo.Results_Referendum",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ResultReference = c.String(),
                        ResultSender_Username = c.String(),
                        ResultSender_UserType = c.Int(nullable: false),
                        ResultSender_Id = c.Guid(nullable: false),
                        PollingCentre_Name = c.String(),
                        PollingCentre_Id = c.Guid(nullable: false),
                        ResultSendDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        LastResultCommandExecutedOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MD_Region",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Code = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastUpdated = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User_Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.User_UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.User_Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.User_Users", t => t.IdentityUser_Id)
                .Index(t => t.RoleId)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.Results_SenatorialLineItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Candidate_FullName = c.String(),
                        Candidate_CandidateType = c.Int(nullable: false),
                        Candidate_Id = c.Guid(nullable: false),
                        ResultCount = c.Int(nullable: false),
                        ModifiedCount = c.Int(nullable: false),
                        ReceivedTime = c.DateTime(nullable: false),
                        SenatorialResult_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Results_Senatorial", t => t.SenatorialResult_Id)
                .Index(t => t.SenatorialResult_Id);
            
            CreateTable(
                "dbo.Results_Senatorial",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ResultReference = c.String(),
                        ResultSender_Username = c.String(),
                        ResultSender_UserType = c.Int(nullable: false),
                        ResultSender_Id = c.Guid(nullable: false),
                        PollingCentre_Name = c.String(),
                        PollingCentre_Id = c.Guid(nullable: false),
                        ResultSendDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        LastResultCommandExecutedOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MD_Settings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Key = c.Int(nullable: false),
                        Value = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastUpdated = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User_Users",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Mobile = c.String(),
                        MainRole = c.Int(),
                        Status = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.User_UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User_Users", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.User_UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.User_Users", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.MD_WardMcas",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Ward_Name = c.String(),
                        Ward_Id = c.Guid(nullable: false),
                        Candidate_FullName = c.String(),
                        Candidate_CandidateType = c.Int(nullable: false),
                        Candidate_Id = c.Guid(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastUpdated = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Ward_Id1 = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MD_Ward", t => t.Ward_Id1)
                .Index(t => t.Ward_Id1);
            
            CreateTable(
                "dbo.MD_Ward",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Constituency_Name = c.String(),
                        Constituency_Id = c.Guid(nullable: false),
                        Code = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastUpdated = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Results_WomenRepLineItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Candidate_FullName = c.String(),
                        Candidate_CandidateType = c.Int(nullable: false),
                        Candidate_Id = c.Guid(nullable: false),
                        ResultCount = c.Int(nullable: false),
                        ModifiedCount = c.Int(nullable: false),
                        ReceivedTime = c.DateTime(nullable: false),
                        WomenRepResult_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Results_WomenRep", t => t.WomenRepResult_Id)
                .Index(t => t.WomenRepResult_Id);
            
            CreateTable(
                "dbo.Results_WomenRep",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ResultReference = c.String(),
                        ResultSender_Username = c.String(),
                        ResultSender_UserType = c.Int(nullable: false),
                        ResultSender_Id = c.Guid(nullable: false),
                        PollingCentre_Name = c.String(),
                        PollingCentre_Id = c.Guid(nullable: false),
                        ResultSendDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        LastResultCommandExecutedOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User_UserRoles", "IdentityUser_Id", "dbo.User_Users");
            DropForeignKey("dbo.User_UserLogins", "IdentityUser_Id", "dbo.User_Users");
            DropForeignKey("dbo.User_UserClaims", "IdentityUser_Id", "dbo.User_Users");
            DropForeignKey("dbo.Results_WomenRepLineItems", "WomenRepResult_Id", "dbo.Results_WomenRep");
            DropForeignKey("dbo.MD_WardMcas", "Ward_Id1", "dbo.MD_Ward");
            DropForeignKey("dbo.Results_SenatorialLineItems", "SenatorialResult_Id", "dbo.Results_Senatorial");
            DropForeignKey("dbo.User_UserRoles", "RoleId", "dbo.User_Roles");
            DropForeignKey("dbo.Results_ReferendumLineItems", "ReferendumResult_Id", "dbo.Results_Referendum");
            DropForeignKey("dbo.Results_PresidentialLineItems", "PresidentialResult_Id", "dbo.Results_Presidential");
            DropForeignKey("dbo.Results_MpLineItems", "MpResult_Id", "dbo.Results_Mp");
            DropForeignKey("dbo.Results_McaLineItems", "McaResult_Id", "dbo.Results_Mca");
            DropForeignKey("dbo.Results_GubernatorialLineItems", "GubernatorialResult_Id", "dbo.Results_Gubernatorial");
            DropForeignKey("dbo.MD_CountyWomenReps", "County_Id1", "dbo.MD_County");
            DropForeignKey("dbo.MD_CountySenators", "County_Id1", "dbo.MD_County");
            DropForeignKey("dbo.MD_CountyGovernors", "County_Id1", "dbo.MD_County");
            DropForeignKey("dbo.MD_ConstituencyMps", "Constituency_Id1", "dbo.MD_Constituency");
            DropIndex("dbo.Results_WomenRepLineItems", new[] { "WomenRepResult_Id" });
            DropIndex("dbo.MD_WardMcas", new[] { "Ward_Id1" });
            DropIndex("dbo.User_UserLogins", new[] { "IdentityUser_Id" });
            DropIndex("dbo.User_UserClaims", new[] { "IdentityUser_Id" });
            DropIndex("dbo.User_Users", "UserNameIndex");
            DropIndex("dbo.Results_SenatorialLineItems", new[] { "SenatorialResult_Id" });
            DropIndex("dbo.User_UserRoles", new[] { "IdentityUser_Id" });
            DropIndex("dbo.User_UserRoles", new[] { "RoleId" });
            DropIndex("dbo.User_Roles", "RoleNameIndex");
            DropIndex("dbo.Results_ReferendumLineItems", new[] { "ReferendumResult_Id" });
            DropIndex("dbo.Results_PresidentialLineItems", new[] { "PresidentialResult_Id" });
            DropIndex("dbo.Results_MpLineItems", new[] { "MpResult_Id" });
            DropIndex("dbo.Results_McaLineItems", new[] { "McaResult_Id" });
            DropIndex("dbo.Results_GubernatorialLineItems", new[] { "GubernatorialResult_Id" });
            DropIndex("dbo.MD_CountyWomenReps", new[] { "County_Id1" });
            DropIndex("dbo.MD_CountySenators", new[] { "County_Id1" });
            DropIndex("dbo.MD_CountyGovernors", new[] { "County_Id1" });
            DropIndex("dbo.MD_ConstituencyMps", new[] { "Constituency_Id1" });
            DropTable("dbo.Results_WomenRep");
            DropTable("dbo.Results_WomenRepLineItems");
            DropTable("dbo.MD_Ward");
            DropTable("dbo.MD_WardMcas");
            DropTable("dbo.User_UserLogins");
            DropTable("dbo.User_UserClaims");
            DropTable("dbo.User_Users");
            DropTable("dbo.MD_Settings");
            DropTable("dbo.Results_Senatorial");
            DropTable("dbo.Results_SenatorialLineItems");
            DropTable("dbo.User_UserRoles");
            DropTable("dbo.User_Roles");
            DropTable("dbo.MD_Region");
            DropTable("dbo.Results_Referendum");
            DropTable("dbo.Results_ReferendumLineItems");
            DropTable("dbo.MD_Race");
            DropTable("dbo.Results_Presidential");
            DropTable("dbo.Results_PresidentialLineItems");
            DropTable("dbo.MD_PollingCentre");
            DropTable("dbo.MD_PoliticalParty");
            DropTable("dbo.Results_Mp");
            DropTable("dbo.Results_MpLineItems");
            DropTable("dbo.Results_Mca");
            DropTable("dbo.Results_McaLineItems");
            DropTable("dbo.Results_Gubernatorial");
            DropTable("dbo.Results_GubernatorialLineItems");
            DropTable("dbo.MD_Election");
            DropTable("dbo.MD_CountyWomenReps");
            DropTable("dbo.MD_CountySenators");
            DropTable("dbo.MD_CountyGovernors");
            DropTable("dbo.MD_County");
            DropTable("dbo.MD_ConstituencyMps");
            DropTable("dbo.MD_Constituency");
            DropTable("dbo.MD_Candidate");
        }
    }
}
