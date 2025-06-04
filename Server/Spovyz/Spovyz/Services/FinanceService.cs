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

        public async System.Threading.Tasks.Task<(ValidityControl.ResultStatus, string?)> AddFinance(string UserName, string Name, uint Cost, string? Description, bool Income_expenditure, bool Current_planned)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "Uživatel nenalezen");

            (ValidityControl.ResultStatus status, string? error) = ValidityControl.Check_FI(Name);
            if (status != ValidityControl.ResultStatus.Ok)
                return (status, error);

            await _financeRepository.AddTask(activeUser, Name, Cost, Description, Income_expenditure, Current_planned);

            return (ValidityControl.ResultStatus.Ok, null);
        }

        public async System.Threading.Tasks.Task<(ValidityControl.ResultStatus, string?)> DeleteFinance(string UserName, uint Id)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "Uživatel nenalezen");

            Finance? finance = await _financeRepository.GetFinanceById(activeUser.Company.Id, Id);

            if (finance == null)
                return (ValidityControl.ResultStatus.NotFound, "Finance nenalezeny");

            await _financeRepository.DeleteFinance(finance);

            return (ValidityControl.ResultStatus.Ok, null);
        }

        public async System.Threading.Tasks.Task<(ValidityControl.ResultStatus, List<NameBasic>?, string?)> GetAllFinances(string UserName, bool Income_expenditure, bool Current_planned)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, null, "Uživatel nenalezen");

            List<NameBasic>? nameBasics = await _financeRepository.GetAllFinances(activeUser.Company.Id, Income_expenditure, Current_planned);

            if (nameBasics == null)
                return (ValidityControl.ResultStatus.NotFound, null, "Finance nenalezeny");

            return (ValidityControl.ResultStatus.Ok, nameBasics, null);
        }

        public async System.Threading.Tasks.Task<(ValidityControl.ResultStatus, FinanceData?, string?)> GetFinanceById(string UserName, uint Id)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, null, "Uživatel nenalezen");

            Finance? finance = await _financeRepository.GetFinanceById(activeUser.Company.Id, Id);

            if (finance == null)
                return (ValidityControl.ResultStatus.NotFound, null, "Finance nenalezeny");

            FinanceData financeData = new FinanceData
            {
                Name = finance.Name,
                Cost = finance.Cost,
                Description = finance.Description
            };

            return (ValidityControl.ResultStatus.Ok, financeData, null);
        }

        public async System.Threading.Tasks.Task<(ValidityControl.ResultStatus, string?, FinanceResult?)> GetFinanceResult(string UserName, bool Current_planned)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "Uživatel nenalezen", null);

            FinanceResult financeResult = await _financeRepository.GetFinanceResult(activeUser.Company.Id, Current_planned);
            
            return (ValidityControl.ResultStatus.Ok, null, financeResult);
        }

        public async System.Threading.Tasks.Task<(ValidityControl.ResultStatus, string?)> UpdateFinance(string UserName, uint Id, string Name, uint Cost, string? Description, bool Income_expenditure, bool Current_planned)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "Uživatel nenalezen");

            (ValidityControl.ResultStatus status, string? error) = ValidityControl.Check_FI(Name);
            if (status != ValidityControl.ResultStatus.Ok)
                return (status, error);

            Finance? finance = await _financeRepository.GetFinanceById(activeUser.Company.Id, Id);
            if (finance == null)
                return (ValidityControl.ResultStatus.NotFound, "Finance nenalezeny");

            await _financeRepository.UpdateTask(finance, Name, Cost, Description, Income_expenditure, Current_planned);

            return (ValidityControl.ResultStatus.Ok, null);
        }
    }
}
