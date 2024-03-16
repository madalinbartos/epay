using System.Text.Json;

namespace epay_rest_server
{
    public class CustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private const string DataFilePath = "customers.json";
        private static readonly object fileLock = new object();

        public CustomerService(ILogger<CustomerService> logger)
        {
            _logger = logger;
        }

        private List<Customer> LoadCustomers()
        {
            if (File.Exists(DataFilePath))
            {
                var jsonData = File.ReadAllText(DataFilePath);
                return JsonSerializer.Deserialize<List<Customer>>(jsonData) ?? new List<Customer>();
            }
            else
            {
                return new List<Customer>();
            }
        }

        public List<Customer> GetCustomers()
        {
            lock (fileLock)
            {
                return LoadCustomers();
            }
        }

        public void AddCustomers(List<Customer> newCustomers)
        {
            lock (fileLock)
            {
                List<Customer> existingCustomers = LoadCustomers();
                foreach (var newCustomer in newCustomers)
                {
                    if (ValidateCustomer(newCustomer, existingCustomers))
                    {
                        InsertSorted(newCustomer, existingCustomers);
                    }
                    else
                    {
                        _logger.LogError($"Validation failed for customer: {newCustomer.FirstName} {newCustomer.LastName}");
                    }
                }
                SaveCustomers(existingCustomers);
            }
        }

        private bool ValidateCustomer(Customer customer, List<Customer> existingCustomers)
        {
            if (string.IsNullOrWhiteSpace(customer.FirstName))
            {
                _logger.LogError("First name is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(customer.LastName))
            {
                _logger.LogError("Last name is required.");
                return false;
            }

            if (customer.Age < 18)
            {
                _logger.LogError("Customer must be at least 18 years old.");
                return false;
            }

            if (existingCustomers.Any(c => c.Id == customer.Id))
            {
                _logger.LogError($"Customer with ID '{customer.Id}' already exists.");
                return false;
            }

            return true;
        }

        private void InsertSorted(Customer newCustomer, List<Customer> existingCustomers)
        {
            int index = 0;

            while (index < existingCustomers.Count &&
                   (string.Compare(existingCustomers[index].LastName, newCustomer.LastName, StringComparison.OrdinalIgnoreCase) < 0 ||
                    (string.Compare(existingCustomers[index].LastName, newCustomer.LastName, StringComparison.OrdinalIgnoreCase) == 0 &&
                     string.Compare(existingCustomers[index].FirstName, newCustomer.FirstName, StringComparison.OrdinalIgnoreCase) < 0)))
            {
                index++;
            }

            existingCustomers.Insert(index, newCustomer);
        }

        private void SaveCustomers(List<Customer> customers)
        {
            var jsonData = JsonSerializer.Serialize(customers);
            File.WriteAllText(DataFilePath, jsonData);
        }
    }
}