using Microsoft.AspNetCore.Mvc;
using CoffeeHandlerSpace;

namespace AprilFoolHandlerSpace
{
    public class AprilFoolHandler : ICoffeeHandler
    {
        private ICoffeeHandler? _nextHandler;

        private readonly IDateTimeProvider _dateTimeProvider;

        public ICoffeeHandler SetNextHandler(ICoffeeHandler nextHandler)
        {
            _nextHandler = nextHandler;
            return _nextHandler;
        }


        public AprilFoolHandler(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }
        public async Task<IActionResult> HandleRequestAsync()
        {
            if (_dateTimeProvider.UtcNow.Month == 4 && _dateTimeProvider.UtcNow.Day == 1)
            {
                // April 1st: Return 418 I'm a teapot
                return new StatusCodeResult(418);
            }

            // Pass the request to the next handler
            return _nextHandler != null ? await _nextHandler.HandleRequestAsync() ?? new NotFoundResult() : new NotFoundResult();
        }
    }
}


public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}

