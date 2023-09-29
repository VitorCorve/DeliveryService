using DeliveryService.ApiAssistant;

string baseUrl = "https://localhost:7106";

var client = new ApiAssistant(baseUrl, new HttpClient());

var courier = new Courier();
courier.CourierId = 1;

var customer = new Customer
{
    CustomerName = ""
};

var customers = await client.CustomersAllAsync();
//var allTickets = await client.TicketsAllAsync(null, null, null, null, string.Empty);

//var result = await client.TicketsPOSTAsync(customers?.FirstOrDefault()?.CustomerId ?? 1);

Console.ReadLine();