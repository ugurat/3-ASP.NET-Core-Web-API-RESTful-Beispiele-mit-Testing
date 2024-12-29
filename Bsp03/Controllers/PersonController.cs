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
        

        // GET: api/<PersonController>
        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get()
        {
            return await Task.FromResult(Ok( await _personService.GetAllPersonsAsync()));
        }


        // GET api/<PersonController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            var person = await _personService.GetPersonByIdAsync(id);
            if (person == null)
                return NotFound($"Person mit ID {id} nicht gefunden.");
            return await Task.FromResult(Ok(person));
        }

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

        
    }
}
