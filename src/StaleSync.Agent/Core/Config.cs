// ReSharper disable InconsistentNaming

namespace StaleSync.Core
{
    public class Config
    {
        public string HostIP { get; set; }

        public int HostRead { get; set; }

        public int HostWrite { get; set; }

        public int Reconnect { get; set; }

        public int Poll { get; set; }
    }
}