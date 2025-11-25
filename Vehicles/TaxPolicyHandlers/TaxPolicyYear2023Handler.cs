using static Vehicles.Constant;

namespace Vehicles.TaxPolicyHandlers;

public sealed class TaxPolicyYear2023Handler : ITaxPolicyYearHandler
{
    public int GetTaxPolicyYear() => 2023;

    public decimal CalculateTax(Vehicle vehicle)
    {
        return vehicle.EvType switch
        {
            EvType.PHEV => 100.0m,
            EvType.BEV => 10.0m,
            _ => throw new ArgumentException($"No rules defined for policy: 2023")
        };
    }
}