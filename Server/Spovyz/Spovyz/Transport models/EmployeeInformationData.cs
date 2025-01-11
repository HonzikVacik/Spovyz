using Spovyz.Models;

namespace Spovyz.Transport_models
{
    public class EmployeeInformationData
    {
        public string Username { get; set; }
        public string SecurityVerification { get; set; }
        public string First_name { get; set; }
        public string Surname { get; set; }
        public string Phone_number { get; set; }
        public string Email { get; set; }
        public DateOnly Date_of_birth { get; set; }
        public Enums.Sex Sex { get; set; }
        public string Pronoun { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int Zip_code { get; set; }
        public string Street { get; set; }
        public uint Descriptive_number { get; set; }
        public Enums.Role Account_type { get; set; }
        public string Supervisor { get; set; }
    }
}
