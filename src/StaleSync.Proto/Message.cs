namespace StaleSync.Proto
{
    public sealed class Message
    {
        public Payload Payload { get; set; }
    }

    public abstract class Payload
    {
    }

    public sealed class Announce : Payload
    {
        public string Id { get; set; }
    }
}