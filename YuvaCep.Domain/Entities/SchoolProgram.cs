using System;
using YuvaCep.Domain.Enums;

namespace YuvaCep.Domain.Entities
{
    public class SchoolProgram
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public ProgramType Type { get; set; }

        // SINIF BİLGİSİ (Burası Kilit Nokta) 🗝️
        // Öğretmen buraya sınıf ID'sini yazacak.
        public Guid? ClassId { get; set; }
        public Class? Class { get; set; }
    }
}