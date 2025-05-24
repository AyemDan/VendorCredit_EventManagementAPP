using EventBookingApp.Application.Interfaces;
using EventBookingApp.Infrastructure;
using EventBookingApp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Get connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("No connection string");
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

// Register AppDbContext with MySQL
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
// );

// Register services
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IBookingService, BookingService>();


var app = builder.Build();

// Optional: test database connection on startup
// try
// {
//     using (var scope = app.Services.CreateScope())
//     {
//         var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//         if (db.Database.CanConnect())
//         {
//             Console.WriteLine("✅ Connected to MySQL!");
//         }
//         else
//         {
//             Console.WriteLine("❌ Failed to connect.");
//         }
//     }
// }
// catch (Exception ex)
// {
//     Console.WriteLine($"❌ DB error: {ex.Message}");
// }

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // optional, but good for prod
app.UseAuthorization(); // optional, if you're adding auth later

app.MapControllers();

app.Run();
