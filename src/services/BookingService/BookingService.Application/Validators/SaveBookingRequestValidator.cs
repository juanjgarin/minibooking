using BookingService.Application.DTOs.Requests;
using FluentValidation;

namespace BookingService.Application.Validators;

public class SaveBookingRequestValidator : AbstractValidator<SaveBookingRequest>
{
    public SaveBookingRequestValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty();

        RuleFor(x => x.SpaceName)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("EndDate must be after StartDate.");

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}
