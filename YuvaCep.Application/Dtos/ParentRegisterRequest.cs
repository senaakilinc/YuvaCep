namespace YuvaCep.Application.DTOs
{
    public class ParentRegisterRequest
    {
        public string TCIDNumber { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        // Veli kayıt olurken öğrenciyi bağlamak için kod gerekiyor:
        public string StudentReferenceCode { get; set; }
    }
}