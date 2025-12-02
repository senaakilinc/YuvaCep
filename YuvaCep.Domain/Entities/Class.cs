using System;

namespace YuvaCep.Domain.Entities
{
    public class Class
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty; // Papatyalar
        public int YearLevel { get; set; } // 4 yaş
        public int MaxCapacity { get; set; } // Kontenjan 20

        // Bu sınıfta kimler var? (Sonra bağlayacağız)
        // public ICollection<Student> Students { get; set; }
    }
}
