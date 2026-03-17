using FluentValidation;

namespace ApplicationService.Features.CreateApplication;

public class CreateApplicationValidator
    : AbstractValidator<CreateApplicationCommand>
{
    public CreateApplicationValidator()
    {
        RuleFor(x => x.JobId)
            .GreaterThan(0).WithMessage("JobId is required.");

        RuleFor(x => x.CandidateName)
            .NotEmpty().WithMessage("Candidate name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

        RuleFor(x => x.CandidateEmail)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.ResumeText)
            .NotEmpty().WithMessage("Resume text is required.");
    }
}