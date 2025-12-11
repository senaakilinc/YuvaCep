using Microsoft.EntityFrameworkCore;
using System;
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
        public DbSet<Student> Students { get; set; }
        public DbSet<DailyReport> DailyReports { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<NutritionProgram> NutritionPrograms { get; set; }
        public DbSet<LessonProgram> LessonPrograms { get; set; }
        // Rozet Sistemi Tabloları
        public DbSet<Badge> Badges { get; set; }
        public DbSet<StudentBadge> StudentBadges { get; set; }
        public DbSet<StudentMood> StudentMoods { get; set; }
        // Badges

        // Ara Tablolar
        public DbSet<TeacherClass> TeacherClasses { get; set; }
        public DbSet<ParentStudent> ParentStudent { get; set; }


        // 3. Özel Ayarlar (Composite Key Tanımları)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Öğretmen-Sınıf tablosunun iki anahtarı var (TeacherId + ClassId)
            modelBuilder.Entity<TeacherClass>()
                .HasKey(tc => new { tc.TeacherId, tc.ClassId });

            // Veli-Çocuk tablosunun iki anahtarı var (ParentId + StudentId)
            modelBuilder.Entity<ParentStudent>()
                .HasKey(pc => new { pc.ParentId, pc.StudentId });

            base.OnModelCreating(modelBuilder);
        }
    }
}