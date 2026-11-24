using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCampusServicePortal.Data;
using SmartCampusServicePortal.Models;

namespace SmartCampusServicePortal.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _context.ServiceRequests
                .OrderByDescending(r => r.SubmissionDate)
                .ToListAsync();

            return View(requests);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest request)
        {
            if (ModelState.IsValid)
            {
                request.Status = "Pending";
                request.SubmissionDate = DateTime.Now;

                _context.ServiceRequests.Add(request);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    "Service request submitted successfully!";

                return RedirectToAction(nameof(Index));
            }

            return View(request);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var request = await _context.ServiceRequests
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
                return NotFound();

            return View(request);
        }

        public IActionResult Track()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Track(int id)
        {
            var request = await _context.ServiceRequests
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
            {
                ViewBag.ErrorMessage = "Request not found.";
                return View();
            }

            return View("Details", request);
        }
    }
}
