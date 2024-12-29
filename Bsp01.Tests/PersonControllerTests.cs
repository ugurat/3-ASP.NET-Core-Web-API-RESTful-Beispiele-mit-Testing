using Bsp01.Controllers;
using Bsp01.Models;
using Bsp01.Services;

namespace Bsp01.Tests;

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
    public void Get_ShouldReturnAllPersons()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Get();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count); // Es sollten 3 Personen existieren
    }

    
    [Fact]
    public void Get_WithValidId_ShouldReturnPerson()
    {
        // Arrange
        var controller = CreateController();
        var personId = 1;

        // Act
        var result = controller.Get(personId);

        // Assert
        Assert.NotNull(result); // Ergebnis darf nicht null sein
        Assert.Equal("Max", result.Vorname); // Prüfen des Vornamens
        Assert.Equal("Mustermann", result.Nachname); // Prüfen des Nachnamens
    }

    [Fact]
    public void Get_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var controller = CreateController();
        var personId = 99;

        // Act
        var result = controller.Get(personId);

        // Assert
        Assert.Null(result); // Ergebnis soll null sein
    }

    [Fact]
    public void Post_ShouldAddNewPerson()
    {
        // Arrange
        var controller = CreateController();
        var newPerson = new Person { PersonId = 4, Vorname = "Anna", Nachname = "Muster" };

        // Act
        controller.Post(newPerson);
        var result = controller.Get();

        // Assert
        Assert.Equal(4, result.Count); // Erwartet 4 Einträge
        Assert.Contains(result, p => p.PersonId == 4 && p.Vorname == "Anna" && p.Nachname == "Muster");
    }

    [Fact]
    public void Post_ReturnsNull_WhenPersonIsNull()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Post(null);

        // Assert
        Assert.Null(result);
    }

    
    [Fact]
    public void Put_ShouldUpdateExistingPerson()
    {
        // Arrange
        var controller = CreateController();
        var updatedPerson = new Person { Vorname = "Updated", Nachname = "Name" };

        // Act
        controller.Put(1, updatedPerson);
        var result = controller.Get(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated", result.Vorname); // Geänderter Vorname
        Assert.Equal("Name", result.Nachname); // Geänderter Nachname
    }

    [Fact]
    public void Put_ReturnsNull_WhenIdIsZero()
    {
        // Arrange
        var controller = CreateController();
        var person = new Person { Vorname = "Anna", Nachname = "Muster" };

        // Act
        var result = controller.Put(0, person);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Put_ReturnsNull_WhenPersonIsNull()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Put(1, null);

        // Assert
        Assert.Null(result);
    }


    
    [Fact]
    public void Delete_ShouldRemovePerson()
    {
        // Arrange
        var controller = CreateController();
        var personId = 1;

        // Act
        controller.Delete(personId);
        var result = controller.Get(personId);

        // Assert
        Assert.Null(result); // Person soll entfernt sein
        Assert.Equal(2, controller.Get().Count); // Erwartet 2 verbleibende Einträge
    }
 
    [Fact]
    public void Delete_ReturnsFalse_WhenIdIsZero()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Delete(0);

        // Assert
        Assert.False(result);
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