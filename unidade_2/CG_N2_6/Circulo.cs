/**
  Autor: Dalton Solano dos Reis
**/

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
    internal class Circulo : ObjetoGeometria
    {
        private Ponto4D[] pontos;
        private Ponto4D centro;
        public Circulo(string rotulo, Objeto paiRef, Ponto4D ptoCentral, double raio) : base(rotulo, paiRef)
        {
            this.centro = ptoCentral;
            for (int i = 0; i < 360; i += 5)
            {
                base.PontosAdicionar(Matematica.GerarPtosCirculo(i, raio));
            }
        }

        protected override void DesenharObjeto()
        {
            GL.PointSize(4);
            GL.Begin(PrimitiveType.Points);
            foreach (Ponto4D pto in pontosLista)
            {
                GL.Vertex2(pto.X + centro.X, pto.Y +  centro.Y);
            }
            GL.End();
        }
        //TODO: melhorar para exibir não só a lista de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Retangulo: " + base.rotulo + "\n";
            for (var i = 0; i < pontosLista.Count; i++)
            {
                retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
            }
            return (retorno);
        }

    }
}