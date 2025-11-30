using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YuvaCep.Domain.Entities;

namespace YuvaCep.Persistence.Contexts
{
    public class YuvaCepDbContext : DbContext
    {
        // 1. Yapıcı Metot: Ayarları alır (Postgre bağlantısı vb.)
        public YuvaCepDbContext(DbContextOptions<YuvaCepDbContext> options) : base(options)
        {
        }

        // 2. Tablolarımız
        public DbSet<User> Users { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<DailyReport> DailyReports { get; set; }
        public DbSet<Announcement> Announcements { get; set; }

        // Ara Tablolar
        public DbSet<TeacherClass> TeacherClasses { get; set; }
        public DbSet<ParentChild> ParentChildren { get; set; }


        // 3. Özel Ayarlar (Composite Key Tanımları)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Öğretmen-Sınıf tablosunun iki anahtarı var (TeacherId + ClassId)
            modelBuilder.Entity<TeacherClass>()
                .HasKey(tc => new { tc.TeacherId, tc.ClassId });

            // Veli-Çocuk tablosunun iki anahtarı var (ParentId + ChildId)
            modelBuilder.Entity<ParentChild>()
                .HasKey(pc => new { pc.ParentId, pc.ChildId });

            base.OnModelCreating(modelBuilder);
        }
    }
}