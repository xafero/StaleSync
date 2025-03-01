using System.IO;
using System.Text;

namespace StaleSync.Proto
{
    public static class ConfigFile<T>
    {
        public static T Config { get; private set; }

        public static void Load(string file = "appCfg.json")
        {
            var dir = Reflect.GetTypeDir(typeof(Reflect));
            var path = Path.Combine(dir, file);
            var json = File.ReadAllText(path, Encoding.UTF8);
            var cfg = JsonTool.FromJson<T>(json);
            Config = cfg;
        }
    }
}