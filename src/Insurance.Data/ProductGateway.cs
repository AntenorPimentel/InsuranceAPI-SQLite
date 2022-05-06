using Dapper;
using Insurance.Business;
using Insurance.Business.Models;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Insurance.Data
{
    public class ProductGateway : APIClientBase, IProductGateway
    {
        private readonly string _dbConnectionString;

        public ProductGateway(SqliteConfiguration configuration)
        {
            _dbConnectionString = configuration.DBConnectionString;
        }

        public ProductGateway(HttpClient client, SqliteConfiguration configuration) : base(client)
        {
            _dbConnectionString = configuration.DBConnectionString;
        }

        public async Task<Product> GetProductById(int productId)
        {
            var response = await GetAsync($"/products/{productId}");

            return response == null ? null : JsonConvert.DeserializeObject<Product>(response);
        }

        public async Task<ProductType> GetProductTypeById(int productTypeId)
        {
            var response = await GetAsync($"/product_types/{productTypeId}");

            return response == null ? null : JsonConvert.DeserializeObject<ProductType>(response);
        }

        public async Task SaveProductTypesAsync(IEnumerable<ProductType> productTypes)
        {
            var conn = new SqliteConnection(_dbConnectionString);
            await conn.ExecuteAsync("INSERT INTO [ProductType] VALUES (@Id, @Name, @CanBeInsured, @SurchargeRate)", productTypes);
        }

        public async Task UpdateProductTypeSurcharge(int productTypeId, decimal surchargeRate)
        {
            var conn = new SqliteConnection(_dbConnectionString);
            await conn.ExecuteAsync($"UPDATE ProductType SET SurchargeRate = {surchargeRate} WHERE Id = {productTypeId}");
        }

        public async Task<ProductType> GetSurchargeRate(int productTypeId)
        {
            var conn = new SqliteConnection(_dbConnectionString);
            return await conn.QueryFirstAsync<ProductType>($"SELECT [Id], [Name], [CanBeInsured], [SurchargeRate] FROM [ProductType] WHERE[Id] == {productTypeId}");
        }
    }
}