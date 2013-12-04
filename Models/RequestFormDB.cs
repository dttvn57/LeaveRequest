using LeaveRequest.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace LeaveRequest.Models
{
    public class RequestFormDB : DbContext
    {
        public RequestFormDB()
            : base("name=DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<RequestFormDB, LeaveRequest.Migrations.Configuration>("DefaultConnection"));
            //Database.SetInitializer<RequestFormDB>(new DropCreateDatabaseAlways<RequestFormDB>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        public DbSet<RequestForm> RequestForms { get; set; }
        public DbSet<Signature> Signatures { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<PayPeriod> PayPeriods { get; set; }
    }
}