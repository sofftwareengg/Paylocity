using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private BackendService backendService;

    public DependentsController()
    {
        this.backendService = new();
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependent = await backendService.GetDependentAsync(id);

        if (dependent == null)
        {
            return NotFound();
        }

        var response = new ApiResponse<GetDependentDto>
        {
            Data = dependent,
            Success = true
        };

        return response;
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = await backendService.GetAllDependentAsync();

        var response = new ApiResponse<List<GetDependentDto>>
        {
            Data = dependents,
            Success = true
        };

        return response;
    }
}
