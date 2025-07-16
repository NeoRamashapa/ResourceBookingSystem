using InternalResourceBookingSystem.Models;
using InternalResourceBookingSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace InternalResourceBookingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IResourceRepository _resourceRepository;
        private readonly IBookingRepository _bookingRepository;
        public HomeController(ILogger<HomeController> logger,
            IBookingRepository bookingRepository,
            IResourceRepository resourceRepository)
        {
            _logger = logger;
            _bookingRepository = bookingRepository;
            _resourceRepository = resourceRepository;
        }

        public async Task<IActionResult> Index()
        {
            var allResources =  await _resourceRepository.GetAllAsync();
            var allBookings = await _bookingRepository.GetAllAsync();

            var today = DateTime.Today;
            var now = DateTime.Now;

            var todayBookings = allBookings
                .Where (b => b.StartTime.HasValue && b.StartTime.Value.Date == today)
                .ToList();
            
            var confictsToday = todayBookings
                .GroupBy(b => b.ResourceId)
                .Count(g => g.Count() > 1);

            var activeBookings = allBookings 
                .Where (b => b.StartTime <= DateTime.Now && b.EndTime >= now)
                .ToList();

            // Count current meetings happening today (currently ongoing)
            var currentMeetings = allBookings
                .Where(b => b.StartTime <= now && b.EndTime >= now && b.StartTime.Value.Date == today)
                .Count();

            // Count upcoming meetings (starting later today or in the future)
            var upcomingMeetings = allBookings
                .Where(b => b.StartTime > now)
                .Count();

            // Rooms not booked at the current moment
            var availableRooms = allResources.Where(r =>
                !activeBookings.Any(b => b.ResourceId == r.Id)).ToList();

            var activeBookingsToday = await _bookingRepository.GetTodayBookingsCountAsync();

            ViewData["ActiveBookingsToday"] = currentMeetings;
            ViewData["RoomCount"] = allResources.Count;
            ViewData["BookingCount"] = allBookings.Count;
            ViewData["UpcomingBookings"] = upcomingMeetings;
            ViewData["AvailableRooms"] = todayBookings;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
