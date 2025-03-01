namespace StaleSync.Proto
{
    public sealed class Announce : Payload
    {
        public string Id { get; set; }

        public CommKind Kind { get; set; }
    }
}