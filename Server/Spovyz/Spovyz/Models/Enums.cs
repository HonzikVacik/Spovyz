namespace Spovyz.Models
{
    public class Enums
    {
        public enum Role
        {
            [RoleCesky("Majitel")] Owner,
            [RoleCesky("Ředitel")] Manager,
            [RoleCesky("Vedoucí")] Supervisor,
            [RoleCesky("Zaměstnanec")] Worker,
            [RoleCesky("Účetní")] Accountant,
            [RoleCesky("Administrátor")] Admin
        }

        public enum StatementType
        {
            [RoleCesky("Projekt")] Project,
            [RoleCesky("Task")] Task,
            [RoleCesky("Administrace")] Administration,
            [RoleCesky("Školení")] Training,
            [RoleCesky("Služební cesta")] BusinessTrip,
            [RoleCesky("Volno")] FreeTime,
            [RoleCesky("Dovolená")] Holiday
        }

        public enum ProjectStatus
        {
            [RoleCesky("Nový")] New,
            [RoleCesky("Aktivní")] Active,
            [RoleCesky("Vyřešený")] Resolved,
            [RoleCesky("Uzavřený")] Deleted
        }

        public enum Sex
        {
            [RoleCesky("Muž")] Man,
            [RoleCesky("Žena")] Woman,
            [RoleCesky("Nebinární")] Nonbinary,
            [RoleCesky("Jiné")] Other
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
