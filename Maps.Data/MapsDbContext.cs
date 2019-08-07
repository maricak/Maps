using Maps.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maps.Data
{
    public class MapsDbContext : IdentityDbContext<User>
    {
        public MapsDbContext()
            : base("MapsConnection", throwIfV1Schema: false)
        {
        }

        public static MapsDbContext Create()
        {
            return new MapsDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("Claim");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("Login");
            modelBuilder.Entity<IdentityRole>().ToTable("Role");
        }

        public virtual DbSet<Map> Maps { get; set; }
        public virtual DbSet<Layer> Layers { get; set; }
    }
}
