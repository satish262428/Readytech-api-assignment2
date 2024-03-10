
using StackExchange.Redis;
using CoffeeHandlerSpace;
using AprilFoolHandlerSpace;
using TemperatureHandlerSpace;
using FifthCallHandlerSpace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = ConfigurationOptions.Parse("localhost:6379");
            return ConnectionMultiplexer.Connect(configuration);
        });

// Register Redis database
builder.Services.AddSingleton<IDatabase>(sp =>
{
    var connectionMultiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
    return connectionMultiplexer.GetDatabase();
});

// Register services
builder.Services.AddSingleton<ICallCountService, CallCountService>();
builder.Services.AddSingleton<IWeatherService, WeatherService>();
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

builder.Services.AddHttpClient();

builder.Services.AddSingleton<ICoffeeHandler>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var callCountService = provider.GetRequiredService<ICallCountService>();
    var weatherService = provider.GetRequiredService<IWeatherService>();
    var dateTimeProvider = provider.GetRequiredService<IDateTimeProvider>();

    // Define the handlers in the desired order
    var handlerTypes = new List<Type>
    {
        typeof(AprilFoolHandler),
        typeof(FifthCallHandler),
        typeof(TemperatureHandler)
    };

    // Build the chain of responsibility
    ICoffeeHandler? firstHandler = null;
    ICoffeeHandler? previousHandler = null;

    foreach (var handlerType in handlerTypes)
    {
        var handler = (ICoffeeHandler)ActivatorUtilities.CreateInstance(provider, handlerType);

        if (firstHandler == null)
        {
            // Set the first handler in the chain
            firstHandler = handler;
        }

        if (previousHandler != null)
        {
            // Set up the chain
            previousHandler.SetNextHandler(handler);
        }

        // Update the previous handler
        previousHandler = handler;
    }

    return firstHandler ?? throw new InvalidOperationException("No coffee handler is configured.");
});


builder.Services.AddCors(options =>
{

    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder
                .AllowAnyOrigin()  // Allow all origins
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseAuthorization();
app.MapControllers();


app.Run();
