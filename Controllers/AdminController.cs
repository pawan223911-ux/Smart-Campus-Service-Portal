using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCampusServicePortal.Data;

namespace SmartCampusServicePortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? status)
        {
            var requests = _context.ServiceRequests.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                requests = requests.Where(r => r.Status == status);
            }

            return View(await requests
                .OrderByDescending(r => r.SubmissionDate)
                .ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(
            int id, string status, string? remarks)
        {
            var request = await _context.ServiceRequests.FindAsync(id);

            if (request == null)
                return NotFound();

            if (status != "Pending" &&
                status != "In Progress" &&
                status != "Completed")
            {
                return BadRequest();
            }

            request.Status = status;
            request.AdminRemarks = remarks;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Status updated successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
