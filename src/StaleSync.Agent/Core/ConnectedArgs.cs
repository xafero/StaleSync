using System;
using StaleSync.Proto;

namespace StaleSync.Core
{
    public sealed class ConnectedArgs : EventArgs
    {
        public string ServerId { get; set; }

        public ConnKind Kind { get; set; }
    }
}