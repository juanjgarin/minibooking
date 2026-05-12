namespace CustomerService.Application.DTOs.Requests;

public class SaveCustomerRequest
{
    public Guid? Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;
}
