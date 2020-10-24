using System;
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL;

namespace gcgcg
{
    internal class Circulo : ObjetoGeometria
    {

        //Necessário guardar o centro, pois cada vez que é desenhado é necessário calcular a posição
        //Com base nele
        private Ponto4D centro;

        public Circulo(string rotulo, Objeto paiRef, Ponto4D centro, Double raio) : base(rotulo, paiRef)
        {
            this.centro = centro;
            
            //Adiciona 72 pontos em 360° a cada 5° no raio especificado
            for (int i = 0; i < 360; i += 5)
            {
                base.PontosAdicionar(Matematica.GerarPtosCirculo(i, raio));
            }

        }

        protected override void DesenharObjeto()
        {
            //Tamanho dos pontos
            GL.PointSize(4);

            //Usa primitiva Points
            GL.Begin(PrimitiveType.Points);

            //Para cada ponto na lista de pontos
            foreach (Ponto4D pto in pontosLista)
            {
                //Desenha um ponto na posição relativa ao centro
                //Com isso se a posicao do ponto "centro" for alterada, ao desenhar os pontos suas posições também são alteradas
                GL.Vertex2(pto.X + centro.X, pto.Y +  centro.Y);
            }

            GL.End();
        }



    }
}
