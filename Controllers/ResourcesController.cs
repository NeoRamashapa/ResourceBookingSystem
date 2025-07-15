using InternalResourceBookingSystem.Models;
using InternalResourceBookingSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InternalResourceBookingSystem.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly IResourceRepository _resourceRepository;

        public ResourcesController(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        // GET: Resources
        public async Task<IActionResult> Index()
        {
            return View(await _resourceRepository.GetAllAsync());
        }

        // GET: Resources/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resource = await _resourceRepository.GetByIdAsync(id.Value);

            if (resource == null)
            {
                return NotFound();
            }

            //Attempt to filter for upcoming bookings
            resource.Bookings = resource.Bookings
                .Where(b => b.StartTime > DateTime.Now)
                .OrderBy(b => b.StartTime)
                .ToList();

            return View(resource);
        }

        // GET: Resources/Create
        public async Task<IActionResult> Create(int? resourceId)
        {
            var resources = await _resourceRepository.GetAllAsync();

            if (resourceId.HasValue)
            {
                ViewData["ResourceId"] = new SelectList(resources, "Id", "Name", resourceId.Value);
            }
            else
            {
                ViewData["ResourceId"] = new SelectList(resources, "Id", "Name");
            }
                return View();
        }

        // POST: Resources/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Location,Capacity,IsAvailable")] Resource resource)
        {
            if (ModelState.IsValid)
            {
                await _resourceRepository.AddAsync(resource);

                return RedirectToAction(nameof(Index));
            }
            return View(resource);
        }

        // GET: Resources/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(await _resourceRepository.GetAllAsync());

        }

        // POST: Resources/Edit/5
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Location,Capacity,IsAvailable")] Resource resource)
        {
            if (id != resource.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _resourceRepository.UpdateAsync(resource);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _resourceRepository.GetByIdAsync(resource.Id);   
                    if (exists == null) 
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

            return View(resource);
        }

        // GET: Resources/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resource = await _resourceRepository.GetByIdAsync(id.Value);
            if (resource == null)
            {
                return NotFound();
            }

            return View(resource);
        }

        // POST: Resources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
            await _resourceRepository.DeleteAsync(id);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
