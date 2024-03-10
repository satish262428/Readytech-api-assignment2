using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoffeeHandlerSpace;
using Moq;
using NUnit.Framework;

namespace CoffeeHandlerTests.Tests
{
    [TestFixture]
    public class CoffeeHandlerTests
    {
        [Test]
        public async Task HandleRequestAsync_ReturnsActionResult()
        {
            // Arrange
            var coffeeHandlerMock = new Mock<ICoffeeHandler>();
            var expectedResult = new OkObjectResult("Mocked result");

            coffeeHandlerMock.Setup(handler => handler.HandleRequestAsync()).ReturnsAsync(expectedResult);

            var handlerUnderTest = coffeeHandlerMock.Object;

            // Act
            var result = await handlerUnderTest.HandleRequestAsync();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void SetNextHandler_ReturnsSameInstance()
        {
            // Arrange
            var coffeeHandlerMock = new Mock<ICoffeeHandler>();
            var nextHandlerMock = new Mock<ICoffeeHandler>();

            coffeeHandlerMock.Setup(handler => handler.SetNextHandler(nextHandlerMock.Object)).Returns(nextHandlerMock.Object);

            var handlerUnderTest = coffeeHandlerMock.Object;

            // Act
            var result = handlerUnderTest.SetNextHandler(nextHandlerMock.Object);

            // Assert
            Assert.IsInstanceOf<ICoffeeHandler>(result);
            Assert.AreEqual(nextHandlerMock.Object, result);
        }
    }
}
