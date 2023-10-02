namespace Api.Dtos.Paycheck;

public class GetPaycheckDto
{
	public int Id { get; set; }
	public int EmployeeId { get; set; }
	public int CheckNumber { get; set; }
	public decimal TotalAnnualSalary { get; set; }
	public decimal Deduction { get; set; }
}
