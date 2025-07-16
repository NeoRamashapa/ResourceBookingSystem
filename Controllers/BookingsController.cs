using InternalResourceBookingSystem.Data;
using InternalResourceBookingSystem.Models;
using InternalResourceBookingSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static System.Collections.Specialized.BitVector32;

namespace InternalResourceBookingSystem.Controllers
{
    public class BookingsController : Controller
    {
        private readonly IBookingRepository _bookingRepository;

        private readonly IResourceRepository _resourceRepository;

        public BookingsController(IBookingRepository bookingRepository, IResourceRepository resourceRepository)
        {
            _bookingRepository = bookingRepository;
            _resourceRepository = resourceRepository;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            var resources = await _resourceRepository.GetAllAsync();

            ViewBag.Resources = resources;
            return View(bookings);
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _bookingRepository.GetByIdAsync(id.Value);
            if (booking == null)
            {
                return NotFound();
            }

            var resource = await _resourceRepository.GetByIdIncludeBookingAsync(booking.ResourceId.Value);
            ViewData["ResourceName"] = resource.Name;

            return View(booking);
        }

        // GET: Bookings/Create
        public async Task<IActionResult> Create()
        {
            var resources = await _resourceRepository.GetAllAsync();
            
                ViewData["ResourceId"] = new SelectList(resources, "Id", "Name");
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ResourceId,StartTime,EndTime,BookedBy,Purpose")] Booking booking)
        {
            if(booking.EndTime <= booking.StartTime)
            {
                ModelState.AddModelError("", "End time must be after start time.");
            }

            if (!await IsValidBooking(booking))
            {
                var resources = await _resourceRepository.GetAllAsync();
                ViewData["ResourceId"] = new SelectList(resources, "Id", "Name", booking.ResourceId);
                return View(booking);
            }

            await _bookingRepository.AddAsync(booking);
            return RedirectToAction(nameof(Index));
        }
        private async Task<bool> IsValidBooking(Booking booking)
        {
            if(booking.StartTime < DateTime.Now)
            {
                ModelState.AddModelError("", "Start time must be in the future.");
                return false;
            }

            if (await _bookingRepository.HasRoomConflict(booking))
            {
                ModelState.AddModelError("",
                    "This Meeting room is already booked during the requested time. " +
                    "Please choose another slot or meeting room, or adjust your times.");
                return false;
            }

            if (await _bookingRepository.HasUserConflict(booking))
            {
                ModelState.AddModelError("",
                    "You already have another booking during this time.");
                return false;
            }

            return true;
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _bookingRepository.GetByIdAsync(id.Value);
            if (booking == null)
            {
                return NotFound();
            }

            var resources = await _resourceRepository.GetAllAsync();
            ViewData["ResourceId"] = new SelectList(resources, "Id", "Name", booking.ResourceId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ResourceId,StartTime,EndTime,BookedBy,Purpose")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if(booking.EndTime <= booking.StartTime)
            {
                ModelState.AddModelError("", "End time must be after start time.");
            }

            if (!await IsValidBooking(booking))
            {
                var resources = await _resourceRepository.GetAllAsync();
                ViewData["ResourceId"] = new SelectList(resources, "Id", "Name", booking.ResourceId);
                return View(booking);
            }

            await _bookingRepository.UpdateAsync(booking);
            return RedirectToAction(nameof(Index));
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _bookingRepository.GetByIdAsync(id.Value);
               
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _bookingRepository.DeleteAsync(id);
                TempData["ShowDeleteToast"] = true;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while deleting the booking.");
            }
            return RedirectToAction(nameof(Index));
        }

    }
}