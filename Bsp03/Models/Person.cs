using System.ComponentModel.DataAnnotations;

namespace Bsp03.Models;

public class Person
{
    [Key] // DataAnnotations
    public int PersonId { get; set; }
    
    public string? Vorname { get; set; }
    public string? Nachname { get; set; }
}
