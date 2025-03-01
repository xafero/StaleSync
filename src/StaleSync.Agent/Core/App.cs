using StaleSync.Proto;

namespace StaleSync.Core
{
    public static class App
    {
        public static void Run()
        {
            ConfigFile<Config>.Load();
            var cfg = ConfigFile<Config>.Config;

            var client = Client.Instance;
            client.Connect(cfg);
        }
    }
}