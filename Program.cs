﻿using Knight.UserDatabase.Database;
using Knight.UserDatabase.Exceptions;
using Knight.UserDatabase.Logging;

namespace Knight.UserDatabase
{
    class Program
    {

        private static void DataBaseTest1()
        {
            // UserDatabase db = new UserDatabase();
            // UserDatabase db = new UserDatabase("cs_dbtest");
            Database.UserDatabase db = new Database.UserDatabase("cs_dbtest", true);

            // db.CreateDataBase("cs_dbtest");
            // db.CreateDataBase("cs_dbtest"); // Error

            db.CreateTables();
            db.RegisterNewUser("Knight2000", "knight@mymail.com", "123");
            db.RegisterNewUser("Knight2001", "knight@yourmail.com", "1234");
            db.RegisterNewUser("Knight2002", "knight@gmail.com", "12345");
            db.RegisterNewUser("Knight2003", "knight@freemail.hu", "123456");
            db.RegisterNewUser("Knight2004", "knight@nomail.com", "1234567");
            db.RegisterNewUser("Knight2005", "knight@profmail.org", "12345678");
            // db.RegisterNewUser("Knight2005", "knight@profmail.org", "12345678"); // Error
            db.ShowAllUsers();
            
            // int logged_in = db.LoginUser("Knight2000", "123");
            // int logged_in = db.LoginUser("knight@mymail.com", "123"); // Can also login using email
            int logged_in = db.LoginUser("knight@gmail.com", "12345"); // Will return 3
            if(logged_in != -1)
            {
                // Console.WriteLine("Login successful!");
                Log.LogSuccess("Login successful!");
            }
            else
            {
                // Console.WriteLine("Login failed!");
                // Console.WriteLine("Incorrect username or password!");
                Log.LogError("Login failed!");
                Log.LogError("Incorrect username or password!");
            }

            db.ShowAllUsers();
            // db.DeleteDataBase("none"); // Error
            db.DeleteDataBase("cs_dbtest");
        }

        private static void UserTest1()
        {
            Database.UserDatabase db = new Database.UserDatabase("userTest", true);
            // UserDatabase db = new UserDatabase();
            // UserDatabase db = new UserDatabase("userTest");
            // db.CreateDataBase("userTest");

            User user1 = new User();
            User user2 = new User();
            User user3 = new User();
            User user4 = new User();
            try
            {
                db.CreateTables();

                user1 = new User("testaccount", "test@testmail.com", "123test", db); // Registers new user
                user2 = new User("testaccount", "test@testmail.com", "123test", db); // Creates an instance with the already existing id
                user3 = new User("testuser", "testuser@testmail.com", "123", db);
                user4 = new User("testaccount", "test@testmail.com", "123", db); // Error, incorrect login info
                user2.Login("testaccount", "123test"); // Logs in the user
                if(user2.LoggedIN)
                {
                    Log.LogSuccess("Login successful");
                }
                db.FillUserInformation(user2.Id, "John", "Smith", null, 0); // Creates user info
                db.FillUserInformation(user2.Id, null, null, "male", 0); // Modifies existing user info
                db.FillUserInformation(user2.Id, "James", null, null, 0); // Overrides already existing user info
                db.FillUserInformation(user4.Id, "Mikey", "Mouse", "male", 0); // Fill info for a user that is inactive
                
            }
            catch (UserDatabaseException e)
            {
                Log.LogError(e);
            }

            Console.WriteLine($"UserId: {user1.Id} LoggedIn: {user1.LoggedIN}");
            Console.WriteLine($"UserId: {user2.Id} LoggedIn: {user2.LoggedIN}");
            Console.WriteLine($"UserId: {user3.Id} LoggedIn: {user3.LoggedIN}");
            Console.WriteLine($"UserId: {user4.Id} LoggedIn: {user4.LoggedIN}");

            Console.WriteLine($"UserId: {user2.Id} active: {db.IsUserActive(user2.Id)}"); // Checks wether the second user is active
            Console.WriteLine($"UserId: {user4.Id} active: {db.IsUserActive(user4.Id)}");

            db.ShowAllUsers();

            db.DeleteDataBase("userTest");
        }

        private static void UserInterfaceDemo()
        {
            Database.UserDatabase db = new Database.UserDatabase("uidemo", true);

            try
            {
                db.CreateTables();

                User testUser = new User("knight", "knight@mail.com", "123", db);

                Console.WriteLine("Please register an account!");

                Console.Write("Username: ");
                string? username = Console.ReadLine();

                Console.Write("Email: ");
                string? email = Console.ReadLine();

                Console.Write("Password: ");
                string? password = Console.ReadLine();

                User user = new User(db);
                if(username != null && email != null && password != null)
                {
                    user.Register(username, email, password);
                }

                db.ShowAllUsers();

                Console.WriteLine("Please log in!");

                Console.Write("Username or email: ");
                username = Console.ReadLine();

                Console.Write("Password: ");
                password = Console.ReadLine();

                if(username != null && password != null)
                {
                    user.Login(username, password);
                }

                if(user.LoggedIN)
                {
                    Log.LogSuccess("Successful login!");
                }
                else
                {
                    Log.LogError("Login failed!");
                }

                Console.WriteLine("Please fill in information about you");

                // db.FillUserInformation(user.Id, "Dominik", "Kocsis", "male", 21);

                Console.Write("Firstname: ");
                string? firstname = Console.ReadLine();

                Console.Write("Lastname: ");
                string? lastname = Console.ReadLine();

                Console.Write("Gender: ");
                string? gender = Console.ReadLine();

                Console.Write("Age: ");
                int age = 0;
                try
                {
                    age = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    age = 0;
                }

                db.FillUserInformation(user.Id, firstname, lastname, gender, age);
                // db.FillUserInformation(user.Id);

                Console.WriteLine("================User information================");
                UserInformation userInfo = db.GetUserInformation(user.Id);
                Console.WriteLine($"Firstname: {userInfo.FirstName}");
                Console.WriteLine($"Lastname: {userInfo.LastName}");
                Console.WriteLine($"Gender: {userInfo.Gender}");
                Console.WriteLine($"Age: {userInfo.Age}");
                
            }
            catch (Exception e)
            {
                Log.LogError(e);
            }

            db.DeleteDataBase("uidemo");
        }

        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();

            // DataBaseTest1();
            // UserTest1();
            UserInterfaceDemo();


            watch.Stop();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Program finished execution in {watch.ElapsedMilliseconds}ms");
            Console.ResetColor();
            
            Console.Write("Press any key to exit");
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}