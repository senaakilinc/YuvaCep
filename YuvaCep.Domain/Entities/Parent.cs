using System.ComponentModel.DataAnnotations;
using YuvaCep.Domain.Enums;

namespace YuvaCep.Domain.Entities
{
    public class Parent : User
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string ReferenceCode { get; set; } = string.Empty; // Öğrenci eşleşmesi için

        // Öğrenci İlişkisi
        public ICollection<ParentStudent> ParentStudents { get; set; } = new List<ParentStudent>();
    }
}