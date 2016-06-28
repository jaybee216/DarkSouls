using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using Model;
using DS2 = Model.DS2;
using DS3 = Model.DS3;


namespace DataAccess
{
    public class DarkSoulsIIContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public DarkSoulsIIContext()
            : base("DarkSoulsIIContext")
        {
        }

        static DarkSoulsIIContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            //Database.SetInitializer<DarkSoulsIIContext>(new MigrateDatabaseToLatestVersion<DarkSoulsIIContext, Migrations.Configuration>());
            Database.SetInitializer<DarkSoulsIIContext>(null);
        }

        public static DarkSoulsIIContext Create()
        {
            return new DarkSoulsIIContext();
        }

        #region DS2
        public DbSet<DS2.DS2Weapon> Weapons { get; set; }
        public DbSet<DS2.WeaponCategory> WeaponCategories { get; set; }
        public DbSet<DS2.WeaponAttackValues> WeaponAttackValues { get; set; }
        public DbSet<DS2.Infusion> Infusions { get; set; }
        #endregion

        #region DS3
        public DbSet<DS3.Weapon> Ds3Weapons { get; set; }
        public DbSet<DS3.WeaponType> Ds3WeaponTypes { get; set; }
        public DbSet<DS3.WeaponValues> Ds3WeaponValues { get; set; }
        public DbSet<DS3.InfusionType> InfusionTypes { get; set; }
        public DbSet<DS3.StartingClass> StartingClasses { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<ApplicationRole>().ToTable("Role");
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRole");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("UserLogin");

            modelBuilder.Entity<DS2.DS2Weapon>().ToTable("Weapon", "dbo");
            modelBuilder.Entity<DS2.Infusion>().ToTable("Infusion", "dbo");

            modelBuilder.Entity<DS3.Weapon>().ToTable("Weapon", "DS3");
            modelBuilder.Entity<DS3.WeaponType>().ToTable("WeaponType", "DS3");
            modelBuilder.Entity<DS3.WeaponValues>().ToTable("WeaponValues", "DS3");
            modelBuilder.Entity<DS3.Armor>().ToTable("Armor", "DS3");
            modelBuilder.Entity<DS3.ArmorType>().ToTable("ArmorType", "DS3");
            modelBuilder.Entity<DS3.InfusionType>().ToTable("Infusion", "DS3");
            modelBuilder.Entity<DS3.StartingClass>().ToTable("StartingClass", "DS3");
        }
    }
}