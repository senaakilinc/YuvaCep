namespace YuvaCep.Mobile.Dtos
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FullName => $"{Name} {Surname}";
        public Guid ClassId { get; set; }
        public string ClassName { get; set; }
    }
}