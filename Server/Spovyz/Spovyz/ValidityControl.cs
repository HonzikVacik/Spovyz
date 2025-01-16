using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Spovyz.Models;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using static Spovyz.Models.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace Spovyz
{
    public class ValidityControl
    {
        public string Check(ApplicationDbContext _context, uint activeUserCompanyId, string username, string password, string securityVerification, int accountType, int sex, string email, string phoneNumber, DateOnly dateOfBirth, bool controlUsername, bool controlPassword, bool controlSecurityVerification)
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

        private bool Username(string username, uint activeUserCompanyId, ApplicationDbContext _context)
        {
            Employee[] e = [.. _context.Employees.Include(e => e.Company).Where(e => e.Username == username && e.Company.Id == activeUserCompanyId)];
            if (e == null || e.Length == 0)
                return true;
            else
                return false;
        }

        public bool Password(string password)
        {
            string pattern = @"(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^a-zA-Z\d])";
            if (Regex.IsMatch(password, pattern) && password.Length >= 8)
                return true;
            else
                return false;
        }

        private bool SecurityVerification(string securityVerification, string username, ApplicationDbContext _context)
        {
            Employee[] e = [.. _context.Employees.Include(e => e.Company).Where(e => e.Username == username && e.SecurityVerification == securityVerification)];
            if (e == null || e.Length == 0)
                return true;
            else
                return false;
        }

        private bool AccountType(int accountType)
        {
            if (accountType >= 0 && accountType < Enum.GetNames(typeof(Enums.Role)).Length)
                return true;
            else
                return false;
        }

        private bool Sex(int sex)
        {
            if (sex >= 0 && sex < Enum.GetNames(typeof(Enums.Sex)).Length)
                return true;
            else
                return false;
        }

        private bool DateOfBirth(DateOnly dateOfBirth)
        {
            if(dateOfBirth < DateOnly.FromDateTime(DateTime.Now.Date))
                return true;
            else
                return false;
        }

        private bool Email(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (Regex.IsMatch(email, pattern))
                return true;
            else
                return false;
        }

        private bool PhoneNumber(string phoneNumber)
        {
            string pattern = @"^\d+$";
            if (Regex.IsMatch(phoneNumber, pattern) && phoneNumber.Length == 9)
                return true;
            else
                return false;
        }

        public bool AllFilledOut(string username, string password, string securityVerification, string firstName, string surname, string phoneNumber, string email, DateOnly dateOfBirth, int sex, string pronoun, string country, string city, int zipCode, string street, uint decNumber, int accountType)
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

        public bool PutAllFilledOut(string username, string securityVerification, string firstName, string surname, string phoneNumber, string email, DateOnly dateOfBirth, int sex, string pronoun, string country, string city, int zipCode, string street, uint decNumber, int accountType)
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
    }
}
