using System;
using System.Collections.Generic;
using System.Text;

namespace Websocketsteste.COREDG
{
    public class Celula
    {
        // Start is called before the first frame update*****
        public string tipoCelula; // parede, chao, pSala, porta, caminho, entrada, contornoDungeon; (adicionado entradaSecreta)
        public string subTipoCelula; //esquerda, direita, cima, baixo, quinaEC, quinaEB, quinaDC, quinaDB,(quina de psalas ->) quinaECp, quinaEBp, quinaDCp, quinaDBp (<- quina de psalas),innerDB, innerEB, innerEC, innerDC, centro, individual,(para entrada) cimae, baixoe ,esquerdae, direitae (para entrada);
        public Celula adjdir, adjesq, adjcima, adjbaixo;
        public int idCelula = 0;
        public int idSalaPai = -1;
        public bool chave = false;
        public Vetor2 coordCelulaDun;
        public Vetor2 coordCelulaSala;
        public Dungeon dungeonPai;
        public int idCaminho = 100;
        public bool ehPorta = false;
        public bool ehComplementoPorta1 = false;
        public bool ehComplementoPorta2 = false;
        public bool ehEntrada = false, ehEntradaSecreta = false;
        public bool spawn = false, subSpawn = false;//subSpawn é a area de seguranca ao redor do player, para monstros não spawnarem muito perto dele, no inicio.
        public bool spawnBoss = false;//Possivel unir com subSpawns caso o portal para o boss ocupe mais de uma celula.
        public string subTipoSpawnBoss; //identificar os subtipos do altar do boss, caso tenha.
        public bool spawnsbBoss = false, spawnsbPlayer = false; //identificar a posicao inicial do player e do boss na sala do boss.
        public bool spawnMonstro = false;//identificar se já tem um monstro spawnando na celula;
        public bool spawnItem = false; //identificar se já tem um item spawnando na celula;

        public Celula(Vetor2 coordCelulaDun, string tipoCelula)
        {
            //Pegando a cordenada da celula em questão.
            this.coordCelulaDun = coordCelulaDun;
            //Pegando o tipo da celula em questão.
            this.tipoCelula = tipoCelula;
            idCelula = idCelula++;

        }
        public void pegarAdjacentes(Celula celula)
        {
            celula.adjesq = null;
            celula.adjdir = null;
            celula.adjcima = null;
            celula.adjbaixo = null;
            if (celula.coordCelulaDun.x - 1 >= 0) { celula.adjesq = celula.dungeonPai.dungeon[celula.coordCelulaDun.x - 1, celula.coordCelulaDun.y]; }
            if (celula.coordCelulaDun.x + 1 <= celula.dungeonPai.tamanhoDungeon.x - 1) { celula.adjdir = celula.dungeonPai.dungeon[celula.coordCelulaDun.x + 1, celula.coordCelulaDun.y]; }
            if (celula.coordCelulaDun.y - 1 >= 0) { celula.adjcima = celula.dungeonPai.dungeon[celula.coordCelulaDun.x, celula.coordCelulaDun.y - 1]; }
            if (celula.coordCelulaDun.y + 1 <= celula.dungeonPai.tamanhoDungeon.y - 1) { celula.adjbaixo = celula.dungeonPai.dungeon[celula.coordCelulaDun.x, celula.coordCelulaDun.y + 1]; }
        }

        public static int checarTipoAdjacentes(Celula celula, string tipoChecado, int area)
        {
            int contador = 0;

            for (int i = 1; i <= area; i++)
            {
                if (celula.coordCelulaDun.x - i >= 0)
                {
                    if (celula.dungeonPai.dungeon[celula.coordCelulaDun.x - i, celula.coordCelulaDun.y].tipoCelula == tipoChecado) { contador++; }
                }
                if (celula.coordCelulaDun.x + i < celula.dungeonPai.tamanhoDungeon.x)
                {
                    if (celula.dungeonPai.dungeon[celula.coordCelulaDun.x + i, celula.coordCelulaDun.y].tipoCelula == tipoChecado) { contador++; }
                }
                if (celula.coordCelulaDun.y + i < celula.dungeonPai.tamanhoDungeon.y)
                {
                    if (celula.dungeonPai.dungeon[celula.coordCelulaDun.x, celula.coordCelulaDun.y + i].tipoCelula == tipoChecado) { contador++; }
                }
                if (celula.coordCelulaDun.y - i >= 0)
                {
                    if (celula.dungeonPai.dungeon[celula.coordCelulaDun.x, celula.coordCelulaDun.y - i].tipoCelula == tipoChecado) { contador++; }
                }


            }

            return contador;

        }

        public static int ChecarTipoAreaAdjacente(Celula celula, string tipoChecado, int area)
        {
            int contador = 0;

            for (int i = celula.coordCelulaDun.x - area; i <= celula.coordCelulaDun.x + area; i++)
            {
                for (int j = celula.coordCelulaDun.y - area; j <= celula.coordCelulaDun.y + area; j++)
                {
                    if (i >= 0 && j >= 0 && i < celula.dungeonPai.tamanhoDungeon.x && j < celula.dungeonPai.tamanhoDungeon.y)
                    {
                        if (celula.dungeonPai.dungeon[i, j].tipoCelula == tipoChecado)
                        {
                            contador++;
                        }
                    }

                }
            }

            return contador;

        }
    }
    }







