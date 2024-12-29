
# Bsp03 - Web API - PersonController (async Task) + ActionResult 

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

Der Code registriert den `PersonService` als Singleton und integriert Swagger/OpenAPI-Dienste 
für die API-Dokumentation. Im Entwicklungsmodus werden Swagger-Endpunkte und eine Benutzeroberfläche 
zur API-Erkundung aktiviert, um die API während der Entwicklung leichter zugänglich zu machen.


---

### Models / Person.cs

````csharp

using System.ComponentModel.DataAnnotations;

namespace Bsp03.Models;

public class Person
{
    
    [Key] // DataAnnotations
    public int PersonId { get; set; }
    
    public string? Vorname { get; set; }
    public string? Nachname { get; set; }
    
}

````

Das Modell 'Person' im Namespace 'Bsp03.Models' repräsentiert eine Entität in einer Datenbank und 
wird in ASP.NET Core-Anwendungen verwendet, um Daten zu verarbeiten. Die Klasse 'Person' ist 
mit dem Attribut `[Key]` ausgestattet, welches durch die DataAnnotations-Bibliothek bereitgestellt wird 
und `PersonId` als Primärschlüssel der zugehörigen Datenbanktabelle definiert. 
Die Eigenschaften `Vorname` und `Nachname` sind als nullable Typen (string?) deklariert, was bedeutet, 
dass sie keine Werte enthalten können. Diese Modellstruktur erlaubt es, Personeninformationen 
wie Vor- und Nachnamen zu speichern, während die PersonId eine eindeutige Identifikation jeder Person gewährleistet.


### Services / PersonService.cs

````csharp

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

````

Die Klasse `PersonService` bietet CRUD-Operationen für eine interne Liste von Personen. Sie wurde asynchron gestaltet, 
indem alle Methoden auf `async` und `Task` umgestellt wurden, um zukünftige Erweiterungen wie Datenbankoperationen zu unterstützen.

Die Liste wird im Konstruktor mit Beispieldaten initialisiert. Methoden wie 
`GetAllPersonsAsync`, `GetPersonByIdAsync`, `AddPersonAsync`, `UpdatePersonAsync` und `DeletePersonAsync` ermöglichen 
das Abrufen, Hinzufügen, Aktualisieren und Löschen von Personen, während die asynchrone Implementierung 
eine nicht-blockierende Ausführung gewährleistet. Dies verbessert die Skalierbarkeit und erlaubt den nahtlosen Wechsel 
zu datenbankbasierten Operationen.


### Controllers / PersonController.cs | Konstruktor

````csharp

using Bsp03.Models;
using Bsp03.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bsp03.Controllers
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

        // weitere Implenmentierungen
        
    }
}
        
````

Die Klasse `PersonController` ist ein API-Controller, der die Route `api/person` bereitstellt. Sie nutzt Dependency Injection, 
um auf den asynchronen `PersonService` zuzugreifen. Dies ermöglicht die Implementierung von Endpunkten für CRUD-Operationen 
auf der Personenliste. Weitere Implementierungen wie GET-, POST-, PUT- und DELETE-Methoden können hinzugefügt werden, 
um die API-Funktionalität abzurunden.


### Controllers / PersonController.cs | Get()

````csharp

        // GET: api/<PersonController>
        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get()
        {
            return await Task.FromResult(Ok( await _personService.GetAllPersonsAsync()));
        }
        
````

Die Methode `Get` im `PersonController` verarbeitet GET-Anfragen an die Route `api/person` asynchron. 
Sie ruft `GetAllPersonsAsync` aus dem `PersonService` auf, um alle Personen abzurufen, und gibt das Ergebnis 
mit einer `200 OK`-Antwort zurück. Durch die asynchrone Implementierung wird die Verarbeitung effizienter, 
besonders bei zukünftigen datenbankbasierten Operationen.


### Controllers / PersonController.cs | Get(int id)

````csharp

        // GET api/<PersonController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            var person = await _personService.GetPersonByIdAsync(id);
            if (person == null)
                return NotFound($"Person mit ID {id} nicht gefunden.");
            return await Task.FromResult(Ok(person));
        }
        
````

Die Methode `Get(int id)` verarbeitet GET-Anfragen an die Route `api/person/{id}` asynchron. 
Sie ruft die Methode `GetPersonByIdAsync` aus dem `PersonService` auf, um eine Person anhand der ID zu suchen. 
Wird keine Person gefunden, gibt sie eine `404 Not Found`-Antwort mit einer Fehlermeldung zurück. 
Andernfalls wird die gefundene Person mit einer `200 OK`-Antwort zurückgegeben. Die asynchrone Struktur ermöglicht 
eine effiziente Verarbeitung und künftige Erweiterungen.


### Controllers / PersonController.cs | Post()

````csharp

        // POST api/<PersonController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Person person)
        {
            
            if (person == null)
                return BadRequest("Person-Objekt darf nicht null sein.");
            
            if (!IsNameValid(person.Vorname))
                return BadRequest("Vorname ist ungültig!");

            if (!IsNameValid(person.Nachname))
                return BadRequest("Nachname ist ungültig!");

            await _personService.AddPersonAsync(person);
            return await Task.FromResult(CreatedAtAction(nameof(Get), new { id = person.PersonId }, person));
        }
        
````

Die Methode `Post` verarbeitet POST-Anfragen an die Route `api/person` asynchron und dient zum Hinzufügen einer neuen Person.

Zunächst wird geprüft, ob das übergebene `Person`-Objekt nicht `null` ist und ob Vor- und Nachname mit der Methode `IsNameValid` 
gültig sind. Ungültige Eingaben führen zu einer `400 Bad Request`-Antwort mit entsprechenden Fehlermeldungen.

Ist die Validierung erfolgreich, wird die Person mit `AddPersonAsync` zum `PersonService` hinzugefügt. 
Als Antwort wird eine `201 Created`-Antwort zurückgegeben, die die URL des neuen Endpunkts sowie das erstellte Objekt enthält. 
Die asynchrone Implementierung unterstützt eine reibungslose Verarbeitung in komplexeren Szenarien.


### Controllers / PersonController.cs | Put()

````csharp

        // PUT api/<PersonController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Person person)
        {
            if (id <= 0)
                return BadRequest("ID muss größer als 0 sein.");

            if (person == null)
                return BadRequest("Person-Objekt darf nicht null sein.");

            if (!IsNameValid(person.Vorname))
                return BadRequest("Vorname ist ungültig!");

            if (!IsNameValid(person.Nachname))
                return BadRequest("Nachname ist ungültig!");

            var success = await _personService.UpdatePersonAsync(id, person);
            if (!success) return NotFound();
 
            return await Task.FromResult(Ok(person));
        }

````

Die Methode `Put` verarbeitet PUT-Anfragen an die Route `api/person/{id}` asynchron und dient zur Aktualisierung einer bestehenden Person.

Zunächst wird überprüft, ob die übergebene ID größer als 0 ist und das `Person`-Objekt gültig ist. Vor- und Nachname werden 
mit der Methode `IsNameValid` validiert. Ungültige Eingaben führen zu einer `400 Bad Request`-Antwort mit entsprechenden Fehlermeldungen.

Wird die Person mit `UpdatePersonAsync` im `PersonService` erfolgreich aktualisiert, gibt die Methode die aktualisierten Daten 
mit einer `200 OK`-Antwort zurück.


### Controllers / PersonController.cs | Delete(int id)

````csharp

        // DELETE api/<PersonController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if(id <= 0)
                return NotFound($"Person mit ID {id} ist ungültig.");
            
            var personToRemove = await _personService.GetPersonByIdAsync(id);

            if (personToRemove == null)
                return NotFound($"Person mit ID {id} nicht gefunden.");

            bool result = await _personService.DeletePersonAsync(id);
            if(result)
                return await Task.FromResult(Ok($"Person mit ID {id} wurde gelöscht."));
            else
                return await Task.FromResult(NotFound($"Person mit ID {id} wurde gelöscht.")); 
        }

````



### Controllers / PersonController.cs | Hilfsmethoden

````csharp

        // Hilfsmethoden für die Namensvalidierung
        
        [HttpGet("validateName")]
        public bool ValidateName(string name)
        {
            return IsNameValid(name);
        }

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

````

Die Methode `ValidateName` ist ein Endpunkt unter der Route `api/person/validateName`, der die Gültigkeit 
eines Namens überprüft. Mithilfe der Methode `IsNameValid` wird sichergestellt, dass der Name nicht leer ist, 
zwischen 1 und 41 Zeichen lang ist und keine ungültigen Zeichen enthält. Die Überprüfung auf ungültige Zeichen 
erfolgt durch die Methode `ContainsInvalidCharacters`, die nach Ziffern oder bestimmten Sonderzeichen sucht. 
Der Endpunkt gibt `true` zurück, wenn der Name gültig ist, andernfalls `false`.
