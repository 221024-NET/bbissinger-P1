using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Logic
{
    public class Account
    {
        // Fields 

        public string email { get; set; }
        public string password { get; set; }


        // Constructor 
        public Account(string email, string password)
        {
            this.email = email;
            this.password = password;
        }

        public Account() { }


        // Methods 


    }
}