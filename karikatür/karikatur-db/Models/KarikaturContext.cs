using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace karikatur_db.Models
{
    public partial class KarikaturContext : DbContext
    {
        public KarikaturContext()
        {
        }

        public KarikaturContext(DbContextOptions<KarikaturContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdsCodes> AdsCodes { get; set; }
        public virtual DbSet<Cartoon> Cartoon { get; set; }
        public virtual DbSet<CartoonImages> CartoonImages { get; set; }
        public virtual DbSet<CartoonLikes> CartoonLikes { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<CategoryStories> CategoryStories { get; set; }
        public virtual DbSet<CategoryVideos> CategoryVideos { get; set; }
        public virtual DbSet<Drawer> Drawer { get; set; }
        public virtual DbSet<Languages> Languages { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<NotificationToken> NotificationToken { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<StarVideos> StarVideos { get; set; }
        public virtual DbSet<Stars> Stars { get; set; }
        public virtual DbSet<Stories> Stories { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<VideoTagVideos> VideoTagVideos { get; set; }
        public virtual DbSet<VideoTags> VideoTags { get; set; }
        public virtual DbSet<Videos> Videos { get; set; }
        public virtual DbSet<Watchings> Watchings { get; set; }

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

            modelBuilder.Entity<AdsCodes>(entity =>
            {
                entity.Property(e => e.AppleTouchIcon114x114)
                    .HasColumnName("apple_touch_icon_114x114")
                    .HasMaxLength(350);

                entity.Property(e => e.AppleTouchIcon120x120)
                    .HasColumnName("apple_touch_icon_120x120")
                    .HasMaxLength(350);

                entity.Property(e => e.AppleTouchIcon144x144)
                    .HasColumnName("apple_touch_icon_144x144")
                    .HasMaxLength(350);

                entity.Property(e => e.AppleTouchIcon152x152)
                    .HasColumnName("apple_touch_icon_152x152")
                    .HasMaxLength(350);

                entity.Property(e => e.AppleTouchIcon180x180)
                    .HasColumnName("apple_touch_icon_180x180")
                    .HasMaxLength(350);

                entity.Property(e => e.AppleTouchIcon57x57)
                    .HasColumnName("apple_touch_icon_57x57")
                    .HasMaxLength(350);

                entity.Property(e => e.AppleTouchIcon60x60)
                    .HasColumnName("apple_touch_icon_60x60")
                    .HasMaxLength(350);

                entity.Property(e => e.AppleTouchIcon72x72)
                    .HasColumnName("apple_touch_icon_72x72")
                    .HasMaxLength(350);

                entity.Property(e => e.AppleTouchIcon76x76)
                    .HasColumnName("apple_touch_icon_76x76")
                    .HasMaxLength(350);

                entity.Property(e => e.BodyScript).HasColumnName("body_script");

                entity.Property(e => e.ExoclickSiteVerification)
                    .HasColumnName("exoclick_site_verification")
                    .HasMaxLength(350);

                entity.Property(e => e.HeadScripts).HasColumnName("head_scripts");

                entity.Property(e => e.HomepageDescription)
                    .HasColumnName("Homepage_description")
                    .HasMaxLength(350);

                entity.Property(e => e.HomepageTitle)
                    .HasColumnName("Homepage_title")
                    .HasMaxLength(150);

                entity.Property(e => e.Icon16x16)
                    .HasColumnName("icon_16x16")
                    .HasMaxLength(350);

                entity.Property(e => e.Icon192x192)
                    .HasColumnName("icon_192x192")
                    .HasMaxLength(350);

                entity.Property(e => e.Icon32x32)
                    .HasColumnName("icon_32x32")
                    .HasMaxLength(350);

                entity.Property(e => e.Icon96x96)
                    .HasColumnName("icon_96x96")
                    .HasMaxLength(350);

                entity.Property(e => e.ManifestJson)
                    .HasColumnName("manifest_json")
                    .HasMaxLength(350);

                entity.Property(e => e.MsapplicationTileColor)
                    .HasColumnName("msapplication_TileColor")
                    .HasMaxLength(350);

                entity.Property(e => e.MsapplicationTileImage)
                    .HasColumnName("msapplication_TileImage")
                    .HasMaxLength(350);

                entity.Property(e => e.Msvalidate01)
                    .HasColumnName("msvalidate_01")
                    .HasMaxLength(350);

                entity.Property(e => e.SiteLink).HasMaxLength(150);

                entity.Property(e => e.SiteName).HasMaxLength(150);

                entity.Property(e => e.StoryDetailBottomBanner).HasColumnName("story_detail_bottom_banner");

                entity.Property(e => e.StoryDetailPopunder).HasColumnName("story_detail_popunder");

                entity.Property(e => e.StoryDetailTopBanner).HasColumnName("story_detail_top_banner");

                entity.Property(e => e.StoryDetalRightBanner1).HasColumnName("story_detal_right_banner_1");

                entity.Property(e => e.StoryDetalRightBanner2).HasColumnName("story_detal_right_banner_2");

                entity.Property(e => e.StoryListDescription)
                    .HasColumnName("StoryList_description")
                    .HasMaxLength(350);

                entity.Property(e => e.StoryListTitle)
                    .HasColumnName("StoryList_title")
                    .HasMaxLength(150);

                entity.Property(e => e.ThemeColor)
                    .HasColumnName("theme_color")
                    .HasMaxLength(350);

                entity.Property(e => e.VideoDetailBottom).HasColumnName("video_detail_bottom");

                entity.Property(e => e.VideoDetailPopunder).HasColumnName("video_detail_popunder");

                entity.Property(e => e.VideoDetailRightBanner1).HasColumnName("video_detail_right_banner_1");

                entity.Property(e => e.VideoDetailRightBanner2).HasColumnName("video_detail_right_banner_2");

                entity.Property(e => e.VideoListBanner1).HasColumnName("video_list_banner_1");

                entity.Property(e => e.VideoListBanner2).HasColumnName("video_list_banner_2");

                entity.Property(e => e.VideoListBanner3).HasColumnName("video_list_banner_3");

                entity.Property(e => e.VideoListBanner4).HasColumnName("video_list_banner_4");

                entity.Property(e => e.VideoScript).HasColumnName("video_script");

                entity.Property(e => e.YandexVerification)
                    .HasColumnName("yandex_verification")
                    .HasMaxLength(350);
            });

            modelBuilder.Entity<Cartoon>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Rank).HasDefaultValueSql("((999))");

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

                entity.Property(e => e.Rank).HasDefaultValueSql("((9999))");

                entity.HasOne(d => d.Cartoon)
                    .WithMany(p => p.CartoonImages)
                    .HasForeignKey(d => d.CartoonId)
                    .HasConstraintName("FK_CartoonImages_Cartoon");
            });

            modelBuilder.Entity<CartoonLikes>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.UniqUserKey)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Cartoon)
                    .WithMany(p => p.CartoonLikes)
                    .HasForeignKey(d => d.CartoonId)
                    .HasConstraintName("FK_CartoonLikes_Cartoon");
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.Property(e => e.ImageSource)
                    .IsRequired()
                    .HasMaxLength(550);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<CategoryStories>(entity =>
            {
                entity.HasKey(e => new { e.CategoryId, e.StoryId });

                entity.HasIndex(e => e.StoryId);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CategoryStories)
                    .HasForeignKey(d => d.CategoryId);

                entity.HasOne(d => d.Story)
                    .WithMany(p => p.CategoryStories)
                    .HasForeignKey(d => d.StoryId);
            });

            modelBuilder.Entity<CategoryVideos>(entity =>
            {
                entity.HasKey(e => new { e.CategoryId, e.VideoId });

                entity.HasIndex(e => e.VideoId);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CategoryVideos)
                    .HasForeignKey(d => d.CategoryId);

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.CategoryVideos)
                    .HasForeignKey(d => d.VideoId);
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

            modelBuilder.Entity<Languages>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.FlagSource)
                    .IsRequired()
                    .HasMaxLength(550);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(300);
            });

            modelBuilder.Entity<NotificationToken>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Device)
                    .IsRequired()
                    .HasMaxLength(350);

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(350);
            });

            modelBuilder.Entity<Settings>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ProjectKey)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<StarVideos>(entity =>
            {
                entity.HasKey(e => new { e.StarId, e.VideoId });

                entity.HasIndex(e => e.VideoId);

                entity.HasOne(d => d.Star)
                    .WithMany(p => p.StarVideos)
                    .HasForeignKey(d => d.StarId);

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.StarVideos)
                    .HasForeignKey(d => d.VideoId);
            });

            modelBuilder.Entity<Stars>(entity =>
            {
                entity.Property(e => e.ImageSource)
                    .IsRequired()
                    .HasMaxLength(550);

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Stories>(entity =>
            {
                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.ImageSource)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Link)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Title).IsRequired();
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Password).HasDefaultValueSql("(newid())");

                entity.Property(e => e.PasswordExpirationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<VideoTagVideos>(entity =>
            {
                entity.HasKey(e => new { e.VideoTagId, e.VideoId });

                entity.HasIndex(e => e.VideoId);

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.VideoTagVideos)
                    .HasForeignKey(d => d.VideoId);

                entity.HasOne(d => d.VideoTag)
                    .WithMany(p => p.VideoTagVideos)
                    .HasForeignKey(d => d.VideoTagId);
            });

            modelBuilder.Entity<VideoTags>(entity =>
            {
                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TagText).IsRequired();
            });

            modelBuilder.Entity<Videos>(entity =>
            {
                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.IsPublish)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Link)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.PreviewImageSource)
                    .IsRequired()
                    .HasMaxLength(350);

                entity.Property(e => e.PreviewVideoSource)
                    .IsRequired()
                    .HasMaxLength(350);

                entity.Property(e => e.Title).IsRequired();

                entity.Property(e => e.UniqKey)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.VideoSource)
                    .IsRequired()
                    .HasMaxLength(350);
            });

            modelBuilder.Entity<Watchings>(entity =>
            {
                entity.HasIndex(e => e.VideoRefId);

                entity.Property(e => e.UniqKey).IsRequired();

                entity.HasOne(d => d.VideoRef)
                    .WithMany(p => p.Watchings)
                    .HasForeignKey(d => d.VideoRefId);
            });
        }
    }
}
