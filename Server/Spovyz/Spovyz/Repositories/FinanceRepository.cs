using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.Repositories
{
    public class FinanceRepository : IFinanceRepository
    {
        private readonly ApplicationDbContext _context;

        public FinanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task AddTask(Employee ActiveUser, string Name, uint Cost, string? Description, bool Income_expenditure, bool Current_planned)
        {
            Finance finance = new Finance
            {
                Name = Name,
                Cost = Cost,
                Description = Description,
                Income_expenditure = Income_expenditure,
                Current_planned = Current_planned,
                Employee = ActiveUser
            };
            _context.Finances.Add(finance);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteFinance(Finance finance)
        {
            _context.Remove(finance);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task<List<NameBasic>?> GetAllFinances(uint CompanyId, bool Income_expenditure, bool Current_planned)
        {
            return await _context.Finances
                .Include(f => f.Employee)
                .Where(f => f.Employee.Company.Id == CompanyId && f.Income_expenditure == Income_expenditure && f.Current_planned == Current_planned)
                .Select(f => new NameBasic
                {
                    Id = f.Id,
                    Name = f.Name,
                })
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<Finance?> GetFinanceById(uint CompanyId, uint Id)
        {
            return await _context.Finances
                .Include(f => f.Employee)
                .Where(f => f.Employee.Company.Id == CompanyId && f.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async System.Threading.Tasks.Task<FinanceResult> GetFinanceResult(uint CompanyId, bool Current_planned)
        {
            long revenue = _context.Finances
                .Include(f => f.Employee)
                .ThenInclude(e => e.Company)
                .Where(f => f.Employee.Company.Id == CompanyId && f.Income_expenditure == true && f.Current_planned == Current_planned)
                .Sum(f => f.Cost);

            long expenditure = _context.Finances
                .Include(f => f.Employee)
                .ThenInclude(e => e.Company)
                .Where(f => f.Employee.Company.Id == CompanyId && f.Income_expenditure == false && f.Current_planned == Current_planned)
                .Sum(f => f.Cost);

            long profit = revenue - expenditure;

            return new FinanceResult
            {
                Revenue = revenue,
                Expense = expenditure,
                Profit = profit
            };
        }

        public async System.Threading.Tasks.Task UpdateTask(Finance finance, string Name, uint Cost, string? Description, bool Income_expenditure, bool Current_planned)
        {
            finance.Name = Name;
            finance.Cost = Cost;
            finance.Description = Description;
            finance.Income_expenditure = Income_expenditure;
            finance.Current_planned = Current_planned;
            _context.Update(finance);
            await _context.SaveChangesAsync();
        }
    }
}
