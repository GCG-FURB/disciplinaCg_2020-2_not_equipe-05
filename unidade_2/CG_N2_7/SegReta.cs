

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;
using System;

namespace gcgcg
{
    internal class SegReta : ObjetoGeometria
    {
        private Ponto4D ponto1;

        private double raio = 100;
        private double angulo = 45;
        private Ponto4D ponto2;
        private Ponto4D selecionado;

        private Ponto4D[] pontosComPosicaoInicial; // Lembra a posicao inicial dos pontos
        private Ponto4D[] pontosComPosicaoAtual; // Lembra os pontos em si para poder informar para a Spline
        private int indiceSelecionado;

        public SegReta(string rotulo, Objeto paiRef, Ponto4D[] pontos) : base(rotulo, paiRef)
        {
            ponto1 = pontos[0];
            ponto2 = pontos[1];
            foreach (Ponto4D pto in pontos)
            {
                base.PontosAdicionar(pto);
            }
            selecionado = pontos[pontos.Length - 1];
            indiceSelecionado = pontos.Length - 1;

            pontosComPosicaoInicial = new Ponto4D[pontos.Length];
            pontosComPosicaoAtual = new Ponto4D[pontos.Length];

            int j = 0;
            foreach (Ponto4D ponto in pontos)
            {
              pontosComPosicaoInicial[j] = new Ponto4D(ponto.X,ponto.Y,ponto.Z, 1);
              pontosComPosicaoAtual[j] = pontos[j];
              j++;
            }
        }



        public void diminuir()
        {
            Ponto4D temp1 = Matematica.GerarPtosCirculo(angulo, raio);
            raio -= 2;
            Ponto4D temp2 = Matematica.GerarPtosCirculo(angulo, raio);
            double diferencaX = temp1.X - temp2.X;
            double diferencaY = temp1.Y - temp2.Y;
            ponto2.X -= diferencaX;
            ponto2.Y -= diferencaY;

            base.PontosAlterar(ponto2, 1);
        }

        public void aumentar()
        {
            Ponto4D temp1 = Matematica.GerarPtosCirculo(angulo, raio);
            raio += 2;
            Ponto4D temp2 = Matematica.GerarPtosCirculo(angulo, raio);
            double diferencaX = temp1.X - temp2.X;
            double diferencaY = temp1.Y - temp2.Y;
            ponto2.X -= diferencaX;
            ponto2.Y -= diferencaY;

            base.PontosAlterar(ponto2, 1);
        }

        public void esquerda()
        {
            ponto1.X -= 2;
            base.PontosAlterar(ponto1, 0);

            ponto2.X -= 2;
            base.PontosAlterar(ponto2, 1);

        }
        public void direita()
        {
            ponto1.X += 2;
            base.PontosAlterar(ponto1, 0);

            ponto2.X += 2;
            base.PontosAlterar(ponto2, 1);

        }

        public void girarHorario()
        {
            Ponto4D temp1 = Matematica.GerarPtosCirculo(angulo, raio);
            if (angulo - 2 < 0)
            {
                angulo = 360;
            }
            else
            {
                angulo -= 2;
            }
            Ponto4D temp2 = Matematica.GerarPtosCirculo(angulo, raio);
            double diferencaX = temp1.X - temp2.X;
            double diferencaY = temp1.Y - temp2.Y;
            ponto2.X -= diferencaX;
            ponto2.Y -= diferencaY;
            base.PontosAlterar(ponto2, 1);

        }

        public void girarAntiHorario()
        {
            Ponto4D temp1 = Matematica.GerarPtosCirculo(angulo, raio);
            if (angulo + 2 > 360)
            {
                angulo = 0;
            }
            else
            {
                angulo += 2;
            }
            Ponto4D temp2 = Matematica.GerarPtosCirculo(angulo, raio);
            double diferencaX = temp1.X - temp2.X;
            double diferencaY = temp1.Y - temp2.Y;
            ponto2.X -= diferencaX;
            ponto2.Y -= diferencaY;
            base.PontosAlterar(ponto2, 1);
        }




        protected override void DesenharObjeto()
        {
            GL.LineWidth(2);
            GL.Begin(PrimitiveType.LineStrip);

            foreach (Ponto4D pto in pontosLista)
            {
                GL.Vertex2(pto.X, pto.Y);
            }
            GL.End();
            desenharPontos();
        }


        private void desenharPontos()
        {
            GL.PointSize(8);
            GL.Begin(PrimitiveType.Points);
            GL.Color3(Convert.ToByte(0), Convert.ToByte(0), Convert.ToByte(0));
            foreach (Ponto4D pto in pontosLista)
            {
                if (pto.X == selecionado.X && pto.Y == selecionado.Y)
                {
                    GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
                }
                else
                {
                    GL.Color3(Convert.ToByte(0), Convert.ToByte(0), Convert.ToByte(0));
                }
                GL.Vertex2(pto.X, pto.Y);
            }
            GL.End();
        }

        public void sEsquerda()
        {
            selecionado.X -= 2;
            base.PontosAlterar(selecionado, indiceSelecionado);
        }
        public void sDireita()
        {
            selecionado.X += 2;
            base.PontosAlterar(selecionado, indiceSelecionado);
        }

        public void sCima()
        {
            selecionado.Y += 2;
            base.PontosAlterar(selecionado, indiceSelecionado);
        }

        public void sBaixo()
        {
            selecionado.Y -= 2;
            base.PontosAlterar(selecionado, indiceSelecionado);
        }

        public void seleciona(int numero)
        {
            selecionado = pontosLista[numero - 1];
            indiceSelecionado = numero - 1;
        }

        public void restart()
        {
            for (int i = 0; i < pontosComPosicaoInicial.Length; i++)
            {
                pontosComPosicaoAtual[i].X = pontosComPosicaoInicial[i].X;
                pontosComPosicaoAtual[i].Y = pontosComPosicaoInicial[i].Y;
                pontosComPosicaoAtual[i].Z = pontosComPosicaoInicial[i].Z;
                base.PontosAlterar(pontosComPosicaoAtual[i], i);
            }
            selecionado = pontosLista[indiceSelecionado];
        }

        public Ponto4D[] GetPontos(){
            return this.pontosComPosicaoAtual;
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