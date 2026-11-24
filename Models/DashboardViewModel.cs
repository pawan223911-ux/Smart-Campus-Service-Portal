namespace SmartCampusServicePortal.Models
{
    public class DashboardViewModel
    {
        public int TotalRequests { get; set; }

        public int PendingRequests { get; set; }

        public int InProgressRequests { get; set; }

        public int CompletedRequests { get; set; }

        public List<ServiceRequest> RecentRequests { get; set; }
            = new List<ServiceRequest>();
    }
}
