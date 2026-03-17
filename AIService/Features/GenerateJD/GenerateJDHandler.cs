using AIService.Interfaces;
using MediatR;
using Shared.DTOs;

namespace AIService.Features.GenerateJD;

public class GenerateJDHandler
    : IRequestHandler<GenerateJDCommand, ApiResponse<string>>
{
    private readonly IGeminiService _gemini;

    public GenerateJDHandler(IGeminiService gemini)
    {
        _gemini = gemini;
    }

    public async Task<ApiResponse<string>> Handle(
        GenerateJDCommand request, CancellationToken cancellationToken)
    {
        var prompt = $"""
            You are an expert HR professional. Write a complete professional
            job description for the following role.

            Job Title:  {request.JobTitle}
            Company:    {request.Company}
            Location:   {request.Location}
            Job Type:   {request.JobType}

            Include: Overview, Responsibilities, Requirements, Nice to Have, Benefits.
            Keep it professional and engaging.
            """;

        var result = await _gemini.GenerateAsync(prompt, cancellationToken);
        return ApiResponse<string>.Ok(result);
    }
}