using Microsoft.AspNetCore.Mvc;
using CoffeeHandlerSpace;

namespace FifthCallHandlerSpace
{
    public class FifthCallHandler : ICoffeeHandler
    {
        private ICoffeeHandler? _nextHandler;
        private readonly ICallCountService _callCountService;

        public FifthCallHandler(ICallCountService callCountService)
        {
            _callCountService = callCountService;
        }

        public ICoffeeHandler SetNextHandler(ICoffeeHandler nextHandler)
        {
            _nextHandler = nextHandler;
            return _nextHandler;
        }

        public async Task<IActionResult> HandleRequestAsync()
        {
            // Check if it's the fifth call
            if (_callCountService.ShouldReturn503())
            {
                // Return a 503 Service Unavailable response with an empty body
                return new StatusCodeResult(503);
            }

            // Pass the request to the next handler
            return _nextHandler != null ? await _nextHandler.HandleRequestAsync() ?? new NotFoundResult() : new NotFoundResult();
        }
    }
}
