using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Websocketsteste.SOCKETMANAGER;
using Websocketsteste.GAMECORE;
using Newtonsoft.Json;
using Websocketsteste.MODELS;

namespace Websocketsteste.HANDLERS
{
    public class WebSocketMessageHandler : SocketHandler
    {
        public WebSocketMessageHandler(ConnectionManager connections) : base(connections)
        {

        }

        public override async Task OnConnected(WebSocket socketInserido)
        {
            await base.OnConnected(socketInserido);
            var socketId = connections.PegarIdPorSocket(socketInserido);
            //await SendMessageToAll($"******* {nomeDoUsuario} entrou no chat. ******");
            Jogador jogadorEncontrado = Jogador.ProcurarJogadorConectadoPorWebSocket(socketInserido);
            JogadorModel jogadorModelo = new JogadorModel(jogadorEncontrado.idJogador, jogadorEncontrado.sessaoAtual.idSessao, jogadorEncontrado.nomeJogador, jogadorEncontrado.coordJogadorX, jogadorEncontrado.coordJogadorY, jogadorEncontrado.areaAtual.posicaoMatriz.x, jogadorEncontrado.areaAtual.posicaoMatriz.y, Nucleo.ChecarQtdJogadoresSessao(jogadorEncontrado.sessaoAtual));
            var message1 = JsonConvert.SerializeObject(JogadorModel.ConverterJogadoresSessaoModel(jogadorEncontrado));
            var message2 = JsonConvert.SerializeObject(CelulaModel.ConverterParaCelulaModel(jogadorEncontrado.areaAtual.dungeonInserida));
            var message3 = JsonConvert.SerializeObject(JogadorModel.ModelJogadorDaSessao(jogadorEncontrado));
            //
            string nomeDoUsuario = jogadorEncontrado.nomeJogador;
            await SendMessageToSession(message1, jogadorEncontrado.sessaoAtual);
            await SendMessage(jogadorEncontrado.socketDoJogador, message2);
            await SendMessage(jogadorEncontrado.socketDoJogador, message3);



        }
        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            Console.WriteLine("Entrou no Receive");
            Jogador jogadorEncontrado = Jogador.ProcurarJogadorConectadoPorWebSocket(socket);
            Nucleo.ChecarInput(Encoding.UTF8.GetString(buffer, 0, result.Count), jogadorEncontrado);
            Console.WriteLine("Passou pelo Checar Input");
            JogadorModel jogadorModelo = new JogadorModel(jogadorEncontrado.idJogador, jogadorEncontrado.sessaoAtual.idSessao, jogadorEncontrado.nomeJogador, jogadorEncontrado.coordJogadorX, jogadorEncontrado.coordJogadorX, jogadorEncontrado.areaAtual.posicaoMatriz.x, jogadorEncontrado.areaAtual.posicaoMatriz.y, Nucleo.ChecarQtdJogadoresSessao(jogadorEncontrado.sessaoAtual));
            var message1 = JsonConvert.SerializeObject(JogadorModel.ConverterJogadoresSessaoModel(jogadorEncontrado));
            var message2 = JsonConvert.SerializeObject(CelulaModel.ConverterParaCelulaModel(jogadorEncontrado.areaAtual.dungeonInserida));
            var message3 = JsonConvert.SerializeObject(JogadorModel.ModelJogadorDaSessao(jogadorEncontrado));
            //
            string nomeDoUsuario = jogadorEncontrado.nomeJogador;
            await SendMessageToSession(message1, jogadorEncontrado.sessaoAtual);
            await SendMessage(jogadorEncontrado.socketDoJogador, message2);
            await SendMessage(jogadorEncontrado.socketDoJogador, message3);
            Console.WriteLine("Fim do Receive.");

        }
        public override async Task OnDisconnected(WebSocket socketInserido)
        {   
            var socketId = connections.PegarIdPorSocket(socketInserido);
            string nomeDoUsuario = Jogador.ProcurarJogadorConectadoPorWebSocket(socketInserido).nomeJogador;
            await base.OnDisconnected(socketInserido);
            //await SendMessageToAll($"{nomeDoUsuario} saiu do chat. ),:");
        }

       
    }
}
