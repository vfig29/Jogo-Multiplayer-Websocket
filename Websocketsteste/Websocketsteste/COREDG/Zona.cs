using System;
using System.Collections.Generic;
using System.Text;

namespace Websocketsteste.COREDG
{
    public class Zona
    {
        //Random.Next já foi ajustado nessa classe.( como explicado no documento do bloco de notas Random.Next().txt ).
        public Area[,] zona = new Area[Config.tamanhoMatrizZona.x, Config.tamanhoMatrizZona.y];
        public Area bossArea = null;
        public int numeroDeAreas;
        public int numeroDeAreasSecretas;
        public Vetor2[] coordAreas = new Vetor2[Config.numeroMaxAreas + 5];
        public Vetor2[] coordAreasSecretas = new Vetor2[Config.numeroMaxAreas + 5];
        public Vetor2 areaInicial, areaFinal;

        public Zona()
        {
            PrimeiraArea(this);
            //randomizando numero de areas
            System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
            int nAreas = rnd.Next(Config.numeroMinAreas - 1, Config.numeroMaxAreas); //Randomiza até 1 valor a menos, pois a primeira area já está criada.
            numeroDeAreas = nAreas + 1; //possui um valor a mais, pois a primeira sala já está criada.
                                        //numero de areas randomizado
            PreencherZona(this, zona[(Config.tamanhoMatrizZona.x / 2), (Config.tamanhoMatrizZona.y / 2)], 0, coordAreas, nAreas, rnd);
            PermitirAreasSecretas(); //define se o mapa terá ou não salas secretas.
            AdicionarInfoPortas(this); //Checa as areas adjacentes e determina os lados onde deve possuir uma porta para outra área.
            EscolherAreaInicial(this); //Escolhe uma das areas para ser a area de spawn do personagem.
            EscolherAreaFinal(this); // Define uma das areas para ser a area que spawna o portal para o boss.
            InserirDungeonsZona(this); //Insere dentro das areas uma dungeon que possui saida para os lados que possuem outras areas.
            CriarAreaBoss(this); // criando e alocando a areaDoBoss;
            Console.WriteLine("Areas Secretas:" + numeroDeAreasSecretas);



            //Dungeon dungeonCriada = Grafico.gerarDungeon(zona[50,50]); //foi mudado pra static a funcao gerarDungeon, e adicionado os parametros AreaPai.
            //Grafico.gerarGrafico(zona[50, 50]); //Foi transformado em static, o metodo gerarGrafico e gerar String,

        }

        public void PrimeiraArea(Zona zonaInserida)
        {
            Vetor2 coordAtual = new Vetor2((Config.tamanhoMatrizZona.x / 2), (Config.tamanhoMatrizZona.y / 2));
            Area areaCriada = new Area(coordAtual);
            zonaInserida.zona[coordAtual.x, coordAtual.y] = areaCriada;
            coordAreas[0] = coordAtual;

        }


        public void PreencherZona(Zona zonaInserida, Area areaPartida, int totalAreasCriadas, Vetor2[] coordAreas, int nAreas, System.Random rnd)
        {
            bool permissao;
            Vetor2 novaCoord;
            //numeroDeAreasRandomizado---
            if (totalAreasCriadas >= nAreas)
            {
                //interrompe a recursão.
                return;

            }
            else
            {
                int porta = rnd.Next(0, 4);
                switch (porta)
                {
                    //Não cria;
                    case 0:
                        //baixo
                        //Cria;
                        novaCoord = new Vetor2(areaPartida.posicaoMatriz.x, areaPartida.posicaoMatriz.y + 1);
                        //Checa
                        permissao = true;
                        for (int i = 0; i <= totalAreasCriadas; i++)
                        {
                            if ((coordAreas[i].x == novaCoord.x && coordAreas[i].y == novaCoord.y) || (novaCoord.x >= Config.tamanhoMatrizZona.x || novaCoord.x >= Config.tamanhoMatrizZona.y) || (novaCoord.x < 1 || novaCoord.y < 1))
                            {
                                permissao = false;
                            }
                        }

                        //Insere
                        if (permissao == true)
                        {
                            zonaInserida.zona[areaPartida.posicaoMatriz.x, areaPartida.posicaoMatriz.y + 1] = new Area(novaCoord);
                            totalAreasCriadas++;
                            coordAreas[totalAreasCriadas] = novaCoord;
                        }



                        break;
                    case 1:
                        //cima:
                        //Cria;
                        novaCoord = new Vetor2(areaPartida.posicaoMatriz.x, areaPartida.posicaoMatriz.y - 1);
                        //Checa
                        permissao = true;
                        for (int i = 0; i <= totalAreasCriadas; i++)
                        {
                            if (coordAreas[i].x == novaCoord.x && coordAreas[i].y == novaCoord.y)
                            {
                                permissao = false;
                            }
                        }

                        //Insere
                        if (permissao == true)
                        {
                            zonaInserida.zona[areaPartida.posicaoMatriz.x, areaPartida.posicaoMatriz.y - 1] = new Area(novaCoord);
                            totalAreasCriadas++;
                            coordAreas[totalAreasCriadas] = novaCoord;
                        }
                        break;
                    case 2:
                        //esquerda
                        //Cria;
                        novaCoord = new Vetor2(areaPartida.posicaoMatriz.x - 1, areaPartida.posicaoMatriz.y);
                        //Checa
                        permissao = true;
                        for (int i = 0; i <= totalAreasCriadas; i++)
                        {
                            if (coordAreas[i].x == novaCoord.x && coordAreas[i].y == novaCoord.y)
                            {
                                permissao = false;
                            }
                        }

                        //Insere
                        if (permissao == true)
                        {
                            zonaInserida.zona[areaPartida.posicaoMatriz.x - 1, areaPartida.posicaoMatriz.y] = new Area(novaCoord);
                            totalAreasCriadas++;
                            coordAreas[totalAreasCriadas] = novaCoord;
                        }


                        break;
                    case 3:
                        //direita
                        //Cria;
                        novaCoord = new Vetor2(areaPartida.posicaoMatriz.x + 1, areaPartida.posicaoMatriz.y);
                        //Checa
                        permissao = true;
                        for (int i = 0; i <= totalAreasCriadas; i++)
                        {
                            if (coordAreas[i].x == novaCoord.x && coordAreas[i].y == novaCoord.y)
                            {
                                permissao = false;
                            }
                        }

                        //Insere
                        if (permissao == true)
                        {
                            zonaInserida.zona[areaPartida.posicaoMatriz.x + 1, areaPartida.posicaoMatriz.y] = new Area(novaCoord);
                            totalAreasCriadas++;
                            coordAreas[totalAreasCriadas] = novaCoord;
                        }
                        break;
                }

                //Chama a recursão:
                if (totalAreasCriadas > (nAreas / 4))
                {
                    PreencherZonaLimitado(zonaInserida, totalAreasCriadas, coordAreas, nAreas, rnd);
                }
                else
                {
                    int posicaorandom = rnd.Next(0, totalAreasCriadas + 1);
                    PreencherZona(zonaInserida, zonaInserida.zona[coordAreas[posicaorandom].x, coordAreas[posicaorandom].y], totalAreasCriadas, coordAreas, nAreas, rnd);
                    //Console.WriteLine("entrou" + (totalAreasCriadas + 1));
                }
            }
        }

        public void PreencherZonaLimitado(Zona zonaInserida, int totalAreasCriadas, Vetor2[] coordAreas, int nAreas, System.Random rnd)
        {
            int nAdjacentes;
            bool permissao = false;
            Vetor2 novaCoord;
            //numeroDeAreasRandomizado---
            if (totalAreasCriadas >= nAreas)
            {
                //interrompe a recursão.
                return;

            }
            else
            {
                while (permissao == false)
                {
                    int coordx = rnd.Next(1, Config.tamanhoMatrizZona.x - 2);
                    int coordy = rnd.Next(1, Config.tamanhoMatrizZona.y - 2);
                    nAdjacentes = 0;
                    //checa se a posicao da matriz está vazia
                    if (zonaInserida.zona[coordx, coordy] == null)
                    {
                        //checa os adjacentes:
                        //cima:
                        if (zonaInserida.zona[coordx, coordy - 1] != null)
                        {
                            nAdjacentes++;
                        }
                        //baixo
                        if (zonaInserida.zona[coordx, coordy + 1] != null)
                        {
                            nAdjacentes++;
                        }
                        //esquerda
                        if (zonaInserida.zona[coordx - 1, coordy] != null)
                        {
                            nAdjacentes++;
                        }
                        //direita
                        if (zonaInserida.zona[coordx + 1, coordy] != null)
                        {
                            nAdjacentes++;
                        }
                        if (nAdjacentes > 0 && nAdjacentes < 2)
                        {
                            //insere uma area na posicao da matriz.
                            novaCoord = new Vetor2(coordx, coordy);
                            zonaInserida.zona[coordx, coordy] = new Area(novaCoord);
                            totalAreasCriadas++;
                            coordAreas[totalAreasCriadas] = novaCoord;
                            permissao = true;
                        }
                    }

                }



                //Chama a recursão:
                if (totalAreasCriadas > (nAreas / 4))
                {
                    PreencherZonaLimitado(zonaInserida, totalAreasCriadas, coordAreas, nAreas, rnd);
                }
                else
                {
                    int posicaorandom = rnd.Next(0, totalAreasCriadas + 1);
                    PreencherZona(zonaInserida, zonaInserida.zona[coordAreas[posicaorandom].x, coordAreas[posicaorandom].y], totalAreasCriadas, coordAreas, nAreas, rnd);
                    //Console.WriteLine("entrou" + (totalAreasCriadas + 1));
                }
            }
        }

        public void AdicionarInfoPortas(Zona zonaInserida)
        {

            for (int i = 1; i < Config.tamanhoMatrizZona.x - 1; i++)
            {
                for (int j = 1; j < Config.tamanhoMatrizZona.y - 1; j++)
                {
                    if (zonaInserida.zona[i, j] != null)
                    {
                        //checar lados
                        //cima
                        if (zonaInserida.zona[i, j - 1] != null)
                        {
                            if (zonaInserida.zona[i, j - 1].secreta == false)
                            {
                                zonaInserida.zona[i, j].entradas[2] = zonaInserida.zona[i, j - 1].posicaoMatriz;
                            }
                            if (zonaInserida.zona[i, j - 1].secreta == true)
                            {
                                zonaInserida.zona[i, j].entradasSecretas[2] = zonaInserida.zona[i, j - 1].posicaoMatriz;
                            }
                        }
                        //baixo
                        if (zonaInserida.zona[i, j + 1] != null)
                        {
                            if (zonaInserida.zona[i, j + 1].secreta == false)
                            {
                                zonaInserida.zona[i, j].entradas[3] = zonaInserida.zona[i, j + 1].posicaoMatriz;
                            }
                            if (zonaInserida.zona[i, j + 1].secreta == true)
                            {
                                zonaInserida.zona[i, j].entradasSecretas[3] = zonaInserida.zona[i, j + 1].posicaoMatriz;
                            }
                        }
                        //esquerda
                        if (zonaInserida.zona[i - 1, j] != null)
                        {
                            if (zonaInserida.zona[i - 1, j].secreta == false)
                            {
                                zonaInserida.zona[i, j].entradas[0] = zonaInserida.zona[i - 1, j].posicaoMatriz;
                            }
                            if (zonaInserida.zona[i - 1, j].secreta == true)
                            {
                                zonaInserida.zona[i, j].entradasSecretas[0] = zonaInserida.zona[i - 1, j].posicaoMatriz;
                            }

                        }
                        //direita
                        if (zonaInserida.zona[i + 1, j] != null)
                        {
                            if (zonaInserida.zona[i + 1, j].secreta == false)
                            {
                                zonaInserida.zona[i, j].entradas[1] = zonaInserida.zona[i + 1, j].posicaoMatriz;
                            }
                            if (zonaInserida.zona[i + 1, j].secreta == true)
                            {
                                zonaInserida.zona[i, j].entradasSecretas[1] = zonaInserida.zona[i + 1, j].posicaoMatriz;
                            }
                        }
                        //FIM CHECAGEM
                    }
                }
            }


        }

        public void InserirDungeonsZona(Zona zonaInserida)
        {
            for (int i = 1; i < Config.tamanhoMatrizZona.x - 1; i++)
            {
                for (int j = 1; j < Config.tamanhoMatrizZona.y - 1; j++)
                {
                    if (zonaInserida.zona[i, j] != null)
                    {
                        zonaInserida.zona[i, j].dungeonInserida = Grafico.gerarDungeon(zonaInserida.zona[i, j]);
                    }
                }
            }
        }
        //
        public void EscolherAreaInicial(Zona zonaInserida)
        {
            System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
            int posicaoRandom = rnd.Next(0, numeroDeAreas);
            Vetor2 coordAreaInicial = coordAreas[posicaoRandom];
            zonaInserida.zona[coordAreaInicial.x, coordAreaInicial.y].inicial = true;
            this.areaInicial = coordAreaInicial;



        }
        public void EscolherAreaFinal(Zona zonaInserida)
        {
            System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
            bool loop = true;

            while (loop == true)
            {
                int posicaoRandom = rnd.Next(0, numeroDeAreas);
                Vetor2 coordAreaFinal = coordAreas[posicaoRandom];

                if (zonaInserida.zona[coordAreaFinal.x, coordAreaFinal.y].inicial == false)
                {
                    zonaInserida.zona[coordAreaFinal.x, coordAreaFinal.y].final = true;
                    this.areaFinal = coordAreaFinal;
                    loop = false;
                }
            }
        }
        public void PermitirAreasSecretas()
        {
            System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
            int nMaxAreasSecretas = 1 + (numeroDeAreas / 4);
            int permissao = rnd.Next(0, 4);
            if (true)
            {
                int fatorDeDivisao = rnd.Next(1, 5);
                int nAreasSecretas = rnd.Next(0, nMaxAreasSecretas + 1) - rnd.Next(0, ((nMaxAreasSecretas / fatorDeDivisao) + 1));
                if (nAreasSecretas <= 1)
                {
                    nAreasSecretas = 1;
                }
                numeroDeAreasSecretas = nAreasSecretas;
                CriarAreasSecretas(this, 0, this.coordAreasSecretas, nAreasSecretas, rnd);
            }
        }

        public void CriarAreasSecretas(Zona zonaInserida, int totalAreasCriadas, Vetor2[] coordAreasSecretas, int nAreasSecretas, System.Random rnd)
        {
            int nAdjacentes;
            bool permissao = false;
            Vetor2 novaCoord;
            //numeroDeAreasRandomizado---
            if (totalAreasCriadas >= nAreasSecretas)
            {
                //interrompe a recursão.
                return;

            }
            else
            {
                while (permissao == false)
                {
                    int coordx = rnd.Next(1, Config.tamanhoMatrizZona.x - 2);
                    int coordy = rnd.Next(1, Config.tamanhoMatrizZona.y - 2);
                    nAdjacentes = 0;
                    //checa se a posicao da matriz está vazia
                    if (zonaInserida.zona[coordx, coordy] == null)
                    {
                        //checa os adjacentes:
                        //cima:
                        if (zonaInserida.zona[coordx, coordy - 1] != null)
                        {
                            nAdjacentes++;
                        }
                        //baixo
                        if (zonaInserida.zona[coordx, coordy + 1] != null)
                        {
                            nAdjacentes++;
                        }
                        //esquerda
                        if (zonaInserida.zona[coordx - 1, coordy] != null)
                        {
                            nAdjacentes++;
                        }
                        //direita
                        if (zonaInserida.zona[coordx + 1, coordy] != null)
                        {
                            nAdjacentes++;
                        }
                        if (nAdjacentes > 0)
                        {
                            //insere uma area na posicao da matriz.
                            novaCoord = new Vetor2(coordx, coordy);
                            zonaInserida.zona[coordx, coordy] = new Area(novaCoord);
                            zonaInserida.zona[coordx, coordy].secreta = true;
                            totalAreasCriadas++;
                            coordAreasSecretas[totalAreasCriadas] = novaCoord;
                            permissao = true;
                        }
                    }

                }
                //Chama a recursão:
                CriarAreasSecretas(zonaInserida, totalAreasCriadas, coordAreasSecretas, nAreasSecretas, rnd);
            }
        }
        //
        public void CriarAreaBoss(Zona zonaInserida)
        {
            Vetor2 auxTamMin = new Vetor2(0, 0);
            Vetor2 auxTamMax = new Vetor2(0, 0);
            zonaInserida.bossArea = new Area(null);
            zonaInserida.bossArea.boss = true;
            auxTamMin.x = Config.tamanhoMinDungeon.x;
            auxTamMin.y = Config.tamanhoMinDungeon.y;
            auxTamMax.x = Config.tamanhoMaxDungeon.x;
            auxTamMax.y = Config.tamanhoMaxDungeon.y;
            Config.tamanhoMinDungeon.x = 100;
            Config.tamanhoMinDungeon.y = 40;
            Config.tamanhoMaxDungeon.x = 100;
            Config.tamanhoMaxDungeon.y = 40;
            zonaInserida.bossArea.dungeonInserida = new Dungeon(zonaInserida.bossArea);
            zonaInserida.bossArea.dungeonInserida.dungeon[Config.tamanhoMaxDungeon.x / 2, 2].spawnsbPlayer = true;
            zonaInserida.bossArea.dungeonInserida.dungeon[Config.tamanhoMaxDungeon.x / 2, Config.tamanhoMaxDungeon.y - 3].spawnsbBoss = true;
            Config.tamanhoMinDungeon.x = auxTamMin.x;
            Config.tamanhoMinDungeon.y = auxTamMin.y;
            Config.tamanhoMaxDungeon.x = auxTamMax.x;
            Config.tamanhoMaxDungeon.y = auxTamMax.y;
            for (int i = 1; i < zonaInserida.bossArea.dungeonInserida.tamanhoDungeon.y - 1; i++)
            {
                for (int j = 1; j < zonaInserida.bossArea.dungeonInserida.tamanhoDungeon.x - 1; j++)
                {
                    if (zonaInserida.bossArea.dungeonInserida.dungeon[j, i].tipoCelula != "caminho")
                    {
                        zonaInserida.bossArea.dungeonInserida.dungeon[j, i].tipoCelula = "caminho";
                    }
                }

            }




        }

    }

}
