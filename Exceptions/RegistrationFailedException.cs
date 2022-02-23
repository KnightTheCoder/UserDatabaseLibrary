namespace Knight.UserDatabase.Exceptions
{
    /// <summary>
    /// Occurs when a registration fails
    /// </summary>
    public class RegistrationFailedException: UserDatabaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationFailedException"></see> class with a default error message.
        /// </summary>
        public RegistrationFailedException()
        : base("Registration failed")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationFailedException"></see> class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public RegistrationFailedException(string message)
        : base (message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationFailedException"></see> class with specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public RegistrationFailedException(string message, Exception innerException) 
        : base(message, innerException)
        {
        }

    }
}