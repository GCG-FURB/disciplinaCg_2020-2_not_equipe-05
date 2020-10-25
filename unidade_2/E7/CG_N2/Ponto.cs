using System;
using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;


namespace gcgcg
{
    //Classe básica para representar o ponto no meio do círculo
    internal class Ponto : ObjetoGeometria
    {
        Ponto4D pto;
        int tamanho;
        public Ponto(string rotulo, Objeto paiRef, Ponto4D pto, int tamanho) : base(rotulo, paiRef)
        {
            base.PontosAdicionar(pto);
            this.pto = pto;
            this.tamanho = tamanho;
        }

        protected override void DesenharObjeto()
        {
            GL.PointSize(tamanho);
            GL.Begin(PrimitiveType.Points);
            GL.Vertex2(pto.X, pto.Y);

            GL.End();
        }
        

    }
}


