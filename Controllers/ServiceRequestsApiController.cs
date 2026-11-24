using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCampusServicePortal.Data;
using SmartCampusServicePortal.Models;

namespace SmartCampusServicePortal.Controllers
{
    [Route("api/servicerequests")]
    [ApiController]
    public class ServiceRequestsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServiceRequestsApiController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceRequest>>>
            GetAllRequests()
        {
            var requests = await _context.ServiceRequests
                .OrderByDescending(r => r.SubmissionDate)
                .ToListAsync();

            return Ok(requests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceRequest>>
            GetRequestById(int id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);

            if (request == null)
                return NotFound();

            return Ok(request);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceRequest>>
            CreateRequest(ServiceRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            request.SubmissionDate = DateTime.Now;
            request.Status = "Pending";

            _context.ServiceRequests.Add(request);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetRequestById),
                new { id = request.Id },
                request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRequest(
            int id, ServiceRequest request)
        {
            if (id != request.Id)
                return BadRequest("Request ID mismatch.");

            var existingRequest =
                await _context.ServiceRequests.FindAsync(id);

            if (existingRequest == null)
                return NotFound();

            existingRequest.StudentName = request.StudentName;
            existingRequest.StudentRollNo = request.StudentRollNo;
            existingRequest.Category = request.Category;
            existingRequest.Description = request.Description;
            existingRequest.Status = request.Status;
            existingRequest.AdminRemarks = request.AdminRemarks;

            await _context.SaveChangesAsync();

            return Ok(existingRequest);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id, StatusUpdateDto dto)
        {
            var request = await _context.ServiceRequests.FindAsync(id);

            if (request == null)
                return NotFound();

            request.Status = dto.Status;

            if (dto.AdminRemarks != null)
                request.AdminRemarks = dto.AdminRemarks;

            await _context.SaveChangesAsync();

            return Ok(request);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);

            if (request == null)
                return NotFound();

            _context.ServiceRequests.Remove(request);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Request deleted successfully."
            });
        }
    }

    public class StatusUpdateDto
    {
        public string Status { get; set; } = "Pending";

        public string? AdminRemarks { get; set; }
    }
}
