namespace BOM.Services.Api.Models
{
    public class ConnectRequest
    {
        public string SessionId { get; set; }
    }

    public class ConnectResponse
    {
        public string Token { get; set; }
    }
}
