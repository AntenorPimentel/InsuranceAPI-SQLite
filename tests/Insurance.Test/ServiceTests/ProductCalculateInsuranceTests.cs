using AutoMapper;
using Insurance.Business;
using Insurance.Business.Infrastructure;
using Insurance.Business.Models;
using Insurance.Business.Profiles;
using Insurance.Tests.Extensions;
using Moq;
using Xunit;

namespace Insurance.Tests.ServiceTests
{
    public class ProductCalculateInsuranceTests
    {
        protected readonly IMapper _mapper;
        private readonly Mock<IProductGateway> _mockProductGateway;
        private readonly ProductService _sut;

        public ProductCalculateInsuranceTests()
        {
            _mockProductGateway = new Mock<IProductGateway>();

            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProductServiceProfile());
            }).CreateMapper();

            _sut = new ProductService(_mockProductGateway.Object, _mapper);
        }

        [Fact]
        public async void CalculateInsurance_Given_SalesPriceBetween500And2000Euro_Should_Add1000EuroToInsuranceCost()
        {
            _mockProductGateway.Setup(g => g.GetProductById(It.IsAny<int>()))
                .ReturnsAsync(new Product().Build().WithSalesPrice(600));
            _mockProductGateway.Setup(g => g.GetProductTypeById(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());
            _mockProductGateway.Setup(g => g.GetSurchargeRate(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());

            var result = await _sut.CalculateInsurance(productId: 1);

            Assert.Equal(1000, result.InsuranceValue);
        }

        [Fact]
        public async void CalculateInsurance_Given_SalesPrice500Euro_Should_Add1000EuroToInsuranceCost()
        {
            _mockProductGateway.Setup(g => g.GetProductById(It.IsAny<int>()))
                .ReturnsAsync(new Product().Build().WithSalesPrice(500));
            _mockProductGateway.Setup(g => g.GetProductTypeById(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());
            _mockProductGateway.Setup(g => g.GetSurchargeRate(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());

            var result = await _sut.CalculateInsurance(productId: 1);

            Assert.Equal(1000, result.InsuranceValue);
        }

        [Fact]
        public async void CalculateInsurance_Given_SalesPrice2000Euro_Should_Add2000EuroToInsuranceCost()
        {
            _mockProductGateway.Setup(g => g.GetProductById(It.IsAny<int>()))
                .ReturnsAsync(new Product().Build().WithSalesPrice(2000));
            _mockProductGateway.Setup(g => g.GetProductTypeById(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());
            _mockProductGateway.Setup(g => g.GetSurchargeRate(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());

            var result = await _sut.CalculateInsurance(productId: 1);

            Assert.Equal(2000, result.InsuranceValue);
        }

        [Fact]
        public async void CalculateInsurance_Given_SalesPriceLessThan500Euro_Should_AddZeroEuroToInsuranceCost()
        {
            _mockProductGateway.Setup(g => g.GetProductById(It.IsAny<int>()))
                .ReturnsAsync(new Product().Build().WithSalesPrice(499));
            _mockProductGateway.Setup(g => g.GetProductTypeById(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());
            _mockProductGateway.Setup(g => g.GetSurchargeRate(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());

            var result = await _sut.CalculateInsurance(productId: 1);

            Assert.Equal(0, result.InsuranceValue);
        }

        [Fact]
        public async void CalculateInsurance_Given_ProductTypeCanNotBeInsured_Then_NeverAddInsuranceCost()
        {
            _mockProductGateway.Setup(g => g.GetProductById(It.IsAny<int>()))
                .ReturnsAsync(new Product().Build().WithSalesPrice(500));
            _mockProductGateway.Setup(g => g.GetProductTypeById(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build().WithCanBeInsured(false));
            _mockProductGateway.Setup(g => g.GetSurchargeRate(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());

            var result = await _sut.CalculateInsurance(productId: 1);

            Assert.Equal(0, result.InsuranceValue);
        }

        [Fact]
        public async void CalculateInsurance_Given_ProductTypeIsLaptop_Then_Add500EuroToFinalInsuranceCost()
        {
            _mockProductGateway.Setup(g => g.GetProductById(It.IsAny<int>()))
                .ReturnsAsync(new Product().Build().WithSalesPrice(600));
            _mockProductGateway.Setup(g => g.GetProductTypeById(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build().WithProductTypeId((int)Enums.ProductNameTypes.Laptops));
            _mockProductGateway.Setup(g => g.GetSurchargeRate(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());

            var result = await _sut.CalculateInsurance(productId: 1);

            Assert.Equal(1500, result.InsuranceValue);
        }

        [Fact]
        public async void CalculateInsurance_Given_ProductTypeIsSmartphone_Then_Add500EuroToFinalInsuranceCost()
        {
            _mockProductGateway.Setup(g => g.GetProductById(It.IsAny<int>()))
                .ReturnsAsync(new Product().Build().WithSalesPrice(600));
            _mockProductGateway.Setup(g => g.GetProductTypeById(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build().WithProductTypeId((int)Enums.ProductNameTypes.Smartphones));
            _mockProductGateway.Setup(g => g.GetSurchargeRate(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());

            var result = await _sut.CalculateInsurance(productId: 1);

            Assert.Equal(1500, result.InsuranceValue);
        }

        [Fact]
        public async void CalculateInsurance_Given_ProductCannotBeInsuredAndProductTypeIsSmartphone_Then_AddZeroEuroToInsuranceCost()
        {
            _mockProductGateway.Setup(g => g.GetProductById(It.IsAny<int>()))
                .ReturnsAsync(new Product().Build().WithSalesPrice(600));
            _mockProductGateway.Setup(g => g.GetProductTypeById(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build()
                .WithCanBeInsured(false).WithProductTypeId((int)Enums.ProductNameTypes.Smartphones));
            _mockProductGateway.Setup(g => g.GetSurchargeRate(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());

            var result = await _sut.CalculateInsurance(productId: 1);

            Assert.Equal(0, result.InsuranceValue);
        }

        [Fact]
        public async void CalculateInsurance_Given_SalesPrice600EuroAndSurchargeRate10percent_Then_Add60EuroToFinalInsuranceCost()
        {
            _mockProductGateway.Setup(g => g.GetProductById(It.IsAny<int>()))
                .ReturnsAsync(new Product().Build().WithSalesPrice(600));
            _mockProductGateway.Setup(g => g.GetProductTypeById(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build().WithCanBeInsured(true));
            _mockProductGateway.Setup(g => g.GetSurchargeRate(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build().WithSurchargeRate(10));

            var result = await _sut.CalculateInsurance(productId: 1);

            Assert.Equal(1060, result.InsuranceValue);
        }

        [Fact]
        public async void CalculateInsurance_Try_GetProductDetailsAndGetProductTypeDetails_Then_Add60EuroToFinalInsuranceCost()
        {
            _mockProductGateway.Setup(g => g.GetProductById(It.IsAny<int>()))
                .ReturnsAsync(new Product().Build());
            _mockProductGateway.Setup(g => g.GetProductTypeById(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());
            _mockProductGateway.Setup(g => g.GetSurchargeRate(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());

            await _sut.CalculateInsurance(productId: 1);

            _mockProductGateway.Verify(s => s.GetProductById(It.IsAny<int>()), Times.Once());
            _mockProductGateway.Verify(s => s.GetProductTypeById(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public async void CalculateInsurance_When_GetProductIdReturnsNull_Then_ThrowProductNotFoundException()
        {
            _mockProductGateway.Setup(g => g.GetProductById(It.IsAny<int>()))
                .ReturnsAsync(new Product().Build().WithProductNull());

            var ex = await Assert.ThrowsAsync<ProductNotFoundException>(() => _sut.CalculateInsurance(productId: 1));

            _mockProductGateway.Verify(s => s.GetProductById(It.IsAny<int>()), Times.Once());
            _mockProductGateway.Verify(s => s.GetProductTypeById(It.IsAny<int>()), Times.Never());
        }

        [Fact]
        public async void CalculateInsurance_When_GetProductTypeReturnsNull_Then_ThrowProductNotFoundException()
        {
            _mockProductGateway.Setup(g => g.GetProductById(It.IsAny<int>()))
                .ReturnsAsync(new Product().Build());
            _mockProductGateway.Setup(g => g.GetProductTypeById(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build().WithProductTypeNull());

            var ex = await Assert.ThrowsAsync<ProductTypeNotFoundException>(() => _sut.CalculateInsurance(productId: 1));

            _mockProductGateway.Verify(s => s.GetProductById(It.IsAny<int>()), Times.Once());
            _mockProductGateway.Verify(s => s.GetProductTypeById(It.IsAny<int>()), Times.Once());
        }
    }
}