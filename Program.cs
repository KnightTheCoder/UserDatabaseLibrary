﻿using Knight.MysqlTest2.DB;
using Knight.MysqlTest2.Exceptions;
using Knight.MysqlTest2.Logging;

namespace Knight.MysqlTest2
{
    class Program
    {

        private static void DataBaseTest1()
        {
            // UserDatabase db = new UserDatabase();
            // UserDatabase db = new UserDatabase("cs_dbtest");
            UserDatabase db = new UserDatabase("cs_dbtest", true);

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
            UserDatabase db = new UserDatabase("userTest", true);
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
                user2 = new User("testaccount", "test@testmail.com", "123test", db); // Loggs in because user already exists
                user4 = new User("testuser", "testuser@testmail.com", "123", db);
                user3 = new User("testaccount", "test@testmail.com", "123", db); // Error, incorrect login info
                
            }
            catch (UserDatabaseException e)
            {
                Log.LogError(e);
            }

            Console.WriteLine($"UserId: {user1.Id} LoggedIn: {user1.LoggedIN}");
            Console.WriteLine($"UserId: {user2.Id} LoggedIn: {user2.LoggedIN}");
            Console.WriteLine($"UserId: {user3.Id} LoggedIn: {user3.LoggedIN}");
            Console.WriteLine($"UserId: {user4.Id} LoggedIn: {user4.LoggedIN}");

            db.ShowAllUsers();

            db.DeleteDataBase("userTest");
        }
        public static void Main(string[] args)
        {
            // DataBaseTest1();
            UserTest1();
        }
    }
}