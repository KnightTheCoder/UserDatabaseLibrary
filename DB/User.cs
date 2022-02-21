using Knight.MysqlTest2.Exceptions;

namespace Knight.MysqlTest2.DB
{
    /// <summary>
    /// Represents a user on a user database
    /// </summary>
    public class User
    {
        private int id;
        private UserDatabase db;
        private bool loggedIn;

        /// <summary>
        /// The user's id
        /// </summary>
        public int Id { get => this.id;}
        public bool LoggedIN { get=> this.loggedIn; }
        

        /// <summary>
        /// Intializes an empty <see cref="User"></see> with a default connection.
        /// </summary>
        public User()
        {
            this.id = -1;
            this.db = new UserDatabase();
        }

        /// <summary>
        /// Intializes an empty <see cref="User"></see> with a database connection.
        /// </summary>
        /// <param name="database">Database where the user is</param>
        public User(UserDatabase database)
        {
            this.id = -1;
            this.db = database;
        }

        /// <summary>
        /// Intializes a user with all information given and a database connection.
        /// </summary>
        /// <param name="username">The user's username</param>
        /// <param name="email">The user's email</param>
        /// <param name="password">The user's password</param>
        /// <param name="database">Database where the user is</param>
        public User(string username, string email, string password, UserDatabase database)
        {
            this.db = database;
            try
            {
                this.Register(username, email, password);
            }
            catch(UserAlreadyExistsException)
            {
                this.id = this.db.GetUserId(username, email, password);
            }
            
        }

        /// <summary>
        /// Registers the user with the specified information
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <param name="email">Email of the user</param>
        /// <param name="password">Password of the user</param>
        public void Register(string username, string email, string password)
        {
            this.id = this.db.RegisterNewUser(username, email, password);
        }

        /// <summary>
        /// Logs in the user with the specified credentials
        /// </summary>
        /// <param name="username">Username or email of the user</param>
        /// <param name="password">Password of the user</param>
        public void Login(string username, string password)
        {
            try
            {
                this.id = this.db.LoginUser(username, password);
                if(this.id != -1)
                {
                    this.loggedIn = true;
                }
            }
            catch (QueryFailedException e)
            {
                this.id = -1;
                throw new LoginFailedException($"Error while logging in: {e.Message}");
            }
        }
    }
}