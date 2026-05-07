using MediatR;
using SmartCity.Application.Features.Issues.DTOs;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Issues.Queries.GetIssueById
{
    public class GetIssueByIdQueryHandler
        : IRequestHandler<GetIssueByIdQuery, AdminIssueDetailsDto>
    {
        private readonly IIssueRepository _issueRepository;

        public GetIssueByIdQueryHandler(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        public async Task<AdminIssueDetailsDto> Handle(
            GetIssueByIdQuery request,
            CancellationToken cancellationToken)
        {
            var issue = await _issueRepository.GetByIdWithDetailsAsync(request.Id);

            if (issue == null)
                throw new Exception("Issue not found");

            var assignment = issue.Assignments?
                .LastOrDefault();

            return new AdminIssueDetailsDto
            {
                Id = issue.Id,
                Description = issue.Description,
                Type = issue.Type.ToString(),
                Status = issue.Status.ToString(),

                // ✅ CORRECT MAPPING
                BeforeImagePath = issue.ImagePath,
                AfterImagePath = issue.ResolvedImagePath,

                AssignedWorkerName = assignment?.Worker?.User?.Name,

                Salary = assignment?.Salary ?? 0,
                Deadline = assignment?.Deadline,

                RejectionReason = issue.RejectionReason
            };
        }
    }
}