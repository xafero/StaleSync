using Cfg = StaleSync.Proto.ConfigFile<StaleSync.Core.Config>;

namespace StaleSync.Core
{
    public static class App
    {
        public static void Run()
        {
            Cfg.Load();
            var cfg = Cfg.Config;

            var client = Client.Instance;
            client.Connect(cfg);
        }
    }
}