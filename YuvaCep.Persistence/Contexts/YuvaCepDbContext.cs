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
        public DbSet<MonthlyPlan> MonthlyPlans { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

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


            //       BAŞLANGIÇ VERİLERİ (SEED DATA)

            // 1. ADIM: SABİT ID'LER
            var classId = Guid.Parse("11111111-1111-1111-1111-111111111111"); // Sınıf ID
            var studentId1 = Guid.Parse("22222222-2222-2222-2222-222222222222"); // Öğrenci Ali
            var studentId2 = Guid.Parse("33333333-3333-3333-3333-333333333333"); // Öğrenci Ayşe

            var badgeGoldId = Guid.Parse("88888888-8888-8888-8888-888888888888");
            var badgeSilverId = Guid.Parse("99999999-9999-9999-9999-999999999999");
            var badgeWeeklyId = Guid.Parse("77777777-7777-7777-7777-777777777777");


            modelBuilder.Entity<Class>().HasData(
                new Class
                {
                    Id = classId,
                    Name = "Papatyalar Sınıfı",
                    YearLevel = "4-5 Yaş Grubu"
                }
            );

            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = studentId1,
                    Name = "Ali Yılmaz",
                    TCIDNumber = "11111111111",
                    ClassId = classId
                },
                new Student
                {
                    Id = studentId2,
                    Name = "Ayşe Demir",
                    TCIDNumber = "22222222222",
                    ClassId = classId
                }
            );

            modelBuilder.Entity<Badge>().HasData(
                new Badge
                {
                    Id = badgeGoldId,
                    Name = "Süper Başarı",
                    Code = "GOLD",
                    ImagePath = "badges/gold.png" 
                },
                // SILVER
                new Badge
                {
                    Id = badgeSilverId,
                    Name = "Örnek Davranış",
                    Code = "SILVER",
                    ImagePath = "badges/silver.png" 
                },
                // WEEKLY (Haftanın Yıldızı)
                new Badge
                {
                    Id = badgeWeeklyId,
                    Name = "Haftanın Yıldızı",
                    Code = "WEEKLY",
                    ImagePath = "badges/weekly.png" 
                }
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}