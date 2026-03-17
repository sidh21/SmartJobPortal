using AIService.Interfaces;
using MediatR;
using Shared.DTOs;

namespace AIService.Features.RankCandidates;

public class RankCandidatesHandler
    : IRequestHandler<RankCandidatesCommand, ApiResponse<string>>
{
    private readonly IGeminiService _gemini;

    public RankCandidatesHandler(IGeminiService gemini)
    {
        _gemini = gemini;
    }

    public async Task<ApiResponse<string>> Handle(
        RankCandidatesCommand request, CancellationToken cancellationToken)
    {
        var candidatesList = string.Join("\n\n", request.Candidates.Select(c =>
            $"ApplicationId: {c.ApplicationId}\nName: {c.CandidateName}\nResume: {c.ResumeText}"
        ));

        var prompt = $"""
            You are an expert HR recruiter. Rank these candidates for the job below.

            JOB DESCRIPTION:
            {request.JobDescription}

            CANDIDATES:
            {candidatesList}

            Rank them from best to worst fit.
            For each candidate give: Rank, Name, ApplicationId, Score (0-100), Reason.
            Be concise and professional.
            """;

        var result = await _gemini.GenerateAsync(prompt, cancellationToken);
        return ApiResponse<string>.Ok(result);
    }
}