using AIService.DTOs;
using AIService.Interfaces;
using AIService.Models;
using MediatR;
using Shared.DTOs;
using System.Text.Json;

namespace AIService.Features.ResumeScreening;

public class ScreenResumeHandler
    : IRequestHandler<ScreenResumeCommand, ApiResponse<ScreenResumeResult>>
{
    private readonly IGeminiService _gemini;
    private readonly IAIRepository _repository;

    public ScreenResumeHandler(IGeminiService gemini, IAIRepository repository)
    {
        _gemini = gemini;
        _repository = repository;
    }

    public async Task<ApiResponse<ScreenResumeResult>> Handle(
        ScreenResumeCommand request, CancellationToken cancellationToken)
    {
        var jsonFormat = """
            {
              "matchScore": <number 0-100>,
              "strengths": ["strength1", "strength2", "strength3"],
              "gaps": ["gap1", "gap2"],
              "recommendation": "<Shortlist or Reject or Maybe>"
            }
            """;

        var prompt = $"""
            You are an expert HR recruiter. Analyze this resume against the job description.
            Respond in pure JSON only — no markdown, no explanation, no code blocks.

            JOB DESCRIPTION:
            {request.JobDescription}

            RESUME:
            {request.ResumeText}

            Respond ONLY with this exact JSON format:
            {jsonFormat}
            """;

        var aiResponse = await _gemini.GenerateAsync(prompt, cancellationToken);

        var cleanJson = aiResponse
            .Replace("```json", "")
            .Replace("```", "")
            .Trim();

        var result = JsonSerializer.Deserialize<ScreenResumeResult>(
            cleanJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        )!;

        await _repository.SaveAIResultAsync(new AIResult
        {
            ApplicationId = request.ApplicationId,
            JobId = request.JobId,
            MatchScore = result.MatchScore,
            Strengths = string.Join(", ", result.Strengths),
            Gaps = string.Join(", ", result.Gaps),
            Recommendation = result.Recommendation
        });

        return ApiResponse<ScreenResumeResult>.Ok(result);
    }
}