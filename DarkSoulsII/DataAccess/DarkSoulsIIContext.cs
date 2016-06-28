using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using DarkSoulsII.Models;

namespace DarkSoulsII.DataAccess
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

        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<WeaponCategory> WeaponCategories { get; set; }
        public DbSet<WeaponAttackValues> WeaponAttackValues { get; set; }
        public DbSet<Infusion> Infusions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<ApplicationRole>().ToTable("Role");
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRole");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("UserLogin");
        }
    }
}