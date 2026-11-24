using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCampusServicePortal.Data;
using SmartCampusServicePortal.Models;

namespace SmartCampusServicePortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                TotalRequests = await _context.ServiceRequests.CountAsync(),

                PendingRequests = await _context.ServiceRequests
                    .CountAsync(r => r.Status == "Pending"),

                InProgressRequests = await _context.ServiceRequests
                    .CountAsync(r => r.Status == "In Progress"),

                CompletedRequests = await _context.ServiceRequests
                    .CountAsync(r => r.Status == "Completed"),

                RecentRequests = await _context.ServiceRequests
                    .OrderByDescending(r => r.SubmissionDate)
                    .Take(5)
                    .ToListAsync()
            };

            return View(model);
        }
    }
}
