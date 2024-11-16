namespace Spovyz.Models
{
    public class Employee
    {
        public required uint Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string First_name { get; set; }
        public required string Surname { get; set; }
        public required string Phone_number { get; set; }
        public required string Email { get; set; }
        public required DateOnly Date_of_birth { get; set; }
        public required Enums.Sex Sex { get; set; }
        public required string Pronoun { get; set; }
        public required string Country { get; set; }
        public required string City { get; set; }
        public required int Zip_code { get; set; }
        public required string Street { get; set; }
        public required uint Descriptive_number { get; set; }
        public required Enums.Role Account_type { get; set; }
        public Employee? Supervisor { get; set; }
        public required uint Pay { get; set; }
        public required Company CompanyId { get; set; }
    }
}
