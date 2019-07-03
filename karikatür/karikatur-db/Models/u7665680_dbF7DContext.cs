using Microsoft.EntityFrameworkCore;

namespace karikatur_db.Models
{
    public partial class u7665680_dbF7DContext : DbContext
    {
        public u7665680_dbF7DContext()
        {
        }

        public u7665680_dbF7DContext(DbContextOptions<u7665680_dbF7DContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cartoon> Cartoon { get; set; }
        public virtual DbSet<CartoonImages> CartoonImages { get; set; }
        public virtual DbSet<Drawer> Drawer { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=94.73.150.3;Initial Catalog=u7665680_dbF7D; User Id=u7665680_userF7D;Password=AEtx19I1");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Cartoon>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Drawers)
                    .WithMany(p => p.Cartoon)
                    .HasForeignKey(d => d.DrawersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cartoon_Drawer");
            });

            modelBuilder.Entity<CartoonImages>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ImageSrc)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Cartoon)
                    .WithMany(p => p.CartoonImages)
                    .HasForeignKey(d => d.CartoonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartoonImages_Cartoon");
            });

            modelBuilder.Entity<Drawer>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.LogoSrc)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Password).HasDefaultValueSql("(newid())");
            });
        }
    }
}
