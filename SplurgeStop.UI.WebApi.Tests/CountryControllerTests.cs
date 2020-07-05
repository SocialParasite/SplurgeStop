using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuidHelpers;
using Moq;
using SplurgeStop.Domain.CountryProfile.DTO;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.Shared;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile;
using SplurgeStop.UI.WebApi.Controllers;
using Xunit;

namespace SplurgeStop.UI.WebApi.Tests
{
    [Trait("Unit tests", "WebApi/CountryController")]
    public sealed class CountryControllerTests
    {
        private static List<CountryDto> MockCountries()
        {
            var mockCountries = new List<CountryDto>();

            for (int i = 1; i <= 10; i++)
            {
                mockCountries.Add(new CountryDto
                {
                    Id = SequentialGuid.NewSequentialGuid(),
                    Name = $"Country-{i}",
                });
            }

            return mockCountries;
        }

        [Fact]
        public async Task Get_All_Countries()
        {
            List<CountryDto> mockCountries = MockCountries();

            var mockRepository = new Mock<ICountryRepository>();
            mockRepository.Setup(repo => repo.GetAllCountryDtoAsync())
                .Returns(() => Task.FromResult(mockCountries.AsEnumerable()));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var countryService = new CountryService(mockRepository.Object, mockUnitOfWork.Object);

            var countryController = new CountryController(countryService);
            var result = await countryController.GetCountries();

            Assert.Equal(10, result.Count());
            mockRepository.Verify(mock => mock.GetAllCountryDtoAsync(), Times.Once());
        }


        [Fact]
        public async Task Valid_Id_Returns_Country()
        {
            var mockCountry = new Mock<Country>();
            var id = Guid.NewGuid();

            var mockCountryService = new Mock<ICountryService>();
            mockCountryService.Setup(s => s.GetCountryAsync(id))
                .Returns(() => Task.FromResult(mockCountry.Object));

            var countryController = new CountryController(mockCountryService.Object);
            var result = await countryController.GetCountry(id);

            mockCountryService.Verify(mock => mock.GetCountryAsync(id), Times.Once());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Invalid_Id_Returns_Null()
        {
            var mockCountry = new Mock<Country>();
            var id = Guid.NewGuid();

            var mockCountryService = new Mock<ICountryService>();
            mockCountryService.Setup(s => s.GetCountryAsync(Guid.NewGuid()))
                .Returns(() => Task.FromResult(mockCountry.Object));

            var countryController = new CountryController(mockCountryService.Object);

            var result = await countryController.GetCountry(id);
            Assert.Null(result);
        }
    }
}
