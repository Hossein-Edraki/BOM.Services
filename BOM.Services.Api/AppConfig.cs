namespace BOM.Services.Api
{
    public class AppConfig
    {
        public string OpenviduUrl { get; set; }
        public int OpenviduTimeout { get; set; }
        public string OpenviduSecret { get; set; }
        public string OpenviduSessionUrl { get; set; }
        public string OpenviduConnectionUrl { get; set; }
        public string JwtSecret { get; set; }
    }
}
