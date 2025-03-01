using System;
using System.IO;

namespace StaleSync.Proto
{
    public static class Reflect
    {
        public static string GetTypeDir(Type type)
        {
            var asm = type.Assembly;
            var dll = Path.GetFullPath(asm.Location);
            var dir = Path.GetDirectoryName(dll);
            return dir;
        }
    }
}