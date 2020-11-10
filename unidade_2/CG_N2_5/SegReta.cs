/**
  Autor: Dalton Solano dos Reis
**/

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
    internal class SegReta : ObjetoGeometria
    {

        //Necessário manter a posição do ponto de dentro
        private Ponto4D ponto1;

        //Necessário manter o raio atual
        private double raio = 100;
        //Necessário manter o angulo atual
        private double angulo = 45;

        //Necessário manter a posição do ponto de fora
        private Ponto4D ponto2;

        //Para o sr palito funcionar, é necessário passar dois pontos em conformidade com o angulo de 45° e raio 100    
        public SegReta(string rotulo, Objeto paiRef, Ponto4D pto1, Ponto4D pto2) : base(rotulo, paiRef)
        {
            ponto1 = pto1;
            ponto2 = pto2;
            base.PontosAdicionar(ponto1);
            base.PontosAdicionar(ponto2);
        }



        //Método de diminuir
        public void diminuir()
        {
            //Calcula a formula do ponto baseado no centro 0,0 com o raio e angulo atual
            Ponto4D temp1 = Matematica.GerarPtosCirculo(angulo, raio);

            //Diminui o raio atual
            raio -= 2;

            //Calcula a formula do ponto baseado no centro 0,0 após a mudança do raio
            Ponto4D temp2 = Matematica.GerarPtosCirculo(angulo, raio);

            //É medido a diferença entre os dois pontos (qual mudança os pontos reais irão sofrer)
            double diferencaX = temp1.X - temp2.X;
            double diferencaY = temp1.Y - temp2.Y;

            //Aplica a diferença levando em conta a posição atual dos pontos do Sr. Palito
            ponto2.X -= diferencaX;
            ponto2.Y -= diferencaY;

            base.PontosAlterar(ponto2, 1);
        }

        //Método de aumentar
        public void aumentar()
        {
            //Mesmo esquema que o método diminuir porém o raio é aumentado
            Ponto4D temp1 = Matematica.GerarPtosCirculo(angulo, raio);
            raio += 2;
            Ponto4D temp2 = Matematica.GerarPtosCirculo(angulo, raio);
            double diferencaX = temp1.X - temp2.X;
            double diferencaY = temp1.Y - temp2.Y;
            ponto2.X -= diferencaX;
            ponto2.Y -= diferencaY;

            base.PontosAlterar(ponto2, 1);
        }

        //Método para deslocar-se a direita
        public void esquerda()
        {

            //Diminui duas unidades da posição atual do primeiro ponto
            ponto1.X -= 2;

            //Altera o ponto presente na lista (substitui pelo ponto1, o ponto no indice 0)
            base.PontosAlterar(ponto1, 0);

            //Diminui duas unidades da posição atual do segundo ponto
            ponto2.X -= 2;

            //Altera o ponto presente na lista (substitui pelo ponto2, o ponto no indice 1)
            base.PontosAlterar(ponto2, 1);

        }

        //Método de deslocação a direita
        public void direita()
        {
            //Mesmo esquema que o método de deslocamento a esquerda
            ponto1.X += 2;
            base.PontosAlterar(ponto1, 0);

            ponto2.X += 2;
            base.PontosAlterar(ponto2, 1);

        }

        //Método para girar o palito em sentido horário
        public void girarHorario()
        {
            //Calcula a formula do ponto baseado no centro 0,0 com o raio e angulo atual
            Ponto4D temp1 = Matematica.GerarPtosCirculo(angulo, raio);

            //Se o angulo - 2 for menor que zero... Retorna à 360 (para completar o giro)
            if (angulo - 2 < 0)
            {
                angulo = 360;
            }
            //Caso contrario é diminuido 2 do angulo
            else
            {
                angulo -= 2;
            }
            //Calcula a formula do ponto baseado no centro 0,0 com o raio modificado
            Ponto4D temp2 = Matematica.GerarPtosCirculo(angulo, raio);

            //É medido a diferença entre os dois pontos (qual mudança os pontos reais irão sofrer)
            double diferencaX = temp1.X - temp2.X;
            double diferencaY = temp1.Y - temp2.Y;

            //Aplica a diferença levando em conta a posição atual dos pontos do Sr. Palito
            ponto2.X -= diferencaX;
            ponto2.Y -= diferencaY;

            //Altera o ponto na lista
            base.PontosAlterar(ponto2, 1);

        }

        //Mesmo esquema do método anterior
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
            GL.Begin(base.PrimitivaTipo);
            foreach (Ponto4D pto in pontosLista)
            {
                GL.Vertex2(pto.X, pto.Y);
            }
            GL.End();
        }


    }
}