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
    public class PhotosessionStatusController : Controller
    {
        private readonly DbphotoSessionContext _context;

        public PhotosessionStatusController(DbphotoSessionContext context)
        {
            _context = context;
        }

        // GET: PhotosessionStatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.PhotosessionStatuses.ToListAsync());
        }

        // GET: PhotosessionStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photosessionStatus = await _context.PhotosessionStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photosessionStatus == null)
            {
                return NotFound();
            }

            //return View(photosessionStatus);
            return RedirectToAction("Index", "Photosessions", new { id = photosessionStatus.Id, name = photosessionStatus.StatusName, aaa = 3 });
        }

        // GET: PhotosessionStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PhotosessionStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StatusName, Id")] PhotosessionStatus photosessionStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(photosessionStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(photosessionStatus);
        }

        // GET: PhotosessionStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photosessionStatus = await _context.PhotosessionStatuses.FindAsync(id);
            if (photosessionStatus == null)
            {
                return NotFound();
            }
            return View(photosessionStatus);
        }

        // POST: PhotosessionStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StatusName, Id")] PhotosessionStatus photosessionStatus)
        {
            if (id != photosessionStatus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photosessionStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotosessionStatusExists(photosessionStatus.Id))
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
            return View(photosessionStatus);
        }

        // GET: PhotosessionStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photosessionStatus = await _context.PhotosessionStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photosessionStatus == null)
            {
                return NotFound();
            }

            return View(photosessionStatus);
        }

        // POST: PhotosessionStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photosessionStatus = await _context.PhotosessionStatuses.FindAsync(id);
            if (photosessionStatus != null)
            {
                _context.PhotosessionStatuses.Remove(photosessionStatus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotosessionStatusExists(int id)
        {
            return _context.PhotosessionStatuses.Any(e => e.Id == id);
        }
    }
}
