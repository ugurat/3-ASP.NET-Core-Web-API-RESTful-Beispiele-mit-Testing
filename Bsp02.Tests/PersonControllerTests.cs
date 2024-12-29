using Bsp02.Controllers;
using Bsp02.Models;
using Bsp02.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bsp02.Tests;

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

    
    /*
    private PersonService _personService;

    public PersonControllerTests()
    {
        _personService = new PersonService(); // Servisi bir kez oluştur
    }

    private PersonController CreateController()
    {
        return new PersonController(_personService); // Aynı servis örneğini kullan
    }
    */
    
    /*
    private PersonController CreateController()
    {
        var service = new PersonService(); // Service wird initialisiert
        return new PersonController(service); // Controller mit Service erstellen
    }
    */
    
    [Fact]
    public void Get_ReturnsAllPersons() // Prüft, ob alle Personen zurückgegeben werden
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Get() as ActionResult<List<Person>>;

        // Assert
        var okResult =
            Assert.IsType<OkObjectResult>(result.Result); // Stellt sicher, dass das Ergebnis vom Typ OkObjectResult ist
        var persons =
            Assert.IsType<List<Person>>(okResult.Value); // Stellt sicher, dass der Wert vom Typ List<Person> ist
        Assert.Equal(3, persons.Count); // Überprüft, ob die Liste drei Personen enthält
    }

    [Theory]
    [InlineData(1, "Max")]
    [InlineData(2, "Erika")]
    [InlineData(3, "Felix")]
    public void Get_ById_ReturnsCorrectPerson(int id, string expectedVorname)
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Get(id) as ActionResult<Person>;

        // Assert
        var okResult =
            Assert.IsType<OkObjectResult>(result.Result); // Stellt sicher, dass das Ergebnis vom Typ OkObjectResult ist
        var person = Assert.IsType<Person>(okResult.Value); // Stellt sicher, dass der Wert vom Typ Person ist
        Assert.Equal(expectedVorname, person.Vorname);
    }

    [Fact]
    public void Post_AddsNewPersonToList()
    {
        // Arrange
        var controller = CreateController();
        var newPerson = new Person { PersonId = 4, Vorname = "Anna", Nachname = "Musterfrau" };

        // Act
        var result = controller.Post(newPerson);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var createdPerson = (okResult.Value as Person);
        Assert.NotNull(createdPerson);
        Assert.Equal("Anna", createdPerson?.Vorname);
        
        // Assert
        var resultAfterAdd = controller.Get() as ActionResult<List<Person>>;
        var okResultAfterAdd = Assert.IsType<OkObjectResult>(resultAfterAdd.Result); // Stellt sicher, dass das Ergebnis vom Typ OkObjectResult ist
        var personsAfterAdd = Assert.IsType<List<Person>>(okResultAfterAdd.Value); // Stellt sicher, dass der Wert vom Typ List<Person> ist
        Assert.NotNull(personsAfterAdd);
        Assert.Equal(4, personsAfterAdd.Count); // Überprüft, ob die Liste 2  Personen enthält

    }

    [Fact]
    public async Task Post_ReturnsBadRequest_WhenPersonIsNull()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Post(null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Person-Objekt darf nicht null sein.", badRequestResult.Value);
    }

    
    [Fact]
    public void Put_UpdatesExistingPerson()
    {
        // Arrange
        var controller = CreateController();
        var updatedPerson = new Person { PersonId = 1, Vorname = "Maximilian", Nachname = "Mustermann" };

        // Act
        var result = controller.Put(1, updatedPerson);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var updatedResult = (okResult.Value as Person);
        Assert.NotNull(updatedResult);
        Assert.Equal("Maximilian", updatedResult?.Vorname);
        Assert.Equal("Mustermann", updatedResult?.Nachname);
    }

    [Fact]
    public void Put_ReturnsBadRequest_WhenIdIsZero()
    {
        // Arrange
        var controller = CreateController();
        var person = new Person { Vorname = "Anna", Nachname = "Muster" };

        // Act
        var result = controller.Put(0, person);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("ID muss größer als 0 sein.", badRequestResult.Value);
    }

    [Fact]
    public void Put_ReturnsBadRequest_WhenPersonIsNull()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Put(1, null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Person-Objekt darf nicht null sein.", badRequestResult.Value);
    }

    
    [Fact]
    public void Delete_RemovesPersonFromList()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Delete(1);

        // Assert
        Assert.IsType<OkResult>(result);
        var resultAfterDelete = controller.Get() as ActionResult<List<Person>>;
        var okResultAfterDelete = Assert.IsType<OkObjectResult>(resultAfterDelete.Result); // Stellt sicher, dass das Ergebnis vom Typ OkObjectResult ist
        var personsAfterDelete = Assert.IsType<List<Person>>(okResultAfterDelete.Value); // Stellt sicher, dass der Wert vom Typ List<Person> ist
        Assert.NotNull(personsAfterDelete);
        Assert.Equal(2, personsAfterDelete.Count); // Überprüft, ob die Liste 2  Personen enthält
        
    }
    
    [Fact]
    public void Delete_ReturnsNotFound_WhenIdIsZero()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Delete(0);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result);
    }


    [Theory]
    [InlineData("A", true)]
    [InlineData("MaximilianMaximilianMaximilianMaximilian", true)]
    [InlineData("", false)]
    [InlineData("123", false)]
    [InlineData("Max@", false)]
    [InlineData("Max-Max", true)]
    [InlineData("Max123", false)]
    public void IsNameValid_ReturnsExpectedResult(string name, bool expected)
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.GetType()
            .GetMethod("IsNameValid",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .Invoke(controller, new object[] { name });

        // Assert
        Assert.Equal(expected, result);
    }
}