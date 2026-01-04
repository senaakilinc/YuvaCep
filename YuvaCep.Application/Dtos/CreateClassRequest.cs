namespace YuvaCep.Application.Dtos
{
    public class CreateClassRequest
    {
        public string ClassName { get; set; }
        public string AgeGroup { get; set; } // "3-4 Yaş", "4-5 Yaş" gibi
    }
}