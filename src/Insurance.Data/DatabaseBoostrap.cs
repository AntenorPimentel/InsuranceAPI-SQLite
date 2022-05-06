using Dapper;
using Insurance.Business.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insurance.Data
{
    public class DatabaseBoostrap
    {
        private readonly SqliteConfiguration _dbConfiguration;

        public DatabaseBoostrap(SqliteConfiguration config)
        {
            _dbConfiguration = config;
        }

        public async Task Setup(IEnumerable<ProductType> productTypes)
        {
            using var connection = new SqliteConnection(_dbConfiguration.DBConnectionString);

            var table = connection.Query<string>("SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'ProductType';");
            var tableName = table.FirstOrDefault();
            if (!string.IsNullOrEmpty(tableName) && tableName == "ProductType")
                return;

            await connection.ExecuteAsync(
                "CREATE TABLE [ProductType] (" +
                "[Id] INTEGER NOT NULL, " +
                "[Name] TEXT NOT NULL, " +
                "[CanBeInsured] BOOLEAN NOT NULL, " +
                "[SurchargeRate] DECIMAL(5, 2) NULL, " +
                "CONSTRAINT [PK_ProductType] PRIMARY KEY([Id]))");

            await (new ProductGateway(_dbConfiguration)).SaveProductTypesAsync(productTypes);
        }
    }
}