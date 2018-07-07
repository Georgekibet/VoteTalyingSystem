using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using vts.Core.Shared.Entities.Master;
using vts.Core.TransactionalEntities;
using vts.Shared.Entities.Master;

namespace vts.Data.Context
{
    public class VtsContext : DbContext
    {
        public VtsContext(string connectionString) : base(connectionString)
        {
        }

        public VtsContext()
        {
        }

        #region Master Data

        public DbSet<User> Users { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Election> Elections { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<PoliticalParty> PoliticalParties { get; set; }
        public DbSet<PollingCentre> PollingCentres { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<WardMcas> WardMcas { get; set; }
        public DbSet<Constituency> Constituencies { get; set; }
        public DbSet<ConstituencyMps> ConstituencyMps { get; set; }
        public DbSet<County> Counties { get; set; }
        public DbSet<CountyGovernors> CountyGoverners { get; set; }
        public DbSet<CountySenators> CountySenators { get; set; }
        public DbSet<CountyWomenReps> CountyWomenReps { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Settings> Settings { get; set; }

        #endregion Master Data

        #region Results

        public DbSet<PresidentialResult> PresidentialResults { get; set; }
        public DbSet<PresidentialResultLineItem> PresidentialResultLineItems { get; set; }
        public DbSet<GubernatorialResult> GubernatorialResults { get; set; }
        public DbSet<GubernatorialResultLineItem> GubernatorialResultLineItems { get; set; }
        public DbSet<SenatorialResult> SenatorialResults { get; set; }
        public DbSet<SenatorialResultLineItem> SenatorialResultLineItems { get; set; }
        public DbSet<MpResult> MpResults { get; set; }
        public DbSet<MpResultLineItem> MpResultLineItems { get; set; }
        public DbSet<McaResult> McaResults { get; set; }
        public DbSet<McaResultLineItem> McaResultLineItems { get; set; }
        public DbSet<WomenRepResult> WomenRepResults { get; set; }
        public DbSet<WomenRepResultLineItem> WomenRepResultLineItems { get; set; }
        public DbSet<ReferendumResult> ReferendumResults { get; set; }
        public DbSet<ReferendumResultLineItem> ReferendumResultLineItems { get; set; }

        #endregion Results

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
            Configuration.ProxyCreationEnabled = false;

            #region Master Data

            modelBuilder.Entity<User>().ToTable("MD_User");
            modelBuilder.Entity<Candidate>().ToTable("MD_Candidate");
            modelBuilder.Entity<Election>().ToTable("MD_Election");
            modelBuilder.Entity<Race>().ToTable("MD_Race");
            modelBuilder.Entity<PoliticalParty>().ToTable("MD_PoliticalParty");
            modelBuilder.Entity<PollingCentre>().ToTable("MD_PollingCentre");
            modelBuilder.Entity<Ward>().ToTable("MD_Ward");
            modelBuilder.Entity<WardMcas>().ToTable("MD_WardMcas");
            modelBuilder.Entity<Constituency>().ToTable("MD_Constituency");
            modelBuilder.Entity<ConstituencyMps>().ToTable("MD_ConstituencyMps");
            modelBuilder.Entity<County>().ToTable("MD_County");
            modelBuilder.Entity<CountySenators>().ToTable("MD_CountySenators");
            modelBuilder.Entity<CountyGovernors>().ToTable("MD_CountyGovernors");
            modelBuilder.Entity<CountyWomenReps>().ToTable("MD_CountyWomenReps");
            modelBuilder.Entity<Region>().ToTable("MD_Region");
            modelBuilder.Entity<Settings>().ToTable("MD_Settings");

            #endregion Master Data

            #region Results

            modelBuilder.Entity<PresidentialResult>().ToTable("Results_Presidential");
            modelBuilder.Entity<PresidentialResultLineItem>().ToTable("Results_PresidentialLineItems");
            modelBuilder.Entity<GubernatorialResult>().ToTable("Results_Gubernatorial");
            modelBuilder.Entity<GubernatorialResultLineItem>().ToTable("Results_GubernatorialLineItems");
            modelBuilder.Entity<SenatorialResult>().ToTable("Results_Senatorial");
            modelBuilder.Entity<SenatorialResultLineItem>().ToTable("Results_SenatorialLineItems");
            modelBuilder.Entity<MpResult>().ToTable("Results_Mp");
            modelBuilder.Entity<MpResultLineItem>().ToTable("Results_MpLineItems");
            modelBuilder.Entity<McaResult>().ToTable("Results_Mca");
            modelBuilder.Entity<McaResultLineItem>().ToTable("Results_McaLineItems");
            modelBuilder.Entity<WomenRepResult>().ToTable("Results_WomenRep");
            modelBuilder.Entity<WomenRepResultLineItem>().ToTable("Results_WomenRepLineItems");
            modelBuilder.Entity<PresidentialResult>().ToTable("Results_Referendum");
            modelBuilder.Entity<PresidentialResultLineItem>().ToTable("Results_ReferendumLineItems");

            #endregion Results
        }
    }
}