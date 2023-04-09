using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlueMates.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace BlueMates.Controllers
{
    public class EventsController : Controller
    {
        private readonly BlueMatesContext _context;


        public EventsController(BlueMatesContext context)
        {
            _context = context;

        }

        // GET: Events
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return _context.Events != null ?
                        View(await _context.Events.ToListAsync()) :
                        Problem("Entity set 'BlueMatesContext.Events'  is null.");
        }

        [HttpGet("MyEvents")]
        [Authorize]
        public async Task<IActionResult> MyEvents()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;  //TODO find a better way to get id 

            var events = from e in _context.Events
                         where e.OrganizerId == userId
                         select e;
            return View(events);
        }

        [HttpGet("ComingUp")]
        [Authorize]
        public async Task<IActionResult> ComingUp()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;  //TODO find a better way to get id 

            var events = from e in _context.Events
                         where e.StartDate >= DateTime.UtcNow
                         select e;

            return View(events);
        }

        [HttpGet("going/{event_id}")]
        [Authorize]
        public async Task<IActionResult> Going(int? event_id)
        {
            if (event_id == null || _context.Events == null)
            {
                return NotFound();
            }
            
            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == event_id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);



        }

        [HttpPost, ActionName("goingConfirmed")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> GoingConfirmed(int id)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;  //TODO find a better way to get id 

            UsersToEvent usersToEvent = new UsersToEvent();

            usersToEvent.UserId = userId;
            usersToEvent.EventId = id;
            ViewBag.event_id = id;
            usersToEvent.InterestLevel = 2;
            usersToEvent.Validated = false;

            if (ModelState.IsValid)
            {
                _context.Add(usersToEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ComingUp));
            }
            return RedirectToAction(nameof(Index));  //TODO add an error
        } 
        public async Task<IActionResult> Interested(int? event_id)
        {
            if (event_id == null || _context.Events == null)
            {
                return NotFound();
            }
            
            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == event_id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);



        }

        [HttpPost, ActionName("interestedConfirmed")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> InterestedConfirmed(int id)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;  //TODO find a better way to get id 

            UsersToEvent usersToEvent = new UsersToEvent();

            usersToEvent.UserId = userId;
            usersToEvent.EventId = id;
            ViewBag.event_id = id;
            usersToEvent.InterestLevel = 1;
            usersToEvent.Validated = false;

            if (ModelState.IsValid)
            {
                _context.Add(usersToEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ComingUp));
            }
            return RedirectToAction(nameof(Index));  //TODO add an error
        }

       
        [HttpGet("EventsOfInterest")]
        [Authorize]
        public async Task<IActionResult> EventsOfInterest()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;  //TODO find a better way to get id 

            var events = from e in _context.Events
                         join ute in _context.UsersToEvents
                         on  e.Id equals ute.EventId
                         where ute.UserId == userId
                         select new EventsOfInterestResults(e,ute.InterestLevel,ute.Validated);
            
            
            


            return View(events);
        }

        // GET: Events/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,OrganizerId,StartDate,Location,EndDate,Pic")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,OrganizerId,StartDate,Location,EndDate,Pic")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Events == null)
            {
                return Problem("Entity set 'BlueMatesContext.Events'  is null.");
            }
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
          return (_context.Events?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
