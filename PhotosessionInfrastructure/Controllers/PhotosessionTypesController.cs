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
    public class PhotosessionTypesController : Controller
    {
        private readonly DbphotoSessionContext _context;

        public PhotosessionTypesController(DbphotoSessionContext context)
        {
            _context = context;
        }

        // GET: PhotosessionTypes
        public async Task<IActionResult> Index()
        {

            return View(await _context.PhotosessionTypes.ToListAsync());
        }

        // GET: PhotosessionTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photosessionType = await _context.PhotosessionTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photosessionType == null)
            {
                return NotFound();
            }

            //return View(photosessionType);
            return RedirectToAction("Index", "Photosessions", new { id = photosessionType.Id, name = photosessionType.TypeName, aaa = 1 });
        }

        // GET: PhotosessionTypes/Create
        public IActionResult Create()
        {            
          return View();
        }

        // POST: PhotosessionTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeName,Id")] PhotosessionType photosessionType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(photosessionType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(photosessionType);
        }

        // GET: PhotosessionTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photosessionType = await _context.PhotosessionTypes.FindAsync(id);
            if (photosessionType == null)
            {
                return NotFound();
            }
            return View(photosessionType);
        }

        // POST: PhotosessionTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeName,Id")] PhotosessionType photosessionType)
        {
            if (id != photosessionType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photosessionType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotosessionTypeExists(photosessionType.Id))
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
            return View(photosessionType);
        }

        // GET: PhotosessionTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photosessionType = await _context.PhotosessionTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photosessionType == null)
            {
                return NotFound();
            }

            return View(photosessionType);
        }

        // POST: PhotosessionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photosessionType = await _context.PhotosessionTypes.FindAsync(id);
            if (photosessionType != null)
            {
                _context.PhotosessionTypes.Remove(photosessionType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotosessionTypeExists(int id)
        {
            return _context.PhotosessionTypes.Any(e => e.Id == id);
        }
    }
}
