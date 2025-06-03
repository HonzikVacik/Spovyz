using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.IServices;
using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.Services
{
    public class FinanceService : IFinanceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFinanceRepository _financeRepository;

        public FinanceService(ApplicationDbContext context, IFinanceRepository financeRepository)
        {
            _context = context;
            _financeRepository = financeRepository;
        }

        public async Task<(ValidityControl.ResultStatus, string?)> AddFinance(string UserName, string Name, uint Cost, string? Description, bool Income_expenditure, bool Current_planned)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found");

            (ValidityControl.ResultStatus status, string? error) = ValidityControl.Check_FI(Name);
            if (status != ValidityControl.ResultStatus.Ok)
                return (status, error);

            await _financeRepository.AddTask(activeUser, Name, Cost, Description, Income_expenditure, Current_planned);

            return (ValidityControl.ResultStatus.Ok, null);
        }

        public async Task<(ValidityControl.ResultStatus, string?)> DeleteFinance(string UserName, uint Id)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found");

            Finance? finance = await _financeRepository.GetFinanceById(activeUser.Company.Id, Id);

            if (finance == null)
                return (ValidityControl.ResultStatus.NotFound, "Finance not found");

            await _financeRepository.DeleteFinance(finance);

            return (ValidityControl.ResultStatus.Ok, null);
        }

        public async Task<(ValidityControl.ResultStatus, List<NameBasic>?, string?)> GetAllFinances(string UserName, bool Income_expenditure, bool Current_planned)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, null, "User not found");

            List<NameBasic>? nameBasics = await _financeRepository.GetAllFinances(activeUser.Company.Id, Income_expenditure, Current_planned);

            if (nameBasics == null)
                return (ValidityControl.ResultStatus.NotFound, null, "No finances found");

            return (ValidityControl.ResultStatus.Ok, nameBasics, null);
        }

        public async Task<(ValidityControl.ResultStatus, FinanceData?, string?)> GetFinanceById(string UserName, uint Id)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, null, "User not found");

            Finance? finance = await _financeRepository.GetFinanceById(activeUser.Company.Id, Id);

            if (finance == null)
                return (ValidityControl.ResultStatus.NotFound, null, "Finance not found");

            FinanceData financeData = new FinanceData
            {
                Name = finance.Name,
                Cost = finance.Cost,
                Description = finance.Description
            };

            return (ValidityControl.ResultStatus.Ok, financeData, null);
        }

        public async Task<(ValidityControl.ResultStatus, string?, FinanceResult?)> GetFinanceResult(string UserName, bool Current_planned)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found", null);

            FinanceResult financeResult = await _financeRepository.GetFinanceResult(activeUser.Company.Id, Current_planned);
            
            return (ValidityControl.ResultStatus.Ok, null, financeResult);
        }

        public async Task<(ValidityControl.ResultStatus, string?)> UpdateFinance(string UserName, uint Id, string Name, uint Cost, string? Description, bool Income_expenditure, bool Current_planned)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found");

            (ValidityControl.ResultStatus status, string? error) = ValidityControl.Check_FI(Name);
            if (status != ValidityControl.ResultStatus.Ok)
                return (status, error);

            Finance? finance = await _financeRepository.GetFinanceById(activeUser.Company.Id, Id);
            if (finance == null)
                return (ValidityControl.ResultStatus.NotFound, "Finance not found");

            await _financeRepository.UpdateTask(finance, Name, Cost, Description, Income_expenditure, Current_planned);

            return (ValidityControl.ResultStatus.Ok, null);
        }
    }
}
