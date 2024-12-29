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

        
        
        // GET: api/<PersonController>
        [HttpGet]
        public List<Person> Get()
        {
            return _personService.GetAllPersons();
        }
        
        // GET api/<PersonController>/5
        [HttpGet("{id}")]
        public Person? Get(int id)
        {
            var person = _personService.GetPersonById(id);
            return person;
        }
        
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


        
    }
}
