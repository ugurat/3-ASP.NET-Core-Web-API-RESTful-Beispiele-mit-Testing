

# Bsp01 - Web API - PersonController (sync)

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

Der Code registriert zunächst einen `PersonService` als Singleton im Dependency Injection Container, 
wodurch während der gesamten Laufzeit der Anwendung eine einzige Instanz genutzt wird. 
Anschließend werden OpenAPI/Swagger-Dienste hinzugefügt, um eine automatische API-Dokumentation zu generieren. 
In der HTTP-Anfrage-Pipeline wird geprüft, ob die Anwendung im Entwicklungsmodus läuft. 
Falls dies der Fall ist, wird die Swagger-Oberfläche aktiviert, einschließlich eines JSON-Endpunkts 
für die API-Beschreibung und einer interaktiven Benutzeroberfläche zur API-Erkundung.


---

### Models / Person.cs

````csharp
using System.ComponentModel.DataAnnotations;

namespace Bsp01.Models;

public class Person
{
        
    [Key] // DataAnnotations
    public int PersonId { get; set; }
    
    public string? Vorname { get; set; }
    public string? Nachname { get; set; }
}
````

Das Modell 'Person' im Namespace 'Bsp01.Models' repräsentiert eine Entität in der Datenbank. Die Klasse ist mit der Annotation [Key] ausgestattet, welche das Feld 'PersonId' als Primärschlüssel der Tabelle definiert. Die Eigenschaften 'Vorname' und 'Nachname' sind optional und können null-Werte aufnehmen. Diese Struktur ermöglicht das Speichern und Abrufen von Personendaten mit einer eindeutigen Identifikation und optionalen Namen.


### Services / PersonService.cs 

````csharp
    
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

````

Die Klasse `PersonService` verwaltet eine Liste von `Person`-Objekten und bietet grundlegende 
CRUD-Funktionen. Die Liste wird im Konstruktor initialisiert und mit drei Beispielpersonen befüllt.

Die Methode `GetAllPersons` gibt die gesamte Personenliste zurück, während `GetPersonById` 
eine spezifische Person anhand ihrer ID sucht und zurückgibt oder `null`, falls sie nicht existiert.

Neue Personen können mit der Methode `AddPerson` zur Liste hinzugefügt werden. 
Mit der Methode `UpdatePerson` können bestehende Personen anhand ihrer ID aktualisiert werden, 
wobei Vor- und Nachname überschrieben werden. Sie gibt `true` zurück, 
wenn die Aktualisierung erfolgreich war, andernfalls `false`.

Mit der Methode `DeletePerson` kann eine Person anhand ihrer ID gelöscht werden. 
Bei erfolgreichem Löschen wird `true` zurückgegeben, andernfalls `false`. 
Die Klasse dient somit als einfacher Service für die Verwaltung von Personen in einer Liste.


### Controllers / PersonController.cs | Konstruktor

````csharp

using Bsp01.Models;
using Bsp01.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bsp01.Controllers
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

Die Klasse `PersonController` ist ein API-Controller, der die Route `api/person` bereitstellt und 
mit der Attributdeklaration `[ApiController]` gekennzeichnet ist, 
wodurch automatische Validierungen und API-spezifische Funktionen aktiviert werden.

Der Controller enthält eine private Instanz von `PersonService`, 
die über Dependency Injection bereitgestellt wird. Der Konstruktor übernimmt die Instanz 
und ermöglicht so den Zugriff auf die CRUD-Funktionen des `PersonService`.

Die Klasse ist eine Grundlage für die Implementierung von API-Endpunkten, die Operationen 
wie Abrufen, Hinzufügen, Aktualisieren und Löschen von Personen ermöglichen. 
Diese Endpunkte müssen in den Methoden des Controllers definiert werden.


### Controllers / PersonController.cs | Get()

````csharp

        // GET: api/<PersonController>
        [HttpGet]
        public List<Person> Get()
        {
            return _personService.GetAllPersons();
        }
        
````

Die Methode `Get` im `PersonController` ist mit dem HTTP-Verb `[HttpGet]` markiert, 
wodurch sie bei einer GET-Anfrage an die Route `api/person` aufgerufen wird.

Die Methode ruft die `GetAllPersons`-Methode des `PersonService` auf und gibt die gesamte Liste 
der Personen als `List<Person>` zurück. Dadurch stellt dieser Endpunkt eine Möglichkeit bereit, 
alle in der Anwendung gespeicherten Personen abzufragen.


### Controllers / PersonController.cs | Get(int id)

````csharp

        // GET api/<PersonController>/5
        [HttpGet("{id}")]
        public Person? Get(int id)
        {
            var person = _personService.GetPersonById(id);
            return person;
        }

````

Die Methode `Get(int id)` im `PersonController` ist mit `[HttpGet("{id}")]` annotiert, 
wodurch sie GET-Anfragen mit einer spezifischen ID an die Route `api/person/{id}` behandelt.

Die Methode ruft `GetPersonById` des `PersonService` auf und sucht nach einer Person mit der angegebenen ID.

Falls die Person existiert, wird sie zurückgegeben. Existiert keine Person mit dieser ID, 
wird `null` zurückgegeben. Dieser Endpunkt ermöglicht es, detaillierte Informationen zu einer 
einzelnen Person anhand ihrer ID abzufragen.


### Controllers / PersonController.cs | Post()

````csharp

        // POST api/<PersonController>
        [HttpPost]
        public Person Post([FromBody] Person person)
        {
            if (person == null)
                return null;

            if (!IsNameValid(person.Vorname))
                return null;

            if (!IsNameValid(person.Nachname))
                return null;

            _personService.AddPerson(person);
            return person;
        }

````

Die Methode `Post` im `PersonController` ist mit `[HttpPost]` markiert, wodurch sie POST-Anfragen 
an die Route `api/person` verarbeitet.

Die Methode erwartet ein `Person`-Objekt im Anfrage-Body, das mit `[FromBody]`
als Eingabeparameter deklariert ist.

Es wird überprüft:
1. Ob das übermittelte `person`-Objekt `null` ist.
2. Ob der Vorname und Nachname mithilfe der Hilfsmethode `IsNameValid` validiert werden können.

Wenn alle Überprüfungen erfolgreich sind, wird die Person mit `AddPerson` des `PersonService` 
zur Liste hinzugefügt. Die hinzugefügte Person wird als Antwort zurückgegeben.

Falls eine der Bedingungen nicht erfüllt ist, gibt die Methode `null` zurück. 
Dieser Endpunkt dient dazu, neue Personen hinzuzufügen, wobei einfache Validierungen angewendet werden.


### Controllers / PersonController.cs | Put()

````csharp

        // PUT api/<PersonController>/5
        [HttpPut("{id}")]
        public Person Put(int id, [FromBody] Person person)
        {
            
            if (id <= 0)
                return null;
            
            if (person == null)
                return null;
            
            if (!IsNameValid(person.Vorname))
                return null;

            if (!IsNameValid(person.Nachname))
                return null;

            var success = _personService.UpdatePerson(id, person);
            if (!success) return null;

            return person;
        }

````

Die Methode `Put` im `PersonController` ist mit `[HttpPut("{id}")]` annotiert und 
verarbeitet PUT-Anfragen an die Route `api/person/{id}`.

Die Methode nimmt zwei Parameter entgegen:
1. Die ID der Person, die aktualisiert werden soll.
2. Ein `Person`-Objekt aus dem Anfrage-Body, das mit `[FromBody]` bereitgestellt wird.

Es werden mehrere Validierungen durchgeführt:
1. Die ID muss größer als 0 sein.
2. Das `person`-Objekt darf nicht `null` sein.
3. Der Vorname und Nachname werden mithilfe der Methode `IsNameValid` überprüft.

Wenn die Validierungen erfolgreich sind, wird die Methode `UpdatePerson` des `PersonService` 
aufgerufen, um die Person zu aktualisieren. War die Aktualisierung erfolgreich, wird 
das aktualisierte `person`-Objekt zurückgegeben. Andernfalls gibt die Methode `null` zurück.

Dieser Endpunkt ermöglicht die Aktualisierung bestehender Personendaten, wobei 
grundlegende Überprüfungen und Validierungen integriert sind.


### Controllers / PersonController.cs | Delete(int id)

````csharp

        // DELETE api/<PersonController>/5
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            if(id <= 0)
                return false;
            
            var personToRemove = _personService.GetPersonById(id);

            if (personToRemove == null)
                return false;

            bool result = _personService.DeletePerson(id);
            if(result)
                return true;
            else
                return false;
        }

````

Die Methode `Delete` im `PersonController` ist mit `[HttpDelete("{id}")]` annotiert und 
verarbeitet DELETE-Anfragen an die Route `api/person/{id}`.

Die Methode überprüft:
1. Ob die angegebene ID größer als 0 ist. Ist dies nicht der Fall, wird `false` zurückgegeben.
2. Ob eine Person mit der angegebenen ID existiert, indem die Methode `GetPersonById` des `PersonService` aufgerufen wird. Existiert keine solche Person, wird ebenfalls `false` zurückgegeben.

Falls eine Person existiert, wird `DeletePerson` des `PersonService` aufgerufen, 
um die Person aus der Liste zu entfernen. Das Ergebnis wird als `true` oder `false` zurückgegeben, 
abhängig davon, ob der Löschvorgang erfolgreich war.

Dieser Endpunkt ermöglicht das Löschen einer Person anhand ihrer ID, wobei 
eine Rückmeldung über den Erfolg des Vorgangs gegeben wird.


### Controllers / PersonController.cs | Hilfsmethoden

````csharp

        // Hilfsmethoden für die Namensvalidierung
        private bool IsNameValid(string? name)
        {
            if (string.IsNullOrEmpty(name)) return false; // Name darf nicht leer sein
            if (name.Length < 1 || name.Length > 41) return false; // Name muss zwischen 1 und 41 Zeichen lang sein
            if (ContainsInvalidCharacters(name)) return false; // Name darf keine ungültigen Zeichen enthalten

            return true; // Name ist gültig
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

Die Hilfsmethoden und der zusätzliche Endpunkt im `PersonController` dienen der Namensvalidierung:

1. **`IsNameValid`**:
    - Überprüft, ob der Name nicht leer ist (`string.IsNullOrEmpty`).
    - Stellt sicher, dass die Länge des Namens zwischen 1 und 41 Zeichen liegt.
    - Prüft mit der Methode `ContainsInvalidCharacters`, ob der Name ungültige Zeichen enthält.
    - Gibt `true` zurück, wenn alle Bedingungen erfüllt sind, andernfalls `false`.

2. **`ContainsInvalidCharacters`**:
    - Sucht nach ungültigen Zeichen im Namen. Ein Name darf keine Ziffern enthalten (`char.IsDigit`) und 
      keine Sonderzeichen wie `+*/=%&$@!^`.

3. **`ValidateName`**:
    - Ein zusätzlicher API-Endpunkt, der GET-Anfragen an die Route `api/person/validateName` verarbeitet.
    - Ruft die Methode `IsNameValid` auf und gibt `true` zurück, wenn der übergebene Name gültig ist, 
      andernfalls `false`.
    - Dient dazu, Namen vor der Weiterverarbeitung oder Speicherung zu überprüfen.

Diese Hilfsmethoden und der Endpunkt sorgen für eine zentrale und konsistente Namensprüfung in der Anwendung.

