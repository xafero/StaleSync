using System.Collections.Generic;
using System.IO;

namespace StaleSync.Proto
{
    public static class CollTool
    {
        public static string TrimOrNull(string text)
        {
            return text == null || (text = text.Trim()).Length < 1 ? null : text;
        }

        public static string ReadLineTrim(StreamReader reader)
        {
            var line = reader.ReadLine();
            return TrimOrNull(line);
        }

        public static T TryDequeue<T>(Queue<T> queue)
        {
            if (queue.Count < 1)
                return default;
            var item = queue.Dequeue();
            return item;
        }
    }
}