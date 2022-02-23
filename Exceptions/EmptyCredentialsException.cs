namespace Knight.UserDatabase.Exceptions
{
    /// <summary>
    /// Occurs when a user's credentials are empty
    /// </summary>
    public class EmptyCredentialsException: UserDatabaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyCredentialsException"></see> class with a default error message.
        /// </summary>
        public EmptyCredentialsException()
        : base("Login failed")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyCredentialsException"></see> class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public EmptyCredentialsException(string message)
        : base (message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyCredentialsException"></see> class with specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public EmptyCredentialsException(string message, Exception innerException) 
        : base(message, innerException)
        {
        }
    }
}