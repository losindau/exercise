namespace Vehicles;

public class Vehicle
{
    public Vehicle()
    {
        Id = "";
        Make = "";
        Model = "";
        ModelYear = 0;
        City = "";
        County = "";
        State = "";
        EvType = "";
    }

    public string Id { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public string MakeAndModel => $"{Make} {Model}";
    public int ModelYear { get; set; }
    public string City { get; set; }
    public string County { get; set; }
    public string State { get; set; }
    public string EvType { get; set; }
    public string CAFVEligibility { get; set; }
    public int EvRange { get; set; }
    public decimal Tax { get; set; }

    public override bool Equals(object obj)
    {
        if (obj is not Vehicle other)
            return false;

        return other.Id == this.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return $"{ModelYear} {MakeAndModel} ({Id})";
    }
}
