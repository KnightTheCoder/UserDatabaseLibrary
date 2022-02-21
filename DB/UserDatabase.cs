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
        /// Intializes a mysql connection without any database
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
                    if(this.GetUserId(username, email, password) != -1)
                    {
                        throw new UserAlreadyExistsException("User already exists");
                    }
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
                catch (InvalidOperationException)
                {
                    throw new QueryFailedException("Couldn't register new user");
                }
                catch (MySqlException)
                {
                    throw new UserAlreadyExistsException("User already exists");
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
        /// <exception cref="QueryFailedException">Thrown when login has failed</exception>
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
                catch (MySqlException)
                {
                    throw new QueryFailedException("Couldn't log in");
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets the id of a user
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <param name="email">Email of the user</param>
        /// <param name="password">Password of the user</param>
        /// <returns>The id of a user, -1 if it doesn't exist</returns>
        /// <exception cref="QueryFailedException">Thrown when the user couldn't be found</exception>
        public int GetUserId(string username, string email, string password)
        {
            int user_id = -1;
            if(this.IsOpen)
            {
                try
                {
                    string query = "SELECT id FROM users WHERE username=@username AND email=@email AND password=@password";
                    MySqlCommand cmd = new MySqlCommand(query, this.connection);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Prepare();

                    string? result = null;
                    try
                    {
                        result = cmd.ExecuteScalar().ToString();
                    }
                    catch (NullReferenceException)
                    {
                        user_id = -1;
                    }

                    if(result != null)
                    {
                        user_id = int.Parse(result);
                    }
                }
                catch (MySqlException)
                {
                    throw new QueryFailedException("Couldn't find user");
                }
            }

            return user_id;
        }

        /// <summary>
        /// Fills out user information with the specified values
        /// </summary>
        /// <param name="user_id">Id of the user</param>
        /// <param name="firstname">Firstname of the user</param>
        /// <param name="lastname">Lastname of the user</param>
        /// <param name="gender">Gender of the user</param>
        /// <exception cref="QueryFailedException">Thrown when the user information couldn't be filled</exception>
        public void FillUserInformation(int user_id, string? firstname = null, string? lastname = null, string? gender = null)
        {
            if(this.IsOpen)
            {
                try
                {
                    if(this.IsUserInformationAlreadyFilled(user_id))
                    {
                        UserInformation userInfo = this.GetUserInformation(user_id);
                        if(firstname == null)
                        {
                            firstname = userInfo.FirstName;
                        }
                        if(lastname == null)
                        {
                            lastname = userInfo.LastName;
                        }
                        if(gender == null)
                        {
                            gender = userInfo.Gender;
                        }

                        string query = "UPDATE userinformation SET firstname=@firstname, lastname=@lastname, gender=@gender WHERE user_id=@user_id";
                        MySqlCommand cmd = new MySqlCommand(query, this.connection);
                        cmd.Parameters.AddWithValue("@user_id", user_id);
                        cmd.Parameters.AddWithValue("@firstname", firstname);
                        cmd.Parameters.AddWithValue("@lastname", lastname);
                        cmd.Parameters.AddWithValue("@gender", gender);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        string query = "INSERT INTO userinformation(user_id, firstname, lastname, gender) VALUES (@user_id, @firstname, @lastname, @gender)";
                        MySqlCommand cmd = new MySqlCommand(query, this.connection);
                        cmd.Parameters.AddWithValue("@user_id", user_id);
                        cmd.Parameters.AddWithValue("@firstname", firstname);
                        cmd.Parameters.AddWithValue("@lastname", lastname);
                        cmd.Parameters.AddWithValue("@gender", gender);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (MySqlException)
                {
                    throw new QueryFailedException("Couldn't fill user information");
                }
            }
        }

        /// <summary>
        /// Checks wether the user information is already filled
        /// </summary>
        /// <param name="user_id">Id of the user</param>
        /// <returns>Wether the user information is filled</returns>
        /// <exception cref="QueryFailedException">Thrown when user information couldn't be found</exception>
        public bool IsUserInformationAlreadyFilled(int user_id)
        {
            bool userInfoFilled = false;
            if(this.IsOpen)
            {
                try
                {
                    string query = "SELECT COUNT(*) FROM userinformation WHERE user_id=@user_id";
                    MySqlCommand cmd = new MySqlCommand(query, this.connection);
                    cmd.Parameters.AddWithValue("@user_id", user_id);
                    cmd.Prepare();
                    string? result = cmd.ExecuteScalar().ToString();
                    if(result != null)
                    {
                        if(int.Parse(result) == 1)
                        {
                            userInfoFilled = true;
                        }
                    }
                }
                catch (MySqlException)
                {
                    throw new QueryFailedException("Couldn't find user information");
                }
            }
            return userInfoFilled;
        }

        /// <summary>
        /// Gets user information in a <see cref="UserInformation"></see> object
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns>The user's information in a <see cref="UserInformation"></see> object</returns>
        public UserInformation GetUserInformation(int user_id)
        {
            UserInformation userInfo = new UserInformation();
            try
            {
                if(this.IsUserInformationAlreadyFilled(user_id))
                {
                    string query = "SELECT firstname, lastname, gender FROM userinformation WHERE user_id=@user_id";
                    MySqlCommand cmd = new MySqlCommand(query, this.connection);
                    cmd.Parameters.AddWithValue("@user_id", user_id);
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    while(dataReader.Read())
                    {
                        string? firstName = dataReader.GetValue(0)?.ToString();
                        string? lastName = dataReader.GetValue(1)?.ToString();
                        string? gender = dataReader.GetValue(2)?.ToString();
                        userInfo = new UserInformation(firstName, lastName, gender);
                    }
                    dataReader.Close();
                }
            }
            catch (MySqlException)
            {
                throw new QueryFailedException("Couldn't get user information");
            }

            return userInfo;
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