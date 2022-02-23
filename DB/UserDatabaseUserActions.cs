using MySql.Data.MySqlClient;
using Knight.MysqlTest2.Exceptions;

namespace Knight.MysqlTest2.DB
{
    public partial class UserDatabase
    {
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
                    if(firstname == string.Empty)
                    {
                        firstname = null;
                    }
                    if(lastname == string.Empty)
                    {
                        lastname = null;
                    }
                    if(gender == string.Empty)
                    {
                        gender = null;
                    }

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
    }

}