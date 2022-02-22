using System.Text.Json;

namespace Knight.MysqlTest2.Utils
{
    /// <summary>
    /// A static class containing connection utilities
    /// </summary>
    public static class ConnectionUtils
    {
        /// <summary>
        /// Gets the connection information from a json file
        /// </summary>
        /// <param name="filePath">Location of the json file</param>
        /// <returns>Connection info as a <see cref="DatabaseConectionInfo"></see> object</returns>
        public static DatabaseConectionInfo? GetConnectionInfo(string filePath)
        {
            FileStream infoFile = File.Open(filePath, FileMode.Open);
            DatabaseConectionInfo? connectionInfo = JsonSerializer.Deserialize<DatabaseConectionInfo>(infoFile);

            infoFile.Dispose();

            return connectionInfo;
        }
    }
}