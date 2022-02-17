using MySql.Data.MySqlClient;
using Knight.MysqlTest2.Exceptions;

namespace Knight.MysqlTest2.DB
{
    /// <summary>
    /// A mysql database object
    /// </summary>
    public class UserDatabase
    {
        /// <summary>
        /// Mysql connection object
        /// </summary>
        private MySqlConnection connection;

        /// <summary>
        /// Checks wether the mysql connection is open
        /// </summary>
        /// <returns>Open state</returns>
        public bool IsOpen {get => connection.State.ToString() == "Open";}

        /// <summary>
        /// Creates a mysql connection without any database
        /// </summary>
        /// <exception cref="DatabaseConnectionFailedException">Thrown when a connection is not made</exception>
        public UserDatabase()
        {
            string host = "localhost";
            string username = "root";
            string password = "DataBaseKnight2000";

            this.connection = new MySqlConnection();

            try
            {
                string connStr = $"server={host};username={username};password={password};";

                this.connection = new MySqlConnection(connStr);
                connection.Open();
            }
            catch (MySqlException e)
            {
                throw new DatabaseConnectionFailedException($"Error while connecting to mysql server: {e.Message}");
            }
        }
        
        /// <summary>
        /// Creates a mysql connection and connects to a database
        /// </summary>
        /// <param name="database">Database to connect to</param>
        /// <exception cref="DatabaseConnectionFailedException">Thrown when a connection is not made</exception>
        public UserDatabase(string database)
        {
            string host = "localhost";
            string username = "root";
            string password = "DataBaseKnight2000";
            this.connection = new MySqlConnection();

            try
            {
                string connStr = $"server={host};username={username};password={password};";

                this.connection = new MySqlConnection(connStr);
                connection.Open();
                this.ConnectToDataBase(database);
            }
            catch (MySqlException e)
            {
                throw new DatabaseConnectionFailedException($"Error while connecting to mysql server: {e.Message}");
            }
        }

        /// <summary>
        /// Creates a mysql connection and connects to a database or creates one if it doesn't exist
        /// </summary>
        /// <param name="database">Database to connect to</param>
        /// <exception cref="DatabaseConnectionFailedException">Thrown when a connection is not made</exception>
        public UserDatabase(string database, bool createIfNotExists)
        {
            string host = "localhost";
            string username = "root";
            string password = "DataBaseKnight2000";
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
            catch (MySqlException e)
            {
                throw new DatabaseConnectionFailedException($"Error while connecting to mysql server: {e.Message}");
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
                catch (MySqlException e)
                {
                    throw new QueryFailedException($"Error while creating database: {e.Message}");
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
                catch (MySqlException e)
                {
                    throw new QueryFailedException($"Error while deleting database: {e.Message}");
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
                        "PRIMARY KEY(id)" +
                        ");";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();

                    query = "CREATE TABLE userinformation (" +
                        "user_id INT," + 
                        "firstname VARCHAR(60)," +
                        "lastname VARCHAR(60)," +
                        "gender VARCHAR(30)," +
                        "PRIMARY KEY(user_id)," +
                        "FOREIGN KEY(user_id) REFERENCES users(id)" +
                        ");";
                    cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {
                    throw new QueryFailedException($"Error while creating user table: {e.Message}");
                }
            }
        }

        /// <summary>
        /// Registers a new user with a username, an email and a password
        /// </summary>
        /// <param name="username">Username of the new user</param>
        /// <param name="password">Password of the new user</param>
        /// <param name="email">Email of the new user</param>
        /// <returns>Registered user's id or -1 if failed</returns>
        /// <exception cref="QueryFailedException">Thrown when registering the new user failed</exception>
        /// <exception cref="UserAlreadyExistsException">Thrown when the registered user already exists</exception>
        public int RegisterNewUser(string username, string email, string password)
        {
            if(this.IsOpen)
            {
                try
                {
                    string query = "INSERT INTO users (username, password, email) VALUES ( @username, @password, @email)";
                    MySqlCommand cmd = new MySqlCommand(query, this.connection);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@email", email);

                    cmd.Prepare();

                    cmd.ExecuteNonQuery();
                    
                    query = "SELECT id FROM users WHERE username=@username AND email=@email AND password=@password";
                    cmd = new MySqlCommand(query, this.connection);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@email", email);

                    cmd.Prepare();

                    string? result = cmd.ExecuteScalar().ToString();
                    if(result != null)
                    {
                        int user_id = int.Parse(result);
                        return user_id;
                    }

                }
                catch (InvalidOperationException e)
                {
                    throw new QueryFailedException($"Error while registering new user: {e.Message}");
                }
                catch (MySqlException e)
                {
                    throw new UserAlreadyExistsException($"Error while registering new user: {e.Message}");
                }
            }
            return -1;
        }

        /// <summary>
        /// Logs in using a username(or email) and a password
        /// </summary>
        /// <param name="username">Username or email of the user</param>
        /// <param name="password">Password of the user</param>
        /// <returns>The user's id or -1 if failed</returns>
        /// <exception cref="QueryFailedException">Thrown when login has fail</exception>
        public int LoginUser(string username, string password)
        {
            if(this.IsOpen)
            {
                try
                {
                    int matches = 0;
                    string query = "SELECT COUNT(*) FROM users WHERE (username=@username OR email=@email) AND password=@password LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(query, this.connection);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@email", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    cmd.Prepare();

                    // Get the amount of rows returned and check if it equals 1
                    string? result = cmd.ExecuteScalar().ToString();
                    if(result != null)
                    {
                        matches = int.Parse(result);
                    }

                    if(matches == 1)
                    {
                        query = "SELECT id FROM users WHERE (username=@username OR email=@email) AND password=@password LIMIT 1";
                        cmd = new MySqlCommand(query, this.connection);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@email", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        cmd.Prepare();

                        result = cmd.ExecuteScalar().ToString();
                        if(result != null)
                        {
                            int user_id = int.Parse(result);

                            query = "UPDATE users SET loggedin=TRUE WHERE id=@id";
                            cmd = new MySqlCommand(query, this.connection);
                            cmd.Parameters.AddWithValue("@id", result);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();

                            return user_id;
                        }
                    }
                    
                }
                catch (MySqlException e)
                {
                    throw new QueryFailedException($"Error while logging in: {e.Message}");
                }
            }
            return -1;
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

                    while(dataReader.Read())
                    {
                        Console.WriteLine("Users:");
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
                catch (MySqlException e)
                {
                    throw new QueryFailedException($"Error while showing users: {e.Message}");
                }
            }
        }
    }
}