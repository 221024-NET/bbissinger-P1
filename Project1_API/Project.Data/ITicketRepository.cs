using Project1.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Data
{
    public interface ITicketRepository
    {

        public bool makeTicket(string? connectionString, double amount, string description);

        public IEnumerable<Ticket> GetAvailableTickets(string? connectionString);

        public IEnumerable<Ticket> GetPendingTickets(string? connectionString);

        public void updateTicketStatus(string? connectionString, int id, string updateStatus);


    };
}
