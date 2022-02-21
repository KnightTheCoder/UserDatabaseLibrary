namespace Knight.MysqlTest2.DB
{
    public class UserInformation
    {
        private string? firstName;
        private string? lastName;
        private string? gender;

        public string? FirstName { get => this.firstName; }
        public string? LastName { get => this.lastName; }
        public string? Gender { get => this.gender; }

        public UserInformation()
        {
            this.firstName = null;
            this.lastName = null;
            this.gender = null;
        }

        public UserInformation(string? firstName, string? lastName, string? gender)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.gender = gender;
        }
    }
}