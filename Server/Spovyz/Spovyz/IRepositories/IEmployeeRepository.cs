using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<Employee[]> GetEmployeesByIds(uint[] employees);
        Task<Employee[]> GetAllEmployees(uint CompanyId);
        Task<NameBasic[]> GetEmployeesToProject(uint CompanyId);
        Task<NameBasic[]> GetEmployeesToTask(uint CompanyId, uint ProjectId);
        Task<uint[]?> GetEmployeesIdsByProjectId(uint ProjectId);
        Task<uint[]> GetEmployeesIdsByTaskId(uint TaskId);
        Task<Transport_models.EmployeeSalary[]> EmployeeSalary(uint CompanyId);
        System.Threading.Tasks.Task UpdateEmployeeSalary(uint companyId, uint employeeId, uint salary);
    }
}
