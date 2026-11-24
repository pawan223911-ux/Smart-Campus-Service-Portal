using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCampusServicePortal.Data;
using SmartCampusServicePortal.Models;

namespace SmartCampusServicePortal.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .OrderByDescending(s => s.RegisteredDate)
                .ToListAsync();

            return View(students);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Student student)
        {
            if (ModelState.IsValid)
            {
                bool exists = await _context.Students.AnyAsync(
                    s => s.StudentIdNumber == student.StudentIdNumber);

                if (exists)
                {
                    ModelState.AddModelError(
                        "StudentIdNumber",
                        "Student ID already exists.");

                    return View(student);
                }

                student.RegisteredDate = DateTime.Now;

                _context.Students.Add(student);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    "Student registered successfully!";

                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                return NotFound();

            ViewBag.Requests = await _context.ServiceRequests
                .Where(r => r.StudentRollNo == student.StudentIdNumber)
                .OrderByDescending(r => r.SubmissionDate)
                .ToListAsync();

            return View(student);
        }
    }
}
