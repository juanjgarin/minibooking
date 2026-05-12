namespace BookingService.Application.Options;

/// <summary>
/// HTTP client settings for calling the CustomerService API.
/// </summary>
public class CustomerServiceOptions
{
    public const string SectionName = "CustomerService";

    /// <summary>
    /// Base URL of CustomerService (e.g. http://localhost:5222).
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// Timeout for outbound HTTP calls.
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;
}
