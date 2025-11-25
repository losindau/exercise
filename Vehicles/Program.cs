namespace Vehicles.TaxPolicyHandlers;

static class Program
{
    /// <summary>
    /// Reports the most popular EV car model in Washington state.
    /// </summary>
    public static void Main(string[] args)
    {
        Console.WriteLine($"Loading data...");
        DateTime start = DateTime.Now;
        var db = new VehicleDatabase();
        Console.WriteLine($"Loaded data after {DateTime.Now - start}");

        var taxPolicyFactory = new TaxPolicyFactory();
        taxPolicyFactory.RegisterHandlersFromAssembly(typeof(TaxPolicyFactory).Assembly);
        taxPolicyFactory.RegisterHandler(new TaxPolicyYear2025Handler(db));
        var registry = new VehicleRegistry(db, taxPolicyFactory);
        Console.WriteLine($"The most popular EV model overall is {registry.GetMostPopularModel()}");
    }
}
