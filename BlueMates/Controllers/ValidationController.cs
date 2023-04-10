using BlueMates.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Claims;

namespace BlueMates.Controllers
{
    public class ValidationController : Controller
    {
        private readonly BlueMatesContext _context;

        public ValidationController(BlueMatesContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("EventValidations/{id}")]
        [Authorize]
        public IActionResult EventValidations(int id)
        {
            
            var validations = from v in _context.Validations
                              where v.EventId == id
                              select v;

            return View(validations);

        }

        [Authorize]
        public async Task<IActionResult> Validate(int? event_id)
        {
            if (event_id == null || _context.Events == null)  //TODO: check if event is already validated
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == event_id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewBag.EventId = @event.Id;
            return View();
        }

        [HttpPost, ActionName("validateConfirmed")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ValidateConfirmed(float Lat, float Long, int id, IFormFile file)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;  //TODO find a better way to get id 

            var validation = new Validation();
            validation.UserId = userId;
            validation.EventId = id;
            validation.Time = DateTime.Now;
            validation.Lat = Lat;
            validation.Long = Long;
            string adress = SavePic(userId,id,file);
            validation.Pic = adress;
            
            if (ModelState.IsValid)
            {
                _context.Add(validation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EventValidations), new { id = id });
            }
            return RedirectToAction(nameof(Index));  //TODO add an error
        }
        public string SavePic(string userId, int eventId, IFormFile file)
        {
           
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                string fileName = userId + "-" + eventId + ".jpeg";
                string destFile = @"ValidationPics/" + fileName;
                System.IO.File.WriteAllBytes(destFile, fileBytes);
                return destFile;
            }
        }
    }
}
