using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Websocketsteste.COREDG;
using System.Net.WebSockets;

namespace Websocketsteste.GAMECORE
{

    public class Jogador
    {
        private static int contadorConvidados;

        public Jogador(WebSocket socketDoJogador_)
        {
            idJogador = Guid.NewGuid();
            nomeJogador = GerarNome();
            socketDoJogador = socketDoJogador_;
            moveDelayReference = Config.moveDelayReferenceInicial;
            moveDelay = moveDelayReference;
            moveSpeed = Config.moveSpeedInicial;


        }

        public static void ValidarUmJogadorComSocket(WebSocket socketInserido)
        {
            Jogador jogadorInstanciado = new Jogador(socketInserido);
            //Falta validar com o retorno da funcao, pra caso todas as sessões estejam ocupadas e o retorno seja falso.
            Nucleo.AlocarEmSessao(jogadorInstanciado);

        }

        public static Jogador ProcurarJogadorConectadoPorWebSocket(WebSocket socketInserido)
        {
            foreach (Jogador j in Nucleo.jogadoresConectados)
            {
                if (j.socketDoJogador == socketInserido)
                {
                    return j;
                }
            }

            return null;

        }

        public static async Task RemoverJogadorConectado(Jogador jogadorInserido)
        {
            //Remove da lista de jogadores conectados
            foreach (Jogador j in Nucleo.jogadoresConectados)
            {
                if (j == jogadorInserido)
                {
                    Nucleo.jogadoresConectados.Remove(j);
                    break;
                }
            }
            //Remove do array de jogadores da Sessao:
            for (int i = 0; i < Config.maxJogadoresSessao; i++)
            {
                if (jogadorInserido.sessaoAtual.jogadoresNaSessao[i] == jogadorInserido)
                {
                    jogadorInserido.sessaoAtual.jogadoresNaSessao[i] = null;
                    break;
                }
            }

        }

        public string GerarNome()
        {
            string nomeGerado = "Convidado " + contadorConvidados;
            contadorConvidados++;

            return nomeGerado;
        }


        public Guid idJogador { get; }
        public string nomeJogador { get; set; }
        public int coordJogadorX { get; set; }

        public int coordJogadorY { get; set; }

        public Area areaAtual { get; set; }

        public Sessao sessaoAtual { get; set; }

        public bool estaConectado { get; set; }

        public WebSocket socketDoJogador { get; set; }

        public int moveDelayReference { get; set; }

        public int moveDelay { get; set; }

        public float moveSpeed { get; set; }





    }

}
