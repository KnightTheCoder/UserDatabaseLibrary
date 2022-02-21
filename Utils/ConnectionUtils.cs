using System.Text.Json;

namespace Knight.MysqlTest2.Utils
{
    public static class ConnectionUtils
    {
        public static DatabaseConectionInfo? GetConnectionInfo(string filePath)
        {
            FileStream infoFile = File.Open(filePath, FileMode.Open);
            DatabaseConectionInfo? connectionInfo = JsonSerializer.Deserialize<DatabaseConectionInfo>(infoFile);

            infoFile.Dispose();

            return connectionInfo;
        }
    }
}