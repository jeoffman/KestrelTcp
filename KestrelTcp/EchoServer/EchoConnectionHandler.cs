﻿using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace KestrelTcp.EchoServer
{
    public class EchoConnectionHandler : ConnectionHandler
    {
        private readonly ILogger<EchoConnectionHandler> _logger;

        public EchoConnectionHandler(ILogger<EchoConnectionHandler> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync(ConnectionContext connection)
        {
            _logger.LogInformation(connection.ConnectionId + " connected");

            while (true)
            {
                ReadResult result = await connection.Transport.Input.ReadAsync();
                ReadOnlySequence<byte> buffer = result.Buffer;

                foreach (ReadOnlyMemory<byte> segment in buffer)
                {
                    await connection.Transport.Output.WriteAsync(segment);
                }

                if (result.IsCompleted)
                    break;

                connection.Transport.Input.AdvanceTo(buffer.End);
            }

            _logger.LogInformation(connection.ConnectionId + " disconnected");
        }
    }
}