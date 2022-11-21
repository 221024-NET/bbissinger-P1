
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Project1.InOut
{
    public class AccountIO
    {
        public int MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to my app!");
            Console.WriteLine("The Workplace's Website");
            Console.WriteLine("");
            Console.WriteLine("Option 1. Register as a new user/account");
            Console.WriteLine("Option 2. Login as Employee");
            Console.WriteLine("Option 3. Login as Manager");
            Console.WriteLine("Please type 1 or 2 or 3 to navigate");

            int num = 0;
            string answer = Console.ReadLine();
            bool loop = Int32.TryParse(answer, out num);

            while (!loop || num < 1 || num > 3)
            {
                Console.WriteLine("Error, please try again.\n");
                Console.WriteLine("");
                Console.WriteLine("Option 1. Register as a new user/account");
                Console.WriteLine("Option 2. Login as Employee");
                Console.WriteLine("Option 3. Login as Manager");
                Console.WriteLine("Please type 1 or 2 or 3 to navigate");

                answer = Console.ReadLine();
                loop = Int32.TryParse(answer, out num);
            }
            return num;
        }

        public static string getEmail()
        {
            string email;
            Console.WriteLine("Please enter your email:");
            email = Console.ReadLine();

            return email; 
        }

        public static string getPassword()
        {
            string password;
            Console.WriteLine("Please enter your password: ");
            password = Console.ReadLine();

            return password;


        }


    }
    
}
