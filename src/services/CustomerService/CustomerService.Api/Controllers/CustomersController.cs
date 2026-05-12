using CustomerService.Application.DTOs.Requests;
using CustomerService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerAppService _customerAppService;

    public CustomersController(ICustomerAppService customerAppService)
    {
        _customerAppService = customerAppService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customerAppService.GetAllAsync();
        return Ok(customers);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var customer = await _customerAppService.GetByIdAsync(id);
        if (customer is null)
            return NotFound();

        return Ok(customer);
    }

    [HttpPost("upsert")]
    public async Task<IActionResult> CreateOrUpdate([FromBody] SaveCustomerRequest request)
    {
        var customer = await _customerAppService.CreateOrUpdateAsync(request);
        return Ok(customer);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _customerAppService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
