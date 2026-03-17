using MediatR;
using Shared.DTOs;

namespace AIService.Features.RankCandidates;

public record CandidateInfo(
    int ApplicationId,
    string CandidateName,
    string ResumeText
);

public record RankCandidatesCommand(
    string JobDescription,
    List<CandidateInfo> Candidates
) : IRequest<ApiResponse<string>>;