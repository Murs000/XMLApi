using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Xml;

namespace XMLApi.Controllers;

[ApiController]
[Route("[controller]")]
public class XController : ControllerBase
{
    [HttpGet]
    public IActionResult Get(string name,string surname,string fatherName)
    {
        List<string> names= [];
        String URLString = @"http://www.hms.gov.az/frq-content/nov_snk_v1/DOMESTIC.xml";
        
        XmlTextReader reader = new XmlTextReader (URLString);

        bool b = false;
        while (reader.Read())
        {
            if(reader.NodeType == XmlNodeType.Element && reader.Name == "NAME_ORIGINAL_SCRIPT")
            {
                b = true;
            }
            if(reader.NodeType == XmlNodeType.Text && b)
            {
                names.Add(reader.Value);
                b = false;
            }
        }
        List<string[]> nameParts = [];
        foreach(var pName in names)
        {
            var namePart = pName.Split();
            nameParts.Add(namePart);
        }
        List<Person> persons= Serialize(nameParts);
        foreach(var person in persons)
        {
            if(person.Name == name && person.Surname == surname && person.FatherName == fatherName)
            {
                throw new Exception("You can not autorize");
            }
        }
        return Ok();
    }
    private List<Person> Serialize(List<string[]> names)
    {
        List<Person> persons = [];
        foreach(var name in names)
        {
            Person person =new Person(name[1],name[0],name[2]);
            persons.Add(person);
        }
        return persons;
    }
    public record Person(string Name,string Surname,string FatherName);
}

