

# # Bsp02 - Web API - PersonController (sync) + ActionResult

URL: http://localhost:5000/swagger/index.html

---

### Nuget-Pakete installieren

```` bash
Swashbuckle.AspNetCore 7.2.0
````

---

### Program.cs

```` csharp

builder.Services.AddSingleton<PersonService>();

// Swagger/OpenAPI-Dienste hinzufügen
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// HTTP-Anfrage-Pipeline konfigurieren
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Swagger JSON-Endpunkt
    app.UseSwaggerUI(); // Swagger-Oberfläche
}

````

Der Code registriert den `PersonService` als Singleton und fügt Swagger/OpenAPI-Dienste 
für die automatische API-Dokumentation hinzu. Im Entwicklungsmodus werden die Swagger-Dokumentation und 
eine Benutzeroberfläche zur API-Erkundung aktiviert.

---

### Models / Person.cs

````csharp

using System.ComponentModel.DataAnnotations;

namespace Bsp02.Models;

public class Person
{
    
    [Key] // DataAnnotations
    public int PersonId { get; set; }
    
    public string? Vorname { get; set; }
    public string? Nachname { get; set; }
    
}

````

Die Klasse `Person` definiert ein Modell mit einer eindeutigen ID (`PersonId`), die 
durch das `[Key]`-Attribut als Primärschlüssel markiert ist. Zusätzlich enthält sie 
optionale Eigenschaften für den Vor- und Nachnamen, die die grundlegenden Daten einer Person repräsentieren.

### Services / PersonService.cs 

````csharp

using Bsp02.Models;

namespace Bsp02.Services;

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

````

Die Klasse `PersonService` verwaltet eine Liste von Personen und bietet Methoden für CRUD-Operationen. 
Im Konstruktor wird die Liste mit Beispieldaten initialisiert. Mit den Methoden können 
alle Personen abgefragt, einzelne Personen anhand ihrer ID gesucht, neue Personen hinzugefügt, 
bestehende Personen aktualisiert und Personen gelöscht werden. Die Logik basiert auf der 
internen Verwaltung einer Liste, wodurch der Service als einfacher Speicher agiert.


### Controllers / PersonController.cs | Konstruktor

````csharp

using Bsp02.Models;
using Bsp02.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bsp02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {

        private readonly PersonService _personService;

        public PersonController(PersonService personService)
        {
            _personService = personService;
        }
        
        // weitere Implementierungen
        
    }
}

````

Der `PersonController` stellt einen API-Controller dar, der die Route `api/person` bereitstellt. 
Er nutzt Dependency Injection, um auf den `PersonService` zuzugreifen, wodurch CRUD-Operationen 
auf Personen ermöglicht werden. Weitere Endpunkte für spezifische Funktionen können 
innerhalb der Klasse implementiert werden.


### Controllers / PersonController.cs | Get()

````csharp

        // GET: api/<PersonController>
        [HttpGet]
        public ActionResult<List<Person>> Get()
        {
            return Ok(_personService.GetAllPersons());
        }
        
````

Die `Get`-Methode des `PersonController` verarbeitet GET-Anfragen an die Route `api/person`. 
Sie ruft die Methode `GetAllPersons` des `PersonService` auf, um die gesamte Liste der Personen 
abzurufen, und gibt das Ergebnis mit einem `200 OK`-Status zurück. Dies ermöglicht es, 
alle gespeicherten Personen abzufragen.


### Controllers / PersonController.cs | Get(int id)

````csharp

        // GET api/<PersonController>/5
        [HttpGet("{id}")]
        public ActionResult<Person> Get(int id)
        {
            var person = _personService.GetPersonById(id);
            if (person == null)
                return NotFound($"Person mit ID {id} nicht gefunden.");
            return Ok(person);
        }
        
````

Die `Get`-Methode mit der Parameter-ID verarbeitet GET-Anfragen an die Route `api/person/{id}`. 
Sie ruft die Methode `GetPersonById` des `PersonService` auf, um eine spezifische Person anhand 
ihrer ID zu suchen. Wenn keine Person gefunden wird, gibt die Methode eine `404 Not Found`-Antwort 
mit einer Fehlermeldung zurück. Wird die Person gefunden, erfolgt eine Rückgabe mit `200 OK` und 
den Personendaten.


### Controllers / PersonController.cs | Post()

````csharp

        // POST api/<PersonController>
        [HttpPost]
        public IActionResult Post([FromBody] Person person)
        {
            
            if (person == null)
                return BadRequest("Person-Objekt darf nicht null sein.");

            if (!IsNameValid(person.Vorname))
                return BadRequest("Vorname ist ungültig!");

            if (!IsNameValid(person.Nachname))
                return BadRequest("Nachname ist ungültig!");

            _personService.AddPerson(person);
            return Ok(person);
        }

        
````

Die `Post`-Methode verarbeitet POST-Anfragen an die Route `api/person`. 
Sie erwartet ein `Person`-Objekt im Anfrage-Body.

Die Methode validiert das Objekt:
- Es darf nicht `null` sein, sonst wird eine `400 Bad Request`-Antwort mit einer Fehlermeldung zurückgegeben.
- Vor- und Nachname werden mit der Methode `IsNameValid` geprüft. Ungültige Eingaben führen ebenfalls 
  zu einer `400 Bad Request`-Antwort.

Wenn alle Prüfungen bestanden sind, wird die Person über den `PersonService` hinzugefügt. 
Die Methode gibt die hinzugefügte Person mit einer `200 OK`-Antwort zurück.


### Controllers / PersonController.cs | Put()

````csharp

        // PUT api/<PersonController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Person person)
        {
            if (id <= 0)
                return BadRequest("ID muss größer als 0 sein.");

            if (person == null)
                return BadRequest("Person-Objekt darf nicht null sein.");

            if (!IsNameValid(person.Vorname))
                return BadRequest("Vorname ist ungültig!");

            if (!IsNameValid(person.Nachname))
                return BadRequest("Nachname ist ungültig!");

            var success = _personService.UpdatePerson(id, person);
            if (!success) return NotFound();

            return Ok(person);
        }

````

Die `Put`-Methode verarbeitet PUT-Anfragen an die Route `api/person/{id}` und dient zum Aktualisieren 
einer Person anhand ihrer ID.

Die Methode validiert die Eingaben:
- Die ID muss größer als 0 sein, andernfalls wird eine `400 Bad Request`-Antwort zurückgegeben.
- Das `Person`-Objekt darf nicht `null` sein.
- Vor- und Nachname werden mit `IsNameValid` geprüft. Ungültige Eingaben führen ebenfalls 
  zu einer `400 Bad Request`-Antwort.

Wenn die Validierungen erfolgreich sind, wird die Methode `UpdatePerson` des `PersonService` aufgerufen.
- War die Aktualisierung nicht erfolgreich (z. B. wenn die Person nicht existiert), wird 
  eine `404 Not Found`-Antwort zurückgegeben.
- Bei Erfolg wird die aktualisierte Person mit einer `200 OK`-Antwort zurückgegeben.


### Controllers / PersonController.cs | Delete(int id)

````csharp

        // DELETE api/<PersonController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if(id<=0)
                return NotFound();
            
            var personToRemove = _personService.GetPersonById(id);

            if (personToRemove == null)
                return NotFound();

            bool result = _personService.DeletePerson(id);
            if(result)
                return Ok();
            else
                return NotFound();
        }

````

Die `Delete`-Methode verarbeitet DELETE-Anfragen an die Route `api/person/{id}` und ermöglicht 
das Löschen einer Person anhand ihrer ID.

Die Methode prüft:
- Ob die ID gültig ist (größer als 0). Andernfalls wird eine `404 Not Found`-Antwort zurückgegeben.
- Ob eine Person mit der angegebenen ID existiert. Existiert keine, erfolgt ebenfalls 
  eine `404 Not Found`-Antwort.

Wenn die Person gefunden wurde, wird die Methode `DeletePerson` des `PersonService` aufgerufen:
- Ist das Löschen erfolgreich, wird eine `200 OK`-Antwort zurückgegeben.
- Andernfalls erfolgt eine erneute `404 Not Found`-Antwort.

Der Endpunkt bietet somit eine klare Struktur für das Löschen von Daten mit entsprechenden Rückmeldungen.


### Controllers / PersonController.cs | Hilfsmethoden

````csharp

        // Hilfsmethoden für die Namensvalidierung
        private bool IsNameValid(string? name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            if (name.Length < 1 || name.Length > 41) return false;
            if (ContainsInvalidCharacters(name)) return false;

            return true;
        }

        private bool ContainsInvalidCharacters(string name)
        {
            return name.Any(char.IsDigit) || name.Any(ch => "+*/=%&$@!^".Contains(ch));
        }
        
        [HttpGet("validateName")]
        public bool ValidateName(string name)
        {
            return IsNameValid(name);
        }
        
````

Die Namensvalidierung erfolgt über die Methode `IsNameValid`, die sicherstellt, dass 
der Name nicht leer ist, eine Länge zwischen 1 und 41 Zeichen aufweist und keine ungültigen Zeichen 
wie Ziffern oder bestimmte Sonderzeichen enthält. Die Methode `ContainsInvalidCharacters` prüft dabei 
gezielt auf diese unerlaubten Zeichen. Der zusätzliche Endpunkt `ValidateName` ermöglicht es, 
Namen direkt über eine GET-Anfrage auf ihre Gültigkeit zu überprüfen und liefert das Ergebnis 
als Boolean zurück.

