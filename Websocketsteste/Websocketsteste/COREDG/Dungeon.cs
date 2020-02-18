using System;
using System.Collections.Generic;
using System.Text;


namespace Websocketsteste.COREDG
{
    public class Dungeon
    {
        // Start is called before the first frame update*******
        public Vetor2 tamanhoDungeon;
        public int[] coordEntradasX = new int[4]; //0 - esquerda, 1 - direita, 2 - cima, 3 - baixo.
        public int[] coordEntradasY = new int[4]; //0 - esquerda, 1 - direita, 2 - cima, 3 - baixo.
        public int[] coordEntradasSecretasX = new int[4]; //0 - esquerda, 1 - direita, 2 - cima, 3 - baixo.
        public int[] coordEntradasSecretasY = new int[4]; //0 - esquerda, 1 - direita, 2 - cima, 3 - baixo.
        public Celula[,] dungeon = new Celula[Config.tamanhoMaxDungeon.x, Config.tamanhoMaxDungeon.y];
        public Area areaPai;
        public Sala[] salas = new Sala[Config.numeroMaxSalas];
        public int idDungeon = 0;
        public int numeroSalas = 0;
        public int numeroEntradas = 0;
        public bool terminarCaminho;
        public bool terminarPivot;
        public bool errorCaminho = false;
        public bool errorFiltro = true;



        public Dungeon(Area areaPai)
        {
            this.numeroSalas = 0;
            this.areaPai = areaPai;
            //Instanciando a Randomizacao
            System.Random tamanho = new System.Random(Guid.NewGuid().GetHashCode());
            // Gerando valores aleatorios de coordenada x e y, para instanciar o vector.
            int x = tamanho.Next(Config.tamanhoMinDungeon.x, Config.tamanhoMaxDungeon.x);
            int y = tamanho.Next(Config.tamanhoMinDungeon.y, Config.tamanhoMaxDungeon.y);
            Vetor2 tamanhoDungeon = new Vetor2(x, y);
            Celula[,] dungeon = new Celula[tamanhoDungeon.x, tamanhoDungeon.y];
            this.tamanhoDungeon = tamanhoDungeon;
            idDungeon++;
            //Preenchendo a Dungeon.
            for (int i = 0; i < tamanhoDungeon.y; i++)
            {
                for (int j = 0; j < tamanhoDungeon.x; j++)
                {
                    if (j == 0 || i == 0 || j == (tamanhoDungeon.x - 1) || i == (tamanhoDungeon.y - 1))
                    {
                        Vetor2 coordPreenchida = new Vetor2(j, i);
                        Celula celulaPreencheu = new Celula(coordPreenchida, "contornoDungeon");
                        //adicionando subtipo*
                        if (j == 0 && i == 0)
                        {
                            celulaPreencheu.subTipoCelula = "quinaEC";

                        }
                        if (j == 0 && i == (tamanhoDungeon.y - 1))
                        {
                            celulaPreencheu.subTipoCelula = "quinaEB";

                        }
                        if (j == 0 && i != (tamanhoDungeon.y - 1) && i != 0)
                        {
                            celulaPreencheu.subTipoCelula = "esquerda";

                        }
                        if (j == (tamanhoDungeon.x - 1) && i == 0)
                        {
                            celulaPreencheu.subTipoCelula = "quinaDC";

                        }
                        if (j == (tamanhoDungeon.x - 1) && i == (tamanhoDungeon.y - 1))
                        {
                            celulaPreencheu.subTipoCelula = "quinaDB";

                        }
                        if (j == (tamanhoDungeon.x - 1) && i != (tamanhoDungeon.y - 1) && i != 0)
                        {
                            celulaPreencheu.subTipoCelula = "direita";

                        }
                        if (i == 0 && j != 0 && j != (tamanhoDungeon.x - 1))
                        {
                            celulaPreencheu.subTipoCelula = "cima";

                        }
                        if (i == tamanhoDungeon.y - 1 && j != 0 && j != (tamanhoDungeon.x - 1))
                        {
                            celulaPreencheu.subTipoCelula = "baixo";

                        }

                        dungeon[j, i] = celulaPreencheu;
                    }
                    else
                    {
                        Vetor2 coordPreenchida = new Vetor2(j, i);
                        Celula celulaPreencheu = new Celula(coordPreenchida, "parede");
                        dungeon[j, i] = celulaPreencheu;
                    }
                }

            }
            //Alocando Salas;
            int numeroSalas = tamanho.Next(Config.numeroMinSalas, Config.numeroMaxSalas);
            for (int i = 0; i < numeroSalas; i++)
            {
                Sala salaInstanciada = new Sala();
                int xSala = tamanho.Next(2, tamanhoDungeon.x - salaInstanciada.tamanhoSala.x - 2);
                int ySala = tamanho.Next(2, tamanhoDungeon.y - salaInstanciada.tamanhoSala.y - 2);
                Vetor2 coordSala = new Vetor2(xSala, ySala);
                salaInstanciada.coordAncoraDungeon = coordSala;
                //Checando se já possui alguma sala nas celulas randomizadas.
                bool checkOk = true; // 2 - Pode fazer a dungeon. false - Não pode fazer a dungeon.
                for (int j = 0; j < salaInstanciada.tamanhoSala.y; j++)
                {
                    for (int k = 0; k < salaInstanciada.tamanhoSala.x; k++)
                    {

                        if ((dungeon[(coordSala.x + k), (coordSala.y + j)].tipoCelula) != "parede" || (dungeon[(coordSala.x + k), (coordSala.y + j - 1)].tipoCelula) == "porta" || (dungeon[(coordSala.x + k), (coordSala.y + j + 1)].tipoCelula) == "porta" || (dungeon[(coordSala.x + k - 1), (coordSala.y + j)].tipoCelula) == "porta" || (dungeon[(coordSala.x + k + 1), (coordSala.y + j)].tipoCelula) == "porta") // ou se os adjacentes da celula sejam porta(importante implementar em caso de erro).
                        {
                            checkOk = false;
                        }



                    }


                }
                //Após, a checar. Caso o check seja true, pode fazer a sala.
                if (checkOk == true)
                {
                    //chance de precisar de chave
                    if (this.numeroSalas == 0)
                    {
                        salaInstanciada.chave = false;
                    }
                    else
                    {
                        System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
                        int chanceChave = rnd.Next(1, 11);
                        if (chanceChave == 2 || chanceChave == 8)
                        {
                            salaInstanciada.chave = true;
                            salaInstanciada.trancado = true;
                        }
                        else
                        {
                            salaInstanciada.chave = false;
                            salaInstanciada.trancado = false;
                        }
                    }
                    this.numeroSalas++;
                    salaInstanciada.idSala = this.numeroSalas - 1;
                    salaInstanciada.dificuldade = Sala.AdicionarDificuldade();
                    Sala.AdicionarIdSalas(salaInstanciada);
                    salas[this.numeroSalas - 1] = salaInstanciada;
                    for (int j = 0; j < salaInstanciada.tamanhoSala.y; j++)
                    {
                        for (int k = 0; k < salaInstanciada.tamanhoSala.x; k++)
                        {
                            dungeon[coordSala.x + k, coordSala.y + j] = salaInstanciada.sala[k, j];
                            Vetor2 novaCoord = new Vetor2(coordSala.x + k, coordSala.y + j);
                            dungeon[coordSala.x + k, coordSala.y + j].coordCelulaDun = novaCoord;

                        }


                    }

                }
            }

            //Adicionando Entradas;
            for (int i = 0; i < 4; i++)
            {
                if (areaPai.entradas[i] != null)
                {
                    this.numeroEntradas++;
                    int xEntrada = tamanho.Next(2, tamanhoDungeon.x - 3 + 1);//next ajustado
                    int yEntrada = tamanho.Next(2, tamanhoDungeon.y - 3 + 1);//next ajustado
                    Vetor2 coordPreenchida2 = new Vetor2(0, 0);
                    string subtipo = null;
                    switch (i)
                    {
                        case 0:
                            //esquerda
                            coordPreenchida2 = new Vetor2(0, yEntrada);
                            coordEntradasX[0] = coordPreenchida2.x;
                            coordEntradasY[0] = coordPreenchida2.y;
                            subtipo = "esquerdae";
                            break;
                        case 1:
                            //direita
                            coordPreenchida2 = new Vetor2(tamanhoDungeon.x - 1, yEntrada);
                            coordEntradasX[1] = coordPreenchida2.x;
                            coordEntradasY[1] = coordPreenchida2.y;
                            subtipo = "direitae";
                            break;
                        case 2:
                            //cima
                            coordPreenchida2 = new Vetor2(xEntrada, 0);
                            coordEntradasX[2] = coordPreenchida2.x;
                            coordEntradasY[2] = coordPreenchida2.y;
                            subtipo = "cimae";
                            break;
                        case 3:
                            //baixo
                            coordPreenchida2 = new Vetor2(xEntrada, tamanhoDungeon.y - 1);
                            coordEntradasX[3] = coordPreenchida2.x;
                            coordEntradasY[3] = coordPreenchida2.y;
                            subtipo = "baixoe";
                            break;
                    }

                    Celula celulaPreencheu = new Celula(coordPreenchida2, "entrada");
                    celulaPreencheu.ehEntrada = true;
                    celulaPreencheu.ehEntradaSecreta = false;
                    celulaPreencheu.subTipoCelula = subtipo;
                    //

                    //
                    dungeon[coordPreenchida2.x, coordPreenchida2.y] = celulaPreencheu;
                    dungeon[coordPreenchida2.x, coordPreenchida2.y].coordCelulaDun.x = coordPreenchida2.x;
                    dungeon[coordPreenchida2.x, coordPreenchida2.y].coordCelulaDun.y = coordPreenchida2.y;


                }
            }

            //Adicionando Entradas Secretas;
            for (int i = 0; i < 4; i++)
            {
                if (areaPai.entradasSecretas[i] != null)
                {
                    this.numeroEntradas++;
                    int xEntrada = tamanho.Next(2, tamanhoDungeon.x - 3 + 1); //next ajustado
                    int yEntrada = tamanho.Next(2, tamanhoDungeon.y - 3 + 1); //next ajustado
                    Vetor2 coordPreenchida2 = new Vetor2(0, 0);
                    string subtipo = null;
                    switch (i)
                    {
                        case 0:
                            //esquerda
                            coordPreenchida2 = new Vetor2(0, yEntrada);
                            coordEntradasX[0] = coordPreenchida2.x;
                            coordEntradasY[0] = coordPreenchida2.y;
                            coordEntradasSecretasX[0] = coordPreenchida2.x;
                            coordEntradasSecretasY[0] = coordPreenchida2.y;
                            subtipo = "esquerdae";
                            break;
                        case 1:
                            //direita
                            coordPreenchida2 = new Vetor2(tamanhoDungeon.x - 1, yEntrada);
                            coordEntradasX[1] = coordPreenchida2.x;
                            coordEntradasY[1] = coordPreenchida2.y;
                            coordEntradasSecretasX[1] = coordPreenchida2.x;
                            coordEntradasSecretasY[1] = coordPreenchida2.y;
                            subtipo = "direitae";
                            break;
                        case 2:
                            //cima
                            coordPreenchida2 = new Vetor2(xEntrada, 0);
                            coordEntradasX[2] = coordPreenchida2.x;
                            coordEntradasY[2] = coordPreenchida2.y;
                            coordEntradasSecretasX[2] = coordPreenchida2.x;
                            coordEntradasSecretasY[2] = coordPreenchida2.y;
                            subtipo = "cimae";
                            break;
                        case 3:
                            //baixo
                            coordPreenchida2 = new Vetor2(xEntrada, tamanhoDungeon.y - 1);
                            coordEntradasX[3] = coordPreenchida2.x;
                            coordEntradasY[3] = coordPreenchida2.y;
                            coordEntradasSecretasX[3] = coordPreenchida2.x;
                            coordEntradasSecretasY[3] = coordPreenchida2.y;
                            subtipo = "baixoe";
                            break;
                    }

                    Celula celulaPreencheu = new Celula(coordPreenchida2, "entrada");
                    celulaPreencheu.ehEntrada = true;
                    celulaPreencheu.ehEntradaSecreta = true;
                    celulaPreencheu.subTipoCelula = subtipo;
                    dungeon[coordPreenchida2.x, coordPreenchida2.y] = celulaPreencheu;
                    dungeon[coordPreenchida2.x, coordPreenchida2.y].coordCelulaDun.x = coordPreenchida2.x;
                    dungeon[coordPreenchida2.x, coordPreenchida2.y].coordCelulaDun.y = coordPreenchida2.y;
                }
            }


            this.dungeon = dungeon;
        }


        public void superSalas(Dungeon dungeonInserida)
        {
            for (int i = 1; i < dungeonInserida.tamanhoDungeon.x - 1; i++)
            {
                for (int j = 1; j < dungeonInserida.tamanhoDungeon.y - 1; j++)
                {
                    if (dungeonInserida.dungeon[i, j].tipoCelula == "pSala" || dungeonInserida.dungeon[i, j].tipoCelula == "porta")
                    {
                        dungeonInserida.dungeon[i, j].pegarAdjacentes(dungeonInserida.dungeon[i, j]);
                        if (dungeonInserida.dungeon[i, j].adjcima != null && dungeonInserida.dungeon[i, j].adjbaixo != null && dungeonInserida.dungeon[i, j].adjesq != null && dungeonInserida.dungeon[i, j].adjdir != null)
                        {
                            if (dungeonInserida.dungeon[i, j].adjcima.tipoCelula != "parede" && dungeonInserida.dungeon[i, j].adjbaixo.tipoCelula != "parede" && dungeonInserida.dungeon[i, j].adjesq.tipoCelula != "parede" && dungeonInserida.dungeon[i, j].adjdir.tipoCelula != "parede" && dungeonInserida.dungeon[i, j].adjcima.tipoCelula != "contornoDungeon" && dungeonInserida.dungeon[i, j].adjbaixo.tipoCelula != "contornoDungeon" && dungeonInserida.dungeon[i, j].adjesq.tipoCelula != "contornoDungeon" && dungeonInserida.dungeon[i, j].adjdir.tipoCelula != "contornoDungeon")
                            {
                                if (dungeonInserida.dungeon[i, j].adjcima.tipoCelula == "pSala" || dungeonInserida.dungeon[i, j].adjbaixo.tipoCelula == "pSala" || dungeonInserida.dungeon[i, j].adjesq.tipoCelula == "pSala" || dungeonInserida.dungeon[i, j].adjdir.tipoCelula == "pSala")
                                {
                                    dungeonInserida.dungeon[i, j].tipoCelula = "chao";
                                    dungeonInserida.dungeon[i, j].subTipoCelula = "chao"; //apenas para remover o subtipo das pSalas;
                                    dungeonInserida.dungeon[i, j].ehPorta = false;
                                }

                            }
                        }
                    }
                }
            }
        }

        public void IgualarSuperSala(Dungeon dungeonInserida)
        {
            bool[] idsemchave = new bool[dungeonInserida.numeroSalas + 2];
            for (int k = 0; k < dungeonInserida.numeroSalas + 2; k++)
            {
                idsemchave[k] = false;

            }

            for (int i = 1; i < dungeonInserida.tamanhoDungeon.x - 1; i++)
            {
                for (int j = 1; j < dungeonInserida.tamanhoDungeon.y - 1; j++)
                {
                    if (dungeonInserida.dungeon[i, j].idSalaPai != -1)
                    {
                        if (dungeonInserida.dungeon[i, j].idSalaPai != dungeonInserida.dungeon[i + 1, j].idSalaPai && dungeonInserida.dungeon[i + 1, j].idSalaPai != -1)
                        {
                            idsemchave[dungeonInserida.dungeon[i, j].idSalaPai] = true;
                        }
                        if (dungeonInserida.dungeon[i, j].idSalaPai != dungeonInserida.dungeon[i - 1, j].idSalaPai && dungeonInserida.dungeon[i - 1, j].idSalaPai != -1)
                        {
                            idsemchave[dungeonInserida.dungeon[i, j].idSalaPai] = true;
                        }
                        if (dungeonInserida.dungeon[i, j].idSalaPai != dungeonInserida.dungeon[i, j + 1].idSalaPai && dungeonInserida.dungeon[i, j + 1].idSalaPai != -1)
                        {
                            idsemchave[dungeonInserida.dungeon[i, j].idSalaPai] = true;
                        }
                        if (dungeonInserida.dungeon[i, j].idSalaPai != dungeonInserida.dungeon[i, j - 1].idSalaPai && dungeonInserida.dungeon[i, j - 1].idSalaPai != -1)
                        {
                            idsemchave[dungeonInserida.dungeon[i, j].idSalaPai] = true;
                        }

                    }
                }
            }

            for (int k = 0; k < dungeonInserida.numeroSalas + 2; k++)
            {
                if (idsemchave[k] == true)
                {
                    dungeonInserida.salas[k].chave = false;
                    dungeonInserida.salas[k].trancado = false;
                    for (int i = 1; i < dungeonInserida.tamanhoDungeon.x - 1; i++)
                    {
                        for (int j = 1; j < dungeonInserida.tamanhoDungeon.y - 1; j++)
                        {
                            if (dungeonInserida.dungeon[i, j].idSalaPai == k)
                            {
                                dungeonInserida.dungeon[i, j].chave = false;
                            }

                        }
                    }

                }

            }



        }

        public void InserirCaminhos(Dungeon dungeonInserida)
        {
            int contadorErrorCaminho = 0;
            //Declarando Vetor que armazena as coordenadas das portas e entradas.
            Vetor2[] coordPortasEntradas = new Vetor2[dungeonInserida.numeroEntradas + (Config.numeroMaxPortas * dungeonInserida.numeroSalas)];
            int contadorCoord = 0;
            //Varrendo a dungeon e armazenando as coordenadas das portas e entradas dentro do vetor.
            for (int i = 0; i < dungeonInserida.tamanhoDungeon.y; i++)
            {
                for (int j = 0; j < dungeonInserida.tamanhoDungeon.x; j++)
                {
                    if (dungeonInserida.dungeon[j, i].tipoCelula == "porta" || dungeonInserida.dungeon[j, i].tipoCelula == "entrada")
                    {
                        dungeonInserida.dungeon[j, i].pegarAdjacentes(dungeonInserida.dungeon[j, i]);
                        if (dungeonInserida.dungeon[j, i].adjbaixo != null && dungeonInserida.dungeon[j, i].adjbaixo.tipoCelula == "parede")
                        {
                            coordPortasEntradas[contadorCoord] = dungeonInserida.dungeon[j, i].adjbaixo.coordCelulaDun;
                            contadorCoord++;
                        }
                        if (dungeonInserida.dungeon[j, i].adjcima != null && dungeonInserida.dungeon[j, i].adjcima.tipoCelula == "parede")
                        {
                            coordPortasEntradas[contadorCoord] = dungeonInserida.dungeon[j, i].adjcima.coordCelulaDun;
                            contadorCoord++;
                        }
                        if (dungeonInserida.dungeon[j, i].adjdir != null && dungeonInserida.dungeon[j, i].adjdir.tipoCelula == "parede")
                        {
                            coordPortasEntradas[contadorCoord] = dungeonInserida.dungeon[j, i].adjdir.coordCelulaDun;
                            contadorCoord++;
                        }
                        if (dungeonInserida.dungeon[j, i].adjesq != null && dungeonInserida.dungeon[j, i].adjesq.tipoCelula == "parede")
                        {
                            coordPortasEntradas[contadorCoord] = dungeonInserida.dungeon[j, i].adjesq.coordCelulaDun;
                            contadorCoord++;
                        }

                    }
                }

            }
            //Fim da Varredura.
            //Randomizando uma coordenada de partida para criação dos caminhos da dungeon.
            dungeonInserida.terminarCaminho = true;
            while (dungeonInserida.terminarCaminho)
            {
                dungeonInserida.CriarCaminho(dungeonInserida, coordPortasEntradas, contadorCoord, contadorErrorCaminho);
                contadorErrorCaminho++;
                if (contadorErrorCaminho >= 9999999)
                {
                    dungeonInserida.errorCaminho = true;
                    break;


                }
            }
            PivotCaminho(dungeonInserida, contadorCoord);




            int contadorDebug;
            for (int i = 0; i < dungeonInserida.tamanhoDungeon.x; i++)
            {
                contadorDebug = 0;
                for (int j = 0; j < dungeonInserida.tamanhoDungeon.y; j++)
                {
                    contadorDebug = 0;
                    if (dungeonInserida.dungeon[i, j].tipoCelula == "parede")
                    {
                        dungeonInserida.dungeon[i, j].pegarAdjacentes(dungeonInserida.dungeon[i, j]);
                        if (dungeonInserida.dungeon[i, j].adjcima != null && dungeonInserida.dungeon[i, j].adjbaixo != null && dungeonInserida.dungeon[i, j].adjesq != null && dungeonInserida.dungeon[i, j].adjdir != null)
                        {
                            if (dungeonInserida.dungeon[i, j].adjcima.tipoCelula == "caminho") { contadorDebug++; }
                            if (dungeonInserida.dungeon[i, j].adjbaixo.tipoCelula == "caminho") { contadorDebug++; }
                            if (dungeonInserida.dungeon[i, j].adjesq.tipoCelula == "caminho") { contadorDebug++; }
                            if (dungeonInserida.dungeon[i, j].adjdir.tipoCelula == "caminho") { contadorDebug++; }
                            if (contadorDebug >= 3) { dungeonInserida.dungeon[i, j].tipoCelula = "caminho"; }
                        }
                    }

                }
            }

        }

        public void CriarCaminho(Dungeon dungeonInserida, Vetor2[] coordPartida, int contadorCoord, int contadorErrorCaminho)
        {
            System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
            int condicaoCompleta = 0;
            for (int i = 0; i < contadorCoord; i++)
            {
                if (dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].idCaminho == 100 || dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].idCaminho == i)
                {
                    dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].pegarAdjacentes(dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y]);
                    int direcao = rnd.Next(1, 101);//next ajustado
                    dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].tipoCelula = "caminho";
                    dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].idCaminho = i;
                    if (direcao > 1 && direcao <= 25)//cima
                    {

                        if (dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjcima != null)
                        {
                            if (dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjcima.tipoCelula == "parede" || dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjcima.tipoCelula == "caminho")
                            {
                                //coordPartida[i].x = dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjcima.coordCelulaDun.x;
                                coordPartida[i].y = coordPartida[i].y - 1;

                            }

                        }
                    }
                    if (direcao > 25 && direcao <= 50)//baixo
                    {
                        if (dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjbaixo != null)
                        {
                            if (dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjbaixo.tipoCelula == "parede" || dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjbaixo.tipoCelula == "caminho")
                            {
                                //coordPartida[i].x = dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjbaixo.coordCelulaDun.x;
                                coordPartida[i].y = coordPartida[i].y + 1;

                            }

                        }
                    }
                    if (direcao > 50 && direcao <= 75)//esquerda
                    {
                        if (dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjesq != null)
                        {
                            if (dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjesq.tipoCelula == "parede" || dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjesq.tipoCelula == "caminho")
                            {
                                if (dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjesq.coordCelulaDun != null)
                                {
                                    coordPartida[i].x = coordPartida[i].x - 1;
                                    //coordPartida[i].y = dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjesq.coordCelulaDun.y;
                                }


                            }
                        }
                    }
                    if (direcao > 75 && direcao <= 100)//direita
                    {
                        if (dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjdir != null)
                        {
                            if (dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjdir.tipoCelula == "parede" || dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjdir.tipoCelula == "caminho")
                            {
                                coordPartida[i].x = coordPartida[i].x + 1;
                                //coordPartida[i].y = dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjdir.coordCelulaDun.y;
                            }
                        }
                    }
                }
                else
                {
                    condicaoCompleta++;
                    if (condicaoCompleta == contadorCoord - 1) { dungeonInserida.terminarCaminho = false; }// original: condicaoCompleta == contadorCoord - 1
                }
            }
            if (contadorErrorCaminho >= 9999999)
            {
                dungeonInserida.errorCaminho = true;
                return;
            }
        }

        public void PivotCaminho(Dungeon dungeonInserida, int contadorCoord)
        {
            int contadorErroCaminho = 0;
            bool geracaoCoord = true;
            System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
            Vetor2 coordPartida = new Vetor2(0, 0);
            //gerando coordenada inicial.
            while (geracaoCoord)
            {
                coordPartida.x = rnd.Next(1, dungeonInserida.tamanhoDungeon.x - 2);
                coordPartida.y = rnd.Next(1, dungeonInserida.tamanhoDungeon.y - 2);
                if (dungeonInserida.dungeon[coordPartida.x, coordPartida.y].tipoCelula == "parede" || dungeonInserida.dungeon[coordPartida.x, coordPartida.y].tipoCelula == "caminho")
                {
                    geracaoCoord = false;
                }

            }
            //preence o vetor com todas as possibilidades de id.
            int[] todasIds = new int[contadorCoord];
            for (int i = 0; i < contadorCoord; i++)
            {
                todasIds[i] = i;
            }
            //Constroi o caminho pivot-----------------------------

            int direcao;
            dungeonInserida.terminarPivot = true;
            while (dungeonInserida.terminarPivot)
            {
                int condicaoCompleta = 0;
                dungeonInserida.dungeon[coordPartida.x, coordPartida.y].pegarAdjacentes(dungeonInserida.dungeon[coordPartida.x, coordPartida.y]);
                direcao = rnd.Next(1, 101); // next ajustado
                dungeonInserida.dungeon[coordPartida.x, coordPartida.y].tipoCelula = "caminho";
                for (int i = 0; i < contadorCoord; i++)
                {
                    if (todasIds[i] == dungeonInserida.dungeon[coordPartida.x, coordPartida.y].idCaminho)
                    {
                        todasIds[i] = 200;
                    }
                }
                if (direcao > 1 && direcao <= 25)//cima
                {

                    if (dungeonInserida.dungeon[coordPartida.x, coordPartida.y].adjcima != null)
                    {
                        if (dungeonInserida.dungeon[coordPartida.x, coordPartida.y].adjcima.tipoCelula == "parede" || dungeonInserida.dungeon[coordPartida.x, coordPartida.y].adjcima.tipoCelula == "caminho")
                        {
                            //coordPartida[i].x = dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjcima.coordCelulaDun.x;
                            coordPartida.y = coordPartida.y - 1;

                        }

                    }
                }
                if (direcao > 25 && direcao <= 50)//baixo
                {
                    if (dungeonInserida.dungeon[coordPartida.x, coordPartida.y].adjbaixo != null)
                    {
                        if (dungeonInserida.dungeon[coordPartida.x, coordPartida.y].adjbaixo.tipoCelula == "parede" || dungeonInserida.dungeon[coordPartida.x, coordPartida.y].adjbaixo.tipoCelula == "caminho")
                        {
                            //coordPartida[i].x = dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjbaixo.coordCelulaDun.x;
                            coordPartida.y = coordPartida.y + 1;

                        }

                    }
                }
                if (direcao > 50 && direcao <= 75)//esquerda
                {
                    if (dungeonInserida.dungeon[coordPartida.x, coordPartida.y].adjesq != null)
                    {
                        if (dungeonInserida.dungeon[coordPartida.x, coordPartida.y].adjesq.tipoCelula == "parede" || dungeonInserida.dungeon[coordPartida.x, coordPartida.y].adjesq.tipoCelula == "caminho")
                        {
                            if (dungeonInserida.dungeon[coordPartida.x, coordPartida.y].adjesq.coordCelulaDun != null)
                            {
                                coordPartida.x = coordPartida.x - 1;
                                //coordPartida[i].y = dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjesq.coordCelulaDun.y;
                            }


                        }
                    }
                }
                if (direcao > 75 && direcao <= 100)//direita
                {
                    if (dungeonInserida.dungeon[coordPartida.x, coordPartida.y].adjdir != null)
                    {
                        if (dungeonInserida.dungeon[coordPartida.x, coordPartida.y].adjdir.tipoCelula == "parede" || dungeonInserida.dungeon[coordPartida.x, coordPartida.y].adjdir.tipoCelula == "caminho")
                        {
                            coordPartida.x = coordPartida.x + 1;
                            //coordPartida[i].y = dungeonInserida.dungeon[coordPartida[i].x, coordPartida[i].y].adjdir.coordCelulaDun.y;
                        }
                    }
                }




                //checando se já se passou por todas as ids possiveis.
                for (int i = 0; i < contadorCoord; i++)
                {
                    if (todasIds[i] == 200)
                    {
                        condicaoCompleta++;
                        if (condicaoCompleta >= contadorCoord) { dungeonInserida.terminarPivot = false; }//original: condicaoCompleta >= contadorCoord - 1 (testando se a formula atual remove o bug de uma das salas n se conectar c as outras)
                        else { contadorErroCaminho++; }
                    }
                }
                //DEBUG LOOP INFINITO(CONDICAO NUNCA ATENDIDA)
                if (contadorErroCaminho >= 9999999)
                {
                    dungeonInserida.errorCaminho = true;
                    break;


                }
            }
        }
        public void inserirDungeonPai(Dungeon dungeonInserida)
        {
            for (int i = 0; i < dungeonInserida.tamanhoDungeon.y; i++)
            {
                for (int j = 0; j < dungeonInserida.tamanhoDungeon.x; j++)
                {
                    dungeonInserida.dungeon[j, i].dungeonPai = dungeonInserida;
                }

            }
        }

        public void DebugPosCaminho(Dungeon dungeonInserida)
        {
            for (int i = 0; i < dungeonInserida.tamanhoDungeon.y; i++)
            {
                for (int j = 0; j < dungeonInserida.tamanhoDungeon.x; j++)
                {
                    if (dungeonInserida.dungeon[j, i].ehPorta == true)
                    {
                        dungeonInserida.dungeon[j, i].tipoCelula = "porta";
                    }
                    if (dungeonInserida.dungeon[j, i].ehEntrada == true)
                    {
                        dungeonInserida.dungeon[j, i].tipoCelula = "entrada";
                    }
                    if (dungeonInserida.dungeon[j, i].ehEntradaSecreta == true)
                    {
                        dungeonInserida.dungeon[j, i].tipoCelula = "entrada";
                    }
                }

            }

        }

        public static void ReestabelecerCoords(Dungeon dungeonInserida)
        {
            for (int i = 0; i < dungeonInserida.tamanhoDungeon.y; i++)
            {
                for (int j = 0; j < dungeonInserida.tamanhoDungeon.x; j++)
                {
                    dungeonInserida.dungeon[j, i].coordCelulaDun.x = j;
                    dungeonInserida.dungeon[j, i].coordCelulaDun.y = i;
                }

            }

        }
        public bool FiltroCaminho(Dungeon dungeonInserida)
        {
            int contadorCaminho = 0;
            int areaDungeon = (dungeonInserida.tamanhoDungeon.y - 2) * (dungeonInserida.tamanhoDungeon.x - 2);
            for (int i = 1; i < dungeonInserida.tamanhoDungeon.y - 1; i++)
            {
                for (int j = 1; j < dungeonInserida.tamanhoDungeon.x - 1; j++)
                {
                    if (dungeonInserida.dungeon[j, i].tipoCelula == "caminho")
                    {
                        contadorCaminho++;
                    }
                }

            }

            if (contadorCaminho > (areaDungeon * 45 / 100))
            {
                //foi retirado o for que preenchia a dungeon com paredes, por aparentemente ser desnecessario, uma vez que se retorna o valor "true", para substituir a dungeon por outra.
                return true;
            }
            return false;
        }
        public void InserirSubtipos(Dungeon dungeonInserida)//Insere os subtipos em celulas.
        {
            for (int i = 1; i < dungeonInserida.tamanhoDungeon.y - 1; i++)
            {
                for (int j = 1; j < dungeonInserida.tamanhoDungeon.x - 1; j++)
                {
                    dungeonInserida.dungeon[j, i].pegarAdjacentes(dungeonInserida.dungeon[j, i]);
                    //SUBTIPOS DA PAREDE AJUSTES INICIAIS:
                    if (dungeonInserida.dungeon[j, i].tipoCelula == "parede")
                    {
                        //subtipo esquerda:
                        if (dungeonInserida.dungeon[j, i].adjesq.tipoCelula != "parede" && dungeonInserida.dungeon[j, i].adjdir.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjcima.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjbaixo.tipoCelula == "parede")
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "esquerda";
                        }
                        //subtipo direita:
                        else if (dungeonInserida.dungeon[j, i].adjesq.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjdir.tipoCelula != "parede" && dungeonInserida.dungeon[j, i].adjcima.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjbaixo.tipoCelula == "parede")
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "direita";
                        }
                        //subtipo cima:
                        else if (dungeonInserida.dungeon[j, i].adjesq.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjdir.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjcima.tipoCelula != "parede" && dungeonInserida.dungeon[j, i].adjbaixo.tipoCelula == "parede")
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "cima";
                        }
                        //subtipo baixo:
                        else if (dungeonInserida.dungeon[j, i].adjesq.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjdir.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjcima.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjbaixo.tipoCelula != "parede")
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "baixo";
                        }
                        //subtipo individual
                        else
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "individual";
                        }
                        //subtipo quinaEC:
                        if (dungeonInserida.dungeon[j, i].adjesq.tipoCelula != "parede" && dungeonInserida.dungeon[j, i].adjdir.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjcima.tipoCelula != "parede" && dungeonInserida.dungeon[j, i].adjbaixo.tipoCelula == "parede")
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "quinaEC";
                        }
                        //subtipo quinaEB:
                        if (dungeonInserida.dungeon[j, i].adjesq.tipoCelula != "parede" && dungeonInserida.dungeon[j, i].adjdir.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjcima.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjbaixo.tipoCelula != "parede")
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "quinaEB";
                        }
                        //subtipo quinaDC:
                        if (dungeonInserida.dungeon[j, i].adjesq.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjdir.tipoCelula != "parede" && dungeonInserida.dungeon[j, i].adjcima.tipoCelula != "parede" && dungeonInserida.dungeon[j, i].adjbaixo.tipoCelula == "parede")
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "quinaDC";
                        }
                        //subtipo quinaDB:
                        if (dungeonInserida.dungeon[j, i].adjesq.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjdir.tipoCelula != "parede" && dungeonInserida.dungeon[j, i].adjcima.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjbaixo.tipoCelula != "parede")
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "quinaDB";
                        }
                        //subtipo centro
                        if (dungeonInserida.dungeon[j, i].adjesq.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjdir.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjcima.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjbaixo.tipoCelula == "parede")
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "centro";
                        }

                    }

                }

            }
            //SUBTIPOS PAREDES AJUSTES FINAIS(RODA DUAS VEZES DENTRO DO FOR):
            for (int check = 0; check < 2; check++)
            {
                //----------------LACO FOR 2 VEZES -----------------
                for (int i = 1; i < dungeonInserida.tamanhoDungeon.y - 1; i++)
                {
                    for (int j = 1; j < dungeonInserida.tamanhoDungeon.x - 1; j++)
                    {
                        dungeonInserida.dungeon[j, i].pegarAdjacentes(dungeonInserida.dungeon[j, i]);
                        if (dungeonInserida.dungeon[j, i].tipoCelula == "parede")
                        {
                            //SUBTIPOS QUINA:
                            //subtipo quinaEC:
                            if ((dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "cima" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "quinaDC" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "innerEC") && (dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "esquerda" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "quinaEB" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "innerEC"))
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "quinaEC";
                            }
                            //subtipo quinaEB:
                            if ((dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "baixo" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "quinaDB" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "innerEB") && (dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "quinaEC" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "esquerda" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "innerEB"))
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "quinaEB";
                            }
                            //subtipo quinaDC:
                            if ((dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "cima" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "quinaEC" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "innerDC") && (dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "direita" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "quinaDB" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "innerDC"))
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "quinaDC";
                            }
                            //subtipo quinaDB:
                            if ((dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "baixo" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "quinaEB" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "innerDB") && (dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "direita" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "quinaDC" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "innerDB"))
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "quinaDB";
                            }
                            //SUBTIPOS INNER:
                            if (dungeonInserida.dungeon[j, i].adjesq.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjdir.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjcima.tipoCelula == "parede" && dungeonInserida.dungeon[j, i].adjbaixo.tipoCelula == "parede")
                            {
                                //subtipo innerEB
                                if ((dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "baixo" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "quinaEB") && (dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "esquerda" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "quinaEB"))
                                {
                                    dungeonInserida.dungeon[j, i].subTipoCelula = "innerEB";
                                }
                                //subtipo innerEC
                                if ((dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "cima" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "quinaEC") && (dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "esquerda" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "quinaEC"))
                                {
                                    dungeonInserida.dungeon[j, i].subTipoCelula = "innerEC";
                                }
                                //subtipo innerDC
                                if ((dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "cima" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "quinaDC") && (dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "direita" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "quinaDC"))
                                {
                                    dungeonInserida.dungeon[j, i].subTipoCelula = "innerDC";
                                }
                                //subtipo innerDB
                                if ((dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "baixo" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "quinaDB") && (dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "direita" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "quinaDB"))
                                {
                                    dungeonInserida.dungeon[j, i].subTipoCelula = "innerDB";
                                }
                            }
                            //SUBTIPOS LADO:
                            //subtipo cima
                            if ((dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "cima" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "quinaEC" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "innerDC") && (dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "cima" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "quinaDC" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "innerEC"))
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "cima";
                            }
                            //subtipo baixo
                            if ((dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "baixo" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "quinaEB" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "innerDB") && (dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "baixo" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "quinaDB" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "innerEB"))
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "baixo";
                            }
                            //subtipo direita
                            if ((dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "direita" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "quinaDC" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "innerDB") && (dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "direita" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "quinaDB" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "innerDC"))
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "direita";
                            }
                            //subtipo esquerda
                            if ((dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "esquerda" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "quinaEC" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "innerEB") && (dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "esquerda" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "quinaEB" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "innerEC"))
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "esquerda";
                            }
                            //SUBTIPOS DIAGONAL:
                            //diagonalDC(innerEC e innerDB ao mesmo tempo)
                            if (dungeonInserida.dungeon[j, i].subTipoCelula == "innerEC")
                            {
                                if ((dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "baixo" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "quinaDB") && (dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "direita" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "quinaDB"))
                                {
                                    dungeonInserida.dungeon[j, i].subTipoCelula = "diagonalDC";
                                }
                            }
                            if (dungeonInserida.dungeon[j, i].subTipoCelula == "innerDB")
                            {
                                if ((dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "cima" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "quinaEC") && (dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "esquerda" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "quinaEC"))
                                {
                                    dungeonInserida.dungeon[j, i].subTipoCelula = "diagonalDC";
                                }
                            }
                            //diagonalEC(innerEB e innerDC ao mesmo tempo)
                            if (dungeonInserida.dungeon[j, i].subTipoCelula == "innerEB")
                            {
                                if ((dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "cima" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "quinaDC") && (dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "direita" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "quinaDC"))
                                {
                                    dungeonInserida.dungeon[j, i].subTipoCelula = "diagonalEC";
                                }
                            }
                            if (dungeonInserida.dungeon[j, i].subTipoCelula == "innerDC")
                            {
                                if ((dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "baixo" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "quinaEB") && (dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "esquerda" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "quinaEB"))
                                {
                                    dungeonInserida.dungeon[j, i].subTipoCelula = "diagonalEC";
                                }
                            }

                        }
                    }

                }
            }//--------------------FIM DO LACO FOR 2 VEZES-------------------
             //DEBUG BLOCOS INDIVIDUAIS DESCONEXOS:
            for (int i = 0; i < dungeonInserida.tamanhoDungeon.y; i++)
            {
                for (int j = 0; j < dungeonInserida.tamanhoDungeon.x; j++)
                {
                    if (dungeonInserida.dungeon[j, i].tipoCelula == "parede")
                    {
                        //subtipo cima
                        if (dungeonInserida.dungeon[j, i].subTipoCelula == "cima")
                        {
                            if ((dungeonInserida.dungeon[j, i].adjesq.subTipoCelula != "cima" && dungeonInserida.dungeon[j, i].adjesq.subTipoCelula != "quinaEC" && dungeonInserida.dungeon[j, i].adjesq.subTipoCelula != "innerDC" && dungeonInserida.dungeon[j, i].adjesq.subTipoCelula != "diagonalEC") || (dungeonInserida.dungeon[j, i].adjdir.subTipoCelula != "cima" && dungeonInserida.dungeon[j, i].adjdir.subTipoCelula != "quinaDC" && dungeonInserida.dungeon[j, i].adjdir.subTipoCelula != "innerEC" && dungeonInserida.dungeon[j, i].adjdir.subTipoCelula != "diagonalDC"))
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "individual";
                            }
                        }
                        //subtipo baixo
                        if (dungeonInserida.dungeon[j, i].subTipoCelula == "baixo")
                        {
                            if ((dungeonInserida.dungeon[j, i].adjesq.subTipoCelula != "baixo" && dungeonInserida.dungeon[j, i].adjesq.subTipoCelula != "quinaEB" && dungeonInserida.dungeon[j, i].adjesq.subTipoCelula != "innerDB" && dungeonInserida.dungeon[j, i].adjesq.subTipoCelula != "diagonalDC") || (dungeonInserida.dungeon[j, i].adjdir.subTipoCelula != "baixo" && dungeonInserida.dungeon[j, i].adjdir.subTipoCelula != "quinaDB" && dungeonInserida.dungeon[j, i].adjdir.subTipoCelula != "innerEB" && dungeonInserida.dungeon[j, i].adjdir.subTipoCelula != "diagonalEC"))
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "individual";
                            }
                        }
                        //subtipo esquerda
                        if (dungeonInserida.dungeon[j, i].subTipoCelula == "esquerda")
                        {
                            if ((dungeonInserida.dungeon[j, i].adjcima.subTipoCelula != "esquerda" && dungeonInserida.dungeon[j, i].adjcima.subTipoCelula != "quinaEC" && dungeonInserida.dungeon[j, i].adjcima.subTipoCelula != "innerEB" && dungeonInserida.dungeon[j, i].adjcima.subTipoCelula != "diagonalEC") || (dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula != "esquerda" && dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula != "quinaEB" && dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula != "innerEC" && dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula != "diagonalDC"))
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "individual";
                            }
                        }
                        //subtipo direita
                        if (dungeonInserida.dungeon[j, i].subTipoCelula == "direita")
                        {
                            if ((dungeonInserida.dungeon[j, i].adjcima.subTipoCelula != "direita" && dungeonInserida.dungeon[j, i].adjcima.subTipoCelula != "quinaDC" && dungeonInserida.dungeon[j, i].adjcima.subTipoCelula != "innerDB" && dungeonInserida.dungeon[j, i].adjcima.subTipoCelula != "diagonalDC") || (dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula != "direita" && dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula != "quinaDB" && dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula != "innerDC" && dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula != "diagonalEC"))
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "individual";
                            }
                        }
                        //subtipo quinaEC
                        if (dungeonInserida.dungeon[j, i].subTipoCelula == "quinaEC")
                        {
                            if ((dungeonInserida.dungeon[j, i].adjdir.subTipoCelula != "cima" && dungeonInserida.dungeon[j, i].adjdir.subTipoCelula != "quinaDC" && dungeonInserida.dungeon[j, i].adjdir.subTipoCelula != "innerEC" && dungeonInserida.dungeon[j, i].adjdir.subTipoCelula != "diagonalDC") || (dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula != "esquerda" && dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula != "quinaEB" && dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula != "innerEC" && dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula != "diagonalDC"))
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "individual";
                            }
                        }
                        //subtipo quinaEB
                        if (dungeonInserida.dungeon[j, i].subTipoCelula == "quinaEB")
                        {
                            if ((dungeonInserida.dungeon[j, i].adjdir.subTipoCelula != "baixo" && dungeonInserida.dungeon[j, i].adjdir.subTipoCelula != "quinaDB" && dungeonInserida.dungeon[j, i].adjdir.subTipoCelula != "innerEB" && dungeonInserida.dungeon[j, i].adjdir.subTipoCelula != "diagonalEC") || (dungeonInserida.dungeon[j, i].adjcima.subTipoCelula != "esquerda" && dungeonInserida.dungeon[j, i].adjcima.subTipoCelula != "quinaEC" && dungeonInserida.dungeon[j, i].adjcima.subTipoCelula != "innerEB" && dungeonInserida.dungeon[j, i].adjcima.subTipoCelula != "diagonalEC"))
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "individual";
                            }
                        }
                        //subtipo quinaDC
                        if (dungeonInserida.dungeon[j, i].subTipoCelula == "quinaDC")
                        {
                            if ((dungeonInserida.dungeon[j, i].adjesq.subTipoCelula != "cima" && dungeonInserida.dungeon[j, i].adjesq.subTipoCelula != "quinaEC" && dungeonInserida.dungeon[j, i].adjesq.subTipoCelula != "innerDC" && dungeonInserida.dungeon[j, i].adjesq.subTipoCelula != "diagonalEC") || (dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula != "direita" && dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula != "quinaDB" && dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula != "innerDC" && dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula != "diagonalEC"))
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "individual";
                            }
                        }
                        //subtipo quinaDB
                        if (dungeonInserida.dungeon[j, i].subTipoCelula == "quinaDB")
                        {
                            if ((dungeonInserida.dungeon[j, i].adjesq.subTipoCelula != "baixo" && dungeonInserida.dungeon[j, i].adjesq.subTipoCelula != "quinaEB" && dungeonInserida.dungeon[j, i].adjesq.subTipoCelula != "innerDB" && dungeonInserida.dungeon[j, i].adjesq.subTipoCelula != "diagonalDC") || (dungeonInserida.dungeon[j, i].adjcima.subTipoCelula != "direita" && dungeonInserida.dungeon[j, i].adjcima.subTipoCelula != "quinaDC" && dungeonInserida.dungeon[j, i].adjcima.subTipoCelula != "innerDB" && dungeonInserida.dungeon[j, i].adjcima.subTipoCelula != "diagonalDC"))
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "individual";
                            }
                        }


                    }
                }

            }
            //DEBUG DE SUBTIPO DO  tipo PSALA.

            //Debug duas quinas opostas lado a lado.(em observacao pra ver se ta tudo certo)-------
            for (int i = 1; i < dungeonInserida.tamanhoDungeon.y - 1; i++)
            {
                for (int j = 1; j < dungeonInserida.tamanhoDungeon.x - 1; j++)
                {
                    if (dungeonInserida.dungeon[j, i].tipoCelula == "pSala")
                    {
                        //quinaEB c/ quinaDB
                        if (dungeonInserida.dungeon[j, i].subTipoCelula == "quinaEBp")
                        {
                            if (dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "quinaDBp" && dungeonInserida.dungeon[j, i].adjcima.tipoCelula == "chao")
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "baixop";
                            }
                        }
                        //quinaEC c/ quinaDC
                        if (dungeonInserida.dungeon[j, i].subTipoCelula == "quinaECp")
                        {
                            if (dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "quinaDCp" && dungeonInserida.dungeon[j, i].adjbaixo.tipoCelula == "chao")
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "cimap";
                            }
                        }
                        //quinaEC c/ quinaEB
                        if (dungeonInserida.dungeon[j, i].subTipoCelula == "quinaECp")
                        {
                            if (dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "quinaEBp" && dungeonInserida.dungeon[j, i].adjdir.tipoCelula == "chao")
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "esquerdap";
                            }
                        }
                        //quinaDC c/ quinaDB
                        if (dungeonInserida.dungeon[j, i].subTipoCelula == "quinaDCp")
                        {
                            if (dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "quinaDBp" && dungeonInserida.dungeon[j, i].adjesq.tipoCelula == "chao")
                            {
                                dungeonInserida.dungeon[j, i].subTipoCelula = "direitap";
                            }
                        }
                    }
                }

            }
            //-----
            // debug celulas caminho onde deveria ser celula pSala.(falta algumas situacoes, como por exemplo as quinas, ou a cima dos innerp)---------
            for (int i = 1; i < dungeonInserida.tamanhoDungeon.y - 1; i++)
            {
                for (int j = 1; j < dungeonInserida.tamanhoDungeon.x - 1; j++)
                {
                    if (dungeonInserida.dungeon[j, i].tipoCelula == "caminho")
                    {
                        //caminho no lado esquerdo.
                        if (dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "esquerdap" && dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "esquerdap")
                        {
                            dungeonInserida.dungeon[j, i].tipoCelula = "pSala";
                            dungeonInserida.dungeon[j, i].subTipoCelula = "esquerdap";

                        }
                        //caminho no lado direito.
                        if (dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "direitap" && dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "direitap")
                        {
                            dungeonInserida.dungeon[j, i].tipoCelula = "pSala";
                            dungeonInserida.dungeon[j, i].subTipoCelula = "direitap";

                        }
                        //caminho em cima.
                        if (dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "cimap" && dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "cimap")
                        {
                            dungeonInserida.dungeon[j, i].tipoCelula = "pSala";
                            dungeonInserida.dungeon[j, i].subTipoCelula = "cimap";

                        }
                        //caminho em baixo.
                        if (dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "baixop" && dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "baixop")
                        {
                            dungeonInserida.dungeon[j, i].tipoCelula = "pSala";
                            dungeonInserida.dungeon[j, i].subTipoCelula = "baixop";

                        }
                    }
                }

            }
            //-----



            for (int j = 1; j < dungeonInserida.tamanhoDungeon.y - 1; j++)
            {
                for (int i = 1; i < dungeonInserida.tamanhoDungeon.x - 1; i++)
                {
                    dungeonInserida.dungeon[i, j].pegarAdjacentes(dungeonInserida.dungeon[i, j]);
                    if (dungeonInserida.dungeon[i, j].tipoCelula == "chao")
                    {
                        //subtipo innerECp
                        if ((dungeonInserida.dungeon[i, j].adjesq.subTipoCelula == "cimap" || dungeonInserida.dungeon[i, j].adjesq.subTipoCelula == "quinaECp") && (dungeonInserida.dungeon[i, j].adjcima.tipoCelula == "pSala"))
                        {
                            dungeonInserida.dungeon[i, j].tipoCelula = "pSala";
                            dungeonInserida.dungeon[i, j].subTipoCelula = "innerECp";
                        }
                        else if ((dungeonInserida.dungeon[i, j].adjcima.subTipoCelula == "esquerdap" || dungeonInserida.dungeon[i, j].adjcima.subTipoCelula == "quinaECp") && (dungeonInserida.dungeon[i, j].adjesq.tipoCelula == "pSala"))
                        {
                            dungeonInserida.dungeon[i, j].tipoCelula = "pSala";
                            dungeonInserida.dungeon[i, j].subTipoCelula = "innerECp";

                        }
                        //subtipo innerEBp
                        if ((dungeonInserida.dungeon[i, j].adjesq.subTipoCelula == "baixop" || dungeonInserida.dungeon[i, j].adjesq.subTipoCelula == "quinaEBp") && (dungeonInserida.dungeon[i, j].adjbaixo.tipoCelula == "pSala"))
                        {
                            dungeonInserida.dungeon[i, j].tipoCelula = "pSala";
                            dungeonInserida.dungeon[i, j].subTipoCelula = "innerEBp";
                        }
                        else if ((dungeonInserida.dungeon[i, j].adjbaixo.subTipoCelula == "esquerdap" || dungeonInserida.dungeon[i, j].adjbaixo.subTipoCelula == "quinaEBp") && (dungeonInserida.dungeon[i, j].adjesq.tipoCelula == "pSala"))
                        {
                            dungeonInserida.dungeon[i, j].tipoCelula = "pSala";
                            dungeonInserida.dungeon[i, j].subTipoCelula = "innerEBp";
                        }
                        //subtipo innerDCp
                        if ((dungeonInserida.dungeon[i, j].adjdir.subTipoCelula == "cimap" || dungeonInserida.dungeon[i, j].adjdir.subTipoCelula == "quinaDCp") && (dungeonInserida.dungeon[i, j].adjcima.tipoCelula == "pSala"))
                        {
                            dungeonInserida.dungeon[i, j].tipoCelula = "pSala";
                            dungeonInserida.dungeon[i, j].subTipoCelula = "innerDCp";
                        }
                        else if ((dungeonInserida.dungeon[i, j].adjcima.subTipoCelula == "direitap" || dungeonInserida.dungeon[i, j].adjcima.subTipoCelula == "quinaDCp") && (dungeonInserida.dungeon[i, j].adjdir.tipoCelula == "pSala"))
                        {
                            dungeonInserida.dungeon[i, j].tipoCelula = "pSala";
                            dungeonInserida.dungeon[i, j].subTipoCelula = "innerDCp";
                        }
                        //subtipo innerDBp
                        if ((dungeonInserida.dungeon[i, j].adjdir.subTipoCelula == "baixop" || dungeonInserida.dungeon[i, j].adjdir.subTipoCelula == "quinaDBp") && (dungeonInserida.dungeon[i, j].adjbaixo.tipoCelula == "pSala"))
                        {
                            dungeonInserida.dungeon[i, j].tipoCelula = "pSala";
                            dungeonInserida.dungeon[i, j].subTipoCelula = "innerDBp";
                        }
                        else if ((dungeonInserida.dungeon[i, j].adjbaixo.subTipoCelula == "direitap" || dungeonInserida.dungeon[i, j].adjbaixo.subTipoCelula == "quinaDBp") && (dungeonInserida.dungeon[i, j].adjdir.tipoCelula == "pSala"))
                        {
                            dungeonInserida.dungeon[i, j].tipoCelula = "pSala";
                            dungeonInserida.dungeon[i, j].subTipoCelula = "innerDBp";
                        }
                    }
                }

            }
            //
            for (int i = 1; i < dungeonInserida.tamanhoDungeon.y - 1; i++)
            {
                for (int j = 1; j < dungeonInserida.tamanhoDungeon.x - 1; j++)
                {
                    if (dungeonInserida.dungeon[j, i].tipoCelula == "pSala" || dungeonInserida.dungeon[j, i].tipoCelula == "porta") // ficar atento na condicao, seja adicionado uma segunda celula porta para fazer uma porta maior.
                    {
                        //subtipo cima
                        if ((dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "cimap" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "quinaECp" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "innerDCp") && (dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "cimap" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "quinaDCp" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "innerECp"))
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "cimap";
                        }
                        //subtipo baixo
                        if ((dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "baixop" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "quinaEBp" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "innerDBp") && (dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "baixop" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "quinaDBp" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "innerEBp"))
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "baixop";
                        }
                        //subtipo direita
                        if ((dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "direitap" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "quinaDCp" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "innerDBp") && (dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "direitap" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "quinaDBp" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "innerDCp"))
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "direitap";
                        }
                        //subtipo esquerda
                        if ((dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "esquerdap" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "quinaECp" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "innerEBp") && (dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "esquerdap" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "quinaEBp" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "innerECp"))
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "esquerdap";
                        }
                        //AS QUINAS NAO ESTAO CONSIDERANDO O TIPO PORTA, POIS O TIPO PORTA NAO SPAWNA EM QUINAS.(EM OBSERVACAO)
                        //subtipo quinaEC):
                        if ((dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "cimap" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "quinaDCp" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "innerECp") && (dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "esquerdap" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "quinaEBp" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "innerECp"))
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "quinaECp";
                        }
                        //subtipo quinaEB:
                        if ((dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "baixop" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "quinaDBp" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "innerEBp") && (dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "quinaECp" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "esquerdap" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "innerEBp"))
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "quinaEBp";
                        }
                        //subtipo quinaDC:
                        if ((dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "cimap" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "quinaECp" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "innerDCp") && (dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "direitap" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "quinaDBp" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "innerDCp"))
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "quinaDCp";
                        }
                        //subtipo quinaDB:
                        if ((dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "baixop" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "quinaEBp" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "innerDBp") && (dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "direitap" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "quinaDCp" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "innerDBp"))
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "quinaDBp";
                        }
                        // INNERS (EM OBSERVACAO)
                        //subtipo innerEB
                        if ((dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "baixop" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "quinaEBp" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "innerDBp") && (dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "esquerdap" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "quinaEBp" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "innerECp"))
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "innerEBp";
                        }
                        //subtipo innerEC
                        if ((dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "cimap" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "quinaECp" || dungeonInserida.dungeon[j, i].adjesq.subTipoCelula == "innerDCp") && (dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "esquerdap" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "quinaECp" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "innerEBp"))
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "innerECp";
                        }
                        //subtipo innerDC
                        if ((dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "cimap" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "quinaDCp" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "innerECp") && (dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "direitap" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "quinaDCp" || dungeonInserida.dungeon[j, i].adjcima.subTipoCelula == "innerDBp"))
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "innerDCp";
                        }
                        //subtipo innerDB
                        if ((dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "baixop" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "quinaDBp" || dungeonInserida.dungeon[j, i].adjdir.subTipoCelula == "innerEBp") && (dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "direitap" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "quinaDBp" || dungeonInserida.dungeon[j, i].adjbaixo.subTipoCelula == "innerDCp"))
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "innerDBp";
                        }
                        //subtipo individual(em observacao pra ver se ta tudo certo)
                        if (dungeonInserida.dungeon[j, i].tipoCelula == "pSala" && dungeonInserida.dungeon[j, i].adjcima.tipoCelula == "chao" && dungeonInserida.dungeon[j, i].adjbaixo.tipoCelula == "chao" && dungeonInserida.dungeon[j, i].adjdir.tipoCelula == "chao" && dungeonInserida.dungeon[j, i].adjesq.tipoCelula == "chao")
                        {
                            dungeonInserida.dungeon[j, i].subTipoCelula = "individualp";
                        }

                    }
                }

            }
            //
            for (int i = 1; i < dungeonInserida.tamanhoDungeon.y - 1; i++)
            {
                for (int j = 1; j < dungeonInserida.tamanhoDungeon.x - 1; j++)
                {
                    if (dungeonInserida.dungeon[j, i].tipoCelula == "pSala")
                    {
                        if (dungeonInserida.dungeon[j, i].adjcima.tipoCelula == "porta")
                        {
                            dungeonInserida.dungeon[j, i].ehComplementoPorta1 = true;
                        }
                        if (dungeonInserida.dungeon[j, i].adjbaixo.tipoCelula == "porta")
                        {
                            dungeonInserida.dungeon[j, i].ehComplementoPorta2 = true;
                        }
                        if (dungeonInserida.dungeon[j, i].adjesq.tipoCelula == "porta")
                        {
                            dungeonInserida.dungeon[j, i].ehComplementoPorta1 = true;
                        }
                        if (dungeonInserida.dungeon[j, i].adjdir.tipoCelula == "porta")
                        {
                            dungeonInserida.dungeon[j, i].ehComplementoPorta2 = true;
                        }

                    }
                }

            }

            for (int i = 0; i < dungeonInserida.tamanhoDungeon.y; i++)
            {
                for (int j = 0; j < dungeonInserida.tamanhoDungeon.x; j++)
                {
                    if (dungeonInserida.dungeon[j, i].tipoCelula == "contornoDungeon")
                    {
                        if (i > 0)
                        {
                            if (dungeonInserida.dungeon[j, i - 1].tipoCelula == "entrada" && dungeonInserida.dungeon[j, i - 1].ehEntradaSecreta == false)
                            {
                                dungeonInserida.dungeon[j, i].ehComplementoPorta1 = true;
                            }
                        }
                        if (i < dungeonInserida.tamanhoDungeon.y - 1)
                        {
                            if (dungeonInserida.dungeon[j, i + 1].tipoCelula == "entrada" && dungeonInserida.dungeon[j, i + 1].ehEntradaSecreta == false)
                            {
                                dungeonInserida.dungeon[j, i].ehComplementoPorta2 = true;
                            }
                        }
                        if (j > 0)
                        {
                            if (dungeonInserida.dungeon[j - 1, i].tipoCelula == "entrada" && dungeonInserida.dungeon[j - 1, i].ehEntradaSecreta == false)
                            {
                                dungeonInserida.dungeon[j, i].ehComplementoPorta1 = true;
                            }
                        }
                        if (j < dungeonInserida.tamanhoDungeon.x - 1)
                        {
                            if (dungeonInserida.dungeon[j + 1, i].tipoCelula == "entrada" && dungeonInserida.dungeon[j + 1, i].ehEntradaSecreta == false)
                            {
                                dungeonInserida.dungeon[j, i].ehComplementoPorta2 = true;
                            }
                        }


                    }
                }

            }


        }

        //
        public void AdicionarPlayerSpawn(Area areaPai, Dungeon dungeonInserida)
        {
            if (areaPai.inicial == true)
            {
                bool loop = true;
                System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
                while (loop == true)
                {
                    int coordSpawnx = rnd.Next(1, dungeonInserida.tamanhoDungeon.x - 1);
                    int coordSpawny = rnd.Next(1, dungeonInserida.tamanhoDungeon.y - 1);

                    if (dungeonInserida.dungeon[coordSpawnx, coordSpawny].tipoCelula == "caminho")
                    {
                        dungeonInserida.dungeon[coordSpawnx, coordSpawny].spawn = true;
                        for (int i = coordSpawnx - 2; i <= coordSpawnx + 2; i++)
                        {
                            for (int j = coordSpawny - 2; j <= coordSpawny + 2; j++)
                            {
                                if (i != coordSpawnx || j != coordSpawny)
                                {
                                    if (i > 0 && j > 0 && i < dungeonInserida.tamanhoDungeon.x - 1 && j < dungeonInserida.tamanhoDungeon.y - 1 && dungeonInserida.dungeon[i, j].tipoCelula == "caminho")
                                    {
                                        dungeonInserida.dungeon[i, j].subSpawn = true;
                                    }
                                }
                            }

                        }
                        loop = false;
                    }
                    else
                    {
                        loop = true;
                    }

                }


            }
        }
        //
        public void AdicionarBossSpawn(Area areaPai, Dungeon dungeonInserida)
        {
            if (areaPai.final == true)
            {
                bool permit;
                bool loop = true;
                System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
                while (loop == true)
                {
                    permit = true;
                    int coordSpawnx = rnd.Next(1, dungeonInserida.tamanhoDungeon.x - 1);
                    int coordSpawny = rnd.Next(1, dungeonInserida.tamanhoDungeon.y - 1);

                    if (dungeonInserida.dungeon[coordSpawnx, coordSpawny].tipoCelula == "caminho" || dungeonInserida.dungeon[coordSpawnx, coordSpawny].tipoCelula == "chao")
                    {
                        for (int i = coordSpawnx - 2; i <= coordSpawnx + 2; i++)
                        {
                            for (int j = coordSpawny - 2; j <= coordSpawny + 2; j++)
                            {
                                if (i != coordSpawnx || j != coordSpawny)
                                {
                                    if (i < 0 || j < 0 || i > dungeonInserida.tamanhoDungeon.x - 1 || j > dungeonInserida.tamanhoDungeon.y - 1 || (dungeonInserida.dungeon[i, j].tipoCelula != "chao" && dungeonInserida.dungeon[i, j].tipoCelula != "caminho") || dungeonInserida.dungeon[i, j].chave == true)
                                    {
                                        permit = false;
                                    }
                                }
                            }

                        }

                        if (permit == true)
                        {
                            for (int i = coordSpawnx - 1; i <= coordSpawnx + 1; i++)
                            {
                                for (int j = coordSpawny - 1; j <= coordSpawny + 1; j++)
                                {
                                    if (i > 0 && j > 0 && i < dungeonInserida.tamanhoDungeon.x - 1 && j < dungeonInserida.tamanhoDungeon.y - 1)
                                    {

                                        if (i == coordSpawnx && j == coordSpawny)
                                        {
                                            dungeonInserida.dungeon[i, j].subTipoSpawnBoss = "centro";
                                            dungeonInserida.dungeon[i, j].spawnBoss = true;
                                        }
                                        else
                                        {

                                        }

                                    }
                                }

                            }
                            dungeonInserida.dungeon[coordSpawnx, coordSpawny].spawnBoss = true;
                            loop = false;
                        }
                    }
                    else
                    {
                        loop = true;
                    }

                }


            }
        }

        
    }
}
