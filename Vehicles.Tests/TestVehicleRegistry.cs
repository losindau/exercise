using Vehicles.TaxPolicyHandlers;
using static Vehicles.Constant;

namespace Vehicles.Tests
{
    [TestClass]
    public class TestVehicleRegistry
    {
        private readonly VehicleRegistry registry;
        private readonly VehicleDatabase _db;

        public TestVehicleRegistry()
        {
            _db = new VehicleDatabase();
            var taxPolicyFactory = new TaxPolicyFactory();
            taxPolicyFactory.RegisterHandlersFromAssembly(typeof(TaxPolicyFactory).Assembly);
            taxPolicyFactory.RegisterHandler(new TaxPolicyYear2025Handler(_db));
            registry = new VehicleRegistry(_db, taxPolicyFactory);
        }

        [TestMethod]
        public void ShouldLoadData()
        {
            Assert.AreEqual(11060, registry.GetVehicles().Count(), "Unexpected number of unique vehicles in db");
            Assert.AreEqual(181458, registry.GetRegistrations().Count(), "Unexpected number of registrations in db");
        }

        [TestMethod]
        public void ShouldCalculate2023TaxCorrectly()
        {
            foreach (var v in registry.GetVehicles())
            {
                registry.UpdateTax(v, 2023);
            }
            Assert.AreEqual(531530.0m, registry.GetVehicles().Sum(v => v.Tax));
        }

        [TestMethod]
        public void ShouldCalculate2024TaxCorrectly()
        {
            foreach (var v in registry.GetVehicles())
            {
                registry.UpdateTax(v, 2024);
            }
            Assert.AreEqual(1205890.0m, registry.GetVehicles().Sum(v => v.Tax));
        }

        [TestMethod]
        public void ShouldCalculate2025TaxCorrectly()
        {
            // Arrange 
            const decimal AdditionalTaxForseattle = 7.0m;
            const decimal TaxRebateForUsedVehicle = -10.0m;

            var phevWithCAFVEligibleAtSeattleNotUsed = new Vehicle()
            {
                Id = "TEST_ID_1",
                EvType = EvType.PHEV,
                CAFVEligibility = CAFVEligibility.Eligible,
                City = RegisteredCity.Seattle
            };

            var bevWithCAFVNotEligibleUsedVehicleNotAtSeattle = new Vehicle()
            {
                Id = "TEST_ID_2",
                EvType = EvType.BEV,
                CAFVEligibility = CAFVEligibility.Eligible + "TEST",
                City = RegisteredCity.Seattle + "TEST"
            };

            _db.AddRegistration([phevWithCAFVEligibleAtSeattleNotUsed]);
            _db.GetRegistrations().Add([phevWithCAFVEligibleAtSeattleNotUsed]);
            _db.AddRegistration([bevWithCAFVNotEligibleUsedVehicleNotAtSeattle, bevWithCAFVNotEligibleUsedVehicleNotAtSeattle]);
            _db.GetRegistrations().Add([bevWithCAFVNotEligibleUsedVehicleNotAtSeattle, bevWithCAFVNotEligibleUsedVehicleNotAtSeattle]);

            // Act
            foreach (var v in registry.GetVehicles())
            {
                registry.UpdateTax(v, 2025);
            }

            // Assert
            Assert.AreEqual(50.0m + AdditionalTaxForseattle, phevWithCAFVEligibleAtSeattleNotUsed.Tax);
            Assert.AreEqual(30.0m + TaxRebateForUsedVehicle, bevWithCAFVNotEligibleUsedVehicleNotAtSeattle.Tax);
        }

        [TestMethod]
        public void ShouldFindMostPopularModel()
        {
            Assert.AreEqual("TESLA MODEL S", registry.GetMostPopularModel());
        }
    }
}