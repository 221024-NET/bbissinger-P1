using Project1.Data;
using Project1.Logic; 

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>(true)
    .Build();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:Project1ConnectionString");

builder.Services.AddTransient<AccountSqlRepository>();
builder.Services.AddTransient<TicketSqlRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/existingaccount", (Account acc, AccountSqlRepository repo) =>
{
    return repo.GetLogin(connectionString, acc.email, acc.password);
});

app.MapPost("/newaccount", (AccountSqlRepository repo, Account acc) =>
{
    if (repo.MakeAccount(connectionString, acc.email, acc.email))
    {
        return repo.GetLogin(connectionString, acc.email, acc.password);
    }
    else
    {
        return null;
    }
});

app.MapGet("/alltickets", (TicketSqlRepository repo) =>
{
    return repo.GetAvailableTickets(connectionString);
});

app.MapPost("/maketickets", (TicketSqlRepository repo, Ticket ticket) =>
{
    return repo.makeTicket(connectionString, ticket.amount, ticket.description);
});

app.MapGet("/pendingtickets", (TicketSqlRepository repo) =>
{
    return repo.GetPendingTickets(connectionString);
});

app.MapPut("/updatetickets/{id}", (TicketSqlRepository repo, int id, Ticket ticket) =>
{
    repo.updateTicketStatus(connectionString, ticket.id, ticket.status);
    return Results.NoContent();
});


app.Run();


