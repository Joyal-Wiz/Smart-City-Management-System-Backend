namespace SmartCity.Application.DTOs.Sla
{
    public class OverdueIssueDto
    {
        public Guid IssueId { get; set; }
        public string Title { get; set; }
        public Guid WorkerId { get; set; }
        public string WorkerName { get; set; }
        public DateTime Deadline { get; set; }
        public int EscalationLevel { get; set; }
        public double OverdueMinutes { get; set; }
        public string Status { get; set; }
    }
}