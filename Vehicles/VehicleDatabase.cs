using System.Collections.Generic;
using System.Reflection;

namespace Vehicles;

public class VehicleDatabase
{
    private List<List<Vehicle>> _vehicles;
    private Dictionary<string, List<Vehicle>> _registrations;

    public VehicleDatabase()
    {
        _vehicles = new List<List<Vehicle>>();
        string csvPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ev.csv");
        Load(csvPath);
    }

    /// <summary>
    /// Loads the vehicle data from disk. 
    /// NOTE: the registrations are in chronological order with newest first. That is, the first occurrence of a vehicle
    ///       will be the current registration for that vehicle. Any subsequent occurrence is a previous owner etc.
    /// </summary>
    /// <param name="csvPath"></param>
    private void Load(string csvPath)
    {
        var lines = File.ReadAllLines(csvPath);
        _registrations = new Dictionary<string, List<Vehicle>>();

        foreach (string row in lines.Skip(1))
        {
            var columns = row.Split(',');
            string id = columns[0];
            string county = columns[1];
            string city = columns[2];
            string state = columns[3];
            int modelYear = int.Parse(columns[5]);
            string make = columns[6];
            string model = columns[7];
            string evType = columns[8];
            string cAFVEligibility = columns[9];
            int evRange = int.Parse(columns[10]);

            var entry = new Vehicle
            {
                Id = id,
                State = state,
                City = city,
                County = county,
                Make = make,
                Model = model,
                ModelYear = modelYear,
                EvType = evType,
                CAFVEligibility = cAFVEligibility,
                EvRange = evRange
            };

            if (!_registrations.ContainsKey(id))
            {
                _registrations[id] = new List<Vehicle>();
            }

            _registrations[id].Add(entry);
        }

        _vehicles = _registrations.Select(x => x.Value).ToList();
    }

    public List<List<Vehicle>> GetRegistrations() => _vehicles;

    public IEnumerable<Vehicle> GetVehicles() => _vehicles.Select(x => x[0]);

    public bool CheckVehicleIsUsed(string id) => _registrations.ContainsKey(id) ? _registrations[id].Count > 1 : false;

    public void AddRegistration(List<Vehicle> vehicles) => _registrations.Add(vehicles[0].Id, vehicles);
}
