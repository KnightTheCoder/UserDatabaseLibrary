namespace Knight.MysqlTest2.Exceptions
{
    /// <summary>
    /// Occurs when a user has already been registered
    /// </summary>
    public class UserAlreadyExistsException: UserDatabaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAlreadyExistsException"></see> class with a default error message.
        /// </summary>
        public UserAlreadyExistsException()
        : base("User already exists")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAlreadyExistsException"></see> class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public UserAlreadyExistsException(string message)
        : base (message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAlreadyExistsException"></see> class with specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UserAlreadyExistsException(string message, Exception innerException) 
        : base(message, innerException)
        {
        }

    }
}