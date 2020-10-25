/**
  Autor: Dalton Solano dos Reis
**/

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
    internal class Spline : ObjetoGeometria
    {
        private Ponto4D ptoInfEsq;
        private Ponto4D ptoSupEsq;
        private Ponto4D ptoSupDir;
        private Ponto4D ptoInfDir;
        private int qtdPontos;
        public Spline(string rotulo, Objeto paiRef, Ponto4D ptoInfEsq, Ponto4D ptoSupEsq, Ponto4D ptoSupDir, Ponto4D ptoInfDir) : base(rotulo, paiRef)
        {
            //como default, setando a quantidade de pontos da spline para 10
            this.qtdPontos = 10;

            this.ptoInfDir = ptoInfDir;
            this.ptoInfEsq = ptoInfEsq;
            this.ptoSupDir = ptoSupDir;
            this.ptoSupEsq = ptoSupEsq;

        }

        protected override void DesenharObjeto()
        {
            //remove os todos os pontos para ser gerados novamente, para não causar overflow;
            base.PontosRemoverTodos();
            //chama o metodo para gerar a spline e add na base;
            gerarSpline();
            GL.LineWidth(3);
            GL.Begin(PrimitiveType.LineStrip);
            foreach (Ponto4D pto in pontosLista)
            {
                GL.Vertex2(pto.X, pto.Y);
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


        //método que gera a spline baseado no metodo de bezier
        private void gerarSpline()
        {
            double inc = 1.0 / this.qtdPontos; //aqui se divide pois o valor é de 0 a 1
            double t = 0;
            for (t = 0; t <= 1; t += inc)
            {
                //algoritmo de bezier, conforme passa em sala;
                var umMenosT = (1 - t);
                var pesoP0 = umMenosT * umMenosT * umMenosT;
                var pesoP1 = 3 * t * (umMenosT * umMenosT);
                var pesoP2 = 3 * (t * t) * umMenosT;
                var pesoP3 = t * t * t;

                //cria um ponto na spline
                var valorXpto = (pesoP0 * ptoInfDir.X) + (pesoP1 * ptoSupDir.X) + (pesoP2 * ptoSupEsq.X) + (pesoP3 * ptoInfEsq.X);
                var valorYpto = (pesoP0 * ptoInfDir.Y) + (pesoP1 * ptoSupDir.Y) + (pesoP2 * ptoSupEsq.Y) + (pesoP3 * ptoInfEsq.Y);

                base.PontosAdicionar(new Ponto4D(valorXpto, valorYpto));
            }

            // essa parte é para que caso o t passe de 1, ele faça a ultima reta
            if (t > 1)
            {
                t = 1;
                var umMenosT = (1 - t);
                var pesoP0 = umMenosT * umMenosT * umMenosT;
                var pesoP1 = 3 * t * (umMenosT * umMenosT);
                var pesoP2 = 3 * (t * t) * umMenosT;
                var pesoP3 = t * t * t;

                var valorXpto = (pesoP0 * ptoInfDir.X) + (pesoP1 * ptoSupDir.X) + (pesoP2 * ptoSupEsq.X) + (pesoP3 * ptoInfEsq.X);
                var valorYpto = (pesoP0 * ptoInfDir.Y) + (pesoP1 * ptoSupDir.Y) + (pesoP2 * ptoSupEsq.Y) + (pesoP3 * ptoInfEsq.Y);

                base.PontosAdicionar(new Ponto4D(valorXpto, valorYpto));
            }
        }

        public void diminuirPontos()
        {
            {
                //se for 1, não pode diminuir, senão acontecerá dividão por 0 e trava
                if (qtdPontos != 1)
                    this.qtdPontos--;
            }

        }

        public void aumentarPontos()
        {
            this.qtdPontos++;
        }

    }
}