using Bsp03.Models;
using Bsp03.Services;
using Xunit;
using System.Threading.Tasks;

namespace Bsp03.Tests;

public class PersonServiceTests
{
    private readonly PersonService _service;

    public PersonServiceTests()
    {
        // Initialisiere den Service
        _service = new PersonService();
    }

    [Fact]
    public async Task GetAllPersonsAsync_ShouldReturnAllPersons()
    {
        // Act
        var persons = await _service.GetAllPersonsAsync();

        // Assert
        Assert.NotNull(persons);
        Assert.Equal(3, persons.Count);
    }

    [Fact]
    public async Task GetPersonByIdAsync_ShouldReturnCorrectPerson()
    {
        // Arrange
        int personId = 1;

        // Act
        var person = await _service.GetPersonByIdAsync(personId);

        // Assert
        Assert.NotNull(person);
        Assert.Equal("Max", person?.Vorname);
        Assert.Equal("Mustermann", person?.Nachname);
    }

    [Fact]
    public async Task GetPersonByIdAsync_ShouldReturnNullForInvalidId()
    {
        // Arrange
        int invalidId = 999;

        // Act
        var person = await _service.GetPersonByIdAsync(invalidId);

        // Assert
        Assert.Null(person);
    }

    [Fact]
    public async Task AddPersonAsync_ShouldAddNewPerson()
    {
        // Arrange
        var newPerson = new Person { PersonId = 4, Vorname = "Anna", Nachname = "Musterkind" };

        // Act
        await _service.AddPersonAsync(newPerson);
        var persons = await _service.GetAllPersonsAsync();

        // Assert
        Assert.Equal(4, persons.Count);
        Assert.Contains(persons, p => p.PersonId == 4 && p.Vorname == "Anna" && p.Nachname == "Musterkind");
    }

    [Fact]
    public async Task UpdatePersonAsync_ShouldUpdateExistingPerson()
    {
        // Arrange
        int personId = 1;
        var updatedPerson = new Person { Vorname = "Maximilian", Nachname = "Mustermensch" };

        // Act
        var result = await _service.UpdatePersonAsync(personId, updatedPerson);
        var person = await _service.GetPersonByIdAsync(personId);

        // Assert
        Assert.True(result);
        Assert.Equal("Maximilian", person?.Vorname);
        Assert.Equal("Mustermensch", person?.Nachname);
    }

    [Fact]
    public async Task UpdatePersonAsync_ShouldReturnFalseForInvalidId()
    {
        // Arrange
        int invalidId = 999;
        var updatedPerson = new Person { Vorname = "Invalid", Nachname = "Person" };

        // Act
        var result = await _service.UpdatePersonAsync(invalidId, updatedPerson);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeletePersonAsync_ShouldRemovePerson()
    {
        // Arrange
        int personId = 1;

        // Act
        var result = await _service.DeletePersonAsync(personId);
        var persons = await _service.GetAllPersonsAsync();

        // Assert
        Assert.True(result);
        Assert.Equal(2, persons.Count);
        Assert.DoesNotContain(persons, p => p.PersonId == personId);
    }

    [Fact]
    public async Task DeletePersonAsync_ShouldReturnFalseForInvalidId()
    {
        // Arrange
        int invalidId = 999;

        // Act
        var result = await _service.DeletePersonAsync(invalidId);

        // Assert
        Assert.False(result);
    }
}
