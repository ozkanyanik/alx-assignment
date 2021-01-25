namespace EFCore
{
    public class EfCoreSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public string JWTSecretKey { get; set; }
        public int JWTTokenExpiryTime { get; set; }
    }

    public class ConnectionStrings
    {
        public string DevConn { get; set; }
    }
}
