using epay_rest_server_simulator;
using System.Text;
using System.Text.Json;

class Program
{
    private static readonly Random random = new Random();
    private static int customerIdCounter = 0; 

    static async Task Main()
    {
        // Setting up a new HTTP Client so that we can send requests
        var httpClient = new HttpClient();

        var baseUri = new Uri("https://localhost:7172/api/customers");

        var tasks = new List<Task>();

        // Simulated data
        string[] firstNames = { "Leia", "Sadie", "Jose", "Sara", "Frank", "Dewey", "Tomas", "Joel", "Lukas", "Carlos" };
        string[] lastNames = { "Liberty", "Ray", "Harrison", "Ronan", "Drew", "Powell", "Larsen", "Chan", "Anderson", "Lane" };

        // Sending multiple POST requests with simulated data
        for (int i = 0; i < 5; i++)
        {
            var customers = new List<Customer>
            {
                GenerateRandomCustomer(firstNames, lastNames),
                GenerateRandomCustomer(firstNames, lastNames)
            };
            var content = new StringContent(JsonSerializer.Serialize(customers), Encoding.UTF8, "application/json");
            tasks.Add(PostCustomersAsync(httpClient, baseUri, content));
        }

        // Wait for all POST requests to complete
        await Task.WhenAll(tasks);

        // Send a single GET request
        await GetCustomersAsync(httpClient, baseUri);
    }

    static async Task PostCustomersAsync(HttpClient httpClient, Uri baseUri, StringContent content)
    {
        try
        {
            string requestBody = await content.ReadAsStringAsync();
            Console.WriteLine($"Sending POST request to {baseUri}");
            Console.WriteLine($"Request body: {requestBody}");

            var response = await httpClient.PostAsync(baseUri, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("POST request successful.");
            }
            else
            {
                Console.WriteLine($"POST request failed with status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while sending POST request: {ex.Message}");
        }
    }

    static async Task GetCustomersAsync(HttpClient httpClient, Uri baseUri)
{
    try
    {
        Console.WriteLine($"Sending GET request to {baseUri}");

        var response = await httpClient.GetAsync(baseUri);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"GET request successful. Response: {responseBody}");
        }
        else
        {
            Console.WriteLine($"GET request failed with status code: {response.StatusCode}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error occurred while sending GET request: {ex.Message}");
    }
}

    static Customer GenerateRandomCustomer(string[] firstNames, string[] lastNames)
    {
        return new Customer
        {
            FirstName = firstNames[random.Next(firstNames.Length)],
            LastName = lastNames[random.Next(lastNames.Length)],
            Age = random.Next(10, 90),
            Id = customerIdCounter++
        };
    }
}