using Maps.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Maps.Data
{
    /// <summary>
    /// Database context that maps C# objects rows in the database.
    /// </summary>
    public class MapsDbContext : IdentityDbContext<User>
    {
        public MapsDbContext()
            : base("MapsConnection", throwIfV1Schema: false) { }

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

            modelBuilder.Entity<Entities.Data>()
                .Property(d => d.Values_).HasColumnName("Values");

            // OnDeleteCascade setup
            modelBuilder.Entity<Map>()
                .HasOptional(m => m.User)
                .WithMany(u => u.Maps)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Layer>()
              .HasOptional(l => l.Map)
              .WithMany(m => m.Layers)
              .WillCascadeOnDelete(true);

            modelBuilder.Entity<Column>()
            .HasOptional(c => c.Layer)
            .WithMany(l => l.Columns)
            .WillCascadeOnDelete(true);

            modelBuilder.Entity<Entities.Data>()
              .HasOptional(d => d.Layer)
              .WithMany(l => l.Data)
              .WillCascadeOnDelete(true);
        }

        #region Dbsets
        public virtual DbSet<Map> Maps { get; set; }
        public virtual DbSet<Layer> Layers { get; set; }
        public virtual DbSet<Column> Columns { get; set; }
        public virtual DbSet<Entities.Data> Data { get; set; }
        #endregion
    }
}
