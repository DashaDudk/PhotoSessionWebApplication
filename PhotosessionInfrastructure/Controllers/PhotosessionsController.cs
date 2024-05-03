using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using PhotosessionDomain.Model;
using PhotosessionInfrastructure;

namespace PhotosessionInfrastructure.Controllers
{
    public class PhotosessionsController : Controller
    {
        private readonly DbphotoSessionContext _context;

        public PhotosessionsController(DbphotoSessionContext context)
        {
            _context = context;
        }

        // GET: Photosessions
         public async Task<IActionResult> Index(int? id, string? name, int? aaa)
         {
             if (id == null) return RedirectToAction("Index", "PhotosessionTypes");

            List<Photosession> photosessionByPhotosessionType;

             //знаходження фотосесій за типом
             if (aaa == 1)
             {
                ViewBag.bbb = "Type";
                ViewBag.PhotosessionTypeId = id;
                ViewBag.PhotosessionTypeName = name;
                photosessionByPhotosessionType = await _context.Photosessions.Where(p => p.PhotosessionTypeId == id).Include(p => p.PhotosessionType).Include(p => p.PhotosessionStatus).Include(p => p.PhotosessionLocation).ToListAsync();
             }
             else if (aaa == 2)
             {
                ViewBag.bbb = "Location";
                ViewBag.PhotosessionLocationId = id;
                ViewBag.PhotosessionTypeName = name;
                photosessionByPhotosessionType = await _context.Photosessions.Where(p => p.PhotosessionLocationId == id).Include(p => p.PhotosessionType).Include(p => p.PhotosessionStatus).Include(p => p.PhotosessionLocation).ToListAsync();
             }
             else //if (aaa == 3 )
             {
                ViewBag.bbb = "Status";
                ViewBag.PhotosessionStatusId = id;
                ViewBag.PhotosessionTypeName = name;
                photosessionByPhotosessionType = await _context.Photosessions.Where(p => p.PhotosessionStatusId == id).Include(p => p.PhotosessionType).Include(p => p.PhotosessionStatus).Include(p => p.PhotosessionLocation).ToListAsync();
             }
          
             return View(photosessionByPhotosessionType);
         }
        

        // GET: Photosessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photosession = await _context.Photosessions
                .Include(a => a.PhotosessionLocation)
                .Include(b => b.PhotosessionStatus)
                .Include(p => p.PhotosessionType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photosession == null)
            {
                return NotFound();
            }

            return View(photosession);
        }

        // GET: Photosessions/Create
        public IActionResult Create(int photosessionTypeId, int photosessionStatusId, int photosessionLocationId)
        {

            ViewData["PhotosessionLocationId"] = new SelectList(_context.PhotosessionLocations, "Id", "CityName");
            ViewData["PhotosessionStatusId"] = new SelectList(_context.PhotosessionStatuses, "Id", "StatusName");
            ViewData["PhotosessionTypeId"] = new SelectList(_context.PhotosessionTypes, "Id", "TypeName");
            return View();


        }


        // POST: Photosessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Route("Photosessions/Create")]
        public async Task<IActionResult> Create(int photosessionTypeId, int photosessionStatusId, int photosessionLocationId, [Bind("Id,Price,DateTime,PhotosessionLocationId,Description,PhotosessionTypeId,PhotosessionStatusId")] Photosession photosession)
        {
            PhotosessionType photosessionType = _context.PhotosessionTypes.Include(p => p.Photosessions).FirstOrDefault(p => p.Id == photosession.PhotosessionTypeId);
            photosession.PhotosessionType = photosessionType;

            PhotosessionStatus photosessionStatus = _context.PhotosessionStatuses.Include(b => b.Photosessions).FirstOrDefault(b => b.Id == photosession.PhotosessionStatusId);
            photosession.PhotosessionStatus = photosessionStatus;

            PhotosessionLocation photosessionLocation = _context.PhotosessionLocations.Include(a => a.Photosessions).FirstOrDefault(a => a.Id == photosession.PhotosessionLocationId);
            photosession.PhotosessionLocation = photosessionLocation;

            ModelState.Clear();
            TryValidateModel(photosession);

          //  if (ModelState.IsValid)
            {
                _context.Add(photosession);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Photosessions", new { id = photosessionTypeId, name = ViewBag.PhotosessionTypeName });
                //return RedirectToAction("Index", "Photosessions", new { id = photosessionTypeId });              
            }
            ViewData["PhotosessionLocationId"] = new SelectList(_context.PhotosessionLocations, "Id", "CityName", photosession.PhotosessionLocationId);
            ViewData["PhotosessionStatusId"] = new SelectList(_context.PhotosessionStatuses, "Id", "StatusName", photosession.PhotosessionStatusId);
            ViewData["PhotosessionTypeId"] = new SelectList(_context.PhotosessionTypes, "Id", "TypeName", photosession.PhotosessionTypeId);
            return View(photosession);
        }

        // GET: Photosessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photosession = await _context.Photosessions.FindAsync(id);
            if (photosession == null)
            {
                return NotFound();
            }
            ViewData["PhotosessionLocationId"] = new SelectList(_context.PhotosessionLocations, "Id", "CityName", photosession.PhotosessionLocationId);
            ViewData["PhotosessionStatusId"] = new SelectList(_context.PhotosessionStatuses, "Id", "StatusName", photosession.PhotosessionStatusId);
            ViewData["PhotosessionTypeId"] = new SelectList(_context.PhotosessionTypes, "Id", "TypeName", photosession.PhotosessionTypeId);
            return View(photosession);
        }

        // POST: Photosessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int photosessionTypeId, [Bind("Id,Price,DateTime,PhotosessionLocationId,Description,PhotosessionTypeId,PhotosessionStatusId")] Photosession photosession)
        {
            if (id != photosession.Id)
            {
                return NotFound();
            }

            PhotosessionType photosessionType = _context.PhotosessionTypes.Include(p => p.Photosessions).FirstOrDefault(p => p.Id == photosession.PhotosessionTypeId);
            photosession.PhotosessionType = photosessionType;

            PhotosessionStatus photosessionStatus = _context.PhotosessionStatuses.Include(b => b.Photosessions).FirstOrDefault(b => b.Id == photosession.PhotosessionStatusId);
            photosession.PhotosessionStatus = photosessionStatus;

            PhotosessionLocation photosessionLocation = _context.PhotosessionLocations.Include(a => a.Photosessions).FirstOrDefault(a => a.Id == photosession.PhotosessionLocationId);
            photosession.PhotosessionLocation = photosessionLocation;

            ModelState.Clear();
            TryValidateModel(photosession);

           // if (ModelState.IsValid)
            {
                try
                {
                    var local = _context.Set<Photosession>()
                    .Local
                    .FirstOrDefault(entry => entry.Id.Equals(id));

                    // check if local is not null 
                    if (local != null)
                    {
                        // detach
                        _context.Entry(local).State = EntityState.Detached;
                    }
                    // set Modified flag in your entry
                    _context.Entry(photosession).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotosessionExists(photosession.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Photosessions", new { id = photosessionTypeId, aaa = 1 });
                //return RedirectToAction("Index", "Photosessions", new { id = photosessionTypeId });
                //return RedirectToAction(nameof(Index));
            }
            ViewData["PhotosessionLocationId"] = new SelectList(_context.PhotosessionLocations, "Id", "CityName", photosession.PhotosessionLocationId);
            ViewData["PhotosessionStatusId"] = new SelectList(_context.PhotosessionStatuses, "Id", "StatusName", photosession.PhotosessionStatusId);
            ViewData["PhotosessionTypeId"] = new SelectList(_context.PhotosessionTypes, "Id", "TypeName", photosession.PhotosessionTypeId);
            return View(photosession);
        }

        // GET: Photosessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photosession = await _context.Photosessions
                .Include(a => a.PhotosessionLocation)
                .Include(b => b.PhotosessionStatus)
                .Include(p => p.PhotosessionType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photosession == null)
            {
                return NotFound();
            }

            return View(photosession);
        }

        // POST: Photosessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photosession = await _context.Photosessions.FindAsync(id);
            if (photosession != null)
            {
                _context.Photosessions.Remove(photosession);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PhotosessionsController.Index), "Photosessions");
        }

        private bool PhotosessionExists(int id)
        {
            return _context.Photosessions.Any(e => e.Id == id);
        }
    }
}