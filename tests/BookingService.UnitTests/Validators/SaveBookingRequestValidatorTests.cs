using BookingService.Application.DTOs.Requests;
using BookingService.Application.Validators;
using BookingService.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace BookingService.UnitTests.Validators;

public class SaveBookingRequestValidatorTests
{
    private readonly SaveBookingRequestValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Request_Is_Valid()
    {
        // Arrange
        var request = new SaveBookingRequest
        {
            CustomerId = Guid.NewGuid(),
            SpaceName = "Desk 1",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddHours(1),
            Status = BookingStatus.Pending,
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_EndDate_Is_Not_After_StartDate()
    {
        // Arrange
        var start = DateTime.UtcNow;
        var request = new SaveBookingRequest
        {
            CustomerId = Guid.NewGuid(),
            SpaceName = "Desk",
            StartDate = start,
            EndDate = start,
            Status = BookingStatus.Pending,
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(SaveBookingRequest.EndDate));
    }

    [Fact]
    public void Should_Fail_When_CustomerId_Is_Empty()
    {
        // Arrange
        var request = new SaveBookingRequest
        {
            CustomerId = Guid.Empty,
            SpaceName = "Desk",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddHours(1),
            Status = BookingStatus.Pending,
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(SaveBookingRequest.CustomerId));
    }
}
