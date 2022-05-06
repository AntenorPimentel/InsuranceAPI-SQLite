using Insurance.Business.Models;
using System.Threading.Tasks;

namespace Insurance.Business
{
    public interface IProductGateway
    {
        Task<Product> GetProductById(int productId);
        Task<ProductType> GetProductTypeById(int productTypeId);
        Task UpdateProductTypeSurcharge(int productTypeId, decimal surchargeRate);
        Task<ProductType> GetSurchargeRate(int productTypeId);
    }
}