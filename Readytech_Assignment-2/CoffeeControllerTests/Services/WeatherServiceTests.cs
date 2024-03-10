// using System.Net.Http;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Moq;
// using NUnit.Framework;

// [TestFixture]
// public class WeatherServiceTests
// {
//     [Test]
//     public async Task GetWeatherData_ReturnsWeatherResponse()
//     {
//         // Arrange
//         var httpClientFactoryMock = new Mock<IHttpClientFactory>();
//         var httpClientMock = new Mock<HttpClient>();
//         var expectedWeatherResponse = new WeatherResponse { Main = new MainInfo { Temp = 25.0f } };

//         httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClientMock.Object);
//         httpClientMock.Setup(client => client.GetFromJsonAsync<WeatherResponse>(It.IsAny<string>())).ReturnsAsync(expectedWeatherResponse);

//         var weatherService = new WeatherService(httpClientFactoryMock.Object);

//         // Act
//         var result = await weatherService.GetWeatherData();

//         // Assert
//         Assert.IsNotNull(result);
//         Assert.AreEqual(expectedWeatherResponse.Main.Temp, result.Main.Temp);

//         // Optionally, you can verify that CreateClient and GetFromJsonAsync were called as expected
//         httpClientFactoryMock.Verify(factory => factory.CreateClient(It.IsAny<string>()), Times.Once);
//         httpClientMock.Verify(client => client.GetFromJsonAsync<WeatherResponse>(It.IsAny<string>()), Times.Once);
//     }

//     [Test]
//     public void Constructor_ThrowsException_WhenHttpClientFactoryIsNull()
//     {
//         // Arrange
//         IHttpClientFactory httpClientFactory = null;

//         // Act & Assert
//         Assert.Throws<ArgumentNullException>(() => new WeatherService(httpClientFactory));
//     }

//     [Test]
//     public void GetWeatherData_ThrowsException_WhenWeatherResponseIsNull()
//     {
//         // Arrange
//         var httpClientFactoryMock = new Mock<IHttpClientFactory>();
//         var httpClientMock = new Mock<HttpClient>();

//         httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClientMock.Object);
//         httpClientMock.Setup(client => client.GetFromJsonAsync<WeatherResponse>(It.IsAny<string>())).ReturnsAsync((WeatherResponse)null);

//         var weatherService = new WeatherService(httpClientFactoryMock.Object);

//         // Act & Assert
//         Assert.ThrowsAsync<WeatherServiceException>(() => weatherService.GetWeatherData());
//     }
// }
