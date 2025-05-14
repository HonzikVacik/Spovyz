using Spovyz.Models;

namespace Spovyz.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<Employee[]> GetEmployeesByIds(uint[] employees);
        Task<Employee[]> GetAllEmployees(uint CompanyId);
        Task<uint[]?> GetEmployeesIdsByProjectId(uint ProjectId);
        Task<uint[]> GetEmployeesIdsByTaskId(uint TaskId);
    }
}
