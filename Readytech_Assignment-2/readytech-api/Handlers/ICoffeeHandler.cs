// IRequestHandler.cs
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHandlerSpace
{
    public interface ICoffeeHandler
    {
        Task<IActionResult> HandleRequestAsync();
        ICoffeeHandler SetNextHandler(ICoffeeHandler nextHandler);
    }
}