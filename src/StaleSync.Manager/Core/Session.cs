using System.Collections.Generic;
using StaleSync.Proto;

namespace StaleSync.Manager.Core
{
    public sealed class Session
    {
        public Queue<Message> ReadQueue { get; } = new();
        public Queue<Message> WriteQueue { get; } = new();
    }
}