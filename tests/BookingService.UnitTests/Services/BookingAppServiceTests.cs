using AutoMapper;
using BookingService.Application.DTOs.Requests;
using BookingService.Application.Exceptions;
using BookingService.Application.Interfaces;
using BookingService.Application.Mappings;
using BookingService.Application.Services;
using BookingService.Domain.Entities;
using BookingService.Domain.Enums;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace BookingService.UnitTests.Services;

public class BookingAppServiceTests
{
    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BookingMappingProfile>());
        config.AssertConfigurationIsValid();
        return config.CreateMapper();
    }

    [Fact]
    public async Task Should_Create_Booking_Successfully()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var repo = new Mock<IBookingRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Booking?)null);
        var customerClient = new Mock<ICustomerServiceClient>();
        customerClient.Setup(c => c.CustomerExistsAsync(customerId, default)).ReturnsAsync(true);

        Booking? captured = null;
        repo.Setup(r => r.AddAsync(It.IsAny<Booking>()))
            .Callback<Booking>(b => captured = b)
            .Returns(Task.CompletedTask);

        var sut = new BookingAppService(
            repo.Object,
            customerClient.Object,
            CreateMapper(),
            NullLogger<BookingAppService>.Instance);

        var request = new SaveBookingRequest
        {
            CustomerId = customerId,
            SpaceName = "Room A",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddHours(2),
            Status = BookingStatus.Pending,
        };

        // Act
        var result = await sut.CreateOrUpdateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.SpaceName.Should().Be("Room A");
        result.CustomerId.Should().Be(customerId);
        result.Status.Should().Be(BookingStatus.Pending.ToString());
        captured.Should().NotBeNull();
        captured!.SpaceName.Should().Be("Room A");
        repo.Verify(r => r.AddAsync(It.IsAny<Booking>()), Times.Once);
        repo.Verify(r => r.SaveChangesAsync(), Times.Once);
        customerClient.Verify(c => c.CustomerExistsAsync(customerId, default), Times.Once);
    }

    [Fact]
    public async Task Should_Update_Existing_Booking()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existing = new Booking
        {
            Id = id,
            CustomerId = Guid.NewGuid(),
            SpaceName = "Old",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddHours(1),
            Status = BookingStatus.Pending,
            CreatedAt = DateTime.UtcNow.AddDays(-1),
        };

        var repo = new Mock<IBookingRepository>();
        repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);

        var customerClient = new Mock<ICustomerServiceClient>();

        var sut = new BookingAppService(
            repo.Object,
            customerClient.Object,
            CreateMapper(),
            NullLogger<BookingAppService>.Instance);

        var request = new SaveBookingRequest
        {
            Id = id,
            CustomerId = existing.CustomerId,
            SpaceName = "Updated room",
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(1).AddHours(3),
            Status = BookingStatus.Confirmed,
        };

        // Act
        var result = await sut.CreateOrUpdateAsync(request);

        // Assert
        result.SpaceName.Should().Be("Updated room");
        result.Status.Should().Be(BookingStatus.Confirmed.ToString());
        existing.SpaceName.Should().Be("Updated room");
        existing.Status.Should().Be(BookingStatus.Confirmed);
        customerClient.Verify(c => c.CustomerExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        repo.Verify(r => r.AddAsync(It.IsAny<Booking>()), Times.Never);
        repo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_Change_Booking_Status()
    {
        // Arrange
        var id = Guid.NewGuid();
        var booking = new Booking
        {
            Id = id,
            CustomerId = Guid.NewGuid(),
            SpaceName = "S",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddHours(1),
            Status = BookingStatus.Pending,
        };

        var repo = new Mock<IBookingRepository>();
        repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(booking);

        var sut = new BookingAppService(
            repo.Object,
            Mock.Of<ICustomerServiceClient>(),
            CreateMapper(),
            NullLogger<BookingAppService>.Instance);

        // Act
        var ok = await sut.ChangeStatus(id, BookingStatus.Confirmed);

        // Assert
        ok.Should().BeTrue();
        booking.Status.Should().Be(BookingStatus.Confirmed);
        repo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_Throw_When_Customer_Does_Not_Exist_On_Create()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var repo = new Mock<IBookingRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Booking?)null);

        var customerClient = new Mock<ICustomerServiceClient>();
        customerClient.Setup(c => c.CustomerExistsAsync(customerId, default)).ReturnsAsync(false);

        var sut = new BookingAppService(
            repo.Object,
            customerClient.Object,
            CreateMapper(),
            NullLogger<BookingAppService>.Instance);

        var request = new SaveBookingRequest
        {
            CustomerId = customerId,
            SpaceName = "Room",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddHours(1),
            Status = BookingStatus.Pending,
        };

        // Act
        var act = async () => await sut.CreateOrUpdateAsync(request);

        // Assert
        var assertion = await act.Should().ThrowAsync<CustomerNotFoundException>();
        assertion.Which.CustomerId.Should().Be(customerId);
        repo.Verify(r => r.AddAsync(It.IsAny<Booking>()), Times.Never);
    }

    [Fact]
    public async Task Should_Return_Booking_By_Id()
    {
        // Arrange
        var id = Guid.NewGuid();
        var booking = new Booking
        {
            Id = id,
            CustomerId = Guid.NewGuid(),
            SpaceName = "Lab",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddHours(2),
            Status = BookingStatus.Pending,
        };

        var repo = new Mock<IBookingRepository>();
        repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(booking);

        var sut = new BookingAppService(
            repo.Object,
            Mock.Of<ICustomerServiceClient>(),
            CreateMapper(),
            NullLogger<BookingAppService>.Instance);

        // Act
        var result = await sut.GetByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.SpaceName.Should().Be("Lab");
    }

    [Fact]
    public async Task Should_Return_All_Bookings()
    {
        // Arrange
        var list = new List<Booking>
        {
            new()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                SpaceName = "A",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(1),
                Status = BookingStatus.Pending,
            },
        };

        var repo = new Mock<IBookingRepository>();
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

        var sut = new BookingAppService(
            repo.Object,
            Mock.Of<ICustomerServiceClient>(),
            CreateMapper(),
            NullLogger<BookingAppService>.Instance);

        // Act
        var result = await sut.GetAllAsync();

        // Assert
        result.Should().HaveCount(1);
        result[0].SpaceName.Should().Be("A");
    }
}
