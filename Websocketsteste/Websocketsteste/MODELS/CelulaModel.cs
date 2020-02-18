using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Websocketsteste.COREDG;

namespace Websocketsteste.MODELS
{
    public class CelulaModel
    {

        public CelulaModel(int coordx_, int coordy_, string tipoCel_, int tamanhoDunX_, int tamanhoDunY_, bool secreta_)
        {
            tipoMensagem = "dungeon";
            id = Guid.NewGuid();
            this.coordx = coordx_;
            this.coordy = coordy_;
            this.tipoCel = tipoCel_;
            this.tamanhoDunX = tamanhoDunX_;
            this.tamanhoDunY = tamanhoDunY_;
            secreta = secreta_;

        }

        public static List<CelulaModel> ConverterParaCelulaModel(Dungeon dungeon)
        {
            List<CelulaModel> listaCelulas = new List<CelulaModel>();

            foreach (Celula celula in dungeon.dungeon)
            {
                if (celula != null)
                {
                    listaCelulas.Add(
                        new CelulaModel(celula.coordCelulaDun.x, celula.coordCelulaDun.y, celula.tipoCelula, dungeon.tamanhoDungeon.x, dungeon.tamanhoDungeon.y, celula.ehEntradaSecreta)

                        );

                }

            }



            return listaCelulas;
        }
        public string tipoMensagem { get; }
        public Guid id { get; }
        public int coordx { get; set; }
        public int coordy { get; set; }
        public string tipoCel { get; set; }

        public int tamanhoDunX { get; set; }
        public int tamanhoDunY { get; set; }

        public bool secreta { get; set; }

    }
}
