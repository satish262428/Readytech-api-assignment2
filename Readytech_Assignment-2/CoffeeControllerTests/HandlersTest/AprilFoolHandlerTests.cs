using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoffeeHandlerSpace;
using AprilFoolHandlerSpace;
using Moq;
using NUnit.Framework;

[TestFixture]
public class AprilFoolHandlerTests
{
    [Test]
    public async Task HandleRequestAsync_Returns418ImATeapot_OnApril1st()
    {
        // Arrange
        var mockDateTimeProvider = new Mock<IDateTimeProvider>();
        mockDateTimeProvider.Setup(provider => provider.UtcNow)
            .Returns(new DateTime(DateTime.UtcNow.Year, 4, 1));

        var aprilFoolHandler = new AprilFoolHandler(mockDateTimeProvider.Object);

        // Act
        var result = await aprilFoolHandler.HandleRequestAsync();

        // Assert
        Assert.IsInstanceOf<StatusCodeResult>(result);
        var statusCodeResult = (StatusCodeResult)result;
        Assert.AreEqual(418, statusCodeResult.StatusCode);
    }

    [Test]
    public async Task HandleRequestAsync_ReturnsNotFoundResult_OnNonApril1st()
    {
        // Arrange
        var mockDateTimeProvider = new Mock<IDateTimeProvider>();
        mockDateTimeProvider.Setup(provider => provider.UtcNow)
            .Returns(new DateTime(DateTime.UtcNow.Year, 5, 1));

        var aprilFoolHandler = new AprilFoolHandler(mockDateTimeProvider.Object);

        // Act
        var result = await aprilFoolHandler.HandleRequestAsync();

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task HandleRequestAsync_ReturnsNotFoundResult_WhenNextHandlerIsNull()
    {
        // Arrange
        var mockDateTimeProvider = new Mock<IDateTimeProvider>();
        mockDateTimeProvider.Setup(provider => provider.UtcNow)
            .Returns(new DateTime(DateTime.UtcNow.Year, 5, 1));

        var aprilFoolHandler = new AprilFoolHandler(mockDateTimeProvider.Object);

        // Act
        var result = await aprilFoolHandler.HandleRequestAsync();

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    // Add more test cases for different scenarios as needed
}
