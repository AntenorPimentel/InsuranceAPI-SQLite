using AutoMapper;
using Bogus;
using Insurance.Business;
using Insurance.Business.DTOs;
using Insurance.Business.Infrastructure;
using Insurance.Business.Models;
using Insurance.Business.Profiles;
using Insurance.Tests.Extensions;
using Moq;
using Xunit;

namespace Insurance.Tests.ServiceTests
{
    public class UpdateSurchargeTests
    {
        protected readonly IMapper _mapper;
        private readonly Mock<IProductGateway> _mockProductGateway;
        private readonly ProductService _sut;

        public UpdateSurchargeTests()
        {
            _mockProductGateway = new Mock<IProductGateway>();

            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProductServiceProfile());
            }).CreateMapper();

            _sut = new ProductService(_mockProductGateway.Object, _mapper);
        }

        [Fact]
        public async void UpdateSurcharge_Given_AnValidProductType_Try_ToUpdateProductTypeSurchargeRate()
        {
            var productTypeDto = new Faker<ProductTypeDto>();
            _mockProductGateway.Setup(g => g.GetProductTypeById(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());
            _mockProductGateway.Setup(g => g.GetSurchargeRate(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build().WithZeroSurchargeRate());

            var result = await _sut.UpdateSurcharge(productTypeDto);

            _mockProductGateway.Verify(s => s.UpdateProductTypeSurcharge(It.IsAny<int>(), It.IsAny<decimal>()), Times.Once());
        }

        [Fact]
        public async void UpdateSurcharge_Given_AnInvalidProductType_Try_ThrowsProductTypeNotFoundException()
        {
            var productTypeDto = new Faker<ProductTypeDto>();
            _mockProductGateway.Setup(g => g.GetProductTypeById(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build().WithProductTypeNull());
            _mockProductGateway.Setup(g => g.GetSurchargeRate(It.IsAny<int>()))
                .ReturnsAsync(new ProductType().Build());

            var ex = await Assert.ThrowsAsync<ProductTypeNotFoundException>(() => _sut.UpdateSurcharge(productTypeDto));

            _mockProductGateway.Verify(s => s.UpdateProductTypeSurcharge(It.IsAny<int>(), It.IsAny<decimal>()), Times.Never());
        }
    }
}
