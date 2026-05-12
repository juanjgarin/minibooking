using BookingService.Application.Interfaces;
using BookingService.Application.Mappings;
using BookingService.Application.Services;
using BookingService.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BookingService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IBookingAppService, BookingAppService>();

        services.AddAutoMapper(typeof(BookingMappingProfile));

        services.AddValidatorsFromAssemblyContaining<SaveBookingRequestValidator>();

        return services;
    }
}