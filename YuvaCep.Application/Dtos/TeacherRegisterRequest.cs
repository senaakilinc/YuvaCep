namespace YuvaCep.Application.DTOs
{
    public class TeacherRegisterRequest
    {
        public string TCIDNumber { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
    }
}