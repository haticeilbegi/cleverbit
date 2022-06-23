using CleverBit.Task1.Common.Enums;

namespace CleverBit.Task1.Common.Models
{
    public class IntegrationModel
    {
        public string Url { get; set; }
        public string Token { get; set; }
        public AuthenticationTypes AuthenticationType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
