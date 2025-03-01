using System.IO;
using static StaleSync.Proto.CollTool;
using static StaleSync.Proto.JsonTool;

namespace StaleSync.Proto
{
    public static class Protocol
    {
        public static Announce Announce(string id, CommKind kind)
        {
            return new Announce
            {
                Id = id, Kind = kind
            };
        }

        public static Message Wrap(Payload payload)
        {
            var msg = new Message
            {
                Payload = payload
            };
            return msg;
        }

        public static bool Read(StreamReader reader, out Message msg)
        {
            if (ReadLineTrim(reader) is { } json)
            {
                msg = FromJson<Message>(json);
                return true;
            }
            msg = null;
            return false;
        }

        public static bool Write(StreamWriter writer, Message msg)
        {
            if (ToJson(msg) is { } json)
            {
                writer.WriteLine(json);
                writer.Flush();
                return true;
            }
            return false;
        }
    }
}