using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.IServices;
using Spovyz.Models;
using Spovyz.Transport_models;

namespace Spovyz.Services
{
    public class AccountingService : IAccountingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountingRepository _accountingRepository;

        public AccountingService(ApplicationDbContext context, IAccountingRepository accountingRepository)
        {
            _context = context;
            _accountingRepository = accountingRepository;
        }

        public async Task<(ValidityControl.ResultStatus, string? error, AccountingDataShort[]?)> Get(string UserName)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found", null);

            AccountingDataShort[]? accountingDataShorts = await _accountingRepository.Get(activeUser.Company.Id);
            if (accountingDataShorts == null)
                return (ValidityControl.ResultStatus.NotFound, "No accounting data found", null);

            return (ValidityControl.ResultStatus.Ok, null, accountingDataShorts);
        }

        public async Task<(ValidityControl.ResultStatus, string? error, AccountingDataLong[]?)> GetAll(string UserName)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found", null);

            AccountingDataLong[]? accountingDataLongs = await _accountingRepository.GetAll(activeUser.Company.Id);
            if (accountingDataLongs == null)
                return (ValidityControl.ResultStatus.NotFound, "No accounting data found", null);

            return (ValidityControl.ResultStatus.Ok, null, accountingDataLongs);
        }
    }
}
