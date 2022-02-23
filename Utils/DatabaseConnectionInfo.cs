using System.Text.Json.Serialization;

namespace Knight.UserDatabase.Utils
{
    /// <summary>
    /// Represents connection information for the database
    /// </summary>
    public class DatabaseConectionInfo
    {
        /// <summary>
        /// Name of the hosting site
        /// </summary>
        [JsonPropertyName("host")]
        public string Host { get; set; }
        /// <summary>
        /// Username used for login
        /// </summary>
        [JsonPropertyName("user")]
        public string Username { get; set; }
        /// <summary>
        /// Password used for login
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; }
        
        /// <summary>
        /// Initializes an empty <see cref="DatabaseConectionInfo"></see> object
        /// </summary>
        public DatabaseConectionInfo()
        {
            this.Host = string.Empty;
            this.Username = string.Empty;
            this.Password = string.Empty;
        }
    }
}