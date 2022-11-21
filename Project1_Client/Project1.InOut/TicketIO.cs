namespace Project1.InOut
{ 
    public class TicketIO 
    {
        // Fields 

        // Constructor 

        // Methods 
        public int employeeTicketMenu()
        {
            string num;
            int i = 0;
           
            Console.WriteLine("Welcome to The Employee Ticket Menu");
            Console.WriteLine("1. Create a ticket");
            Console.WriteLine("2. View existing tickets");
            Console.WriteLine("3. Quit");

            while (true)
            {
                num = Console.ReadLine();
                i = Int32.Parse(num);
                if (i < 4 && i > 0)
                {
                    return i;
                }
                Console.WriteLine("Welcome to The Employee Ticket Menu");
                Console.WriteLine("1. Create a ticket");
                Console.WriteLine("2. View existing tickets");
                Console.WriteLine("3. Quit");
            }
        }

        public int managerTicketMenu()
        {
            string num;
            int i = 0;
            Console.WriteLine("Welcome to The Manager Ticket Menu");
            Console.WriteLine("1. View pending tickets");
            Console.WriteLine("2. Update ticket status");
            Console.WriteLine("3. Quit");
            while (true)
            {
                num = Console.ReadLine();
                i = Int32.Parse(num);
                if (i < 3 && i > 0)
                {
                    return i;
                }
                Console.WriteLine("Welcome to The Manager Ticket Menu");
                Console.WriteLine("1. View pending tickets");
                Console.WriteLine("2. Update ticket status");
                Console.WriteLine("3. Quit");
            }

        }

        public int findTicket(List<int> ticketList)
        {
            int id;
            Console.WriteLine("Please enter the ID number of trhe ticke that you would like to accept or decline.\n");
            id = Int32.Parse(Console.ReadLine());
            while (!ticketList.Contains(id))
            {
                Console.WriteLine("Invalid entry. Please try again.\n");
                Console.WriteLine("Please enter the ID number of trhe ticke that you would like to accept or decline.\n");
                id = Int32.Parse(Console.ReadLine());
            }
            return id;
        }

        public string ticketStatus()
        {
            Console.WriteLine("1.Accept");
            Console.WriteLine("2.Decline");
            int entry = Int32.Parse(Console.ReadLine());
            if (entry == 1)
            {
                return "Accept";
            }
            else
            {
                return "Decline";
            }
        }

        public static double getTicketAmount()
            {
                double amount = 0;

                while (true)
                {
                    try
                    {
                        Console.WriteLine("Enter the dollar amount for your reimburstment request: ");
                        amount = Convert.ToDouble(Console.ReadLine());
                    }
                    catch (System.FormatException ex)
                    {
                        Console.WriteLine("Wrong input value, submisison must include cent value to the second demical place.");
                        continue;
                    }
                    return amount;
                }
            }

            public string getTicketDescription()
            {
                string description;

                Console.WriteLine("Enter a description about your ticket:");
                description = Console.ReadLine();

                return description;
            }

            


        

    }
}