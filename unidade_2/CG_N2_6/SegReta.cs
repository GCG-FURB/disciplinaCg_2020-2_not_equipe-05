/**
  Autor: Dalton Solano dos Reis
**/

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;
using System;

namespace gcgcg
{
    internal class SegReta : ObjetoGeometria
    {
        //ter como refetencia o ponto selecionado
        private Ponto4D atual;
        
        //os quatro pontos de controle
        private Ponto4D ptoInfEsq;
        private Ponto4D ptoSupEsq;
        private Ponto4D ptoSupDir;
        private Ponto4D ptoInfDir;


        //variaveis para bkp dos quatro pontos de control, para poder restaurar a origem
        private Ponto4D bkpptoInfEsq;
        private Ponto4D bkpptoSupEsq;
        private Ponto4D bkpptoSupDir;
        private Ponto4D bkpptoInfDir;
        public SegReta(string rotulo, Objeto paiRef, Ponto4D ptoInfEsqP, Ponto4D ptoSupDirP, Ponto4D ptoSupEsqP, Ponto4D ptoInfDirP) : base(rotulo, paiRef)
        {
            // setta como default o ponto selecionado o ponto um, que é o inferior direito
            this.atual = ptoInfDirP;
            this.ptoInfDir = ptoInfDirP;
            this.ptoSupDir = ptoSupDirP;
            this.ptoSupEsq = ptoSupEsqP;
            this.ptoInfEsq = ptoInfEsqP;
            //criar bkps dos pontos de origem para depois poder restaurar no reset
            this.bkpptoInfDir = new Ponto4D(ptoInfDirP.X, ptoInfDirP.Y, ptoInfDirP.Z);
            this.bkpptoSupDir = new Ponto4D(ptoSupDir.X, ptoSupDir.Y, ptoSupDir.Z);
            this.bkpptoSupEsq = new Ponto4D(ptoSupEsq.X, ptoSupEsq.Y, ptoSupEsq.Z);
            this.bkpptoInfEsq = new Ponto4D(ptoInfEsq.X, ptoInfEsq.Y, ptoInfEsq.Z);
            base.PontosAdicionar(this.ptoInfDir);
            base.PontosAdicionar(this.ptoSupDir);
            base.PontosAdicionar(this.ptoSupEsq);
            base.PontosAdicionar(this.ptoInfEsq);
        }

        protected override void DesenharObjeto()
        {
            //chama o desenhar pontos primeiro para que os pontos fiquem embaixo das retas
            desenharPontos();
            GL.LineWidth(2);
            GL.Color3(Convert.ToByte(0), Convert.ToByte(255), Convert.ToByte(255));
            //escolhido LineStrip, para que não feche o quadrado, deixando o lado de baixo em aberto;
            GL.Begin(PrimitiveType.LineStrip);
            foreach (Ponto4D pto in pontosLista)
            {
                GL.Vertex2(pto.X, pto.Y);
            }
            GL.End();

        }

        //método para desenhar os pontos de controle, para ficar mais em destaque
        private void desenharPontos()
        {
            GL.PointSize(8);
            GL.Begin(PrimitiveType.Points);
            foreach (Ponto4D pto in pontosLista)
            {
                //aqui checa se é o ponto selecionado, se for ele pinta de vermelho o ponto
                if (pto.Equals(this.atual))
                {
                    //setta a cor do ponto para vermelho, tem que ser cada parametro em byte se não, não funciona
                    GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
                    GL.Vertex2(pto.X, pto.Y);
                }
                else
                {
                    GL.Color3(Convert.ToByte(0), Convert.ToByte(0), Convert.ToByte(0));
                    GL.Vertex2(pto.X, pto.Y);
                }
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

        //método para resetar para os pontos de origem
        public void resetarPontos()
        {
            //aqui é settado cada parametro para não misturar as referencias de objetos
            this.ptoInfDir.X = this.bkpptoInfDir.X; this.ptoInfDir.Y = this.bkpptoInfDir.Y;
            this.ptoInfEsq.X = this.bkpptoInfEsq.X; this.ptoInfEsq.Y = this.bkpptoInfEsq.Y;
            this.ptoSupDir.X = this.bkpptoSupDir.X; this.ptoSupDir.Y = this.bkpptoSupDir.Y;
            this.ptoSupEsq.X = this.bkpptoSupEsq.X; this.ptoSupEsq.Y = this.bkpptoSupEsq.Y;
            //aqui altera os pontos que estão na base, na ordem
            base.PontosAlterar(ptoInfDir, 0);
            base.PontosAlterar(ptoSupDir, 1);
            base.PontosAlterar(ptoSupEsq, 2);
            base.PontosAlterar(ptoInfEsq, 3);
        }


        // método que muda o ponto selecionado (o que fica em vermelho)
        public void mudarAtual(int i)
        {
            //se for o ponto inferior direito
            if (i == 1)
            {
                this.atual = ptoInfDir;
            }
            //se for o ponto superior direito
            else if (i == 2)
            {
                this.atual = ptoSupDir;
            }
            //se for o ponto superior esquerdo
            else if (i == 3)
            {
                this.atual = ptoSupEsq;
            }
            //se for o ponto inferior esquerdo
            else if (i == 4)
            {
                this.atual = this.ptoInfEsq;
            }

        }

        //diminui em uma unidade a cordenada X, para o objeto ir para a esquerda
        public void esquerda()
        {
            this.atual.X--;
        }

        //aumenta em uma unidade a cordenada X, para o objeto ir para a direita
        public void direita()
        {
            this.atual.X++;
        }

        //aumenta em uma unidade a cordenada Y, para o objeto ir para cima
        public void cima()
        {
            this.atual.Y++;
        }

        //diminui em uma unidade a cordenada Y, para o objeto ir para baixo
        public void baixo()
        {
            this.atual.Y--;
        }
    }
}