using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Websocketsteste.GAMECORE;

namespace Websocketsteste.SOCKETMANAGER
{
    public class ConnectionManager
    {
        private ConcurrentDictionary<Guid, WebSocket> _connections = new ConcurrentDictionary<Guid, WebSocket>();
        public WebSocket PegarSocketPorId(Guid id)
        {
            return _connections.FirstOrDefault(x => x.Key == id).Value;
        }

        public ConcurrentDictionary<Guid, WebSocket> PegarTodasAsConexoes()
        {
            return _connections;
        }

        public Guid PegarIdPorSocket(WebSocket socketInserido)
        {
            return _connections.FirstOrDefault(x => x.Value == socketInserido).Key;
        }

        public async Task RemoverSocketAsync(Guid id)
        {
            _connections.TryRemove(id, out var socket);         
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "socket connection closed", CancellationToken.None);
        }

        public void AdicionarSocket(WebSocket socketInserido)
        {
            Guid id;
            _connections.TryAdd(id = GerarUmaIdParaConexao(), socketInserido);
            Jogador.ValidarUmJogadorComSocket(socketInserido);
        }

        

        private Guid GerarUmaIdParaConexao()
        {
            return Guid.NewGuid();
        }

        


    }
}
