using AIService.DTOs;
using AIService.Features.GenerateJD;
using AIService.Features.RankCandidates;
using AIService.Features.ResumeScreening;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AIService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AIController : ControllerBase
{
    private readonly IMediator _mediator;

    public AIController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("screen-resume")]
    public async Task<IActionResult> ScreenResume([FromBody] ScreenResumeDto dto)
    {
        var command = new ScreenResumeCommand(
            dto.ApplicationId,
            dto.JobId,
            dto.ResumeText,
            dto.JobDescription
        );
        var result = await _mediator.Send(command);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("generate-jd")]
    public async Task<IActionResult> GenerateJD([FromBody] GenerateJDDto dto)
    {
        var command = new GenerateJDCommand(
            dto.JobTitle,
            dto.Company,
            dto.Location,
            dto.JobType
        );
        var result = await _mediator.Send(command);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("rank-candidates")]
    public async Task<IActionResult> RankCandidates(
        [FromBody] RankCandidatesCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}