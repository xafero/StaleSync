using System;

namespace StaleSync.Manager.Core
{
    public static class Log
    {
        public static void WriteLine(string text)
        {
            _append(text);
        }

        private static Action<string> _append;

        public static void SetAction(Action<string> append)
        {
            _append = append;
        }
    }
}