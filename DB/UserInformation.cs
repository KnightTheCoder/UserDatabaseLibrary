namespace Knight.MysqlTest2.DB
{
    /// <summary>
    /// Represents the user's information
    /// </summary>
    public class UserInformation
    {
        private string? firstName;
        private string? lastName;
        private string? gender;
        private int age;

        /// <summary>
        /// The user's first name
        /// </summary>
        public string? FirstName { get => this.firstName; }
        /// <summary>
        /// The user's last name
        /// </summary>
        public string? LastName { get => this.lastName; }
        /// <summary>
        /// The user's gender
        /// </summary>
        public string? Gender { get => this.gender; }
        public int Age { get => this.age; }

        /// <summary>
        /// Initializes an empty <see cref="UserInformation"></see> object
        /// </summary>
        public UserInformation()
        {
            this.firstName = null;
            this.lastName = null;
            this.gender = null;
            this.age = 0;
        }

        /// <summary>
        /// Initializes a <see cref="UserInformation"></see> object with specified information
        /// </summary>
        /// <param name="firstName">Firstname of the user</param>
        /// <param name="lastName">Lastname of the user</param>
        /// <param name="gender">Gender of the user</param>
        public UserInformation(string? firstName, string? lastName, string? gender, int age)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.gender = gender;
            this.age = age;
        }
    }
}