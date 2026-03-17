using ApplicationService.DTOs;
using ApplicationService.Features.CreateApplication;
using ApplicationService.Features.GetApplicationsByJobId;
using ApplicationService.Features.UpdateApplicationStatus;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApplicationController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApplicationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateApplication(
        [FromBody] CreateApplicationDto dto)
    {
        var command = new CreateApplicationCommand(
            dto.JobId,
            dto.CandidateName,
            dto.CandidateEmail,
            dto.ResumeText,
            dto.CoverLetter
        );

        var result = await _mediator.Send(command);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("job/{jobId}")]
    public async Task<IActionResult> GetApplicationsByJobId(int jobId)
    {
        var result = await _mediator
            .Send(new GetApplicationsByJobIdQuery(jobId));
        return Ok(result);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id, [FromBody] UpdateApplicationStatusDto dto)
    {
        var result = await _mediator
            .Send(new UpdateApplicationStatusCommand(id, dto.Status));
        return result.Success ? Ok(result) : BadRequest(result);
    }
}