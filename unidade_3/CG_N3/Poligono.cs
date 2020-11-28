/**
  Autor: Dalton Solano dos Reis
**/

using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using CG_Biblioteca;

// ATENÇÃO: remover: "Privado_"
namespace gcgcg
{
    internal class Poligono : ObjetoGeometria
    {



        public Poligono(char rotulo, Objeto paiRef) : base(rotulo, paiRef)
        {
        }

        //Transformar em aberto ou fechado
        public void mudaPrimitiva()
        {
            if (base.PrimitivaTipo == (PrimitiveType)2)
            {
                base.PrimitivaTipo = (PrimitiveType)3;
            }
            else
            {
                base.PrimitivaTipo = (PrimitiveType)2;
            }

        }

        protected override void DesenharObjeto()
        {

            GL.Begin(base.PrimitivaTipo);
            foreach (Ponto4D pto in pontosLista)
            {
                GL.Vertex2(pto.X, pto.Y);
            }
            GL.End();
        }

        //Retorna a lista de pontos (para poder remover/comparar o mais próx)
        public List<Ponto4D> getPontos()
        {
            return this.pontosLista;
        }

        //Remove o ponto no indice passado
        public void removerPonto(int indice)
        {   
            //Se o indice passado tiver fora da lista, não faz nada
            if (indice > pontosLista.Count)
            { }
            else
            {
                //Copia a lista, omitindo o ponto localizado no indice desejado
                List<Ponto4D> copia = new List<Ponto4D>();
                int i = 0;
                foreach (Ponto4D pto in pontosLista)
                {
                    if (i == indice)
                    {

                    }
                    else
                    {
                        copia.Add(pto);
                    }
                    i++;
                }
                //Substitui a lista atual pela nova sem o ponto desejado
                pontosLista = copia;
                //Atualiza a bbox
                atualizaBBox();
            }


        }

        //Atualiza a bbox
        public void atualizaBBox()
        {
            int i = 0;
            foreach (Ponto4D pto in pontosLista)
            {
                //Se for o primeiro ponto, atribui
                if (i == 0)
                {
                    base.BBox.Atribuir(pto);
                }
                //Senão, só atualiza
                else
                {
                    base.BBox.Atualizar(pto);
                }
                //Processar centro tbm
                base.BBox.ProcessarCentro();
                i++;
            }
        }


        //TODO: melhorar para exibir não só a lista de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Poligono: " + base.rotulo + "\n";
            for (var i = 0; i < pontosLista.Count; i++)
            {
                retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
            }
            return (retorno);
        }

    }
}