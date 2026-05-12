using CustomerService.Application.Interfaces;
using CustomerService.Application.Mappings;
using CustomerService.Application.Services;
using CustomerService.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICustomerAppService, CustomerAppService>();

        services.AddAutoMapper(typeof(CustomerMappingProfile));

        services.AddValidatorsFromAssemblyContaining<SaveCustomerRequestValidator>();

        return services;
    }
}
