
using System.Reflection;
using Bsp03.Controllers;
using Bsp03.Models;
using Bsp03.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bsp03.Tests;

public class PersonControllerTests
{

    private PersonService _personService;

    // Test sınıfı başlatıldığında bir kez çalışır
    public PersonControllerTests()
    {
        _personService = new PersonService(); // Servisi bir kez oluştur
    }

    // Her test için yeni bir controller döner, ancak aynı servis örneğini kullanır
    private PersonController CreateController()
    {
        return new PersonController(_personService); // Aynı servis örneğini kullan
    }
    
    
    
    [Fact]
    public async Task Get_ReturnsAllPersons()
    {
        // Arrange
        var controller = CreateController();
        
        // Act
        var result = await controller.Get();
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var persons = Assert.IsType<List<Person>>(okResult.Value);
        Assert.NotNull(persons);
        Assert.Equal(3, persons.Count); // Es sollten 3 Personen vorhanden sein
    }

    
    [Theory]
    [InlineData(1, "Max")]
    [InlineData(2, "Erika")]
    public async Task Get_ById_ReturnsCorrectPerson(int id, string expectedVorname)
    {
        
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Get(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var person = Assert.IsType<Person>(okResult.Value);
        Assert.NotNull(person);
        Assert.Equal(expectedVorname, person.Vorname);
    }

    [Fact]
    public async Task Post_AddsPersonToList()
    {
        // Arrange
        var controller = CreateController();

        // Arrange
        var newPerson = new Person { PersonId = 4, Vorname = "Anna", Nachname = "Muster" };

        // Act
        var result = await controller.Post(newPerson);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
        var getResult = await controller.Get(4);
        var okResult = Assert.IsType<OkObjectResult>(getResult.Result);
        var person = Assert.IsType<Person>(okResult.Value);
        Assert.NotNull(person);
        Assert.Equal("Anna", person.Vorname);
    }

    [Fact]
    public async Task Post_ReturnsBadRequest_WhenPersonIsNull()
    {
        // Arrange
        var controller = CreateController();
    
        // Act
        var result = await controller.Post(null);
    
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Person-Objekt darf nicht null sein.", badRequestResult.Value);
    }
    
    [Fact]
    public async Task Put_UpdatesPersonDetails()
    {
        // Arrange
        var controller = CreateController();

        // Arrange
        var updatedPerson = new Person { Vorname = "Maximilian", Nachname = "Mustermann" };

        // Act
        var result = await controller.Put(1, updatedPerson);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var getResult = await controller.Get(1);
        var okResult = Assert.IsType<OkObjectResult>(getResult.Result);
        var person = Assert.IsType<Person>(okResult.Value);
        Assert.NotNull(person);
        Assert.Equal("Maximilian", person.Vorname);
    }

    [Fact]
    public async Task Put_ReturnsBadRequest_WhenIdIsZero()
    {
        // Arrange
        var controller = CreateController();
        var person = new Person { Vorname = "Anna", Nachname = "Muster" };

        // Act
        var result = await controller.Put(0, person);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("ID muss größer als 0 sein.", badRequestResult.Value);
    }

    [Fact]
    public async Task Put_ReturnsBadRequest_WhenPersonIsNull()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Put(1, null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Person-Objekt darf nicht null sein.", badRequestResult.Value);
    }

    
    [Fact]
    public async Task Delete_RemovesPersonFromList()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var deleteResult = await controller.Delete(1);

        // Assert
        Assert.IsType<OkObjectResult>(deleteResult);
        var getResult = await controller.Get(1);
        Assert.IsType<NotFoundObjectResult>(getResult.Result);
    }
 
    [Fact]
    public async Task Delete_ReturnsNotFound_WhenIdIsZero()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.Delete(0);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Person mit ID 0 ist ungültig.", notFoundResult.Value);
    }

        
    [Theory]
    [InlineData("A", true)] // Gültig
    [InlineData("MaximilianMaximilianMaximilianMaximilian", true)] // Gültig (41 Zeichen)
    [InlineData("", false)] // Ungültig (leer)
    [InlineData("123", false)] // Ungültig (enthält Zahlen)
    [InlineData("Max@", false)] // Ungültig (enthält mathematische Symbole)
    [InlineData("Max-Max", true)] // Gültig (Bindestrich ist erlaubt)
    [InlineData("Max123", false)] // Ungültig (enthält Zahlen)
    public void ValidateVorname_ReturnsExpectedResult(string vorname, bool expected)
    {
        // Arrange
        var controller = CreateController();
        
        // Arrange & Act
        var result = controller.ValidateName(vorname);

        // Assert
        Assert.Equal(expected, result);
    }

    
}