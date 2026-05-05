using bedandbreakfast1.Data;
using bedandbreakfast1.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the service
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=bedandbreakfast.db"));

builder.Services.AddScoped<BookingService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();