using Microsoft.EntityFrameworkCore;
using YuvaCep.Domain.Entities;

namespace YuvaCep.Persistence.Contexts
{
    public class YuvaCepDbContext : DbContext
    {
        // Yapıcı Metot
        public YuvaCepDbContext(DbContextOptions<YuvaCepDbContext> options) : base(options)
        {
        }

        // Tablolarımız
        public DbSet<User> Users { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<DailyReport> DailyReports { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<FoodList> FoodLists { get; set; }
        public DbSet<LessonProgram> LessonPrograms { get; set; }
        public DbSet<LessonProgramImage> LessonProgramImages { get; set; }

        // Rozet Sistemi Tabloları
        public DbSet<Badge> Badges { get; set; }
        public DbSet<StudentBadge> StudentBadges { get; set; }
        public DbSet<StudentMood> StudentMoods { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Domain.Entities.ActivityChart> ActivityCharts { get; set; }
        public DbSet<Domain.Entities.StudentChartEntry> StudentChartEntries { get; set; }

        // Ara Tablolar
        public DbSet<TeacherClass> TeacherClasses { get; set; }
        public DbSet<ParentStudent> ParentStudents { get; set; }


        // İlişki ve Anahtar Tanımları
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Öğretmen-Sınıf tablosunun iki anahtarı var (TeacherId + ClassId)
            modelBuilder.Entity<TeacherClass>()
                .HasKey(tc => new { tc.TeacherId, tc.ClassId });

            // Veli-Çocuk tablosunun iki anahtarı var (ParentId + StudentId)
            modelBuilder.Entity<ParentStudent>()
                .HasKey(pc => new { pc.ParentId, pc.StudentId });

            // İlişki Davranışları
            modelBuilder.Entity<ParentStudent>()
                .HasOne(ps => ps.Parent)
                .WithMany(p => p.ParentStudents)
                .HasForeignKey(ps => ps.ParentId)
                .OnDelete(DeleteBehavior.Restrict); // Veli silinirse kayıt silinmesin, hata versin

            modelBuilder.Entity<ParentStudent>()
                .HasOne(ps => ps.Student)
                .WithMany(s => s.ParentStudents)
                .HasForeignKey(ps => ps.StudentId)
                .OnDelete(DeleteBehavior.Cascade); // Öğrenci silinirse ilişki de silinsin


            base.OnModelCreating(modelBuilder);
        }
    }
}