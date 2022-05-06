using Insurance.Business.DTOs;
using System.Threading.Tasks;

namespace Insurance.Business
{
    public interface IProductService
    {
        Task<InsuranceDto> CalculateInsurance(int productId);
        Task<InsuranceOrderDto> CalculateInsurance(OrderDto order);
        Task<ProductTypeDto> UpdateSurcharge(ProductTypeDto productTypeDto);
    }
}