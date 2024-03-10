using System.Threading.Tasks;
using CoffeeHandlerSpace;
using FifthCallHandlerSpace;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

[TestFixture]
public class FifthCallHandlerTests
{
    [Test]
    public async Task HandleRequestAsync_Returns503ServiceUnavailable_WhenCallCountIsFifth()
    {
        // Arrange
        var mockCallCountService = new Mock<ICallCountService>();
        mockCallCountService.Setup(service => service.ShouldReturn503())
            .Returns(true);

        var fifthCallHandler = new FifthCallHandler(mockCallCountService.Object);

        // Act
        var result = await fifthCallHandler.HandleRequestAsync();

        // Assert
        Assert.IsInstanceOf<StatusCodeResult>(result);
        var statusCodeResult = (StatusCodeResult)result;
        Assert.AreEqual(503, statusCodeResult.StatusCode);
    }

    [Test]
    public async Task HandleRequestAsync_ReturnsNotFoundResult_WhenCallCountIsNotFifth()
    {
        // Arrange
        var mockCallCountService = new Mock<ICallCountService>();
        mockCallCountService.Setup(service => service.ShouldReturn503())
            .Returns(false);

        var mockNextHandler = new Mock<ICoffeeHandler>();
        mockNextHandler.Setup(handler => handler.HandleRequestAsync())
            .ReturnsAsync(new NotFoundResult());

        var fifthCallHandler = new FifthCallHandler(mockCallCountService.Object);
        fifthCallHandler.SetNextHandler(mockNextHandler.Object);

        // Act
        var result = await fifthCallHandler.HandleRequestAsync();

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task HandleRequestAsync_ReturnsNotFoundResult_WhenNextHandlerIsNull()
    {
        // Arrange
        var mockCallCountService = new Mock<ICallCountService>();
        mockCallCountService.Setup(service => service.ShouldReturn503())
            .Returns(false);

        var fifthCallHandler = new FifthCallHandler(mockCallCountService.Object);

        // Act
        var result = await fifthCallHandler.HandleRequestAsync();

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    // Add more test cases for different scenarios as needed
}
