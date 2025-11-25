namespace Vehicles.TaxPolicyHandlers;

public interface ITaxPolicyYearHandler
{
    public int GetTaxPolicyYear();
    public decimal CalculateTax(Vehicle vehicle);
}