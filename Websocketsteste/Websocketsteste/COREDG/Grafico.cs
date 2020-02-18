using System;
using System.Collections.Generic;
using System.Text;

namespace Websocketsteste.COREDG
{
    class Grafico
    {
        public static int idGrafico;
        public Zona zonaCriada;
        public static bool permitSpawn = false;


        public Grafico()
        {

        }

        public static Dungeon gerarDungeon(Area areaPai)
        {
            //Inicializando Dungeon
            Dungeon dungeonInstanciada = new Dungeon(areaPai);
            while (dungeonInstanciada.errorFiltro || dungeonInstanciada.errorCaminho)
            {

                dungeonInstanciada = new Dungeon(areaPai);
              
                dungeonInstanciada.inserirDungeonPai(dungeonInstanciada);
              
                dungeonInstanciada.superSalas(dungeonInstanciada);
             
                dungeonInstanciada.InserirCaminhos(dungeonInstanciada); //Os valores do debugador anti-crash, são fixos. Em caso de mudar a amplitude da dungeon, é necessario observar esses valores.
              
                dungeonInstanciada.DebugPosCaminho(dungeonInstanciada);
                
                Dungeon.ReestabelecerCoords(dungeonInstanciada); // Para recuperar as coordenadas perdidas.
                
                dungeonInstanciada.errorFiltro = dungeonInstanciada.FiltroCaminho(dungeonInstanciada);
               
                dungeonInstanciada.InserirSubtipos(dungeonInstanciada);
                dungeonInstanciada.IgualarSuperSala(dungeonInstanciada);
                
                //Fim da inicialização.
            }
            //fora do while: acabamentos finais, que dependem de uma dungeon perfeita.
            //se um dia o spawn der erro de loop, por não caber em um corredor ou em outra regiao, é só adicionar um segundo while com o primeiro while e as funcoes de spawn dentro, e criar um bool da mesma forma que foi feito com o errorcaminho e filtro.
            dungeonInstanciada.AdicionarPlayerSpawn(areaPai, dungeonInstanciada);
            dungeonInstanciada.AdicionarBossSpawn(areaPai, dungeonInstanciada);
            //Console.WriteLine("dungeonCriada");
            return dungeonInstanciada;
        }

    }
}
