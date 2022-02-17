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
        /// Creates an empty user with a default connection.
        /// </summary>
        public User()
        {
            this.id = -1;
            this.db = new UserDatabase();
        }

        /// <summary>
        /// Creates an empty user with a database connection.
        /// </summary>
        /// <param name="database">Database where the user is</param>
        public User(UserDatabase database)
        {
            this.id = -1;
            this.db = database;
        }

        /// <summary>
        /// Creates a user with all information given and a database connection.
        /// </summary>
        /// <param name="username">The user's username</param>
        /// <param name="email">The user's email</param>
        /// <param name="password">The user's password</param>
        /// <param name="database">Database where the user is</param>
        /// <exception cref="LoginFailedException">Thrown if the user already exists</exception>
        public User(string username, string email, string password, UserDatabase database)
        {
            this.db = database;
            int result = 0;
            try
            {
                this.Register(username, email, password);
            }
            catch(UserAlreadyExistsException e)
            {
                result = this.db.LoginUser(username, password);
                if(result == -1)
                {
                    throw new LoginFailedException($"Error while creating user: {e.Message}");
                }
                this.loggedIn = true;
            }
            
            this.id = result;
        }

        public void Register(string username, string email, string password)
        {
            this.id = this.db.RegisterNewUser(username, email, password);
        }

        public void Login(string username, string password)
        {
            try
            {
                this.id = this.db.LoginUser(username, password);
            }
            catch (QueryFailedException e)
            {
                this.id = -1;
                throw new LoginFailedException($"Error while logging in: {e.Message}");
            }
        }
    }
}