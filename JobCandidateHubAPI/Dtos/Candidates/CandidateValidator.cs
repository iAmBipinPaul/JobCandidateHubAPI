using FluentValidation;

namespace JobCandidateHubAPI.Dtos.Candidates
{
    public class CandidateValidator:AbstractValidator<CreateOrUpdateCandidateRequestInput>
    {
        public CandidateValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(2, 50);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(2, 50);

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[1-9]\d{1,14}$") // E.164 international phone number format
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            RuleFor(x => x.LinkedInProfileUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.LinkedInProfileUrl))
                .WithMessage("Invalid LinkedIn profile URL");

            RuleFor(x => x.GitHubProfileUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.GitHubProfileUrl))
                .WithMessage("Invalid GitHub profile URL");
           

            RuleFor(x => x.Comments)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.IntervalStateTime)
                .NotNull()
                .When(x => x.IntervalEndTime.HasValue)
                .WithMessage("Start time is required when end time is provided");

            RuleFor(x => x.IntervalEndTime)
                .NotNull()
                .When(x => x.IntervalStateTime.HasValue)
                .WithMessage("End time is required when start time is provided");

            RuleFor(x => x)
                .Must(x => !x.IntervalStateTime.HasValue || !x.IntervalEndTime.HasValue || 
                           x.IntervalStateTime.Value < x.IntervalEndTime.Value)
                .When(x => x.IntervalStateTime.HasValue && x.IntervalEndTime.HasValue)
                .WithMessage("End time must be later than start time");


        }
    }
}
