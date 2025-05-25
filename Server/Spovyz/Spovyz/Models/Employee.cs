namespace Spovyz.Models
{
    public class Employee
    {
        public uint Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte[] Salt { get; set; }
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
        public Employee? Supervisor { get; set; }
        public uint Pay { get; set; }
        public Company Company { get; set; }
        public bool NeedResetPassword { get; set; }
        public ICollection<Project_employee> Project_employees { get; set; }
    }
}
