using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;

namespace Api.Services
{
    public class BackendService
    {
        private Dictionary<int, Employee> employees;
        private Dictionary<int, Dependent> dependents;

        public BackendService()
        {
            employees = new Dictionary<int, Employee>();
            dependents = new Dictionary<int, Dependent>();

            GenerateEmployees();
        }

        public void AddEmployee(Employee employee)
        {
            int id = employee.Id;
            if (employees.ContainsKey(id))
            {
                Console.WriteLine($"Employee with ID {id} already exists.");
            }
            else
            {
                employees.Add(id, employee);
            }
        }

        public void AddDependent(int employeeId, Dependent dependent)
        {
            if (employees.TryGetValue(employeeId, out Employee? employee))
            {
                int id = dependent.Id;
                if (dependents.ContainsKey(id))
                {
                    throw new ArgumentException($"Dependent with ID {id} already exists.");
                }
                else
                {
                    employee.Dependents.Add(dependent);
                    dependents.Add(id, dependent);
                }
            }
            else
            {
                throw new ArgumentException($"Employee with ID {employeeId} not found.");
            }
        }

        public async Task<List<GetEmployeeDto>> GetAllEmployeesAsync()
        {
            return await Task.FromResult(employees.Values.Select(employee => MapToGetEmployeeDto(employee)).ToList());

        }

        public async Task<GetEmployeeDto?> GetEmployeeAsync(int id)
        {
            if (employees.TryGetValue(id, out Employee? employee))
            {
                return await Task.FromResult(MapToGetEmployeeDto(employee!));
            }
            Console.WriteLine($"Employee with ID {id} not found.");
            return await Task.FromResult<GetEmployeeDto?>(null);
        }

        public async Task<List<GetDependentDto>> GetAllDependentAsync()
        {
            return await Task.FromResult(dependents.Values.Select(dependent => MapToGetDependentDto(dependent)).ToList());
        }

        public async Task<GetDependentDto?> GetDependentAsync(int id)
        {
            if (dependents.TryGetValue(id, out Dependent? dependent))
            {
                return await Task.FromResult(MapToGetDependentDto(dependent!));
            }
            Console.WriteLine($"Dependent with ID {id} not found.");
            return await Task.FromResult<GetDependentDto?>(null);
        }

        private void GenerateEmployees()
        {
            Employee employee1 = new()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            };
            AddEmployee(employee1);

            Employee employee2 = new()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
            };
            AddEmployee(employee2);

            Employee employee3 = new()
            {
                Id = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17)
            };
            AddEmployee(employee3);

            Dependent employee2Dep1 = new()
            {
                Id = 1,
                FirstName = "Spouse",
                LastName = "Morant",
                Relationship = Relationship.Spouse,
                DateOfBirth = new DateTime(1998, 3, 3)
            };
            AddDependent(employee2.Id, employee2Dep1);

            Dependent employee2Dep2 = new()
            {
                Id = 2,
                FirstName = "Child1",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2020, 6, 23)
            };
            AddDependent(employee2.Id, employee2Dep2);

            Dependent employee2Dep3 = new()
            {
                Id = 3,
                FirstName = "Child2",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2021, 5, 18)
            };
            AddDependent(employee2.Id, employee2Dep3);

            Dependent employee3Dep1 = new()
            {
                Id = 4,
                FirstName = "DP",
                LastName = "Jordan",
                Relationship = Relationship.DomesticPartner,
                DateOfBirth = new DateTime(1974, 1, 2)
            };
            AddDependent(employee3.Id, employee3Dep1);
        }

        public static GetEmployeeDto MapToGetEmployeeDto(Employee employee)
        {
            return new GetEmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Salary = employee.Salary,
                DateOfBirth = employee.DateOfBirth,
                Dependents = employee.Dependents.Select(dependent => MapToGetDependentDto(dependent)).ToList()
            };
        }

        public static GetDependentDto MapToGetDependentDto(Dependent dependent)
        {
            return new GetDependentDto
            {
                Id = dependent.Id,
                FirstName = dependent.FirstName,
                LastName = dependent.LastName,
                DateOfBirth = dependent.DateOfBirth,
                Relationship = dependent.Relationship
            };
        }
    }
}
