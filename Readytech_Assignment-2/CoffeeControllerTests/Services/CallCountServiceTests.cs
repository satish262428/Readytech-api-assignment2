using Moq;
using NUnit.Framework;
using StackExchange.Redis;

namespace CallCountServiceTests.Tests
{
    [TestFixture]
    public class CallCountServiceTests
    {
        [Test]
        public void ShouldReturn503_ReturnsTrueOnFifthCall()
        {
            // Arrange
            var redisConnectionMock = new Mock<IConnectionMultiplexer>();
            var redisDatabaseMock = new Mock<IDatabase>();
            redisConnectionMock.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(redisDatabaseMock.Object);

            var callCountService = new CallCountService(redisConnectionMock.Object);

            // Act
            for (int i = 1; i <= 5; i++)
            {
                var result = callCountService.ShouldReturn503();

                // Assert
                if (i == 5)
                {
                    Assert.True(result);
                }
                else
                {
                    Assert.False(result);
                }
            }
        }

        [Test]
        public void GetBrewCoffeeCallCount_ReturnsCorrectCountFromRedis()
        {
            // Arrange
            var expectedCount = 42; // Set the expected count for the test
            var redisConnectionMock = new Mock<IConnectionMultiplexer>();
            var redisDatabaseMock = new Mock<IDatabase>();
            redisDatabaseMock.Setup(x => x.StringGet(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).Returns(expectedCount.ToString());
            redisConnectionMock.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(redisDatabaseMock.Object);

            var callCountService = new CallCountService(redisConnectionMock.Object);

            // Act
            var count = callCountService.GetBrewCoffeeCallCount();

            // Assert
            Assert.AreEqual(expectedCount, count);
        }
    }
}
