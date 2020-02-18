using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Websocketsteste.COREDG;

namespace Websocketsteste.GAMECORE
{
    
    public class Sessao
    {
        public Sessao()
        {
            idSessao = Guid.NewGuid();
            zonaDaSessao = new Zona();

        }

        public Guid idSessao { get; }
        public Jogador[] jogadoresNaSessao = new Jogador[Config.maxJogadoresSessao];
        public Zona zonaDaSessao;


    }
}
