using Bogus;
using Insurance.Business.Models;

namespace Insurance.Tests.Extensions
{
    public static class ProductType_Extensions
    {
        public static ProductType Build(this ProductType instance)
        {
            instance = new Faker<ProductType>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Name, new Faker().PickRandom<Enums.ProductNameTypes>().ToString())
                .RuleFor(p => p.SurchargeRate, 0)
                .RuleFor(p => p.CanBeInsured, true);

            return instance;
        }

        public static ProductType WithZeroSurchargeRate(this ProductType instance)
        {
            instance.SurchargeRate = 0;

            return instance;
        }

        public static ProductType WithSurchargeRate(this ProductType instance, decimal surchargeRate)
        {
            instance.SurchargeRate = surchargeRate;

            return instance;
        }

        public static ProductType WithCanBeInsured(this ProductType instance, bool canBeInsured)
        {
            instance.CanBeInsured = canBeInsured;

            return instance;
        }

        public static ProductType WithProductTypeId(this ProductType instance, int productTypeId)
        {
            instance.Id = productTypeId;

            return instance;
        }

        public static ProductType WithProductTypeNull(this ProductType instance)
        {
            instance = null;

            return instance;
        }
    }
}