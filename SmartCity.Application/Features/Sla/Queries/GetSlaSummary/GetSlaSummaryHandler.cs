using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.DTOs.Sla;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Enums;

namespace SmartCity.Application.Features.Sla.Queries.GetSlaSummary
{
    public class GetSlaSummaryHandler : IRequestHandler<GetSlaSummaryQuery, SlaSummaryDto>
    {
        private readonly IApplicationDbContext _context;

        public GetSlaSummaryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SlaSummaryDto> Handle(GetSlaSummaryQuery request, CancellationToken cancellationToken)
        {
            var total = await _context.IssueAssignments.CountAsync(cancellationToken);

            var overdue = await _context.IssueAssignments
                .CountAsync(a => a.IsOverdue && a.Issue.Status != IssueStatus.Resolved, cancellationToken);

            var onTime = await _context.IssueAssignments
                .CountAsync(a => !a.IsOverdue && a.Issue.Status == IssueStatus.Resolved, cancellationToken);

            var critical = await _context.IssueAssignments
                .CountAsync(a => a.IsOverdue && a.EscalationLevel >= 2 && a.Issue.Status != IssueStatus.Resolved, cancellationToken);

            return new SlaSummaryDto
            {
                Total = total,
                OnTime = onTime,
                Overdue = overdue,
                Critical = critical
            };
        }
    }
}