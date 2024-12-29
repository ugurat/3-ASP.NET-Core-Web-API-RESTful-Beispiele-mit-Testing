using Bsp02.Models;
using Bsp02.Services;
using Xunit;

namespace Bsp02.Tests;

public class PersonServiceTests
{
    private readonly PersonService _service;

    public PersonServiceTests()
    {
        // Initialisiere den Service
        _service = new PersonService();
    }

    [Fact]
    public void GetAllPersons_ShouldReturnAllPersons()
    {
        // Act
        var persons = _service.GetAllPersons();

        // Assert
        Assert.NotNull(persons);
        Assert.Equal(3, persons.Count);
    }

    [Fact]
    public void GetPersonById_ShouldReturnCorrectPerson()
    {
        // Arrange
        int personId = 1;

        // Act
        var person = _service.GetPersonById(personId);

        // Assert
        Assert.NotNull(person);
        Assert.Equal("Max", person?.Vorname);
        Assert.Equal("Mustermann", person?.Nachname);
    }

    [Fact]
    public void GetPersonById_ShouldReturnNullForInvalidId()
    {
        // Arrange
        int invalidId = 999;

        // Act
        var person = _service.GetPersonById(invalidId);

        // Assert
        Assert.Null(person);
    }

    [Fact]
    public void AddPerson_ShouldAddNewPerson()
    {
        // Arrange
        var newPerson = new Person { PersonId = 4, Vorname = "Anna", Nachname = "Musterkind" };

        // Act
        _service.AddPerson(newPerson);
        var persons = _service.GetAllPersons();

        // Assert
        Assert.Equal(4, persons.Count);
        Assert.Contains(persons, p => p.PersonId == 4 && p.Vorname == "Anna" && p.Nachname == "Musterkind");
    }

    [Fact]
    public void UpdatePerson_ShouldUpdateExistingPerson()
    {
        // Arrange
        int personId = 1;
        var updatedPerson = new Person { Vorname = "Maximilian", Nachname = "Mustermensch" };

        // Act
        var result = _service.UpdatePerson(personId, updatedPerson);
        var person = _service.GetPersonById(personId);

        // Assert
        Assert.True(result);
        Assert.Equal("Maximilian", person?.Vorname);
        Assert.Equal("Mustermensch", person?.Nachname);
    }

    [Fact]
    public void UpdatePerson_ShouldReturnFalseForInvalidId()
    {
        // Arrange
        int invalidId = 999;
        var updatedPerson = new Person { Vorname = "Invalid", Nachname = "Person" };

        // Act
        var result = _service.UpdatePerson(invalidId, updatedPerson);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DeletePerson_ShouldRemovePerson()
    {
        // Arrange
        int personId = 1;

        // Act
        var result = _service.DeletePerson(personId);
        var persons = _service.GetAllPersons();

        // Assert
        Assert.True(result);
        Assert.Equal(2, persons.Count);
        Assert.DoesNotContain(persons, p => p.PersonId == personId);
    }

    [Fact]
    public void DeletePerson_ShouldReturnFalseForInvalidId()
    {
        // Arrange
        int invalidId = 999;

        // Act
        var result = _service.DeletePerson(invalidId);

        // Assert
        Assert.False(result);
    }
}
