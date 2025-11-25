using Vehicles.TaxPolicyHandlers;

namespace Vehicles;

public class VehicleRegistry
{
    private readonly VehicleDatabase db;
    private readonly TaxPolicyFactory _taxPolicyFactory;

    public VehicleRegistry(
        VehicleDatabase db,
        TaxPolicyFactory taxPolicyFactory)
    {
        this.db = db;
        _taxPolicyFactory = taxPolicyFactory;
    }

    public IEnumerable<Vehicle> GetVehicles() => db.GetVehicles();

    public IEnumerable<Vehicle> GetRegistrations() => db.GetRegistrations().SelectMany(list => list);

    /// <summary>
    /// Updates the tax for a given vehicle and year.
    /// </summary>
    public void UpdateTax(Vehicle vehicle, int year)
    {
        _taxPolicyFactory.TryGetHandler(year, out var taxPolicyHandler);
        vehicle.Tax = taxPolicyHandler.CalculateTax(vehicle);
    }

    public string GetMostPopularModel()
    {
        Dictionary<string, int> counts = new Dictionary<string, int>();
        foreach (var vehicle in db.GetVehicles())
        {
            if (counts.ContainsKey(vehicle.MakeAndModel))
            {
                counts[vehicle.MakeAndModel] = counts[vehicle.MakeAndModel] + 1;
            }
            else
            {
                counts[vehicle.MakeAndModel] = 0;
            }
        }
        int highest = 0;
        string best = "";
        foreach (var kvp in counts)
        {
            if (kvp.Value > highest)
            {
                highest = kvp.Value;
                best = kvp.Key;
            }
        }
        return best;
    }
}