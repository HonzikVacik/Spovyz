using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Spovyz.Models;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Spovyz.Models.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace Spovyz
{
    public static class ValidityControl
    {
        public enum ResultStatus
        {
            Ok,
            Error,
            NotFound
        }

        public static string Check_EI(ApplicationDbContext _context, uint activeUserCompanyId, string username, string password, string securityVerification, int accountType, int sex, string email, string phoneNumber, DateOnly dateOfBirth, bool controlUsername, bool controlPassword, bool controlSecurityVerification)
        {
            if (controlUsername)
            {
                if (!Username(username, activeUserCompanyId, _context))
                    return "e2";
            }
            if(controlPassword)
            {
                if (!Password(password))
                    return "e1";
            }
            if(controlSecurityVerification)
            {
                if (!SecurityVerification(securityVerification, username, _context))
                    return "e3";
            }
            if (!AccountType(accountType))
                return "e1";
            if (!Sex(sex))
                return "e1";
            if (!Email(email))
                return "e1";
            if (!PhoneNumber(phoneNumber))
                return "e1";
            if (!DateOfBirth(dateOfBirth))
                return "e1";
            return "a";
        }

        public static async Task<(ValidityControl.ResultStatus, string?)> Check_PI(ApplicationDbContext _context, uint CompanyId, string ProjectName, string ProjectDescription, uint CustomerId, DateOnly? Deadline, uint[] Employees, int Status, bool checkProjectName)
        {
            Company? company = await ExistCompany(_context, CompanyId);
            if (company is null)
                return (ResultStatus.Error, "Company does not exist");

            if(checkProjectName)
            {
                bool existProjectName = await ExistProjectName(_context, CompanyId, ProjectName);
                if (existProjectName)
                    return (ResultStatus.Error, "Project name already exists");
            }

            if (InvalidStatus(Status))
                return (ResultStatus.Error, "Invalid status");

            if (InvalidDeadLine(Deadline))
                return (ResultStatus.Error, "DeadLine must be newer than yestedrday");

            Customer? customer = await ExistCustomer(_context, CustomerId);
            if (customer is null)
                return (ResultStatus.Error, "Customer does not exist");

            bool existEmployees = await ExistEmployees(_context, CompanyId, Employees);
            if (existEmployees)
                return (ResultStatus.Error, "Some employee does not exist");

            (bool isEmpty, string? error) = ProjectInformationEmpty(ProjectName, ProjectDescription, Employees);
            if (isEmpty)
                return (ResultStatus.Error, error);

            return (ResultStatus.Ok, null);
        }

        public static async Task<(ValidityControl.ResultStatus, string?)> Check_TI(ApplicationDbContext _context, string Name, uint ProjectId, DateOnly? DeadLine, int Status, uint[] Employees, bool checkTaskName)
        {
            (bool isEmpty, string? emptyError) = TaskInformationEmpty(Name, Employees);
            if (isEmpty)
                return (ResultStatus.Error, emptyError);

            if(checkTaskName)
            {
                if (await ExistTaskName(_context, ProjectId, Name))
                    return (ResultStatus.Error, "Task name already exists");
            }

            (bool neplatny, string? error) = InvalidTaskDeadLine(_context, DeadLine, ProjectId);
            if (neplatny)
                return (ResultStatus.Error, error);

            if (InvalidStatus(Status))
                return (ResultStatus.Error, "Invalid status");

            if (await InvalidEmployees(_context, ProjectId, Employees))
                return (ResultStatus.Error, "Some employees are not assigned to the project");

            return (ResultStatus.Ok, null);
        }

        public static (ValidityControl.ResultStatus, string? error) Check_FI(string Name)
        {
            (bool isEmpty, string? emptyError) = FinanceInformationEmpty(Name);
            if (isEmpty)
                return (ResultStatus.Error, emptyError);
            else
                return (ResultStatus.Ok, null);
        }

        public static (ValidityControl.ResultStatus, string? error) Check_SI(byte statementType, DateOnly datum, byte pocetHodin)
        {
            if(InvalidStatementType(statementType))
                return (ResultStatus.Error, "Invalid statement type");
            if(StatementDate(datum))
                return (ResultStatus.Error, "Statement date must be in actual month");
            if(StatementHours(pocetHodin))
                return (ResultStatus.Error, "Number of hours must be between 1 and 24");
            return (ResultStatus.Ok, null);
        }

        public static bool Check_salary(uint salary)
        {
            if (salary > 0)
                return true;
            else
                return false;
        }

        private static bool Username(string username, uint activeUserCompanyId, ApplicationDbContext _context)
        {
            Employee[] e = [.. _context.Employees
                .Include(e => e.Company)
                .Where(e => e.Username == username && e.Company.Id == activeUserCompanyId)];
            if (e == null || e.Length == 0)
                return true;
            else
                return false;
        }

        public static bool Password(string password)
        {
            string pattern = @"(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^a-zA-Z\d])";
            if (Regex.IsMatch(password, pattern) && password.Length >= 8)
                return true;
            else
                return false;
        }

        private static bool SecurityVerification(string securityVerification, string username, ApplicationDbContext _context)
        {
            Employee[] e = [.. _context.Employees
                .Include(e => e.Company)
                .Where(e => e.Username == username && e.SecurityVerification == securityVerification)];
            if (e == null || e.Length == 0)
                return true;
            else
                return false;
        }

        private static bool AccountType(int accountType)
        {
            if (accountType >= 0 && accountType < Enum.GetNames(typeof(Enums.Role)).Length)
                return true;
            else
                return false;
        }

        private static bool Sex(int sex)
        {
            if (sex >= 0 && sex < Enum.GetNames(typeof(Enums.Sex)).Length)
                return true;
            else
                return false;
        }

        private static bool DateOfBirth(DateOnly dateOfBirth)
        {
            if(dateOfBirth < DateOnly.FromDateTime(DateTime.Now.Date))
                return true;
            else
                return false;
        }

        private static bool Email(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (Regex.IsMatch(email, pattern))
                return true;
            else
                return false;
        }

        private static bool PhoneNumber(string phoneNumber)
        {
            string pattern = @"^\d+$";
            if (Regex.IsMatch(phoneNumber, pattern) && phoneNumber.Length == 9)
                return true;
            else
                return false;
        }

        public static bool AllFilledOut_EI(string username, string password, string securityVerification, string firstName, string surname, string phoneNumber, string email, DateOnly dateOfBirth, int sex, string pronoun, string country, string city, int zipCode, string street, uint decNumber, int accountType)
        {
            if (!string.IsNullOrEmpty(username) &&
                !string.IsNullOrEmpty(password) &&
                !string.IsNullOrEmpty(securityVerification) &&
                !string.IsNullOrEmpty(firstName) &&
                !string.IsNullOrEmpty(surname) &&
                !string.IsNullOrEmpty(phoneNumber) &&
                !string.IsNullOrEmpty(email) &&
                !string.IsNullOrEmpty(dateOfBirth.ToString()) &&
                !string.IsNullOrEmpty(sex.ToString()) &&
                !string.IsNullOrEmpty(pronoun) &&
                !string.IsNullOrEmpty(country) &&
                !string.IsNullOrEmpty(city) &&
                !string.IsNullOrEmpty(zipCode.ToString()) &&
                !string.IsNullOrEmpty(street) &&
                !string.IsNullOrEmpty(decNumber.ToString()) &&
                !string.IsNullOrEmpty(accountType.ToString()))
                return true;
            else
                return false;
        }

        public static bool PutAllFilledOut_EI(string username, string securityVerification, string firstName, string surname, string phoneNumber, string email, DateOnly dateOfBirth, int sex, string pronoun, string country, string city, int zipCode, string street, uint decNumber, int accountType)
        {
            if (!string.IsNullOrEmpty(username) &&
                !string.IsNullOrEmpty(securityVerification) &&
                !string.IsNullOrEmpty(firstName) &&
                !string.IsNullOrEmpty(surname) &&
                !string.IsNullOrEmpty(phoneNumber) &&
                !string.IsNullOrEmpty(email) &&
                !string.IsNullOrEmpty(dateOfBirth.ToString()) &&
                !string.IsNullOrEmpty(sex.ToString()) &&
                !string.IsNullOrEmpty(pronoun) &&
                !string.IsNullOrEmpty(country) &&
                !string.IsNullOrEmpty(city) &&
                !string.IsNullOrEmpty(zipCode.ToString()) &&
                !string.IsNullOrEmpty(street) &&
                !string.IsNullOrEmpty(decNumber.ToString()) &&
                !string.IsNullOrEmpty(accountType.ToString()))
                return true;
            else
                return false;
        }

        public static async Task<Company?> ExistCompany(ApplicationDbContext _context, uint CompanyId)
        {
            return await _context.Companies.Where(c => c.Id == CompanyId).FirstOrDefaultAsync();
        }

        private static async Task<bool> ExistProjectName(ApplicationDbContext _context, uint CompanyId, string ProjectName)
        {
            Project_employee? project_employee = await _context.Project_employees
                .Include(pe => pe.Project)
                .Include(pe => pe.Employee)
                .Where(pe => pe.Project.Name == ProjectName && pe.Employee.Company.Id == CompanyId)
                .FirstOrDefaultAsync();
            return !(project_employee is null);
        }

        private static bool InvalidDeadLine(DateOnly? DeadLine)
        {
            if(DeadLine != null)
            {
                if (DeadLine < DateOnly.FromDateTime(DateTime.Now.Date))
                    return true;
                else
                    return false;
            }
            return false;
        }

        private static async Task<Customer?> ExistCustomer(ApplicationDbContext _context, uint CustomerId)
        {
            return await _context.Customers
                .Where(c => c.Id == CustomerId)
                .FirstOrDefaultAsync();
        }

        private static async Task<bool> ExistEmployees(ApplicationDbContext _context, uint CompanyId, uint[] Employees)
        {
            foreach(var employee in Employees)
            {
                if (await _context.Employees
                    .Include(e => e.Company)
                    .Where(e => e.Company.Id == CompanyId)
                    .FirstOrDefaultAsync() is null)
                    return true;
            }
            return false;
        }

        private static (bool, string?) ProjectInformationEmpty(string Name, string Description, uint[] Employees)
        {
            if(string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name))
            {
                return (true, "Project name is empty");
            }
            else if(string.IsNullOrEmpty(Description) || string.IsNullOrWhiteSpace(Description))
            {
                return (true, "Project description is empty");
            }
            else if (Employees.Length == 0)
            {
                return (true, "Project must have at least one employee");
            }
            else
            {
                return (false, null);
            }
        }

        private static async Task<bool> ExistTaskName(ApplicationDbContext _context, uint ProjectId, string TaskName)
        {
            Models.Task? task = await _context.Tasks
                .Include(t => t.Project)
                .Where(t => t.Name == TaskName && t.Project.Id == ProjectId)
                .FirstOrDefaultAsync();
            return !(task is null);
        }

        private static (bool, string?) InvalidTaskDeadLine(ApplicationDbContext _context, DateOnly? DeadLine, uint ProjectId)
        {
            DateOnly? projectDeadLine = _context.Projects
                .Where(p => p.Id == ProjectId)
                .Select(p => p.Dead_line)
                .FirstOrDefault();

            if (DeadLine != null)
            {
                if(projectDeadLine != null)
                {
                    if (DeadLine > projectDeadLine)
                        return (true, "DeadLine must be older than the DeadLine of Project");
                    else
                        return (false, null);
                }
                else
                {
                    if (DeadLine < DateOnly.FromDateTime(DateTime.Now.Date))
                        return (true, "DeadLine must be newer than yestedrday");
                    else
                        return (false, null);
                }
            }
            return (false, null);
        }

        private static bool InvalidStatus(int Status)
        {
            try
            {
                Enums.Status validStatus = (Enums.Status)Status;
                return false;
            }
            catch
            {
                return true;
            }
        }

        public static async Task<bool> InvalidEmployees(ApplicationDbContext _context, uint ProjectId, uint[] Employees)
        {
            foreach (var employee in Employees)
            {
                if (await _context.Project_employees
                    .Include(e => e.Project)
                    .Include(e => e.Employee)
                    .Where(e => e.Project.Id == ProjectId && e.Employee.Id == employee)
                    .FirstOrDefaultAsync() is null)
                    return true;
            }
            return false;
        }

        private static (bool, string?) TaskInformationEmpty(string Name, uint[] Employees)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name))
            {
                return (true, "Task name is empty");
            }
            else if(Employees.Length == 0)
            {
                return (true, "Task must have at least one employee");
            }
            else
            {
                return (false, null);
            }
        }

        private static (bool, string?) FinanceInformationEmpty(string Name)
        {
            if(string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name))
            {
                return (true, "Finance name is empty");
            }
            else
            {
                return (false, null);
            }
        }

        private static bool InvalidStatementType(byte statementType)
        {
            try
            {
                Enums.StatementType validStatementType = (Enums.StatementType)statementType;
                return false;
            }
            catch
            {
                return true;
            }
        }

        private static bool StatementDate(DateOnly datum)
        {
            if (datum.Month == DateTime.Now.Month)
                return false;
            else
                return true;
        }

        private static bool StatementHours(byte pocetHodin)
        {
            if (pocetHodin > 0 && pocetHodin <= 24)
                return false;
            else
                return true;
        }
    }
}
