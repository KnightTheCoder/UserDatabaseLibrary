using System.Text.Json.Serialization;

namespace Knight.MysqlTest2.Utils
{
    public class DatabaseConectionInfo
    {
        [JsonPropertyName("host")]
        public string Host { get; set; }
        [JsonPropertyName("user")]
        public string User { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        
        public DatabaseConectionInfo()
        {
            this.Host = string.Empty;
            this.User = string.Empty;
            this.Password = string.Empty;
        }
    }
}