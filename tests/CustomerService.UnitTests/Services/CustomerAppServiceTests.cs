using AutoMapper;
using CustomerService.Application.DTOs.Requests;
using CustomerService.Application.Interfaces;
using CustomerService.Application.Mappings;
using CustomerService.Application.Services;
using CustomerService.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace CustomerService.UnitTests.Services;

public class CustomerAppServiceTests
{
    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CustomerMappingProfile>());
        config.AssertConfigurationIsValid();
        return config.CreateMapper();
    }

    [Fact]
    public async Task Should_Create_Customer_Successfully()
    {
        // Arrange
        var repo = new Mock<ICustomerRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Customer?)null);

        Customer? captured = null;
        repo.Setup(r => r.AddAsync(It.IsAny<Customer>()))
            .Callback<Customer>(c => captured = c)
            .Returns(Task.CompletedTask);

        var sut = new CustomerAppService(repo.Object, CreateMapper(), NullLogger<CustomerAppService>.Instance);

        var request = new SaveCustomerRequest
        {
            FullName = "Jane Doe",
            Email = "jane@example.com",
            Phone = "+15550001",
        };

        // Act
        var result = await sut.CreateOrUpdateAsync(request);

        // Assert
        result.FullName.Should().Be("Jane Doe");
        result.Email.Should().Be("jane@example.com");
        captured.Should().NotBeNull();
        captured!.FullName.Should().Be("Jane Doe");
        repo.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Once);
        repo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_Update_Customer_Successfully()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existing = new Customer
        {
            Id = id,
            FullName = "Old",
            Email = "old@example.com",
            Phone = "1",
            CreatedAt = DateTime.UtcNow.AddDays(-2),
        };

        var repo = new Mock<ICustomerRepository>();
        repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);

        var sut = new CustomerAppService(repo.Object, CreateMapper(), NullLogger<CustomerAppService>.Instance);

        var request = new SaveCustomerRequest
        {
            Id = id,
            FullName = "New name",
            Email = "new@example.com",
            Phone = "+15550002",
        };

        // Act
        var result = await sut.CreateOrUpdateAsync(request);

        // Assert
        result.FullName.Should().Be("New name");
        result.Email.Should().Be("new@example.com");
        existing.FullName.Should().Be("New name");
        repo.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Never);
        repo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_Return_Customer_By_Id()
    {
        // Arrange
        var id = Guid.NewGuid();
        var customer = new Customer
        {
            Id = id,
            FullName = "Bob",
            Email = "bob@example.com",
            Phone = "123",
        };

        var repo = new Mock<ICustomerRepository>();
        repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(customer);

        var sut = new CustomerAppService(repo.Object, CreateMapper(), NullLogger<CustomerAppService>.Instance);

        // Act
        var result = await sut.GetByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Email.Should().Be("bob@example.com");
    }

    [Fact]
    public async Task Should_Return_All_Customers()
    {
        // Arrange
        var list = new List<Customer>
        {
            new()
            {
                Id = Guid.NewGuid(),
                FullName = "A",
                Email = "a@x.com",
                Phone = "1",
            },
        };

        var repo = new Mock<ICustomerRepository>();
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

        var sut = new CustomerAppService(repo.Object, CreateMapper(), NullLogger<CustomerAppService>.Instance);

        // Act
        var result = await sut.GetAllAsync();

        // Assert
        result.Should().HaveCount(1);
        result[0].FullName.Should().Be("A");
    }
}
