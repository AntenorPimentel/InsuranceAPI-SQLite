using AutoMapper;
using Bogus;
using Insurance.Business;
using Insurance.Business.DTOs;
using Insurance.Business.Models;
using Insurance.Business.Profiles;
using Insurance.Tests.Extensions;
using Moq;
using System.Linq;
using Xunit;

namespace Insurance.Tests.ServiceTests
{
    public class OrderCalculateInsuranceTests
    {
        protected readonly IMapper _mapper;
        private readonly Mock<IProductGateway> _mockProductGateway;
        private readonly ProductService _sut;

        public OrderCalculateInsuranceTests()
        {
            _mockProductGateway = new Mock<IProductGateway>();

            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProductServiceProfile());
            }).CreateMapper();

            _sut = new ProductService(_mockProductGateway.Object, _mapper);
        }

        [Fact]
        public async void CalculateInsurance_Given_OrderWith10Products_Should_EvaluateInsuranceCostForAll()
        {
            var order = new Faker<OrderDto>().RuleFor(p => p.Products, new Faker<ProductDto>().Generate(10));
            _mockProductGateway.Setup(g => g.GetProductById(It.IsAny<int>()))
                .ReturnsAsync(new Product().Build());
            _mockProductGateway.Setup(g => g.GetProductTypeById(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());
            _mockProductGateway.Setup(g => g.GetSurchargeRate(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());

            var result = await _sut.CalculateInsurance(order);

            Assert.Equal(10, result.InsuranceDtos.Count());
            _mockProductGateway.Verify(s => s.GetProductById(It.IsAny<int>()), Times.Exactly(10));
            _mockProductGateway.Verify(s => s.GetProductTypeById(It.IsAny<int>()), Times.Exactly(10));
        }

        [Fact]
        public async void CalculateInsurance_Given_OrderWith1Product_Should_EvaluateInsuranceCostForOne()
        {
            var order = new Faker<OrderDto>().RuleFor(o => o.Products, new Faker<ProductDto>().Generate(1));
            _mockProductGateway.Setup(g => g.GetProductById(It.IsAny<int>()))
                .ReturnsAsync(new Product().Build());
            _mockProductGateway.Setup(g => g.GetProductTypeById(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());
            _mockProductGateway.Setup(g => g.GetSurchargeRate(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());

            var result = await _sut.CalculateInsurance(order);

            Assert.Single(result.InsuranceDtos);
            _mockProductGateway.Verify(s => s.GetProductById(It.IsAny<int>()), Times.Exactly(1));
            _mockProductGateway.Verify(s => s.GetProductTypeById(It.IsAny<int>()), Times.Exactly(1));
        }

        [Fact]
        public async void CalculateInsurance_Given_OrderWith10ProductsWithSalesPrice500Euro_Should_AddToInsuranceTotalValueIs10000()
        {
            var order = new Faker<OrderDto>().RuleFor(p => p.Products, new Faker<ProductDto>().Generate(10));
            _mockProductGateway.Setup(g => g.GetProductById(It.IsAny<int>()))
                 .ReturnsAsync(new Product().Build().WithSalesPrice(500));
            _mockProductGateway.Setup(g => g.GetProductTypeById(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());
            _mockProductGateway.Setup(g => g.GetSurchargeRate(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());

            var result = await _sut.CalculateInsurance(order);

            Assert.Equal(10000, result.InsuranceTotalValue);
        }
    }
}
