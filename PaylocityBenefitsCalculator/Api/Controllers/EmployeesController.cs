using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private BackendService backendService;

    public EmployeesController()
    {
        if (backendService == null)
        {
            backendService = new();
        }
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var employee = await backendService.GetEmployeeAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        var response = new ApiResponse<GetEmployeeDto>
        {
            Data = employee,
            Success = true
        };

        return response;
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        var employees = await backendService.GetAllEmployeesAsync();

        var response = new ApiResponse<List<GetEmployeeDto>>
        {
            Data = employees,
            Success = true
        };

        return response;
    }


    [SwaggerOperation(Summary = "Calculate and Get paycheck for an employee by id")]
    [HttpGet("{id}/paychecks")]
    public async Task<ActionResult<ApiResponse<List<GetPaycheckDto>>>> GetPaychecks(int id)
    {
        var employee = await backendService.GetEmployeeAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        decimal benefitsCostYearly = CalculateBenefitsCost(employee);

        int totalPaychecks = 26;

        float deductionsPerPaycheckInCents = MathF.Floor((float)benefitsCostYearly * 100 / totalPaychecks);
        float extraCents = MathF.Floor(((float)benefitsCostYearly * 100) % totalPaychecks);

        var paychecks = new List<GetPaycheckDto>();
        int checkNumber = 1;
        for (int i = 0; i < 26; i++)
        {
            var paycheck = new GetPaycheckDto
            {
                Id = i,
                CheckNumber = checkNumber++,
                EmployeeId = employee.Id,
                TotalAnnualSalary = employee.Salary,
                Deduction = (decimal)(deductionsPerPaycheckInCents + (extraCents-- > 0 ? 1 : 0)) / 100,
            };
            paychecks.Add(paycheck);
        }

        var result = new ApiResponse<List<GetPaycheckDto>>
        {
            Data = paychecks,
            Success = true
        };

        return result;
    }

    private static decimal CalculateBenefitsCost(GetEmployeeDto employee)
    {
        decimal baseCost = 1000;
        decimal baseDependentCost = 600;
        decimal overageDependentCost = 200;
        decimal additionalCostForHighSalary = 0;
        decimal dependentsCost = 0;

        if (employee.Salary > 80000)
        {
            additionalCostForHighSalary = 0.02M * employee.Salary;
        }

        foreach (var dependent in employee.Dependents)
        {
            dependentsCost += baseDependentCost;
            if (IsDependentOver50(dependent.DateOfBirth))
            {
                dependentsCost += overageDependentCost;
            }
        }

        return ((baseCost + dependentsCost) * 12) + additionalCostForHighSalary;
    }

    private static bool IsDependentOver50(DateTime dateOfBirth)
    {
        int age = DateTime.Today.Year - dateOfBirth.Year;
        if (dateOfBirth > DateTime.Today.AddYears(-age)) age--;
        return age > 50;
    }
}
