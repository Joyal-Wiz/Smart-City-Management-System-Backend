using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.DTOs.Sla;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Enums;

namespace SmartCity.Application.Features.Sla.Queries.GetOverdueIssues
{
    public class GetOverdueIssuesHandler : IRequestHandler<GetOverdueIssuesQuery, List<OverdueIssueDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetOverdueIssuesHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OverdueIssueDto>> Handle(GetOverdueIssuesQuery request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var data = await _context.IssueAssignments
                .Include(a => a.Issue)
                .Include(a => a.Worker)
                    .ThenInclude(w => w.User) 
                .Where(a => a.IsOverdue && a.Issue.Status != IssueStatus.Resolved)
                .Select(a => new OverdueIssueDto
                {
                    IssueId = a.IssueId,
                    Title = a.Issue.Description,
                    WorkerId = a.WorkerId,
                    WorkerName = a.Worker.User.Name,
                    Deadline = a.Deadline,
                    EscalationLevel = a.EscalationLevel,
                    OverdueMinutes = (now - a.Deadline).TotalMinutes,
                    Status = a.Issue.Status.ToString()
                })
                .ToListAsync(cancellationToken);

            return data;
        }
    }
}