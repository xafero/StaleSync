namespace StaleSync.Core
{
    public sealed class TipArg
    {
        public int Timeout { get; set; } = 2000;
        public string Title { get; set; } = nameof(StaleSync);
        public string Text { get; set; } = "?";
    }
}