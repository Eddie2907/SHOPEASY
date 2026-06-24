EventService.cs
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
 
namespace dotnetapp.Services
{
    public class EventService
    {
        private List<Event> events = new List<Event>(){
            new Event{
                EventId = 1,
                Name = "Event 1",
                Date = DateTime.Now.AddDays(7),
                Location = "Location 1"
            },
            new Event{
                EventId = 2,
                Name = "Event 2",
                Date = DateTime.Now.AddDays(14),
                Location = "Location 2"
            },
            new Event{
                EventId = 3,
                Name = "Event 3",
                Date = DateTime.Now.AddDays(21),
                Location = "Location 3"
            }
        };
        public List<Event> GetAllEvents(){
            return events;
        }
        public Event GetEventById(int eventId){
            return events.FirstOrDefault(e => e.EventId == eventId);
        }
        public Event CreateEvent(Event newEvent){
            newEvent.EventId = events.Max(e => e.EventId) + 1;
            events.Add(newEvent);
            return newEvent;
        }
        public bool UpdateEvent(int eventId, Event updatedEvent){
            var ev = events.FirstOrDefault(e => e.EventId == eventId);
            if(ev == null){
                return false;
            }
            ev.Name = updatedEvent.Name;
            ev.Date = updatedEvent.Date;
            ev.Location = updatedEvent.Location;
            return true;
        }
        public void DeleteEvent(int eventId){
            var ev = events.FirstOrDefault(e => e.EventId == eventId);
            if(ev != null){
                events.Remove(ev);
            }
        }
    }
}
 
EventController.cs
 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dotnetapp.Services;
using dotnetapp.Models;
 
namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly EventService db;
        public EventController(EventService db1){
            db = db1;
        }
        [HttpGet]
        public IActionResult GetAllEvents(){
            var events = db.GetAllEvents();
            if(events.Count == 0){
                return NoContent();
            }
            return Ok(events);
        }
        [HttpGet("{eventId}")]
        public IActionResult GetEventById(int eventId){
            var ev = db.GetEventById(eventId);
            if(ev == null){
                return NotFound();
            }
            return Ok(ev);
        }
        [HttpPost]
        public IActionResult CreateEvent([FromBody] Event newEvent){
            if(newEvent == null){
                return BadRequest();
            }
            db.CreateEvent(newEvent);
            return CreatedAtAction(nameof(GetEventById),
            new { eventId = newEvent.EventId},
            newEvent);
        }
        [HttpPut("{eventId}")]
        public IActionResult UpdateEvent(int eventId, [FromBody] Event updatedEvent){
            if(updatedEvent == null){
                return BadRequest();
            }
            bool res = db.UpdateEvent(eventId, updatedEvent);
            if(!res){
                return NotFound();
            }
            return NoContent();
        }
        [HttpDelete("{eventId}")]
        public IActionResult DeleteEvent(int eventId){
            var res = db.GetEventById(eventId);
            if(res == null){
                return NotFound();
            }
            db.DeleteEvent(eventId);
            return NoContent();
        }
       
    }
}
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
namespace dotnetapp.Models
{
    public class Event
    {
        public int EventId{get;set;}
        public string Name{get;set;}
        public DateTime Date{get;set;}
        public string Location{get;set;}
    }
   
}
 
program.cs
 
using dotnetapp.Services;
using dotnetapp.Models;
 
var builder = WebApplication.CreateBuilder(args);
 
// Add Event services to the container.
 
builder.Services.AddControllers();
builder.Services.AddSingleton<EventService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
var app = builder.Build();
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 
app.UseHttpsRedirection();
 
app.UseAuthorization();
 
app.MapControllers();
 
app.Run();
 
 
