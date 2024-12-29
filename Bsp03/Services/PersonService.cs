using Bsp03.Models;

namespace Bsp03.Services;

public class PersonService
{
    
    private List<Person> personList;

    public PersonService()
    {
        // Listeyi başlat
        personList = new List<Person>()
        {
            new Person() { PersonId = 1, Nachname = "Mustermann", Vorname = "Max" },
            new Person() { PersonId = 2, Nachname = "Musterfrau", Vorname = "Erika" },
            new Person() { PersonId = 3, Nachname = "Mustersohn", Vorname = "Felix" },
        };
    }
     
    
    //public List<Person> GetAllPersons() => personList;
    public async Task<List<Person>> GetAllPersonsAsync()
    {
        return personList;
    }
    
    public async Task<Person?> GetPersonByIdAsync(int id)
    {
        return personList.FirstOrDefault(x => x.PersonId == id);
    }
    
    public async Task AddPersonAsync(Person person)
    {
        personList.Add(person);
    }
    
    
    public async Task<bool> UpdatePersonAsync(int id, Person person)
    {
        var existingPerson = personList.FirstOrDefault(x => x.PersonId == id);
        if (existingPerson == null) return false;

        existingPerson.Vorname = person.Vorname;
        existingPerson.Nachname = person.Nachname;

        return true;
    }

    
    public async Task<bool> DeletePersonAsync(int id)
    {
        var person = personList.FirstOrDefault(x => x.PersonId == id);
        if (person == null) return false;

        personList.Remove(person);
        return true;
    }
    
}