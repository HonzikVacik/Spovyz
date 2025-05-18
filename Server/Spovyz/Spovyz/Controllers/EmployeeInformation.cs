using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spovyz.IRepositories;
using Spovyz.Models;
using Spovyz.Transport_models;
using System.Diagnostics.Metrics;
using System.IO;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Threading.Tasks;
using static Spovyz.Models.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Spovyz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeInformation : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAccountingRepository _accountingRepository;

        public EmployeeInformation(ApplicationDbContext context, IEmployeeRepository employeeRepository, IAccountingRepository accountingRepository)
        {
            this._context = context;
            this._employeeRepository = employeeRepository;
            this._accountingRepository = accountingRepository;
        }

        [HttpGet("GetAllEmployees")]
        [Authorize]
        public async Task<IActionResult> GetAllEmployees()
        {
            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());
            Employee[] employees = await _employeeRepository.GetAllEmployees(activeUser.Company.Id);
            List<Transport_models.NameBasic> employeeList = new List<Transport_models.NameBasic>();
            foreach (var employee in employees)
            {
                employeeList.Add(new NameBasic()
                {
                    Id = employee.Id,
                    Name = employee.Username
                });
            }
            return Ok(employeeList);
        }

        [HttpGet("GetEmployeesSalary")]
        [Authorize]
        public async Task<IActionResult> GetEmployeesSalary()
        {
            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());
            EmployeeSalary[] employeeSalaries = await _employeeRepository.EmployeeSalary(activeUser.Company.Id);
            return Ok(employeeSalaries);
        }

        [HttpGet("UpdateEmployeeSalary")]
        [Authorize]
        public async Task<IActionResult> UpdateEmployeeSalary(uint id, uint salary)
        {
            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());
            if(ValidityControl.Check_salary(salary) == false)
                return BadRequest("Salary must be positive number");
            await _employeeRepository.UpdateEmployeeSalary(activeUser.Company.Id, id, salary);
            string? error = await _accountingRepository.UpdateMonthSalary(activeUser.Company.Id, id, salary);
            if (error != null)
                return BadRequest(error);
            return Ok();
        }

        // GET: api/<EmployeeInformation>
        [HttpGet]
        [Authorize]
        public IEnumerable<AdminDashboardData> Get()
        {
            uint i = 0;
            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());
            Employee[] employees = [.. _context.Employees
                .Include(e => e.Company)
                .Where(e => e.Company.Id == activeUser.Company.Id)
                .ToArray()];
            List<AdminDashboardData> data = employees.Select(e => new AdminDashboardData{ Id = i++, Username = e.Username, NeedResetPassword = e.NeedResetPassword}).ToList();
            return data;
        }


        [HttpGet("GetSelf")]
        [Authorize]
        public IActionResult GetSelf()
        {
            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());
            string supervisor = "";
            if(activeUser.Supervisor != null)
                supervisor = activeUser.Supervisor.Id.ToString();
            EmployeeInformationData data = new EmployeeInformationData()
            {
                Username = activeUser.Username,
                SecurityVerification = activeUser.SecurityVerification,
                First_name = activeUser.First_name,
                Surname = activeUser.Surname,
                Phone_number = activeUser.Phone_number,
                Email = activeUser.Email,
                Date_of_birth = activeUser.Date_of_birth,
                Sex = activeUser.Sex,
                Pronoun = activeUser.Pronoun,
                Country = activeUser.Country,
                City = activeUser.City,
                Zip_code = activeUser.Zip_code,
                Street = activeUser.Street,
                Descriptive_number = activeUser.Descriptive_number,
                Account_type = activeUser.Account_type,
                Supervisor = supervisor
            };
            return Ok(data);
        }

        [HttpGet("GetSelfId")]
        [Authorize]
        public IActionResult GetSelfId()
        {
            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());
            return Ok(activeUser.Id);
        }

        [HttpGet("GetName/{id}")]
        [Authorize]
        public IActionResult GetName(int id)
        {
            string error = "e1";
            string username = "";

            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());
            Employee[] employees = [.. _context.Employees
                .Include(e => e.Company)
                .Where(e => e.Company.Id == activeUser.Company.Id)
                .ToArray()];
            if (id < 0 || id >= employees.Length)
                return Ok(new { error });
            return Ok(new { username = employees[id].Username });
        }

        // GET api/<EmployeeInformation>/5
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Get(int id)
        {
            string supervisor = "0";
            string error = "e1";

            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());
            Employee[] employees = [.. _context.Employees
                .Include(e => e.Company)
                .Where(e => e.Company.Id == activeUser.Company.Id)
                .ToArray()];
            if (id < 0 || id >= employees.Length)
                return Ok(new { error });
            Employee result = employees[id];
            EmployeeInformationData data = new EmployeeInformationData()
            {
                Username = result.Username,
                SecurityVerification = result.SecurityVerification,
                First_name = result.First_name,
                Surname = result.Surname,
                Phone_number = result.Phone_number,
                Email = result.Email,
                Date_of_birth = result.Date_of_birth,
                Sex = result.Sex,
                Pronoun = result.Pronoun,
                Country = result.Country,
                City = result.City,
                Zip_code = result.Zip_code,
                Street = result.Street,
                Descriptive_number = result.Descriptive_number,
                Account_type = result.Account_type,
                Supervisor = supervisor
            };
            return Ok(data);
        }

        // POST api/<EmployeeInformation>
        [HttpPost]
        [Authorize]
        public IActionResult Post(string username, string password, string securityVerification, string firstName, string surname, string phoneNumber, string email, DateOnly dateOfBirth, int sex, string pronoun, string country, string city, int zipCode, string street, uint decNumber, int accountType, int supervisorId)
        {
            string accept = "a";

            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());

            if (!ValidityControl.AllFilledOut_EI(username, password, securityVerification, firstName, surname, phoneNumber, email, dateOfBirth, sex, pronoun, country, city, zipCode, street, decNumber, accountType))
                return Ok("e1");

            string validityControlString = ValidityControl.Check_EI(_context, activeUser.Company.Id, username, password, securityVerification, accountType, sex, email, phoneNumber, dateOfBirth, true, true, true);
            if (validityControlString != "a")
                return Ok(validityControlString);

            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            Employee employee = new Employee()
            {
                Username = username,
                Password = hashedPassword,
                Salt = salt,
                SecurityVerification = securityVerification,
                First_name = firstName,
                Surname = surname,
                Phone_number = phoneNumber,
                Email = email,
                Date_of_birth = dateOfBirth,
                Sex = (Enums.Sex)sex,
                Pronoun = pronoun,
                Country = country,
                City = city,
                Zip_code = zipCode,
                Street = street,
                Descriptive_number = decNumber,
                Account_type = (Enums.Role)accountType,
                Supervisor = null,
                Pay = 0,
                Company = activeUser.Company
            };

            _context.Add(employee);
            _context.SaveChanges();

            return Ok(accept);
        }

        // PUT api/<EmployeeInformation>/5
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, string username, string? password, string securityVerification, string firstName, string surname, string phoneNumber, string email, DateOnly dateOfBirth, int sex, string pronoun, string country, string city, int zipCode, string street, uint decNumber, int accountType, int supervisorId)
        {
            string error1 = "e1";
            string error4 = "e4";
            string accept = "a";
            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());

            Employee[] employees = [.. _context.Employees
                .Include(e => e.Company)
                .Where(e => e.Company.Id == activeUser.Company.Id)
                .ToArray()];
            if (id < 0 || id >= employees.Length)
                return Ok(error4);
            Employee result = employees[id];
            
            byte[] salt = result.Salt;
            string hashedPassword = "";
            bool controlUsername = true;
            bool controlPassword = true;
            bool controlSecurityVerification = true;

            if(string.IsNullOrEmpty(password))
            {
                if (!ValidityControl.PutAllFilledOut_EI(username, securityVerification, firstName, surname, phoneNumber, email, dateOfBirth, sex, pronoun, country, city, zipCode, street, decNumber, accountType))
                    return Ok(error1);
                hashedPassword = result.Password;
                controlPassword = false;
            }
            else
            {
                if (!ValidityControl.AllFilledOut_EI(username, password, securityVerification, firstName, surname, phoneNumber, email, dateOfBirth, sex, pronoun, country, city, zipCode, street, decNumber, accountType))
                    return Ok(error1);
            }
            if(username == result.Username)
                controlUsername = false;
            if (securityVerification == result.SecurityVerification)
                controlSecurityVerification = false;

            string validityControlString = ValidityControl.Check_EI(_context, activeUser.Company.Id, username, password, securityVerification, accountType, sex, email, phoneNumber, dateOfBirth, controlUsername, controlPassword, controlSecurityVerification);
            if (validityControlString != "a")
                return Ok(validityControlString);

            if(controlPassword)
                hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password!,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));


            result.Username = username;
            result.Password = hashedPassword;
            result.Salt = salt;
            result.SecurityVerification = securityVerification;
            result.First_name = firstName;
            result.Surname = surname;
            result.Phone_number = phoneNumber;
            result.Email = email;
            result.Date_of_birth = dateOfBirth;
            result.Sex = (Enums.Sex)sex;
            result.Pronoun = pronoun;
            result.Country = country;
            result.City = city;
            result.Zip_code = zipCode;
            result.Street = street;
            result.Descriptive_number = decNumber;
            result.Account_type = (Enums.Role)accountType;
            result.Supervisor = null;
            result.Pay = 0;
            result.Company = activeUser.Company;

            _context.Update(result);
            _context.SaveChanges();

            return Ok(accept);
        }

        [HttpPut("ResetPassword/{id}")]
        [Authorize]
        public IActionResult Put(int id,string password)
        {
            string error = "e1";
            string accept = "a";

            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());
            Employee[] employees = [.. _context.Employees
                .Include(e => e.Company)
                .Where(e => e.Company.Id == activeUser.Company.Id)
                .ToArray()];
            if (id < 0 || id >= employees.Length)
                return Ok(new { error });
            Employee e = employees[id];
            if (!string.IsNullOrEmpty(password))
            {
                if (!ValidityControl.Password(password))
                    return Ok(new { error = "e2" });
                else
                {
                    string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: password!,
                        salt: e.Salt,
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 100000,
                        numBytesRequested: 256 / 8));
                    e.Password = hashedPassword;
                    e.NeedResetPassword = false;
                    _context.Update(e);
                    _context.SaveChanges();
                    return Ok(new { accept });
                }
            }
            else
                return Ok(new { error = "e2" });
        }

        [HttpPut("ChangePassword")]
        public IActionResult ChangePassword(string username, string securityVerification)
        {
            string error = "e1";
            string accept = "a";

            Employee[] employees = [.. _context.Employees
                .Where(e => e.Username == username && e.SecurityVerification == securityVerification)
                .ToArray()];
            if (employees.Length != 0)
            {
                Employee employee = employees[0];
                employee.NeedResetPassword = true;
                _context.Update(employee);
                _context.SaveChanges();
                return Ok(accept);
            }
            else
                return Ok(error);
        }

        // DELETE api/<EmployeeInformation>/5
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            string error = "e1";
            string accept = "a";
            Employee activeUser = _context.Employees.Include(e => e.Company).FirstOrDefault(e => e.Username == User.Identity.Name.ToString());

            Employee[] employees = [.. _context.Employees
                .Include(e => e.Company)
                .Where(e => e.Company.Id == activeUser.Company.Id)
                .ToArray()];
            if (id < 0 || id >= employees.Length)
                return Ok(error);
            Employee result = employees[id];
            _context.Remove(result);
            _context.SaveChanges();
            return Ok(accept);
        }
    }
}
