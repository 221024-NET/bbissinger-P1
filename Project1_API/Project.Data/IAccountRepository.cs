
using Project1.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Data
{
    public interface IAccountRepository
    {
        public bool MakeAccount(string? connectionString, string email, string password);

        public Account GetLogin(string? connectionString, string email, string password); 


    }
}
