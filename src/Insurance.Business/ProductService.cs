using AutoMapper;
using Insurance.Business.DTOs;
using Insurance.Business.Infrastructure;
using Insurance.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insurance.Business
{
    public class ProductService : IProductService
    {
        private readonly IProductGateway _productGateway;
        protected IMapper _mapper;

        public ProductService(IProductGateway productGateway, IMapper mapper)
        {
            _productGateway = productGateway;
            _mapper = mapper;
        }

        public async Task<InsuranceDto> CalculateInsurance(int productId)
        {
            var productDetails = await GetProduct(productId);
            var productType = await GetProductType(productDetails.ProductTypeId);
            decimal surchargeRate = await GetSurchargeRate(productType.Id);

            return new InsuranceDto
            {
                SalesPrice = productDetails.SalesPrice,
                ProductId = productId,
                ProductTypeId = productDetails.ProductTypeId,
                ProductTypeName = productType.Name,
                ProductTypeHasInsurance = productType.CanBeInsured,
                InsuranceValue = CalculateInsuranceValue(productDetails, productType, surchargeRate)
            };
        }

        public async Task<InsuranceOrderDto> CalculateInsurance(OrderDto ordertoInsure)
        {
            var toInsure = await Task.WhenAll(ordertoInsure.Products.Select(async product =>
            {
                var productDetails = await GetProduct(product.Id);
                var productType = await GetProductType(productDetails.ProductTypeId);
                decimal surchargeRate = await GetSurchargeRate(productType.Id);

                return new InsuranceDto
                {
                    SalesPrice = productDetails.SalesPrice,
                    ProductId = productDetails.Id,
                    ProductTypeId = productDetails.ProductTypeId,
                    ProductTypeName = productType.Name,
                    ProductTypeHasInsurance = productType.CanBeInsured,
                    InsuranceValue = CalculateInsuranceValue(productDetails, productType, surchargeRate)
                };
            }));

            return new InsuranceOrderDto(toInsure, CalculateTotalInsuranceValue(toInsure));
        }

        public async Task<ProductTypeDto> UpdateSurcharge(ProductTypeDto productTypeDto)
        {
            var productType = await GetProductType(productTypeDto.Id);
            await UpdateProductTypeSurcharge(productType.Id, productTypeDto.SurchargeRate);

            var surchageRate = await _productGateway.GetSurchargeRate(productType.Id);

            return _mapper.Map<ProductType, ProductTypeDto>(surchageRate);
        }

        private async Task<Product> GetProduct(int productId)
        {
            var productDetails = await _productGateway.GetProductById(productId);

            return productDetails ?? throw new ProductNotFoundException(productId);
        }            

        private async Task<ProductType> GetProductType(int productTypeId)
        {
            var productTypeDetails = await _productGateway.GetProductTypeById(productTypeId);

            return productTypeDetails ?? throw new ProductTypeNotFoundException(productTypeId);
        }

        private async Task<decimal> GetSurchargeRate(int productTypeId)
        {
            var productDetails = await _productGateway.GetSurchargeRate(productTypeId);

            return productDetails.SurchargeRate;
        }

        private async Task UpdateProductTypeSurcharge(int productTypeId, decimal surchargeRate)
        {
            await _productGateway.UpdateProductTypeSurcharge(productTypeId, surchargeRate);
        }

        private decimal CalculateInsuranceValue(Product productDetails, ProductType productType, decimal surchargeRate)
        {
            decimal insuranceValue = 0;

            if (productType.CanBeInsured)
            {
                if (productDetails.SalesPrice >= 2000)
                    insuranceValue += 2000;
                else if (productDetails.SalesPrice >= 500 && productDetails.SalesPrice < 2000)
                    insuranceValue += 1000;

                if (IsLaptopOrSmartphone(productType.Id))
                    insuranceValue += 500;
            }

            insuranceValue += ApplySurchargeRate(productDetails.SalesPrice, surchargeRate);

            return insuranceValue;
        }

        private decimal ApplySurchargeRate(double salesPrice, decimal percentage) =>
            decimal.Round(decimal.Multiply(percentage / 100M, (decimal)salesPrice), 2, MidpointRounding.ToZero);

        private decimal CalculateTotalInsuranceValue(IEnumerable<InsuranceDto> toInsure) =>
            toInsure.Sum(x => x.InsuranceValue) + (toInsure.Any(p => HasDigitalCamera(p.ProductTypeId)) ? 500M : 0M);

        private bool IsLaptopOrSmartphone(int productTypeId) =>
            productTypeId == (int)Enums.ProductNameTypes.Laptops || productTypeId == (int)Enums.ProductNameTypes.Smartphones;

        private bool HasDigitalCamera(int productTypeId) =>
            productTypeId == (int)Enums.ProductNameTypes.DigitalCameras;
    }
}