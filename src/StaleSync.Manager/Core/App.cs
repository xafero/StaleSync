using Cfg = StaleSync.Proto.ConfigFile<StaleSync.Manager.Core.Config>;

namespace StaleSync.Manager.Core
{
    public static class App
    {
        public static void Run()
        {
            Cfg.Load();
            var cfg = Cfg.Config;

            var server = Server.Instance;
            server.Listen(cfg);
        }
    }
}