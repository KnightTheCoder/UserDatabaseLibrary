using MySql.Data.MySqlClient;
using Knight.UserDatabase.Exceptions;

namespace Knight.UserDatabase.Database
{
    public partial class UserDatabase
    {
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
        /// Gets a user's active state
        /// </summary>
        /// <param name="user_id">Id of the user</param>
        /// <returns>Wether the user is active</returns>
        /// <exception cref="QueryFailedException">Thrown when couldn't get the user's active state</exception>
        public bool IsUserActive(int user_id)
        {
            bool isActive = false;
            if(this.IsOpen)
            {
                try
                {
                    string query = "SELECT active FROM users WHERE id=@user_id";
                    MySqlCommand cmd = new MySqlCommand(query, this.connection);
                    cmd.Parameters.AddWithValue("@user_id", user_id);
                    cmd.Prepare();

                    string? result = null;
                    try
                    {
                        result = cmd.ExecuteScalar().ToString();
                    }
                    catch (NullReferenceException)
                    {
                        isActive = false;
                    }
                    if(result != null)
                    {
                        isActive = bool.Parse(result);
                    }
                }
                catch (MySqlException)
                {
                    throw new QueryFailedException("Couldn't get user's active state");
                }
            }
            return isActive;
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
            if(this.IsOpen && this.IsUserActive(user_id))
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
        /// <exception cref="QueryFailedException">Thrown when unable to get a user's information</exception>
        public UserInformation GetUserInformation(int user_id)
        {
            UserInformation userInfo = new UserInformation();
            try
            {
                if(this.IsUserInformationAlreadyFilled(user_id))
                {
                    string query = "SELECT firstname, lastname, gender, age FROM userinformation WHERE user_id=@user_id";
                    MySqlCommand cmd = new MySqlCommand(query, this.connection);
                    cmd.Parameters.AddWithValue("@user_id", user_id);
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    while(dataReader.Read())
                    {
                        string? firstName = dataReader.GetValue(0)?.ToString();
                        string? lastName = dataReader.GetValue(1)?.ToString();
                        string? gender = dataReader.GetValue(2)?.ToString();

                        int age = 0;
                        string? tmpAge = dataReader.GetValue(3)?.ToString();
                        if(tmpAge != null)
                        {
                            age = int.Parse(tmpAge);
                        }
                        userInfo = new UserInformation(firstName, lastName, gender, age);
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

    }
}