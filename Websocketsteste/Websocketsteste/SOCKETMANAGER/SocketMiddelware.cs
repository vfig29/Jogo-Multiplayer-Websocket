using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using Websocketsteste.HANDLERS;
using Websocketsteste.GAMECORE;

namespace Websocketsteste.SOCKETMANAGER
{
    public class SocketMiddelware
    {
        private readonly RequestDelegate _next;

        public SocketMiddelware(RequestDelegate next, SocketHandler handler)
        {
            _next = next;
            this.handler = handler;
        }

        private SocketHandler handler { get; set; }

        public async Task InvokeAsync(HttpContext context)
        {

            if (!context.WebSockets.IsWebSocketRequest)
            {
                return;
            }
            var socket = await context.WebSockets.AcceptWebSocketAsync();
            await handler.OnConnected(socket);
            await Receive(socket, async (result, buffer) => 
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    await handler.Receive(socket, result, buffer);
                }
                else if(result.MessageType == WebSocketMessageType.Close)
                {
                    await handler.OnDisconnected(socket);
                }
            
            
            
            }
            );
        }

        private async Task Receive(WebSocket webSocket, Action<WebSocketReceiveResult, byte[]> messageHandler)
        {
            // tamanho original do buffer 4*1024
            var buffer = new byte[1024];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                messageHandler(result, buffer);
            }
        }
    }
}
