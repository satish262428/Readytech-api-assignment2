using Microsoft.AspNetCore.Mvc;
using CoffeeHandlerSpace;

namespace TemperatureHandlerSpace
{
    public class TemperatureHandler : ICoffeeHandler
    {
        private ICoffeeHandler? _nextHandler;

        private IWeatherService _weatherService;

        public TemperatureHandler(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public ICoffeeHandler SetNextHandler(ICoffeeHandler nextHandler)
        {
            _nextHandler = nextHandler;
            return _nextHandler;
        }

        public async Task<IActionResult> HandleRequestAsync()
        {
            var weatherResponse = await _weatherService.GetWeatherData();
            if (weatherResponse != null && weatherResponse.Main?.Temp > 30)
            {

                // If temperature is greater than 30°C, serve iced coffee
                var response = new
                {
                    message = "Your refreshing iced coffee is ready",
                    prepared = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:sszzz")
                };

                return new OkObjectResult(response);
            }
            else
            {
                // If temperature is 30°C or less, serve hot coffee
                var response = new
                {
                    message = "Your piping hot coffee is ready",
                    prepared = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:sszzz")
                };

                return new OkObjectResult(response);
            }
        }
    }
}
