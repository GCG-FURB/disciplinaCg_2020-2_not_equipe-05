using System;
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL;

namespace gcgcg
{
    internal class Circulo : ObjetoGeometria
    {

        private Ponto4D centro;
        private Ponto4D centroInicial;

        private Ponto4D[] pontosIniciais = new Ponto4D[72];


        public Circulo(string rotulo, Objeto paiRef, Ponto4D centro, Double raio) : base(rotulo, paiRef)
        {
            this.centro = centro;
            this.centroInicial = new Ponto4D(centro.X, centro.Y, 0, 1);
            int j = 0;
            for (int i = 0; i < 360; i += 5, j++)
            {
                Ponto4D p = Matematica.GerarPtosCirculo(i, raio);
                pontosIniciais[j] = new Ponto4D(p.X, p.Y, 0, 1);
                p.X += centro.X;
                p.Y += centro.Y;
                base.PontosAdicionar(p);
            }

        }

        protected override void DesenharObjeto()
        {
            GL.PointSize(4);
            GL.Begin(PrimitiveType.LineLoop);
            int i = 0;
            foreach (Ponto4D pto in pontosLista)
            {
                if (centro.X != centroInicial.X)
                {
                    pto.X = pontosIniciais[i].X + centro.X;
                }
                if (centro.Y != centroInicial.Y)
                {
                    pto.Y = pontosIniciais[i].Y + centro.Y;

                }

                GL.Vertex2(pto.X, pto.Y);
                if (i == 0)
                    base.BBox.Atribuir(pto);
                else
                    base.BBox.Atualizar(pto);
                base.BBox.ProcessarCentro();

                i++;
                // GL.Vertex2(pto.X + centro.X, pto.Y +  centro.Y);
            }

            centroInicial.X = centro.X;
            centroInicial.Y = centro.Y;
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
