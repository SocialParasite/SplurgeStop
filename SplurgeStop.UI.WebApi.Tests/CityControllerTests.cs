using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuidHelpers;
using Moq;
using SplurgeStop.Domain.CityProfile;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.Shared;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile;
using SplurgeStop.UI.WebApi.Controllers;
using Xunit;

namespace SplurgeStop.UI.WebApi.Tests
{
    [Trait("Unit tests", "WebApi/CityController")]
    public sealed class CityControllerTests
    {
        private static List<CityDto> MockCities()
        {
            var mockCities = new List<CityDto>();

            for (int i = 1; i <= 10; i++)
            {
                mockCities.Add(new CityDto
                {
                    Id = SequentialGuid.NewSequentialGuid(),
                    Name = $"City-{i}",
                });
            }

            return mockCities;
        }

        [Fact]
        public async Task Get_All_Cities()
        {
            List<CityDto> mockCities = MockCities();

            var mockRepository = new Mock<IRepository<City, CityDto, CityId>>();
            mockRepository.Setup(repo => repo.GetAllDtoAsync())
                .Returns(() => Task.FromResult(mockCities.AsEnumerable()));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var cityService = new CityService(mockRepository.Object, mockUnitOfWork.Object);

            var cityController = new CityController(cityService);
            var result = await cityController.GetCities();

            Assert.Equal(10, result.Count());
            mockRepository.Verify(mock => mock.GetAllDtoAsync(), Times.Once());
        }


        [Fact]
        public async Task Valid_Id_Returns_City()
        {
            var mockCity = new Mock<City>();
            var id = Guid.NewGuid();

            var mockCityService = new Mock<ICityService>();
            mockCityService.Setup(s => s.GetCityAsync(id))
                .Returns(() => Task.FromResult(mockCity.Object));

            var cityController = new CityController(mockCityService.Object);
            var result = await cityController.GetCity(id);

            mockCityService.Verify(mock => mock.GetCityAsync(id), Times.Once());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Invalid_Id_Returns_Null()
        {
            var mockCity = new Mock<City>();
            var id = Guid.NewGuid();

            var mockCityService = new Mock<ICityService>();
            mockCityService.Setup(s => s.GetCityAsync(Guid.NewGuid()))
                .Returns(() => Task.FromResult(mockCity.Object));

            var cityController = new CityController(mockCityService.Object);

            var result = await cityController.GetCity(id);
            Assert.Null(result);
        }
    }
}
