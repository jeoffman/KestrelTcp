using System;
using System.Buffers;

namespace KestrelTcp.MessageServer
{
    public class MessageParser : IMessageParser
    {
        public bool TryParseMessage(ref ReadOnlySequence<byte> buffer, out Message message)
        {
            // TODO: Implement logic here
            throw new NotImplementedException();
        }
    }
}