using Bsp03.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Servis kaydı (AddSingleton, AddScoped veya AddTransient kullanılabilir)
builder.Services.AddSingleton<PersonService>();

builder.Services.AddControllers();

// Swagger/OpenAPI-Dienste hinzufügen
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Swagger JSON-Endpunkt
    app.UseSwaggerUI(); // Swagger-Oberfläche
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();