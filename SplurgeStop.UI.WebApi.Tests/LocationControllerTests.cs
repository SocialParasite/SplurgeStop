using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuidHelpers;
using Moq;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.Shared;
using SplurgeStop.Domain.StoreProfile.LocationProfile;
using SplurgeStop.UI.WebApi.Controllers;
using Xunit;

namespace SplurgeStop.UI.WebApi.Tests
{
    [Trait("Unit tests", "WebApi/LocationController")]
    public sealed class LocationControllerTests
    {
        private static List<LocationDto> MockLocations()
        {
            var mockLocations = new List<LocationDto>();

            for (int i = 1; i <= 10; i++)
            {
                mockLocations.Add(new LocationDto
                {
                    Id = SequentialGuid.NewSequentialGuid(),
                    CityName = $"City-{i}",
                    CountryName = $"Country-{i}",
                });
            }

            return mockLocations;
        }

        [Fact]
        public async Task Get_All_Locations()
        {
            List<LocationDto> mockLocations = MockLocations();

            var mockRepository = new Mock<ILocationRepository>();
            mockRepository.Setup(repo => repo.GetAllDtoAsync())
                .Returns(() => Task.FromResult(mockLocations.AsEnumerable()));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var locationService = new LocationService(mockRepository.Object, mockUnitOfWork.Object);

            var locationController = new LocationController(locationService);
            var result = await locationController.GetLocations();

            Assert.Equal(10, result.Count());
            mockRepository.Verify(mock => mock.GetAllDtoAsync(), Times.Once());
        }


        [Fact]
        public async Task Valid_Id_Returns_Location()
        {
            var mockLocation = new Mock<Location>();
            var id = Guid.NewGuid();

            var mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(s => s.GetLocationAsync(id))
                .Returns(() => Task.FromResult(mockLocation.Object));

            var locationController = new LocationController(mockLocationService.Object);
            var result = await locationController.GetLocation(id);

            mockLocationService.Verify(mock => mock.GetLocationAsync(id), Times.Once());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Invalid_Id_Returns_Null()
        {
            var mockLocation = new Mock<Location>();
            var id = Guid.NewGuid();

            var mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(s => s.GetLocationAsync(Guid.NewGuid()))
                .Returns(() => Task.FromResult(mockLocation.Object));

            var locationController = new LocationController(mockLocationService.Object);

            var result = await locationController.GetLocation(id);
            Assert.Null(result);
        }
    }
}
