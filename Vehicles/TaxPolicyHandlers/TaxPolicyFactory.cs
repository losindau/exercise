using System.Reflection;

namespace Vehicles.TaxPolicyHandlers;

public class TaxPolicyFactory
{
    private Dictionary<int, ITaxPolicyYearHandler> _taxPolicyHandlers = [];

    public TaxPolicyFactory()
    {
    }

    public bool TryGetHandler(int year, out ITaxPolicyYearHandler handler) => _taxPolicyHandlers.TryGetValue(year, out handler);

    public void RegisterHandler(ITaxPolicyYearHandler handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        var year = handler.GetTaxPolicyYear();

        if (_taxPolicyHandlers.ContainsKey(year))
        {
            throw new InvalidOperationException($"Multiple tax policy handlers registered for one year: {year}");
        }

        _taxPolicyHandlers[year] = handler;
    }

    public void RegisterHandlersFromAssembly(Assembly assembly)
    {
        var taxPolicyHandlers = assembly.GetTypes()
            .Where(type => typeof(ITaxPolicyYearHandler).IsAssignableFrom(type) 
                && type.IsClass 
                && !type.IsAbstract
                && type.GetConstructor(Type.EmptyTypes) != null)
            .Select(type => (ITaxPolicyYearHandler?)Activator.CreateInstance(type))
            .Where(handler => handler != null)
            .Cast<ITaxPolicyYearHandler>()
            .ToList();

        foreach (var handler in taxPolicyHandlers)
        {
            var year = handler.GetTaxPolicyYear();

            if (_taxPolicyHandlers.ContainsKey(year))
            {
                throw new InvalidOperationException($"Multiple tax policy handlers registerd for one year: {year}");
            }

            _taxPolicyHandlers[year] = handler;
        }
    }
}