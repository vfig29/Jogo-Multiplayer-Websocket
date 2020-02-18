using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Websocketsteste.COREDG;
using System.Net.WebSockets;


namespace Websocketsteste.GAMECORE
{
    public class Nucleo
    {
        static public List<Sessao> sessoesCriadas = new List<Sessao>();
        static public List<Jogador> jogadoresConectados = new List<Jogador>();
        static bool movimentando = false;


        public static async Task GameLoop()
        {

            foreach (Jogador j in jogadoresConectados)
            {
                await DecrementosPorTempo(j);
            }

        }

        public static async Task DecrementosPorTempo(Jogador jogadorDoMovimento)
        {
            jogadorDoMovimento.moveDelay--;
        }

        public static void ChecarInput(string mensagem, Jogador jogadorDoMovimento)
        {
            Console.WriteLine("Entrou no Checar Input");
            if (mensagem == "esquerda")
            {
                if (jogadorDoMovimento.moveDelay < 0)
                {

                        MovimentarParaEsquerda(jogadorDoMovimento);
                        jogadorDoMovimento.moveDelay = jogadorDoMovimento.moveDelayReference;

                } 
            }
            if (mensagem == "direita")
            {
                if (jogadorDoMovimento.moveDelay < 0)
                {

                        MovimentarParaDireita(jogadorDoMovimento);
                        jogadorDoMovimento.moveDelay = jogadorDoMovimento.moveDelayReference;


                }
            }
            if (mensagem == "cima")
            {
                if (jogadorDoMovimento.moveDelay < 0)
                {

                        MovimentarParaCima(jogadorDoMovimento);
                        jogadorDoMovimento.moveDelay = jogadorDoMovimento.moveDelayReference;


                }
            }
            if (mensagem == "baixo")
            {
                if (jogadorDoMovimento.moveDelay < 0)
                {
                        MovimentarParaBaixo(jogadorDoMovimento);
                        jogadorDoMovimento.moveDelay = jogadorDoMovimento.moveDelayReference;
                }
            }
            Console.WriteLine("Fim do Checar Input");
        }



        public static void MovimentarParaCima(Jogador jogadorMovimentado)
        {
            if (jogadorMovimentado.coordJogadorY - 1 < 0)
            {
                return;
            }




            Celula celulaAlvo = jogadorMovimentado.areaAtual.dungeonInserida.dungeon[jogadorMovimentado.coordJogadorX, jogadorMovimentado.coordJogadorY - 1];
            string tipoCelulaAlvo = jogadorMovimentado.areaAtual.dungeonInserida.dungeon[jogadorMovimentado.coordJogadorX, jogadorMovimentado.coordJogadorY - 1].tipoCelula;

            if (tipoCelulaAlvo == "caminho" || tipoCelulaAlvo == "chao" || tipoCelulaAlvo == "porta")
            {
                jogadorMovimentado.coordJogadorY = jogadorMovimentado.coordJogadorY - 1;
            }

            if ((tipoCelulaAlvo == "entrada" || tipoCelulaAlvo == "entradaSecreta") && jogadorMovimentado.coordJogadorY - 1 == 0)
            {
                jogadorMovimentado.coordJogadorX = 0;
                jogadorMovimentado.coordJogadorY = 0;
                jogadorMovimentado.areaAtual = jogadorMovimentado.sessaoAtual.zonaDaSessao.zona[jogadorMovimentado.areaAtual.posicaoMatriz.x, jogadorMovimentado.areaAtual.posicaoMatriz.y - 1];
                Console.WriteLine("Antes do TP");
                if (celulaAlvo.ehEntradaSecreta == false)
                {
                    jogadorMovimentado.coordJogadorX = jogadorMovimentado.areaAtual.dungeonInserida.coordEntradasX[3];
                    jogadorMovimentado.coordJogadorY = jogadorMovimentado.areaAtual.dungeonInserida.coordEntradasY[3] - 1;
                }
                else
                {
                    jogadorMovimentado.coordJogadorX = jogadorMovimentado.areaAtual.dungeonInserida.coordEntradasX[3];
                    jogadorMovimentado.coordJogadorY = jogadorMovimentado.areaAtual.dungeonInserida.coordEntradasY[3] - 1;
                }
                
            }
            Console.WriteLine("Depois do TP");
        }
        public static void MovimentarParaBaixo(Jogador jogadorMovimentado)
        {
            Celula celulaAlvo = jogadorMovimentado.areaAtual.dungeonInserida.dungeon[jogadorMovimentado.coordJogadorX, jogadorMovimentado.coordJogadorY + 1];
            string tipoCelulaAlvo = jogadorMovimentado.areaAtual.dungeonInserida.dungeon[jogadorMovimentado.coordJogadorX, jogadorMovimentado.coordJogadorY + 1].tipoCelula;

            if (jogadorMovimentado.coordJogadorY + 1 > jogadorMovimentado.areaAtual.dungeonInserida.tamanhoDungeon.y - 1)
            {
                return;
            }


            if (tipoCelulaAlvo == "caminho" || tipoCelulaAlvo == "chao" || tipoCelulaAlvo == "porta")
            {
                jogadorMovimentado.coordJogadorY = jogadorMovimentado.coordJogadorY + 1;
            }

            if ((tipoCelulaAlvo == "entrada" || tipoCelulaAlvo == "entradaSecreta") && jogadorMovimentado.coordJogadorY + 1 != 0)
            {
                jogadorMovimentado.coordJogadorX = 0;
                jogadorMovimentado.coordJogadorY = 0;
                jogadorMovimentado.areaAtual = jogadorMovimentado.sessaoAtual.zonaDaSessao.zona[jogadorMovimentado.areaAtual.posicaoMatriz.x, jogadorMovimentado.areaAtual.posicaoMatriz.y + 1];
                Console.WriteLine("Antes do TP");
                if (celulaAlvo.ehEntradaSecreta == false)
                {
                    jogadorMovimentado.coordJogadorX = jogadorMovimentado.areaAtual.dungeonInserida.coordEntradasX[2];
                    jogadorMovimentado.coordJogadorY = jogadorMovimentado.areaAtual.dungeonInserida.coordEntradasY[2] + 1;
                }
                else
                {
                    jogadorMovimentado.coordJogadorX = jogadorMovimentado.areaAtual.dungeonInserida.coordEntradasX[2];
                    jogadorMovimentado.coordJogadorY = jogadorMovimentado.areaAtual.dungeonInserida.coordEntradasY[2] + 1;
                }
            }
            Console.WriteLine("Depois do TP");
        }

        public static void MovimentarParaDireita(Jogador jogadorMovimentado)
        {
            if (jogadorMovimentado.coordJogadorX + 1 > jogadorMovimentado.areaAtual.dungeonInserida.tamanhoDungeon.x - 1)
            {
                return;
            }


            Celula celulaAlvo = jogadorMovimentado.areaAtual.dungeonInserida.dungeon[jogadorMovimentado.coordJogadorX + 1, jogadorMovimentado.coordJogadorY];
            string tipoCelulaAlvo = jogadorMovimentado.areaAtual.dungeonInserida.dungeon[jogadorMovimentado.coordJogadorX + 1, jogadorMovimentado.coordJogadorY].tipoCelula;

            if (tipoCelulaAlvo == "caminho" || tipoCelulaAlvo == "chao" || tipoCelulaAlvo == "porta")
            {
                jogadorMovimentado.coordJogadorX = jogadorMovimentado.coordJogadorX + 1;
            }

            if ((tipoCelulaAlvo == "entrada" || tipoCelulaAlvo == "entradaSecreta") && jogadorMovimentado.coordJogadorX + 1 != 0)
            {
                jogadorMovimentado.coordJogadorX = 0;
                jogadorMovimentado.coordJogadorY = 0;
                jogadorMovimentado.areaAtual = jogadorMovimentado.sessaoAtual.zonaDaSessao.zona[jogadorMovimentado.areaAtual.posicaoMatriz.x + 1, jogadorMovimentado.areaAtual.posicaoMatriz.y];
                Console.WriteLine("Antes do TP");
                if (celulaAlvo.ehEntradaSecreta == false)
                {
                    jogadorMovimentado.coordJogadorX = jogadorMovimentado.areaAtual.dungeonInserida.coordEntradasX[0] + 1;
                    jogadorMovimentado.coordJogadorY = jogadorMovimentado.areaAtual.dungeonInserida.coordEntradasY[0];
                }
                else
                {
                    jogadorMovimentado.coordJogadorX = jogadorMovimentado.areaAtual.dungeonInserida.coordEntradasX[0] + 1;
                    jogadorMovimentado.coordJogadorY = jogadorMovimentado.areaAtual.dungeonInserida.coordEntradasY[0];
                }
                
            }
            Console.WriteLine("Depois do TP");
        }

        public static void MovimentarParaEsquerda(Jogador jogadorMovimentado)
        {
            if (jogadorMovimentado.coordJogadorX - 1 < 0)
            {
                return;
            }

            Celula celulaAlvo = jogadorMovimentado.areaAtual.dungeonInserida.dungeon[jogadorMovimentado.coordJogadorX - 1 , jogadorMovimentado.coordJogadorY];
            string tipoCelulaAlvo = jogadorMovimentado.areaAtual.dungeonInserida.dungeon[jogadorMovimentado.coordJogadorX - 1 , jogadorMovimentado.coordJogadorY].tipoCelula;

            if (tipoCelulaAlvo == "caminho" || tipoCelulaAlvo == "chao" || tipoCelulaAlvo == "porta")
            {
                jogadorMovimentado.coordJogadorX = jogadorMovimentado.coordJogadorX - 1;
            }

            if ((tipoCelulaAlvo == "entrada" || tipoCelulaAlvo == "entradaSecreta") && jogadorMovimentado.coordJogadorX - 1 == 0)
            {
                jogadorMovimentado.coordJogadorX = 0;
                jogadorMovimentado.coordJogadorY = 0;
                jogadorMovimentado.areaAtual = jogadorMovimentado.sessaoAtual.zonaDaSessao.zona[jogadorMovimentado.areaAtual.posicaoMatriz.x - 1, jogadorMovimentado.areaAtual.posicaoMatriz.y];
                Console.WriteLine("Antes do TP");
                if (celulaAlvo.ehEntradaSecreta == false)
                {
                    jogadorMovimentado.coordJogadorX = jogadorMovimentado.areaAtual.dungeonInserida.coordEntradasX[1] - 1;
                    jogadorMovimentado.coordJogadorY = jogadorMovimentado.areaAtual.dungeonInserida.coordEntradasY[1];
                }
                else
                {
                    jogadorMovimentado.coordJogadorX = jogadorMovimentado.areaAtual.dungeonInserida.coordEntradasX[1] - 1;
                    jogadorMovimentado.coordJogadorY = jogadorMovimentado.areaAtual.dungeonInserida.coordEntradasY[1];
                }
            }
            Console.WriteLine("Depois do TP");
        }


        public static bool ChecarExisteSessaoCriada()
        {
            if (sessoesCriadas.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool ChecarTodasSessoesCriadasCheias()
        {
            bool todasCheias = true;

            if (sessoesCriadas.Count == Config.MaxSessoesSuportadas)
            {
                foreach (Sessao s in sessoesCriadas)
                {
                    if (!ChecarSessaoCheia(s))
                    {
                        todasCheias = false;
                    }
                }
            }
            return !todasCheias;
        }

        public static bool ChecarSessaoCheia(Sessao sessaoAlvo)
        {
            if (ChecarQtdJogadoresSessao(sessaoAlvo) >= Config.maxJogadoresSessao)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ChecarSessaoVazia(Sessao sessaoAlvo)
        {
            if (ChecarQtdJogadoresSessao(sessaoAlvo) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Requisicao
        public static bool AlocarEmSessao(Jogador jogadorInstanciado)
        {
            Sessao sessaoEscolhida = new Sessao();

            if (ChecarTodasSessoesCriadasCheias())
            {
                if (sessoesCriadas.Count >= Config.MaxSessoesSuportadas)
                {
                    return false;
                }
                else
                {
                    sessoesCriadas.Add(sessaoEscolhida);
                }

            }
            else
            {

                if (ChecarExisteSessaoCriada())
                {
                    foreach (Sessao s in sessoesCriadas)
                    {
                        if (ChecarSessaoCheia(s))
                        {

                        }
                        else
                        {
                            if (ChecarQtdJogadoresSessao(s) >= ChecarQtdJogadoresSessao(sessaoEscolhida))
                            {
                                sessaoEscolhida = s;
                            }
                        }
                    }
                }
                else
                {
                    sessoesCriadas.Add(sessaoEscolhida);
                }



            }

            bool resultado = AdicionarJogadorEmSessao(jogadorInstanciado, sessaoEscolhida);
            jogadoresConectados.Add(jogadorInstanciado);
            jogadorInstanciado.estaConectado = true;
            return resultado;

        }

        public static int ChecarQtdJogadoresSessao(Sessao sessaoAlvo)
        {
            int qtdIdentificada = 0;

            foreach (Jogador j in sessaoAlvo.jogadoresNaSessao)
            {
                if (j != null)
                {
                    qtdIdentificada++;
                }
            }

            return qtdIdentificada;
        }

        public static bool AdicionarJogadorEmSessao(Jogador jogadorAlvo, Sessao sessaoAlvo)
        {
            if(ChecarSessaoCheia(sessaoAlvo))
            {
                return false;
            }
            else
            {
                int slot = ChecarQtdJogadoresSessao(sessaoAlvo);

                sessaoAlvo.jogadoresNaSessao[slot] = jogadorAlvo;
                jogadorAlvo.sessaoAtual = sessaoAlvo;
                //
                int x, y;
                //Caso eu não queria usar o vetor coordAreas:         (não lembro se no coordAreas tá inclusa as áreas secretas, mas acredito que não).(checarei depois no console)
                /*
                List<Area> areasDaZona = new List<Area>();
                foreach (Area a in sessaoAlvo.zonaDaSessao.zona)
                {
                    if (a != null)
                    {
                        areasDaZona.Add(a);
                    }
                }
                */
                //(se eu quiser incluir as areas secretas como possibilidade de spawn, talvez seja melhor usar o codigo a cima usando lista.)
                Random rnd = new Random();
                int indice = rnd.Next(0, sessaoAlvo.zonaDaSessao.numeroDeAreas);
                x = sessaoAlvo.zonaDaSessao.coordAreas[indice].x;
                y = sessaoAlvo.zonaDaSessao.coordAreas[indice].y;
                //
                jogadorAlvo.areaAtual = sessaoAlvo.zonaDaSessao.zona[x,y];
                Vetor2 aux = Area.RandomizarCelulaArea("caminho", jogadorAlvo.areaAtual).coordCelulaDun;
                jogadorAlvo.coordJogadorX = aux.x;
                jogadorAlvo.coordJogadorY = aux.y;


            }
            return true;
        }

        public static Jogador procurarJogadorEmSessao(Guid idJogador, Guid idSessao)
        {
            Sessao sessaoAlvo = null;

            foreach (Sessao s in sessoesCriadas)
            {
                if (idSessao == s.idSessao)
                {
                    sessaoAlvo = s;
                }

            }

            foreach (Jogador j in sessaoAlvo.jogadoresNaSessao)
            {
                if (idJogador == j.idJogador)
                {
                    return j;
                }
            }
            return null;
        }




    }




}
