using Bsp01.Models;

namespace Bsp01.Services;

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
    
    
    public List<Person> GetAllPersons() => personList;

    public Person? GetPersonById(int id) => personList.FirstOrDefault(x => x.PersonId == id);

    public void AddPerson(Person person) => personList.Add(person);

    public bool UpdatePerson(int id, Person person)
    {
        var existingPerson = GetPersonById(id);
        if (existingPerson == null) return false;

        existingPerson.Vorname = person.Vorname;
        existingPerson.Nachname = person.Nachname;

        return true;
    }

    public bool DeletePerson(int id)
    {
        var person = GetPersonById(id);
        if (person == null) return false;

        personList.Remove(person);
        return true;
    }

    
}