using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhotosessionDomain.Model;
using PhotosessionInfrastructure;

namespace PhotosessionInfrastructure.Controllers
{
    public class PhotosessionLocationsController : Controller
    {
        private readonly DbphotoSessionContext _context;

        public PhotosessionLocationsController(DbphotoSessionContext context)
        {
            _context = context;
        }

        // GET: PhotosessionLocations
        public async Task<IActionResult> Index()
        {
            return View(await _context.PhotosessionLocations.ToListAsync());
        }

        // GET: PhotosessionLocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photosessionLocation = await _context.PhotosessionLocations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photosessionLocation == null)
            {
                return NotFound();
            }

            //return View(photosessionLocation);
            return RedirectToAction("Index", "Photosessions", new { id = photosessionLocation.Id, name = photosessionLocation.CityName, aaa = 2 });
        }

        // GET: PhotosessionLocations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PhotosessionLocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CityName, Id")] PhotosessionLocation photosessionLocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(photosessionLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(photosessionLocation);
        }

        // GET: PhotosessionLocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photosessionLocation = await _context.PhotosessionLocations.FindAsync(id);
            if (photosessionLocation == null)
            {
                return NotFound();
            }
            return View(photosessionLocation);
        }

        // POST: PhotosessionLocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CityName, Id")] PhotosessionLocation photosessionLocation)
        {
            if (id != photosessionLocation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photosessionLocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotosessionLocationExists(photosessionLocation.Id))
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
            return View(photosessionLocation);
        }

        // GET: PhotosessionLocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photosessionLocation = await _context.PhotosessionLocations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photosessionLocation == null)
            {
                return NotFound();
            }

            return View(photosessionLocation);
        }

        // POST: PhotosessionLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photosessionLocation = await _context.PhotosessionLocations.FindAsync(id);
            if (photosessionLocation != null)
            {
                _context.PhotosessionLocations.Remove(photosessionLocation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotosessionLocationExists(int id)
        {
            return _context.PhotosessionLocations.Any(e => e.Id == id);
        }
    }
}
