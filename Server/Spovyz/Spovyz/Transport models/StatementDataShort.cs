namespace Spovyz.Transport_models
{
    public class StatementDataShort
    {
        public uint Id { get; set; }
        public DateOnly Datum { get; set; }
        public byte PocetHodin { get; set; }
        public string? Description { get; set; }
    }
}
