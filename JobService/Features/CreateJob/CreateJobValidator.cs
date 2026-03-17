using FluentValidation;

namespace JobService.Features.CreateJob;

public class CreateJobValidator : AbstractValidator<CreateJobCommand>
{
    public CreateJobValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.Company)
            .NotEmpty().WithMessage("Company is required.")
            .MaximumLength(200).WithMessage("Company cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.");

        RuleFor(x => x.JobType)
            .NotEmpty().WithMessage("JobType is required.")
            .Must(x => new[] { "FullTime", "PartTime", "Remote" }.Contains(x))
            .WithMessage("JobType must be FullTime, PartTime or Remote.");
    }
}