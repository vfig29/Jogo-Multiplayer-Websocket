using System;
using System.Collections.Generic;
using System.Text;

namespace Websocketsteste.COREDG
{
    public class Area
    {
        public string tema;
        public int dificuldade = -1; //-1 - sem dificuldade, 0 - meio facil, 1 - normal, 2 - meio dificil, 3 - dificil, 4 - muito dificil, 5 - injusto.
        public Dungeon dungeonInserida;
        public Vetor2[] entradas = { null, null, null, null }; //0 - esquerda, 1 - direita, 2 - cima, 3 - baixo.
        public Vetor2[] entradasSecretas = { null, null, null, null }; //0 - esquerda, 1 - direita, 2 - cima, 3 - baixo.
        public Vetor2 posicaoMatriz;
        public bool inicial = false, final = false, secreta = false, descoberta = false;
        public bool boss = false;

        public Area(Vetor2 posicaoMatriz)
        {
            this.posicaoMatriz = posicaoMatriz;
            tema = "semtema";
            dungeonInserida = null;

            //Toda área possui uma dificuldade, logo será randomizado uma dificuldade:
            System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
            int resultRand = rnd.Next(1, 411);
            if (resultRand >= 1 && resultRand <= 20)
            {
                this.dificuldade = 0;
            }
            if (resultRand >= 21 && resultRand <= 250)
            {
                this.dificuldade = 1;
            }
            if (resultRand >= 251 && resultRand <= 380)
            {
                this.dificuldade = 2;
            }
            if (resultRand >= 381 && resultRand <= 400)
            {
                this.dificuldade = 3;
            }
            if (resultRand >= 401 && resultRand <= 409)
            {
                this.dificuldade = 4;
            }
            if (resultRand >= 410)
            {
                this.dificuldade = 5;
            }

        }
        public static Celula RandomizarCelulaArea(string tipoProcurado, Area areaAlvo)
        {
            List<Celula> celulasAchadas = new List<Celula>();
            foreach (Celula c in areaAlvo.dungeonInserida.dungeon)
            {
                if (c != null)
                {
                    if (c.tipoCelula == tipoProcurado)
                    {
                        celulasAchadas.Add(c);
                    }
                }

            }

            Random rnd = new Random();
            int indice = rnd.Next(0, celulasAchadas.Count);

            return celulasAchadas[indice];


        }
    }
}
