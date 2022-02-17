namespace Knight.MysqlTest2.Exceptions
{
    /// <summary>
    /// Occurs when a login attempt fails
    /// </summary>
    public class LoginFailedException: UserDatabaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginFailedException"></see> class with a default error message.
        /// </summary>
        public LoginFailedException()
        : base("Login failed")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginFailedException"></see> class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public LoginFailedException(string message)
        : base (message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginFailedException"></see> class with specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public LoginFailedException(string message, Exception innerException) 
        : base(message, innerException)
        {
        }
    }
}