using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.IRepositories
{
    public interface IEmployeeRepository
    {
        System.Threading.Tasks.Task<Employee[]> GetEmployeesByIds(uint[] employees);
        System.Threading.Tasks.Task<Employee[]> GetAllEmployees(uint CompanyId);
        System.Threading.Tasks.Task<NameBasic[]> GetAllEmployeesToProject(uint CompanyId);
        System.Threading.Tasks.Task<NameBasic[]> GetEmployeesToProject(uint CompanyId);
        System.Threading.Tasks.Task<NameBasic[]> GetEmployeesToTask(uint CompanyId, uint ProjectId);
        System.Threading.Tasks.Task<uint[]?> GetEmployeesIdsByProjectId(uint ProjectId);
        System.Threading.Tasks.Task<uint[]> GetEmployeesIdsByTaskId(uint TaskId);
        System.Threading.Tasks.Task<Transport_models.EmployeeSalary[]> EmployeeSalary(uint CompanyId);
        System.Threading.Tasks.Task UpdateEmployeeSalary(uint companyId, uint employeeId, uint salary);
    }
}
