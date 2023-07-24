namespace Clinic.AcceptanceTests.Dtos
{
    public class ResultDto
    {
        public string ErrorCode { get; set; } = null!;
        public bool Message { get; set; }
        public bool Success { get; set; }
    }
}
