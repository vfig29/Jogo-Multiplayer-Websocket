using System;
using System.Collections.Generic;
using System.Text;

namespace Websocketsteste.COREDG
{
    public class Config
    {
        public static Vetor2 tamanhoMinSala = new Vetor2(6, 6), tamanhoMaxSala = new Vetor2(18, 18),
                tamanhoMaxDungeon = new Vetor2(80, 40), tamanhoMinDungeon = new Vetor2(25, 25), tamanhoMatrizZona = new Vetor2(15, 15),//tamanhoMatrizZona: numerosPares
                                                                                                                                       //Pontos das Dificuldades das Areas:
                pontoMf = new Vetor2(40, 50), pontoN = new Vetor2(50, 65), pontoMd = new Vetor2(65, 80),
                pontoD = new Vetor2(80, 100), pontoMuD = new Vetor2(90, 150), pontoI = new Vetor2(120, 200),
                //Pontos das Dificuldades das Salas:
                pontoSMf = new Vetor2(20, 30), pontoSN = new Vetor2(25, 40), pontoSMd = new Vetor2(35, 50),
                pontoSD = new Vetor2(45, 60), pontoSMuD = new Vetor2(65, 85), pontoSI = new Vetor2(80, 100);
        //120,70/ 20, 20
        public static int numeroMinPortas = 1, numeroMaxPortas = 2, numeroMinEntradas = 1, numeroMaxEntradas = 3,
                          numeroMinSalas = 10, numeroMaxSalas = 30, numeroMinAreas = 3, numeroMaxAreas = 5, //Numero exato de áreas, sem contar as secretas e a do boss.
                          faseAtual = 2, faseInicial = 1, faseFinal = 5,//minsala e maxsala são o numero de tentativas de posicionar uma sala.
                          tentativaSpawnItemMAX = 10;

        //Game Configuration

        public static int maxJogadoresSessao = 5, MaxSessoesSuportadas = 100, moveDelayReferenceInicial = 8;

        public static float moveSpeedInicial = 1.2f;


        void mudarFase(int proporcaoDaFase)
        {
            //Salas
            tamanhoMinSala.x = tamanhoMinSala.x * proporcaoDaFase;
            tamanhoMinSala.y = tamanhoMinSala.y * proporcaoDaFase;
            tamanhoMaxSala.x = tamanhoMaxSala.x * proporcaoDaFase;
            tamanhoMaxSala.y = tamanhoMaxSala.y * proporcaoDaFase;
            numeroMinSalas = numeroMinSalas * proporcaoDaFase;
            numeroMaxSalas = numeroMinSalas * proporcaoDaFase;
            //Dungeon
            tamanhoMinDungeon.x = tamanhoMinDungeon.x * proporcaoDaFase;
            tamanhoMinDungeon.y = tamanhoMinDungeon.y * proporcaoDaFase;
            tamanhoMaxDungeon.x = tamanhoMaxDungeon.x * proporcaoDaFase;
            tamanhoMaxDungeon.y = tamanhoMaxDungeon.y * proporcaoDaFase;
        }
    }
}
