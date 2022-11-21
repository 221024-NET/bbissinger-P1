using Project1.Logic; 
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Data
{

    public class AccountSqlRepository : IAccountRepository
    {
       
        public bool MakeAccount(string? connectionString, string email, string password)
        {

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            // check if email duplicate exists 
            string checkDuplicate = @"SELECT email FROM Project1.Accounts WHERE email = @email;";
           
            using SqlCommand checkDuplicateCommand = new SqlCommand(checkDuplicate, connection);
            checkDuplicateCommand.Parameters.AddWithValue("@email", email);
            
            using SqlDataReader reader = checkDuplicateCommand.ExecuteReader();

            while (reader.Read())
            {
                return false;
            }
            reader.Close();

            //account creation and insert into database 
            string newUser = @"INSERT into Project1.Accounts (email, password) values (@email, @password);";

            using SqlCommand creationAccount = new SqlCommand(newUser, connection);

            creationAccount.Parameters.AddWithValue("@email", email);
            creationAccount.Parameters.AddWithValue("@password", password);

            creationAccount.ExecuteNonQuery();
            //done adding to our database 

            connection.Close();

            return true;

        } 

        public Account GetLogin(string? connectionString, string email, string password)
        {

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            //selecting from database and making a new object
            string newLogin = @"SELECT id, email, password FROM Project1.Accounts where email = @email and password = @password;";
           
            using SqlCommand creationLogin = new SqlCommand(newLogin, connection);
            creationLogin.Parameters.AddWithValue("@email", email);
            creationLogin.Parameters.AddWithValue("@password", password);

            using SqlDataReader reader = creationLogin.ExecuteReader();

            Account userAccount;

            while (reader.Read())
            {
                //                                 email                  password             
                return userAccount = new Account(reader.GetString(1), reader.GetString(2));

            }

            connection.Close();

            Account zeroAccount = new();

            return zeroAccount; 


        }


    }
}
