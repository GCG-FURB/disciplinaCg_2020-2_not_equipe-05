using System;
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL;

namespace gcgcg
{
    internal class Circulo : ObjetoGeometria
    {

        private Ponto4D[] pontos;
        private Ponto4D centro;
        public Circulo(string rotulo, Objeto paiRef, Ponto4D centro, Double raio) : base(rotulo, paiRef)
        {
            this.centro = centro;
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

        // protected override void DesenharObjeto()
        // {
        //     GL.PointSize(4);
        //     GL.Begin(PrimitiveType.Points);            
        //     GL.Color3(Convert.ToByte(255), Convert.ToByte(255), Convert.ToByte(0));
        //     foreach (var ponto in this.pontos)
        //     {
        //         GL.Vertex3(ponto.X, ponto.Y, 0);
        //     }
        //     GL.End();
        // }



    }
}
