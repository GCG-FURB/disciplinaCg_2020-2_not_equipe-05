/**
  Autor: Dalton Solano dos Reis
**/

/// <summary>
/// fonte: https://stackoverflow.com/questions/4170603/how-do-i-draw-a-cylinder-in-opentk-glu-cylinder
/// </summary>
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using CG_Biblioteca;
using System.Timers;

namespace gcgcg
{
    internal class Nave : ObjetoGeometria
    {
        //TODO: gerar os vetores normais, tem como fazer no link deste exemplo
        private bool exibeVetorNormal = false;
        //TODO: não precisava ter parte negativa, ter um tipo inteiro grande
        protected List<int> listaTopologia = new List<int>();

        private int corPisca = 0;

        System.Timers.Timer poder;
        private Cor cor;
        public Cor Cor { get => cor; set => cor = value; }
        private Ponto4D centro;
        private bool trava = false;

        private Nave raio;

        public Nave(char rotulo, Objeto paiRef, int qualidade, double raio, double altura, Cor cor, Ponto4D centro) : base(rotulo, paiRef)
        {

            System.Timers.Timer travaRaio = new System.Timers.Timer();
            travaRaio.Elapsed += new ElapsedEventHandler(travar);
            travaRaio.Interval = 953;
            travaRaio.Enabled = true;

            this.cor = cor;
            this.centro = centro;
            criaCilindro(qualidade, raio, altura, centro);
            base.FilhoAdicionar(new Nave(rotulo, this, qualidade, raio, altura, new Cor(150, 150, 150), centro, 1));
            base.FilhoAdicionar(new Nave(rotulo, this, qualidade, raio, altura, new Cor(196, 255, 255), centro, 2));
            base.FilhoAdicionar(new Reticula(rotulo, this, centro));
            this.raio = new Nave(rotulo, this, 50, 0.1, -50, new Cor(255, 255, 255), centro, 3);



            System.Timers.Timer animaRaio = new System.Timers.Timer();
            animaRaio.Elapsed += new ElapsedEventHandler(animar);
            animaRaio.Interval = 200;
            animaRaio.Enabled = true;

            poder = new System.Timers.Timer();
            poder.Elapsed += new ElapsedEventHandler(piscar);
            poder.Interval = 300;
        }
        private void travar(object source, ElapsedEventArgs e)
        {
            if (trava == true)
            {
                trava = false;
            }
        }

        private void animar(object source, ElapsedEventArgs e)
        {
            base.FilhoRemover(raio);
        }


        public Ponto4D atirar()
        {
            if (trava == false)
            {
                trava = true;
                base.FilhoAdicionar(raio);
                return new Ponto4D(this.BBox.centro);
            }
            else return null;
        }

        public Nave(char rotulo, Objeto paiRef, int qualidade, double raio, double altura, Cor cor, Ponto4D centro, int parte) : base(rotulo, paiRef)
        {
            this.cor = cor;
            switch (parte)
            {
                case (1):
                    criaCilindro(qualidade, raio * 1.25, altura * 0.75, centro);
                    break;
                case (2):
                    criaCilindro(qualidade, raio * 0.3, altura * 1.75, centro);
                    break;
                case (3):
                    criaCilindro(qualidade, raio, altura, centro);
                    break;
            }

        }


        private void criaCilindro(int qualidade, double raio, double altura, Ponto4D centro)
        {
            int segments = 50; // Números mais altos melhoram a qualidade 
            double radius = raio;    // O raio (largura) do cilindro
            double height = altura;   // A altura do cilindro

            for (double y = 0; y < 2; y++)
            {
                for (double x = 0; x < segments; x++)
                {
                    double theta = (x / (segments - 1)) * 2 * Math.PI;
                    base.PontosAdicionar(new Ponto4D(
                        (float)(radius * Math.Cos(theta)) + centro.X,
                        (float)(height * y) + centro.Y,
                        (float)(radius * Math.Sin(theta) + centro.Z)));
                }
            }
            // ponto do centro da base
            // base.PontosAdicionar(new Ponto4D(0, 0, 0));
            base.PontosAdicionar(centro);

            // ponto do centro da topo
            // base.PontosAdicionar(new Ponto4D(0, height, 0));
            Ponto4D topo = new Ponto4D(centro);
            topo.Y = height;
            base.PontosAdicionar(topo);

            for (int x = 0; x < segments - 1; x++)
            {
                // lados
                listaTopologia.Add(x);
                listaTopologia.Add(x + segments);
                listaTopologia.Add(x + segments + 1);
                listaTopologia.Add(x + segments + 1);
                listaTopologia.Add(x + 1);
                listaTopologia.Add(x);
                // base
                listaTopologia.Add(x);
                listaTopologia.Add(x + 1);
                listaTopologia.Add(segments - 1);
                // topo
                listaTopologia.Add(x + segments + 1);
                listaTopologia.Add(x + segments);
                listaTopologia.Add(segments);
            }
        }

        protected override void DesenharObjeto()
        {
            GL.Begin(PrimitiveType.Triangles);
            GL.Color3(Convert.ToByte(cor.CorR), Convert.ToByte(cor.CorG), Convert.ToByte(cor.CorB));
            foreach (int index in listaTopologia)
                GL.Vertex3(base.pontosLista[index].X, base.pontosLista[index].Y, base.pontosLista[index].Z);
            GL.End();
        }

        public void ativarPiscar()
        {

            poder.Enabled = true;
        }

        private void piscar(object source, ElapsedEventArgs e)
        {
            if (corPisca < 10)
            {

                if (corPisca % 2 == 0)
                {
                    Nave nave = (Nave)base.ObjetosLista[1];
                    nave.cor.CorR = 128;
                    nave.cor.CorG = 0;
                    nave.cor.CorB = 255;

                }
                else
                {
                    Nave nave = (Nave)base.ObjetosLista[1];
                    nave.cor.CorR = 196;
                    nave.cor.CorG = 255;
                    nave.cor.CorB = 255;
                }
                corPisca++;
            }
            else
            {
                poder.Enabled = false;
                corPisca = 0;
            }
            
        }

        //TODO: melhorar para exibir não só a lista de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Cilindro: " + base.rotulo + "\n";
            for (var i = 0; i < pontosLista.Count; i++)
            {
                retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
            }
            return (retorno);
        }

    }
}