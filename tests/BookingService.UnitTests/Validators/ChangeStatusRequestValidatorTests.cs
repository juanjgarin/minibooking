using BookingService.Application.DTOs.Requests;
using BookingService.Application.Validators;
using BookingService.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace BookingService.UnitTests.Validators;

public class ChangeStatusRequestValidatorTests
{
    private readonly ChangeStatusRequestValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Request_Is_Valid()
    {
        // Arrange
        var request = new ChangeStatusRequest
        {
            Id = Guid.NewGuid(),
            Status = BookingStatus.Confirmed,
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Id_Is_Empty()
    {
        // Arrange
        var request = new ChangeStatusRequest
        {
            Id = Guid.Empty,
            Status = BookingStatus.Pending,
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(ChangeStatusRequest.Id));
    }

    [Fact]
    public void Should_Fail_When_Status_Is_Not_Defined_Enum_Value()
    {
        // Arrange
        var request = new ChangeStatusRequest
        {
            Id = Guid.NewGuid(),
            Status = (BookingStatus)99,
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(ChangeStatusRequest.Status));
    }
}
