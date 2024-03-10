// CallCountService.cs
using StackExchange.Redis;
using System;

public class CallCountService : ICallCountService
{
    private int _callCount = 0;
    private readonly IDatabase _redisDatabase;
    private string _brewCoffeeCallCount = "brew-coffee-call-count";

    public CallCountService(IConnectionMultiplexer redisConnection)
    {
        _redisDatabase = redisConnection.GetDatabase();
    }

    public bool ShouldReturn503()
    {
        IncrementCallCount();

        // Check if it's the fifth call
        if (_callCount % 5 == 0)
        {
            ResetCallCount();
            return true;
        }

        return false;
    }

    private void IncrementCallCount()
    {
        _callCount++;
        // Set count in Redis
        _redisDatabase.StringSet(_brewCoffeeCallCount, _callCount.ToString(), TimeSpan.FromMinutes(30));
    }

    private void ResetCallCount()
    {
        _callCount = 0;
        // Cache the call count in Redis
        _redisDatabase.StringSet(_brewCoffeeCallCount, _callCount.ToString(), TimeSpan.FromMinutes(30));
    }

    public int GetBrewCoffeeCallCount()
    {
        RedisValue countValue = _redisDatabase.StringGet(_brewCoffeeCallCount);

        if (!countValue.IsNull && int.TryParse(countValue, out int count))
        {
            return count;
        }

        // Handle the case where the value in Redis is not a valid integer
        throw new InvalidOperationException("Value in Redis is not a valid integer.");
    }
}

public interface ICallCountService
{
    bool ShouldReturn503();
}
