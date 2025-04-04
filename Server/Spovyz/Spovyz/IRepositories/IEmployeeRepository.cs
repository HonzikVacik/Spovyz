namespace Spovyz.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<uint[]?> GetEmployeesIdsByProjectId(uint ProjectId);
    }
}
