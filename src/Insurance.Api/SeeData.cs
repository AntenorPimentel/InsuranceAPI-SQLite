using Insurance.Business;
using Insurance.Business.Models;
using Insurance.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insurance.Api
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var bootstrapper = (DatabaseBoostrap)serviceProvider.GetService(typeof(DatabaseBoostrap));
            var db = (IProductGateway)serviceProvider.GetService(typeof(IProductGateway));
            if (bootstrapper != null && db != null)
            {
                IEnumerable<ProductType> productTypes = new List<ProductType>()
                {
                    new ProductType(){ Id = 21, Name = "Laptops", CanBeInsured = true, SurchargeRate = 0 },
                    new ProductType(){ Id = 32, Name = "Smartphones", CanBeInsured = true, SurchargeRate = 0 },
                    new ProductType(){ Id = 33, Name = "Digital cameras", CanBeInsured = true, SurchargeRate = 0 },
                    new ProductType(){ Id = 35, Name = "SLR cameras", CanBeInsured = false, SurchargeRate = 0 },
                    new ProductType(){ Id = 12, Name = "MP3 players", CanBeInsured = false, SurchargeRate = 0 },
                    new ProductType(){ Id = 124, Name = "Washing machines", CanBeInsured = true, SurchargeRate = 0 },
                    new ProductType(){ Id = 841, Name = "Laptops", CanBeInsured = false, SurchargeRate = 0 }
                };

                await bootstrapper.Setup(productTypes);
            }
        }
    }
}