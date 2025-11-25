using static Vehicles.Constant;

namespace Vehicles.TaxPolicyHandlers
{
    public sealed class TaxPolicyYear2024Handler : ITaxPolicyYearHandler
    {
        public int GetTaxPolicyYear() => 2024;

        public decimal CalculateTax(Vehicle vehicle)
        {
            return (vehicle.EvType, vehicle.EvRange) switch
            {
                (EvType.PHEV, _) => 200.0m,
                (EvType.BEV, >= 100) => 20.0m,
                (EvType.BEV, < 100) => 50.0m,
                _ => throw new ArgumentException($"No rules defined for policy: 2024")
            };
        }
    }
}
