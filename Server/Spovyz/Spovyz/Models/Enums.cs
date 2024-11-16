namespace Spovyz.Models
{
    public class Enums
    {
        public enum Role
        {
            Owner,
            Manager,
            Supervisor,
            Worker,
            Accountant
        }

        public enum StatementType
        {
            Project,
            Task,
            Administration,
            Training,
            BusinessTrip,
            FreeTime,
            Holiday
        }

        public enum Sex
        {
            Man,
            Woman,
            Nonbinary,
            Other
        }

        public enum Month
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }
    }
}
