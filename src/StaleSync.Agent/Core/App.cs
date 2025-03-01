using Cfg = StaleSync.Proto.ConfigFile<StaleSync.Core.Config>;

namespace StaleSync.Core
{
    public static class App
    {
        public static TipShower ShowTip { get; set; }

        public static void Run()
        {
            Cfg.Load();
            var cfg = Cfg.Config;

            var client = Client.Instance;
            client.Connected += OnConnected;
            client.Connect(cfg);
        }

        public static void OnConnected(object sender, ConnectedArgs e)
        {
            var kind = e.Kind.ToString().ToLower();
            ShowTip(new TipArg
            {
                Title = $"Server ({e.ServerId})",
                Text = $"Connected for {kind}!",
                Timeout = 400
            });
        }
    }
}