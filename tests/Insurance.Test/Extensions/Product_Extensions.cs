using Bogus;
using Insurance.Business.Models;

namespace Insurance.Tests.Extensions
{
    public static class Product_Extensions
    {
        public static Product Build(this Product instance)
        {
            instance = new Faker<Product>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Name, "Canon Powershot SX620 HS Black")
                .RuleFor(p => p.ProductTypeId, 0)
                .RuleFor(p => p.SalesPrice, new Faker().Random.Double());

            return instance;
        }

        public static Product WithSalesPrice(this Product instance, double salesPrice)
        {
            instance.SalesPrice = salesPrice;

            return instance;
        }

        public static Product WithProductNull(this Product instance)
        {
            instance = null;

            return instance;
        }
    }
}