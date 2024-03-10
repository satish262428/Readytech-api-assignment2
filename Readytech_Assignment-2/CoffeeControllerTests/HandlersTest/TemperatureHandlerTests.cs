using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoffeeHandlerSpace;
using TemperatureHandlerSpace;
using Moq;
using NUnit.Framework;
using Newtonsoft.Json;

namespace YourNamespace // Replace with your actual namespace
{
    [TestFixture]
    public class TemperatureHandlerTests
    {
        [Test]
        public async Task HandleRequestAsync_ReturnsIcedCoffee_WhenTemperatureIsGreaterThan30()
        {
            // Arrange
            var weatherServiceMock = new Mock<IWeatherService>();
            weatherServiceMock.Setup(service => service.GetWeatherData())
                .ReturnsAsync(new WeatherResponse { Main = new MainInfo { Temp = 35 } }); // Assuming WeatherResponse contains MainInfo

            var temperatureHandler = new TemperatureHandler(weatherServiceMock.Object);

            // Act   
            var result = await temperatureHandler.HandleRequestAsync();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;
            dynamic response = okObjectResult.Value;

            StringAssert.Contains("Your refreshing iced coffee is ready", response.ToString());

            // Reset mocks if needed
            weatherServiceMock.Reset();
        }

        [Test]
        public async Task HandleRequestAsync_ReturnsHotCoffee_WhenTemperatureIs30OrLess()
        {
            // Arrange
            var weatherServiceMock = new Mock<IWeatherService>();
            weatherServiceMock.Setup(service => service.GetWeatherData())
                .ReturnsAsync(new WeatherResponse { Main = new MainInfo { Temp = 30 } }); // Assuming WeatherResponse contains MainInfo

            var temperatureHandler = new TemperatureHandler(weatherServiceMock.Object);

            // Act
            var result = await temperatureHandler.HandleRequestAsync();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;
            dynamic response = okObjectResult.Value;
            StringAssert.Contains("Your piping hot coffee is ready", response.ToString());

            // Reset mocks if needed
            weatherServiceMock.Reset();
        }

        // Add more test cases for different scenarios as needed
    }
}


