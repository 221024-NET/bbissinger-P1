using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project1.Logic;
using Project1.InOut;
using System.Reflection.Metadata;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Sockets;


namespace Project1.Application
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("https://localhost:????/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                Ticket? ticket = null;
                Account? account = null;
                TicketIO io = new TicketIO();
                AccountIO io2 = new AccountIO();
                List<Ticket> tickets;
                List<Account> accounts;

                int num = io2.MainMenu();

                //OPTION REGISTER NEW USER  
                if (num == 1)
                {
                    account = await makeAccountAsync();
                    if (account.email == "default")
                    {
                        Console.WriteLine("Unable to Register a new user at this time.\n Please try again later.");

                        Environment.Exit(0);
                    }
                    Console.WriteLine("User has logged in successfully!");
                    //do I return them to main menu for login here now ??????????

                }
                //OPTION LOGIN AS EMPLOYEE  
                else if (num == 2)
                {
                    account = await existAccountAsync();
                    if (account.email == "default")
                    {
                        Console.WriteLine("Unable to Log the user in at this time.\n Please try again later.");

                        Environment.Exit(0);
                    }
                    Console.WriteLine("Employee login successful");

                    num = io.employeeTicketMenu();

                    //TICKET REIMBURSTMENT CREATION 
                    if (num == 1)
                    {
                        Console.WriteLine("Enter request amount for reimburstment in exact dollar amount: ");
                        double amount = double.Parse(Console.ReadLine().ToString());
                        Console.WriteLine("Enter a description about your ticket: ");
                        string input = Console.ReadLine();

                        var url = await makeTicketAsync(new Ticket(ticket.id, amount, input, "Pending"));
                        Console.WriteLine($"Created at {url}");
                    }

                    //VIEW ALL EXISTING TICKETS 
                    else if (num == 2)
                    {
                        var all = "/tickets/" + account.email;
                        tickets = await GetAvailableTicketsUserAsync(all);
                        foreach (Ticket t in tickets)
                        {
                            printTicket(ticket);
                        }
                    }

                }

                //OPTION LOGIN AS MANAGER  
                else if (num == 3)
                {
                    account = await existAccountAsync();
                    if (account.email == "default")
                    {
                        Console.WriteLine("Unable to Log the user in at this time.\n Please try again later.");

                        Environment.Exit(0);
                    }
                    Console.WriteLine("Account login successful");

                    num = io.managerTicketMenu();

                    //VIEW TICKETS  
                    if (num == 1)
                    {
                        var view = "/tickets/Pending";
                        tickets = await GetPendingTicketsAsync(view);
                        foreach (Ticket t in tickets)
                        {
                            printTicket(t);
                        }
                    }

                    //UPDATE TICKET STATUS 
                    else if (num == 2)
                    {
                        //Show pending list 
                        tickets = await GetPendingTicketsAsync("/tickets/Pending");
                        List<int> update = new List<int>();
                        
                        foreach (Ticket t in tickets)
                        {
                            printTicket(t);
                            update.Add(t.id);
                        }

                        //Find + change status 
                        int status = io.findTicket(update);

                        Ticket index = tickets.Find(x => x.id == status);

                        string change = io.ticketStatus();

                        if (change == "Accept")
                        {
                            ticket = await UpdateToAccept(status, index);
                            printTicket(ticket);
                        }
                        else
                        {
                            ticket = await UpdateToDecline(status, index);
                            printTicket(ticket);
                        }
                    }

                    



                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    





        //aasync methods 

            //async make new account method 
            static async Task<Account> makeAccountAsync()
            {
                Account account;
                Account acc = new Account();
                string? username = "", password = "";

                Console.WriteLine("Please enter email: ");
                username = Console.ReadLine();
                Console.WriteLine("Please enter password: ");
                password = Console.ReadLine();

                if (username == null || password == null)
                {
                    Console.WriteLine("Error, entry not recognized as valid option, try again.");
                    Environment.Exit(1);
                }

                acc.email = username;
                acc.password = password;

                HttpResponseMessage reply = await client.PostAsJsonAsync("make", acc);
                if (reply.IsSuccessStatusCode)
                {
                    account = await reply.Content.ReadAsAsync<Account>();
                }
                else
                {
                    account = new Account();
                }
                return account;
            }


            //async existing account method 
            static async Task<Account> existAccountAsync()
            {
                Account acc = new Account();
                Account account;
                string? username = "", password = "";

                Console.WriteLine("Please enter email: ");
                username = Console.ReadLine();
                Console.WriteLine("Please enter password: ");
                password = Console.ReadLine();

                if (username == null || password == null)
                {
                    Console.WriteLine("Error, entry not recognized as valid option, try again.");
                    Environment.Exit(1);
                }

                acc.email = username;
                acc.password = password;

                HttpResponseMessage reply = await client.PostAsJsonAsync("existing", acc);
                if (reply.IsSuccessStatusCode)
                {
                    acc = await reply.Content.ReadAsAsync<Account>();
                }
                else
                {
                    account = new Account();
                }
                return account;
            }

            //print account details method 
            static void printAccount(Account acc)
            {
                Console.WriteLine($"User information: Email{acc.email}");
            }

            //print ticket details method 
            static void printTicket(Ticket t)
            {
                if (t != null)
                {
                    Console.WriteLine($"Ticket ID: {t.id}\t Amount: {t.amount}\t" +
                $" Description: {t.description}\t Status: {t.status}\t\n");
                }
            }

            //async get email method
            static async Task<List<string>> GetEmailAsync()
            {
                List<Account> accounts = new List<Account>();
                List<string> emails = new List<string>();

                HttpResponseMessage reply = await client.GetAsync("/Accounts");

                if (reply.IsSuccessStatusCode)
                {
                    accounts = await reply.Content.ReadAsAsync<List<Account>>();
                }
                foreach (Account account in accounts)
                {
                    emails.Add(account.email);
                }

                return emails;
            }

            //async make new ticket method
            static async Task<Uri> makeTicketAsync(Ticket ticket)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("/tickets", ticket);
                return response.Headers.Location;
            }

            //async get available tickets method 
            static async Task<List<Ticket>> GetAvailableTicketsUserAsync(string all)
            {
                List<Ticket> tickets = null;
                HttpResponseMessage reply = await client.GetAsync(all);
                if (reply.IsSuccessStatusCode)
                {
                    tickets = await reply.Content.ReadAsAsync<List<Ticket>>();
                }
                else
                {
                    tickets = new List<Ticket>();
                    Console.WriteLine("\nThe User has NO tickets at this time.\n");
                }

                return tickets;
            }

            //aync get pending tickets method 
             static async Task<List<Ticket>> GetPendingTicketsAsync(string update)
             {
                List<Ticket> tickets = null;
                HttpResponseMessage reply = await client.GetAsync(update);
                if (reply.IsSuccessStatusCode)
                {
                    tickets = await reply.Content.ReadAsAsync<List<Ticket>>();
                }
                return tickets;
             }

        //async update ticket status to accept method 
        static async Task<Ticket> UpdateToAccept(int id, Ticket t)
        {
            HttpResponseMessage reply = await client.PutAsJsonAsync($"ticket/Accept/{id}", t);
            reply.EnsureSuccessStatusCode();

            return await reply.Content.ReadAsAsync<Ticket>();
        }


        //async update ticket status to decline method 
        static async Task<Ticket> UpdateToDecline(int id, Ticket t)
        {
            HttpResponseMessage reply = await client.PutAsJsonAsync($"ticket/Decline/{id}", t);
            reply.EnsureSuccessStatusCode();

            return await reply.Content.ReadAsAsync<Ticket>();
        }



    }
       
    
}