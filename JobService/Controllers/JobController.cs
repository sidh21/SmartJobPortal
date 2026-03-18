using JobService.DTOs;
using JobService.Features.CreateJob;
using JobService.Features.DeleteJob;
using JobService.Features.GetAllJobs;
using JobService.Features.GetJobById;
using JobService.Features.UpdateJob;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace JobService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ✅ Public endpoints - No authentication required
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllJobs()
    {
        var result = await _mediator.Send(new GetAllJobsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetJobById(int id)
    {
        var result = await _mediator.Send(new GetJobByIdQuery(id));
        return result.Success ? Ok(result) : NotFound(result);
    }

    // ✅ Protected endpoints - Authentication required
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobDto dto)
    {
        var command = new CreateJobCommand(
            dto.Title,
            dto.Company,
            dto.Description,
            dto.Location,
            dto.Salary,
            dto.JobType
        );
        var result = await _mediator.Send(command);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateJob(int id, [FromBody] UpdateJobDto dto)
    {
        var command = new UpdateJobCommand(
            id,
            dto.Title,
            dto.Company,
            dto.Description,
            dto.Location,
            dto.Salary,
            dto.JobType
        );
        var result = await _mediator.Send(command);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteJob(int id)
    {
        var result = await _mediator.Send(new DeleteJobCommand(id));
        return result.Success ? Ok(result) : NotFound(result);
    }
}