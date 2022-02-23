using MySql.Data.MySqlClient;
using Knight.MysqlTest2.Exceptions;
using Knight.MysqlTest2.Utils;

namespace Knight.MysqlTest2.DB
{
    /// <summary>
    /// A mysql database object
    /// </summary>
    public partial class UserDatabase
    {
        /// <summary>
        /// Mysql connection object
        /// </summary>
        private MySqlConnection connection;

        /// <summary>
        /// Checks wether the mysql connection is open
        /// </summary>
        public bool IsOpen {get => connection.State.ToString() == "Open";}

        /// <summary>
        /// Intializes a mysql connection without any database
        /// </summary>
        /// <exception cref="DatabaseConnectionFailedException">Thrown when a connection is not made</exception>
        public UserDatabase()
        {
            DatabaseConectionInfo? connInfo = ConnectionUtils.GetConnectionInfo(@".\DatabaseConfig\connectionInfo.json");
            string? host = connInfo?.Host;
            string? username = connInfo?.Username;
            string? password = connInfo?.Password;

            this.connection = new MySqlConnection();

            try
            {
                string connStr = $"server={host};username={username};password={password};";

                this.connection = new MySqlConnection(connStr);
                connection.Open();
            }
            catch (MySqlException)
            {
                throw new DatabaseConnectionFailedException($"Couldn't connect to mysql server");
            }
        }
        
        /// <summary>
        /// Intializes a mysql connection and connects to a database
        /// </summary>
        /// <param name="database">Database to connect to</param>
        /// <exception cref="DatabaseConnectionFailedException">Thrown when a connection is not made</exception>
        public UserDatabase(string database)
        {
            DatabaseConectionInfo? connInfo = ConnectionUtils.GetConnectionInfo(@".\DatabaseConfig\connectionInfo.json");
            string? host = connInfo?.Host;
            string? username = connInfo?.Username;
            string? password = connInfo?.Password;
            this.connection = new MySqlConnection();

            try
            {
                string connStr = $"server={host};username={username};password={password};";

                this.connection = new MySqlConnection(connStr);
                connection.Open();
                this.ConnectToDataBase(database);
            }
            catch (MySqlException)
            {
                throw new DatabaseConnectionFailedException($"Couldn't connect to mysql server");
            }
        }

        /// <summary>
        /// Intializes a mysql connection and connects to a database or creates one if it doesn't exist
        /// </summary>
        /// <param name="database">Database to connect to</param>
        /// <exception cref="DatabaseConnectionFailedException">Thrown when a connection is not made</exception>
        public UserDatabase(string database, bool createIfNotExists)
        {
            DatabaseConectionInfo? connInfo = ConnectionUtils.GetConnectionInfo(@".\DatabaseConfig\connectionInfo.json");
            string? host = connInfo?.Host;
            string? username = connInfo?.Username;
            string? password = connInfo?.Password;
            this.connection = new MySqlConnection();

            try
            {
                string connStr = $"server={host};username={username};password={password};";

                this.connection = new MySqlConnection(connStr);
                connection.Open();

                if(!this.ConnectToDataBase(database) && createIfNotExists)
                {
                    this.CreateDataBase(database);
                }
            }
            catch (MySqlException)
            {
                throw new DatabaseConnectionFailedException($"Couldn't connect to mysql server");
            }
        }

        /// <summary>
        /// Closes connection to mysql server when class is deleted
        /// </summary>
        ~UserDatabase()
        {
            this.connection.Close();
        }

        /// <summary>
        /// Creates a specified database
        /// </summary>
        /// <param name="database">Database to create</param>
        /// <exception cref="QueryFailedException">Thrown when creating the database has failed</exception>
        public void CreateDataBase(string database)
        {
            if(this.IsOpen)
            {
                MySqlCommand cmd = new MySqlCommand();
                try
                {
                    cmd = new MySqlCommand($"CREATE DATABASE {database}", this.connection);
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException)
                {
                    throw new QueryFailedException("Couldn't create database");
                }
                finally
                {
                    this.ConnectToDataBase(database);
                }
            }
        }

        /// <summary>
        /// Deletes a specific database
        /// </summary>
        /// <param name="database">Database to delete</param>
        /// <exception cref="QueryFailedException">Thrown when deleting the database has failed</exception>
        public void DeleteDataBase(string database)
        {
            if(this.IsOpen)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand($"DROP DATABASE {database}", this.connection);
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException)
                {
                    throw new QueryFailedException("Couldn't delete database");
                }
            }
        }

        /// <summary>
        /// Connects to a specified database
        /// </summary>
        /// <param name="database">Database to connect to</param>
        /// <returns>Wether the database exists</returns>
        public bool ConnectToDataBase(string database)
        {
            if(this.IsOpen)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand($"Use {database}", this.connection);
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Creates the users and the userinformation table
        /// </summary>
        /// <exception cref="QueryFailedException">Thrown when creating the user tables has failed</exception>
        public void CreateTables()
        {
            if(this.IsOpen)
            {
                try
                {
                    string query = "CREATE TABLE users (" +
                        "id INT AUTO_INCREMENT," + 
                        "username VARCHAR(40) NOT NULL UNIQUE," +
                        "password VARCHAR(30) NOT NULL," +
                        "email VARCHAR(255) NOT NULL UNIQUE," +
                        "loggedin BOOLEAN DEFAULT FALSE," +
                        "active BOOLEAN DEFAULT TRUE," +
                        "PRIMARY KEY(id)" +
                        ");";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();

                    query = "CREATE TABLE userinformation (" +
                        "user_id INT," + 
                        "firstname VARCHAR(60)," +
                        "lastname VARCHAR(60)," +
                        "gender VARCHAR(30)," +
                        "age int," +
                        "PRIMARY KEY(user_id)," +
                        "FOREIGN KEY(user_id) REFERENCES users(id)" +
                        "ON DELETE CASCADE" +
                        ");";
                    cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException)
                {
                    throw new QueryFailedException("Couldn't create user tables");
                }
            }
        }

        /// <summary>
        /// Lists all users
        /// </summary>
        public void ShowAllUsers()
        {
            if(this.IsOpen)
            {
                try
                {
                    string query = "SELECT * FROM users";
                    MySqlCommand cmd = new MySqlCommand(query, this.connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    Console.WriteLine("Users:");
                    while(dataReader.Read())
                    {
                        if(dataReader.HasRows)
                        {
                            for(int i = 0; i < dataReader.FieldCount; i++)
                            {
                                Console.Write($"{dataReader.GetName(i)}: {dataReader.GetValue(i)} ");
                            }
                        }
                        Console.WriteLine();
                    }

                    dataReader.Close();
                }
                catch (MySqlException)
                {
                    throw new QueryFailedException("Couldn't show all users");
                }
            }
        }
    }
}