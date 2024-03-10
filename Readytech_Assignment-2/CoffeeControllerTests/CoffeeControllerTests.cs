using System.Threading.Tasks;
using CoffeeHandlerSpace;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;

[TestFixture]
public class CoffeeControllerTests
{
    [Test]
    public async Task BrewCoffee_ReturnsOkResult_WhenHandledSuccessfully()
    {
        // Arrange
        var mockCoffeeHandler = new Mock<ICoffeeHandler>();
        mockCoffeeHandler.Setup(handler => handler.HandleRequestAsync())
            .ReturnsAsync(new OkResult());

        var coffeeController = new CoffeeController(mockCoffeeHandler.Object);

        // Act
        var result = await coffeeController.BrewCoffee();

        // Assert
        Assert.IsInstanceOf<OkResult>(result);
    }

    // Add more test cases for different scenarios as needed

    [Test]
    public async Task BrewCoffee_ReturnsNotFoundResult_WhenHandledUnsuccessfully()
    {
        // Arrange
        var mockCoffeeHandler = new Mock<ICoffeeHandler>();
        mockCoffeeHandler.Setup(handler => handler.HandleRequestAsync())
            .ReturnsAsync(new NotFoundResult());

        var coffeeController = new CoffeeController(mockCoffeeHandler.Object);

        // Act
        var result = await coffeeController.BrewCoffee();

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }
}
