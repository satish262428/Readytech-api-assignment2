using Microsoft.AspNetCore.Mvc;
using CoffeeHandlerSpace;

[ApiController]
[Route("[controller]")]
public class CoffeeController : ControllerBase
{
    private readonly ICoffeeHandler _coffeeHandler;

    public CoffeeController(ICoffeeHandler coffeeHandler)
    {
        _coffeeHandler = coffeeHandler;
    }

    [HttpGet("brew-coffee")]
    public async Task<IActionResult> BrewCoffee()
    {
        return await _coffeeHandler.HandleRequestAsync();
    }
}
