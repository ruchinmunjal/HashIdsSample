using HashidsNet;
using Microsoft.AspNetCore.Mvc;

namespace HashIds.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly IHashids _hashIds;

    private static readonly List<Person> People = new()
    {
        new Person { Id = 1, FirstName = "Ruchin", LastName = "Munjal" },
        new Person { Id = 2, FirstName = "Shray", LastName = "Munjal" },
        new Person { Id = 3, FirstName = "Parul", LastName = "Srivastava" },
        new Person { Id = 4, FirstName = "Brinda", LastName = "Murali" },
        new Person { Id = 5, FirstName = "Mia", LastName = "Munjal" }
    };

    public HomeController(IHashids hashIds)
    {
        _hashIds = hashIds;
    }

    [HttpGet]
    public IActionResult GetDataForId(int id)
    {
        var mypeep=People.FirstOrDefault(x => x.Id == id);
        if (mypeep==null)
        {
            return NotFound($"Couldn't find any person with the given id:{id}");
        }

        return Ok(mypeep);
    }
    [HttpGet("hashedId/{hashedId}")]
    public IActionResult GetDataForId(string hashedId)
    {
        var id = _hashIds.Decode(hashedId)[0];
        if (id <= 0)
        {
            return BadRequest($"Not a valid Id:{id}");
        }
        var mypeep=People.FirstOrDefault(x => x.Id == id);
        if (mypeep==null)
        {
            return NotFound($"Couldn't find any person with the given id:{id}");
        }

        return Ok(mypeep);
    }
    [HttpGet("hashedId")]
    public IActionResult GetAllDataEncoded()
    {
        var encodedPeople = (from person in People let encodedId = _hashIds.Encode(person.Id) 
                                        select 
                                            new PersonDto
                                            {
                                                Id = encodedId, FirstName = person.FirstName, 
                                                LastName = person.LastName
                                            }).ToList();

        return Ok(encodedPeople);
    }
    
    
}

public record PersonDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
public record Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}