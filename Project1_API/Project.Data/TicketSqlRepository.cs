using Project1.Data;
using Project1.Logic;
using System.Data.SqlClient;

namespace Project1.Data
{
    public class TicketSqlRepository : ITicketRepository
    {
        // Fields 

        // Constructors 

        // Methods

        //making ticket 
        public bool makeTicket(string? connectionString, double amount, string description)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            
            connection.Open();

            //checking for duplicate possiblity 
            string checkDuplicate = @"SELECT amount, description FROM Project1.Logic.Ticket WHERE = @amount and description = @description;";
            
            using SqlCommand checkDuplicateCommand = new SqlCommand(checkDuplicate, connection);
            
            checkDuplicateCommand.Parameters.AddWithValue("@amount", amount);

            checkDuplicateCommand.Parameters.AddWithValue("@description", description);

            using SqlDataReader reader = checkDuplicateCommand.ExecuteReader(); 


            while(reader.Read())
            {
                return false;
            }
            reader.Close();


            // creating fresh new tickets and inserting them into the database 
            string newTicket = @"INSERT into Tickets (amount, description, status) values (@amount, @description, 'pending');";
           
            using SqlCommand creationTicket = new SqlCommand(newTicket, connection);
            creationTicket.Parameters.AddWithValue("@amount", amount);
            creationTicket.Parameters.AddWithValue("@description", description);

            creationTicket.ExecuteNonQuery();

            connection.Close();
            return true;
        }

        // getting all tickets 
        public IEnumerable<Ticket> GetAvailableTickets(string? connectionString)
        {
            List<Ticket> tickets = new List<Ticket>();

            using SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            string getAllTickets = "SELECT id, amount, description, status FROM Tickets;";
          
            using SqlCommand allTickets = new SqlCommand(getAllTickets, connection);
            using SqlDataReader reader = allTickets.ExecuteReader();

            while (reader.Read())
            {
                // ticket class has int, this converts it to double 
                double amount = Convert.ToDouble(reader.GetFloat(1));
                tickets.Add(new Ticket(reader.GetInt32(0), amount, reader.GetString(2), reader.GetString(3))); 

            }

           connection.Close();

            return tickets;
        }


        //getting pending tickets
        public IEnumerable<Ticket> GetPendingTickets(string? connectionString)
        {
            List<Ticket> pendingTickets = new List<Ticket>();

            using SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            string grabPendingTickets = @"SELECT id, amount, description, status FROM Project1.Logic.Ticket WHERE status = 'pending';";
            
            using SqlCommand pendingTicket = new SqlCommand(grabPendingTickets, connection);
            using SqlDataReader reader = pendingTicket.ExecuteReader();

            while (reader.Read())
            {
                // ticket class id is an int, so need to convert to double
                double amount = Convert.ToDouble(reader.GetDecimal(1));
                pendingTickets.Add(new Ticket(reader.GetInt32(0), amount, reader.GetString(2), reader.GetString(3)));
            }

            connection.Close();
            return pendingTickets;
        }

        //update ticket status 
        public void updateTicketStatus(string? connectionString, int id, string updateStatus)
        {
            using SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            string updateTicket = @"UPDATE Project1.Logic.Ticket set status = @updateStatus WHERE id = @id;";
          
            using SqlCommand updateTicketStatus = new SqlCommand(updateTicket, connection);
            updateTicketStatus.Parameters.AddWithValue("@updateStatus", updateStatus);
            updateTicketStatus.Parameters.AddWithValue("@id", id);
          
            using SqlDataReader reader = updateTicketStatus.ExecuteReader();

            connection.Close();

        }



    }
}