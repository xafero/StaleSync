using System;
using System.Drawing;

namespace StaleSync.Resources
{
    internal static class ResLoader
    {
        public static Icon GetIcon(string name)
        {
            var type = typeof(ResLoader);
            var asm = type.Assembly;
            var fqn = $"{type.Namespace}.{name}";
            using var stream = asm.GetManifestResourceStream(fqn);
            if (stream == null)
                throw new InvalidOperationException(fqn);
            return new Icon(stream);
        }
    }
}