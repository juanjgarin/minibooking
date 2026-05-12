using BookingService.Application.Interfaces;
using BookingService.Application.Options;
using BookingService.Infrastructure.Clients;
using BookingService.Infrastructure.Data;
using BookingService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BookingService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<BookingDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IBookingRepository, BookingRepository>();

        services.Configure<CustomerServiceOptions>(
            configuration.GetSection(CustomerServiceOptions.SectionName));

        services.AddHttpClient<ICustomerServiceClient, CustomerServiceHttpClient>((sp, client) =>
        {
            var opts = sp.GetRequiredService<IOptions<CustomerServiceOptions>>().Value;
            if (string.IsNullOrWhiteSpace(opts.BaseUrl))
            {
                throw new InvalidOperationException(
                    "Configuration 'CustomerService:BaseUrl' is required (URL of the CustomerService API).");
            }

            client.BaseAddress = new Uri(opts.BaseUrl.TrimEnd('/') + "/");
            client.Timeout = TimeSpan.FromSeconds(Math.Clamp(opts.TimeoutSeconds, 1, 300));
        });

        return services;
    }
}