using System;
using System.Linq;
using static Vehicles.Constant;

namespace Vehicles.TaxPolicyHandlers;

public sealed class TaxPolicyYear2025Handler : ITaxPolicyYearHandler
{
    private readonly VehicleDatabase _vehicleDatabase;

    public TaxPolicyYear2025Handler(VehicleDatabase vehicleDatabase)
    {
        _vehicleDatabase = vehicleDatabase;
    }

    public int GetTaxPolicyYear() => 2025;

    public decimal CalculateTax(Vehicle vehicle)
    {
        bool isCafvEligible = string.Equals(vehicle.CAFVEligibility, CAFVEligibility.Eligible, StringComparison.OrdinalIgnoreCase);

        decimal tax = vehicle.EvType switch
        {
            EvType.BEV => isCafvEligible ? 15.0m : 30.0m,
            EvType.PHEV => isCafvEligible ? 50.0m : 150.0m,
            _ => throw new ArgumentException($"No rules defined for policy: 2025")
        };

        if (string.Equals(vehicle.City, RegisteredCity.Seattle, StringComparison.OrdinalIgnoreCase))
        {
            tax += 7.0m;
        }

        var isVehicleUsed = _vehicleDatabase.CheckVehicleIsUsed(vehicle.Id);

        if (isVehicleUsed)
        {
            tax -= 10.0m;
        }

        return tax;
    }
}