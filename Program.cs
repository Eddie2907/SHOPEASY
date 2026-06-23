using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicianBookingSystem.Exceptions;
using MusicianBookingSystem.Models;
 
namespace MusicianBookingSystem.Controllers
{
    // [Route("[controller]")]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext db;
 
        public BookingController(ApplicationDbContext db1)
        {
            db = db1;
        }
 
        public IActionResult Index()
        {
            var slots = db.Slots.ToList();
            return View(slots);
        }
 
        public IActionResult Book(int id)
        {
            var slot = db.Slots.FirstOrDefault(s => s.SlotID == id);
            return View(slot);
        }
 
        [HttpPost]
        public IActionResult Book(int id, int Userid)
        {
            try
            {
                var slot = db.Slots.FirstOrDefault(s => s.SlotID == id);
                if (slot == null)
                    return NotFound();
                if (slot.Bookings.Count >= 5)
                    throw new SlotBookingException("Slot is full.");
                if (slot.Bookings.Any(b => b.UserID == Userid))
                    throw new SlotBookingException("You have already booked this slot.");
                Booking booking = new Booking
                {
                    SlotID = id,
                    UserID = Userid
                };
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Summary", new { Userid });
            }
            catch(SlotBookingException ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
        public IActionResult Summary(int Userid)
        {
            var b=db.Bookings.Where(bb=>bb.UserID == Userid).ToList();
            return View(b);
        }
 
 
    }
}
 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicianBookingSystem.Models;
 
namespace MusicianBookingSystem.Controllers
{
    // [Route("[controller]")]
    public class SlotController : Controller
    {
        private readonly ApplicationDbContext db;
 
        public SlotController(ApplicationDbContext db1)
        {
            db=db1;
        }
 
        public IActionResult Index()
        {
            var slots = db.Slots.ToList();
            return View(slots);
        }
    }
}
 
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using MusicianBookingSystem.Models;
 
namespace MusicianBookingSystem.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options){}
 
        public DbSet<Slot>Slots{get;set;}
        public DbSet<Booking>Bookings{get;set;}
    }
}
 
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
 
namespace MusicianBookingSystem.Models
{
    public class Slot
    {
        [Key]
        public int SlotID{get;set;}
        public DateTime Time{get;set;}
        public int Duration{get;set;}
        public int Capacity{get;set;}
        public ICollection<Booking> Bookings{get;set;}
    }
}
 
