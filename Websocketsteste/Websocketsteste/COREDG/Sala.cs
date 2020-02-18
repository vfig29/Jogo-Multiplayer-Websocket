using System;
using System.Collections.Generic;
using System.Text;

namespace Websocketsteste.COREDG
{
    public class Sala
    {
        // Start is called before the first frame update

        //Matriz Sala, com o tamanho
        public Vetor2 tamanhoSala;
        public Celula[,] sala = new Celula[Config.tamanhoMaxSala.x, Config.tamanhoMaxSala.y];
        //Caracteristicas da sala.
        public Vetor2[] coordPortas = new Vetor2[Config.numeroMaxPortas];
        public int numeroPortas;
        public int idSala = 0; // Valor inicial.
        public int dificuldade = -1;
        public Vetor2 coordAncoraDungeon;
        public bool chave = false;
        public bool trancado = false;



        public Sala()
        {
            //Instanciando a Randomizacao
            System.Random tamanho = new System.Random(Guid.NewGuid().GetHashCode());
            // Gerando valores aleatorios de coordenada x e y, para instanciar o vector.
            int x = tamanho.Next(Config.tamanhoMinSala.x, Config.tamanhoMaxSala.x);
            int y = tamanho.Next(Config.tamanhoMinSala.y, Config.tamanhoMaxSala.y);
            Vetor2 tamanhoSala = new Vetor2(x, y);
            Celula[,] sala = new Celula[tamanhoSala.x, tamanhoSala.y];
            this.tamanhoSala = tamanhoSala;
            int numeroPortas = tamanho.Next(Config.numeroMinPortas, Config.numeroMaxPortas);
            this.numeroPortas = numeroPortas;
            //Preenchendo a Sala;
            for (int i = 0; i < tamanhoSala.y; i++)
            {
                for (int j = 0; j < tamanhoSala.x; j++)
                {
                    if (j == 0 || i == 0 || j == (tamanhoSala.x - 1) || i == (tamanhoSala.y - 1))
                    {
                        Vetor2 coordPreenchida = new Vetor2(j, i);
                        Celula celulaPreencheu = new Celula(coordPreenchida, "pSala");
                        //adicionando subtipo*
                        if (j == 0 && i == 0)
                        {
                            celulaPreencheu.subTipoCelula = "quinaECp";

                        }
                        if (j == 0 && i == (tamanhoSala.y - 1))
                        {

                            celulaPreencheu.subTipoCelula = "quinaEBp";

                        }
                        if (j == 0 && i != (tamanhoSala.y - 1) && i != 0)
                        {
                            celulaPreencheu.subTipoCelula = "esquerdap";

                        }
                        if (j == (tamanhoSala.x - 1) && i == 0)
                        {
                            celulaPreencheu.subTipoCelula = "quinaDCp";

                        }
                        if (j == (tamanhoSala.x - 1) && i == (tamanhoSala.y - 1))
                        {

                            celulaPreencheu.subTipoCelula = "quinaDBp";

                        }
                        if (j == (tamanhoSala.x - 1) && i != (tamanhoSala.y - 1) && i != 0)
                        {
                            celulaPreencheu.subTipoCelula = "direitap";

                        }
                        if (i == 0 && j != 0 && j != (tamanhoSala.x - 1))
                        {
                            celulaPreencheu.subTipoCelula = "cimap";

                        }
                        if (i == tamanhoSala.y - 1 && j != 0 && j != (tamanhoSala.x - 1))
                        {

                            celulaPreencheu.subTipoCelula = "baixop";

                        }

                        celulaPreencheu.coordCelulaSala = coordPreenchida;
                        sala[j, i] = celulaPreencheu;
                    }
                    else
                    {
                        Vetor2 coordPreenchida = new Vetor2(j, i);
                        Celula celulaPreencheu = new Celula(coordPreenchida, "chao");
                        celulaPreencheu.coordCelulaSala = coordPreenchida;
                        sala[j, i] = celulaPreencheu;
                    }
                }

            }
            //Adicionando Portas.
            for (int i = 0; i < numeroPortas; i++)
            {
                int xPorta = tamanho.Next(2, tamanhoSala.x - 3);
                int yPorta = tamanho.Next(2, tamanhoSala.y - 3);
                int ladoPorta = tamanho.Next(1, 5);
                Vetor2 coordPreenchida2 = new Vetor2(0, 0);
                string subtipo = "";
                switch (ladoPorta)
                {
                    case 1:
                        coordPreenchida2 = new Vetor2(0, yPorta);
                        subtipo = "esquerdap";
                        break;
                    case 2:
                        coordPreenchida2 = new Vetor2(tamanhoSala.x - 1, yPorta);
                        subtipo = "direitap";
                        break;
                    case 3:
                        coordPreenchida2 = new Vetor2(xPorta, 0);
                        subtipo = "cimap";
                        break;
                    case 4:
                        coordPreenchida2 = new Vetor2(xPorta, tamanhoSala.y - 1);
                        subtipo = "baixop";
                        break;
                }

                Celula celulaPreencheu = new Celula(coordPreenchida2, "porta");
                celulaPreencheu.ehPorta = true;
                celulaPreencheu.subTipoCelula = subtipo;
                celulaPreencheu.coordCelulaSala = coordPreenchida2;
                sala[coordPreenchida2.x, coordPreenchida2.y] = celulaPreencheu;
            }



            this.sala = sala;
        }

        public static void AdicionarIdSalas(Sala salaAlvo)
        {
            for (int i = 0; i < salaAlvo.tamanhoSala.y; i++)
            {
                for (int j = 0; j < salaAlvo.tamanhoSala.x; j++)
                {
                    if (salaAlvo.sala[j, i] != null)
                    {
                        salaAlvo.sala[j, i].idSalaPai = salaAlvo.idSala;
                        salaAlvo.sala[j, i].chave = salaAlvo.chave;
                    }
                }
            }

        }

        public static int AdicionarDificuldade()
        {
            System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
            int resultRand = rnd.Next(1, 411);
            int dificuldade = -1;
            if (resultRand >= 1 && resultRand <= 20)
            {
                dificuldade = 0;
            }
            if (resultRand >= 21 && resultRand <= 250)
            {
                dificuldade = 1;
            }
            if (resultRand >= 251 && resultRand <= 380)
            {
                dificuldade = 2;
            }
            if (resultRand >= 381 && resultRand <= 400)
            {
                dificuldade = 3;
            }
            if (resultRand >= 401 && resultRand <= 409)
            {
                dificuldade = 4;
            }
            if (resultRand >= 410)
            {
                dificuldade = 5;
            }

            return dificuldade;

        }

    }
}
