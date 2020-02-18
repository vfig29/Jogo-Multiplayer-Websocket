using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Websocketsteste.SOCKETMANAGER;
using Websocketsteste.GAMECORE;

namespace Websocketsteste.HANDLERS
{
    public abstract class SocketHandler
    {
        public ConnectionManager connections { get; set; }

        public SocketHandler(ConnectionManager connections_)
        {
            connections = connections_;

        }

        public virtual async Task OnConnected(WebSocket socketInserido)
        {
            await Task.Run(() => { 
                connections.AdicionarSocket(socketInserido);           
            });
        }

        public virtual async Task OnDisconnected(WebSocket socketInserido)
        {
            Jogador.RemoverJogadorConectado(Jogador.ProcurarJogadorConectadoPorWebSocket(socketInserido));
            await connections.RemoverSocketAsync(connections.PegarIdPorSocket(socketInserido));
        }
        public async Task SendMessage(WebSocket socketInserido, string mensagem)
        {
            if(socketInserido.State != WebSocketState.Open)
            {
                return;
            }
            await socketInserido.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(mensagem), 0, mensagem.Length), WebSocketMessageType.Text, true,CancellationToken.None);
        }

        public async Task SenddMessage(Guid id, string message)
        {
            await SendMessage(connections.PegarSocketPorId(id), message);
        }

        public async Task SendMessageToAll(string message)
        {
            foreach (var con in connections.PegarTodasAsConexoes())
            {
                await SendMessage(con.Value, message);
            }
        }

        public async Task SendMessageToSession(string message, Sessao sessaoAtual)
        {
            foreach (Jogador jogador in sessaoAtual.jogadoresNaSessao)
            {
                if (jogador != null)
                {
                    await SendMessage(jogador.socketDoJogador, message);
                }              
            }
        }

        public abstract Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);

    }
}
