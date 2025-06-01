using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.IServices;
using Spovyz.Models;
using Spovyz.Transport_models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Spovyz.Services
{
    public class StatementService : IStatementService
    {
        private readonly ApplicationDbContext _context;
        private readonly IStatementRepository _statementRepository;
        private readonly IAccountingRepository _accountingRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;

        public StatementService(ApplicationDbContext context, IStatementRepository statementRepository, IAccountingRepository accountingRepository, IProjectRepository projectRepository, ITaskRepository taskRepository)
        {
            _context = context;
            _statementRepository = statementRepository;
            _accountingRepository = accountingRepository;
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
        }

        public async Task<(ValidityControl.ResultStatus, string? error)> AddStatement(string UserName, byte StatementType, DateOnly Datum, byte PocetHodin, string? Description)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found");

            string? accountingError = await _accountingRepository.SetAccounting(activeUser.Company.Id, activeUser.Id, activeUser.Pay);
            if (accountingError != null)
                return (ValidityControl.ResultStatus.Error, accountingError);

            Accounting? accounting = await _accountingRepository.GetAccounting(activeUser.Company.Id, activeUser.Id, Datum);
            if (accounting == null)
                return (ValidityControl.ResultStatus.Error, "Accounting not found");

            (ValidityControl.ResultStatus status, string? error) = ValidityControl.Check_SI(StatementType, Datum, PocetHodin);
            if (status != ValidityControl.ResultStatus.Ok)
                return (status, error);

            await _statementRepository.AddStatement(StatementType, Datum, PocetHodin, Description, accounting);

            return (ValidityControl.ResultStatus.Ok, null);
        }

        public async Task<(ValidityControl.ResultStatus, string? error)> DeleteStatement(string UserName, uint Id)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found");

            await _statementRepository.DeleteStatement(activeUser.Id, activeUser.Company.Id, Id);

            return(ValidityControl.ResultStatus.Ok, null);
        }

        public async Task<(ValidityControl.ResultStatus, string? error, StatementDataShort[]?)> GetDay(string UserName, byte Day, uint AccountingId)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found", null);

            Accounting? accounting = await _accountingRepository.GetAccountingById(AccountingId);
            if (accounting == null)
                return (ValidityControl.ResultStatus.Error, "Accounting not found", null);

            DateOnly Datum = new DateOnly(accounting.Year, (byte)accounting.Month, Day);

            StatementDataShort[]? statementDataShorts = await _statementRepository.GetDay(activeUser.Company.Id, Datum);

            return (ValidityControl.ResultStatus.Ok, null, statementDataShorts);
        }

        public async Task<(ValidityControl.ResultStatus, string? error, StatementDataLong?)> GetMonth(string UserName, uint AccountingId)
        {
            Employee? activeUser = await _context.Employees.Include(e => e.Company).FirstOrDefaultAsync(e => e.Username == UserName);
            if (activeUser == null)
                return (ValidityControl.ResultStatus.NotFound, "User not found", null);

            Accounting? accounting = await _accountingRepository.GetAccountingById(AccountingId);
            if (accounting == null)
                return (ValidityControl.ResultStatus.Error, "Accounting not found", null);

            StatementDataLong? statementDataLong = await _statementRepository.GetMonth(accounting);

            return (ValidityControl.ResultStatus.Ok, null, statementDataLong);
        }
    }
}
