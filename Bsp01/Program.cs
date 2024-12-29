// Program.cs

using Bsp01.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<PersonService>();

// Dienste zum Container hinzufügen
builder.Services.AddControllers();

// Swagger/OpenAPI-Dienste hinzufügen
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// HTTP-Anfrage-Pipeline konfigurieren
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger(); // Swagger JSON-Endpunkt
    app.UseSwaggerUI(); // Swagger-Oberfläche
}

app.UseHttpsRedirection(); // HTTPS Umleitung

app.UseAuthorization(); // Autorisierung aktivieren

app.MapControllers(); // Controller-Endpunkte zuordnen

app.Run(); // Anwendung starten