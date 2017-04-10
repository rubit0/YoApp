namespace YoApp.Clients.Models
{
    public class ParsedPhoneNumber
    {
        public string Normalized { get; set; }
        public bool IsValid { get; set; }
        public bool IsMobile { get; set; }
    }
}
