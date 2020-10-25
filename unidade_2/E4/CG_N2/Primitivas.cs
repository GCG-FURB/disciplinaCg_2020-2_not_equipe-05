/**
  Autor: Dalton Solano dos Reis
**/

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
    internal class Primitivas : ObjetoGeometria
    {

        //Variavel que vai de 0 a 9 representando o tipo primitivo
        private static int tipoPrimitivo = 0;

        //Array de cores
        private Cor[] cores;

        //Classe Primitivas, que recebe um array de pontos e cores preferidas
        public Primitivas(string rotulo, Objeto paiRef, Ponto4D[] ptos, Cor[] cores) : base(rotulo, paiRef)
        {
            if (ptos.Length != cores.Length)
            {
                throw new System.Exception("Quantidade de pontos tem que ser igual à quantidade de cores");
            }

            //Adiciona os pontos na lista
            foreach (var ponto in ptos)
            {
                base.PontosAdicionar(ponto);
            }

            this.cores = cores;
        }

        //Método estático que circula pelas primitivas
        public static void mudarPrimitiva()
        {
            if (tipoPrimitivo == 9)
            {
                tipoPrimitivo = 0;
            }
            else
            {
                tipoPrimitivo++;
            }
        }

        protected override void DesenharObjeto()
        {
            //Tamanho da linha
            GL.LineWidth(4);

            //Tamanho dos pontos
            GL.PointSize(5);

            //GL.Begin com o tipo primitivo atual
            GL.Begin((PrimitiveType)tipoPrimitivo);

            //Variável para iterar pelas cores
            int i = 0;

            //Para cada ponto na lista
            foreach (Ponto4D pto in pontosLista)
            {

                //Define a cor do ponto com base na posição do array
                GL.Color3(cores[i].CorR, cores[i].CorG, cores[i].CorB);

                //Desenha o ponto/vértice
                GL.Vertex2(pto.X, pto.Y);

                //Incrementa a variável do índice da cor
                i++;
            }


            GL.End();

        }
    }
}