namespace StaleSync.Proto
{
    public static class Protocol
    {
        public static Announce Announce(string id)
        {
            return new Announce
            {
                Id = id
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
    }
}