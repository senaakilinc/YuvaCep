namespace YuvaCep.Application.Dtos
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;    
        public string Surname { get; set; } = string.Empty;
        public string TCIDNumber { get; set; }
        public string ReferenceCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? HealthNotes { get; set; }
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string ImagePath { get; set; } // Çocuğun fotoğrafı varsa
    }
}