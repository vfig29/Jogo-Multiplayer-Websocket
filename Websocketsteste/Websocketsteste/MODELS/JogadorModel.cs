using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Websocketsteste.COREDG;
using Websocketsteste.GAMECORE;

namespace Websocketsteste.MODELS
{
    public class JogadorModel
    {

        public JogadorModel(Guid idJogador_, Guid idSessaoAtual_, string nomeJogador_, int coordxJog_, int coordyJog_, int areaAtualx_, int areaAtualy_, int qtdAtualJogadoresSessao)
        {
            tipoMensagem = "jogadores";
            idJogadorModel = Guid.NewGuid();
            idJogador = idJogador_;
            idSessaoAtual = idSessaoAtual_;
            nomeJogador = nomeJogador_;
            coordxJog = coordxJog_;
            coordyJog = coordyJog_;
            areaAtualx = areaAtualx_;
            areaAtualy = areaAtualy_;
            qtdJogadoresSessao = qtdAtualJogadoresSessao.ToString() + "/" + Config.maxJogadoresSessao.ToString();
        }


    public static List<JogadorModel> ConverterJogadoresSessaoModel(Jogador jogadorInserido)
        {
            List<JogadorModel> listaJogadores = new List<JogadorModel>();

            listaJogadores.Add(new JogadorModel(jogadorInserido.idJogador, jogadorInserido.sessaoAtual.idSessao, jogadorInserido.nomeJogador, jogadorInserido.coordJogadorX, jogadorInserido.coordJogadorY, jogadorInserido.areaAtual.posicaoMatriz.x, jogadorInserido.areaAtual.posicaoMatriz.y, Nucleo.ChecarQtdJogadoresSessao(jogadorInserido.sessaoAtual)));



            foreach (Jogador j in jogadorInserido.sessaoAtual.jogadoresNaSessao)
            {
                if (j != null && j != jogadorInserido)
                {
                    listaJogadores.Add(new JogadorModel(j.idJogador, j.sessaoAtual.idSessao, j.nomeJogador, j.coordJogadorX, j.coordJogadorY, j.areaAtual.posicaoMatriz.x, j.areaAtual.posicaoMatriz.y, Nucleo.ChecarQtdJogadoresSessao(j.sessaoAtual)));
                }
            }

            return listaJogadores;
        }

        public static List<JogadorModel> ModelJogadorDaSessao(Jogador jogadorInserido)
        {
            List<JogadorModel> listaJogadores = new List<JogadorModel>();
            JogadorModel jogadorEnviado = new JogadorModel(jogadorInserido.idJogador, jogadorInserido.sessaoAtual.idSessao, jogadorInserido.nomeJogador, jogadorInserido.coordJogadorX, jogadorInserido.coordJogadorY, jogadorInserido.areaAtual.posicaoMatriz.x, jogadorInserido.areaAtual.posicaoMatriz.y, Nucleo.ChecarQtdJogadoresSessao(jogadorInserido.sessaoAtual));
            jogadorEnviado.tipoMensagem = "jogadorDoClient";
            listaJogadores.Add(jogadorEnviado);

            return listaJogadores;
        }

        public string tipoMensagem { get; set; }
        public Guid idJogadorModel { get; }
        public Guid idJogador { get; }

        public Guid idSessaoAtual { get; }

        public string nomeJogador { get; set; }

        public int coordxJog { get; set; }

        public int coordyJog { get; set; }

        public int areaAtualx { get; set; }

        public int areaAtualy { get; set; }

        public string qtdJogadoresSessao { get; set; }

    }
}
