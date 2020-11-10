/**
  Autor: Dalton Solano dos Reis
**/

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
    internal class Primitivas : ObjetoGeometria
    {

        private static int tipoPrimitivo = 0;
        private Cor[] cores;

        public Primitivas(string rotulo, Objeto paiRef, Ponto4D[] ptos, Cor[] cores) : base(rotulo, paiRef)
        {
            if (ptos.Length != cores.Length)
            {
                throw new System.Exception("Quantidade de pontos tem que ser = a quantidade de cores");
            }
            foreach (var ponto in ptos)
            {
                base.PontosAdicionar(ponto);
            }
            this.cores = cores;
        }

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
            GL.LineWidth(4);
            GL.PointSize(5);
            GL.Begin((PrimitiveType)tipoPrimitivo);
            int i = 0;
            foreach (Ponto4D pto in pontosLista)
            {
                GL.Color3(cores[i].CorR, cores[i].CorG, cores[i].CorB);
                GL.Vertex2(pto.X, pto.Y);
                i++;
            }
            GL.End();

        }
        //TODO: melhorar para exibir não só a lsita de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto SegReta: " + base.rotulo + "\n";
            for (var i = 0; i < pontosLista.Count; i++)
            {
                retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
            }
            return (retorno);
        }

    }
}