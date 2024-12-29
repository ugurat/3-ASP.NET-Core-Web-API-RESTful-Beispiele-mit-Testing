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
        
        
        // GET: api/<PersonController>
        [HttpGet]
        public ActionResult<List<Person>> Get()
        {
            return Ok(_personService.GetAllPersons());
        }

        
        // GET api/<PersonController>/5
        [HttpGet("{id}")]
        public ActionResult<Person> Get(int id)
        {
            var person = _personService.GetPersonById(id);
            if (person == null)
                return NotFound($"Person mit ID {id} nicht gefunden.");
            return Ok(person);
        }

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
               
    }
}