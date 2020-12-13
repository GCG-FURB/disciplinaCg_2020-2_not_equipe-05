
/**
  Autor: Dalton Solano dos Reis
**/

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;
using System;
using System.Collections.Generic;

namespace gcgcg
{
    internal class TQ : ObjetoGeometria
    {
        private Cor cor = new Cor(0, 0, 0);

        private bool tqEspecial;
        public bool Tqespecial { get => tqEspecial; set => tqEspecial = value; }
        private int vida = 0;
        public int Vida { get => vida; set => vida = value; }

        private int posicao;
        private bool mirou = false;

        public int Posicao { get => posicao; set => posicao = value; }
        public TQ(char rotulo, Objeto paiRef, Ponto4D centro, bool tqEspecial) : base(rotulo, paiRef)
        {
            this.tqEspecial = tqEspecial;
            if (tqEspecial)
            {
                this.vida = 3;
                cor.CorR = 214;
                cor.CorG = 133;
                cor.CorB = 22;
            }
            else
            {
                cor.CorR = 0;
                cor.CorG = 100;
                cor.CorB = 0;
                this.vida = 1;
            }
            double x = centro.X;
            double y = centro.Y;
            double z = centro.Z;

            base.PontosAdicionar(new Ponto4D(x - 1.2, y - 0.75, z + 2.5)); // PtoA listaPto[0]
            base.PontosAdicionar(new Ponto4D(x + 1.2, y - 0.75, z + 2.5)); // PtoB listaPto[1]
            base.PontosAdicionar(new Ponto4D(x + 1.2, y + 0.75, z + 2.5)); // PtoC listaPto[2]
            base.PontosAdicionar(new Ponto4D(x - 1.2, y + 0.75, z + 2.5)); // PtoD listaPto[3]
            base.PontosAdicionar(new Ponto4D(x - 1.2, y - 0.75, z - 2.0)); // PtoE listaPto[4]
            base.PontosAdicionar(new Ponto4D(x + 1.2, y - 0.75, z - 2.0)); // PtoF listaPto[5]
            base.PontosAdicionar(new Ponto4D(x + 1.2, y + 1, z - 2.0)); // PtoG listaPto[6]
            base.PontosAdicionar(new Ponto4D(x - 1.2, y + 1, z - 2.0)); // PtoH listaPto[7]


            // base.FilhoAdicionar(new TQCima(rotulo, paiRef, centro));

            base.FilhoAdicionar(new TQ(rotulo, paiRef, centro, 1, tqEspecial));


        }

        public TQ(char rotulo, Objeto paiRef, Ponto4D centro, int parte, bool tqEspecial) : base(rotulo, paiRef)
        {
            switch (parte)
            {
                case (1):
                    if (tqEspecial)
                    {
                        cor.CorR = 126;
                        cor.CorG = 79;
                        cor.CorB = 14;
                    }
                    else
                    {
                        cor.CorR = 22;
                        cor.CorG = 74;
                        cor.CorB = 0;
                    }

                    base.PontosAdicionar(new Ponto4D(centro.X - 0.8, centro.Y - 0.8 + 1.5, centro.Z + 1)); // PtoA listaPto[0]
                    base.PontosAdicionar(new Ponto4D(centro.X + 0.8, centro.Y - 0.8 + 1.5, centro.Z + 1)); // PtoB listaPto[1]
                    base.PontosAdicionar(new Ponto4D(centro.X + 0.8, centro.Y + 0.2 + 1.5, centro.Z + 1)); // PtoC listaPto[2]
                    base.PontosAdicionar(new Ponto4D(centro.X - 0.8, centro.Y + 0.2 + 1.5, centro.Z + 1)); // PtoD listaPto[3]
                    base.PontosAdicionar(new Ponto4D(centro.X - 0.8, centro.Y - 1 + 1.5, centro.Z - 1)); // PtoE listaPto[4]
                    base.PontosAdicionar(new Ponto4D(centro.X + 0.8, centro.Y - 1 + 1.5, centro.Z - 1)); // PtoF listaPto[5]
                    base.PontosAdicionar(new Ponto4D(centro.X + 0.8, centro.Y + 0.3 + 1.5, centro.Z - 1)); // PtoG listaPto[6]
                    base.PontosAdicionar(new Ponto4D(centro.X - 0.8, centro.Y + 0.3 + 1.5, centro.Z - 1)); // PtoH listaPto[7]
                    base.FilhoAdicionar(new TQ(rotulo, paiRef, centro, 2, tqEspecial));
                    break;
                case (2):
                    cor.CorR = 0;
                    cor.CorG = 0;
                    cor.CorB = 0;
                    base.PontosAdicionar(new Ponto4D(centro.X - 0.1, centro.Y - 0.0 + 1.5, centro.Z + 4)); // PtoA listaPto[0]
                    base.PontosAdicionar(new Ponto4D(centro.X + 0.1, centro.Y - 0.0 + 1.5, centro.Z + 4)); // PtoB listaPto[1]
                    base.PontosAdicionar(new Ponto4D(centro.X + 0.1, centro.Y + 0.2 + 1, centro.Z + 4)); // PtoC listaPto[2]
                    base.PontosAdicionar(new Ponto4D(centro.X - 0.1, centro.Y + 0.2 + 1, centro.Z + 4)); // PtoD listaPto[3]
                    base.PontosAdicionar(new Ponto4D(centro.X - 0.1, centro.Y - 0.0 + 1.5, centro.Z - 0.3)); // PtoE listaPto[4]
                    base.PontosAdicionar(new Ponto4D(centro.X + 0.1, centro.Y - 0.0 + 1.5, centro.Z - 0.3)); // PtoF listaPto[5]
                    base.PontosAdicionar(new Ponto4D(centro.X + 0.1, centro.Y + 0.2 + 1, centro.Z - 0.3)); // PtoG listaPto[6]
                    base.PontosAdicionar(new Ponto4D(centro.X - 0.1, centro.Y + 0.2 + 1, centro.Z - 0.3)); // PtoH listaPto[7]
                    break;
            }
        }

        protected override void DesenharObjeto()
        {       // Sentido anti-horário
            GL.Begin(PrimitiveType.Quads);
            // Face da frente
            GL.Color3(Convert.ToByte(cor.CorR), Convert.ToByte(cor.CorG), Convert.ToByte(cor.CorB));
            GL.Normal3(0, 0, 1);
            GL.Vertex3(base.pontosLista[0].X, base.pontosLista[0].Y, base.pontosLista[0].Z);    // PtoA
            GL.Vertex3(base.pontosLista[1].X, base.pontosLista[1].Y, base.pontosLista[1].Z);    // PtoB
            GL.Vertex3(base.pontosLista[2].X, base.pontosLista[2].Y, base.pontosLista[2].Z);    // PtoC
            GL.Vertex3(base.pontosLista[3].X, base.pontosLista[3].Y, base.pontosLista[3].Z);    // PtoD
                                                                                                // Face do fundo
            GL.Color3(Convert.ToByte(cor.CorR), Convert.ToByte(cor.CorG), Convert.ToByte(cor.CorB));
            GL.Normal3(0, 0, -1);
            GL.Vertex3(base.pontosLista[4].X, base.pontosLista[4].Y, base.pontosLista[4].Z);    // PtoE
            GL.Vertex3(base.pontosLista[7].X, base.pontosLista[7].Y, base.pontosLista[7].Z);    // PtoH
            GL.Vertex3(base.pontosLista[6].X, base.pontosLista[6].Y, base.pontosLista[6].Z);    // PtoG
            GL.Vertex3(base.pontosLista[5].X, base.pontosLista[5].Y, base.pontosLista[5].Z);    // PtoF
                                                                                                // Face de cima
            GL.Color3(Convert.ToByte(cor.CorR), Convert.ToByte(cor.CorG), Convert.ToByte(cor.CorB));
            GL.Normal3(0, 1, 0);
            GL.Vertex3(base.pontosLista[3].X, base.pontosLista[3].Y, base.pontosLista[3].Z);    // PtoD
            GL.Vertex3(base.pontosLista[2].X, base.pontosLista[2].Y, base.pontosLista[2].Z);    // PtoC
            GL.Vertex3(base.pontosLista[6].X, base.pontosLista[6].Y, base.pontosLista[6].Z);    // PtoG
            GL.Vertex3(base.pontosLista[7].X, base.pontosLista[7].Y, base.pontosLista[7].Z);    // PtoH
                                                                                                // Face de baixo
            GL.Color3(Convert.ToByte(cor.CorR), Convert.ToByte(cor.CorG), Convert.ToByte(cor.CorB));
            GL.Color4(0, 0, 255, 2);
            GL.Normal3(0, -1, 0);
            GL.Vertex3(base.pontosLista[0].X, base.pontosLista[0].Y, base.pontosLista[0].Z);    // PtoA
            GL.Vertex3(base.pontosLista[4].X, base.pontosLista[4].Y, base.pontosLista[4].Z);    // PtoE
            GL.Vertex3(base.pontosLista[5].X, base.pontosLista[5].Y, base.pontosLista[5].Z);    // PtoF
            GL.Vertex3(base.pontosLista[1].X, base.pontosLista[1].Y, base.pontosLista[1].Z);    // PtoB
                                                                                                // Face da direita
            GL.Color3(Convert.ToByte(cor.CorR), Convert.ToByte(cor.CorG), Convert.ToByte(cor.CorB));
            GL.Normal3(1, 0, 0);
            GL.Vertex3(base.pontosLista[1].X, base.pontosLista[1].Y, base.pontosLista[1].Z);    // PtoB
            GL.Vertex3(base.pontosLista[5].X, base.pontosLista[5].Y, base.pontosLista[5].Z);    // PtoF
            GL.Vertex3(base.pontosLista[6].X, base.pontosLista[6].Y, base.pontosLista[6].Z);    // PtoG
            GL.Vertex3(base.pontosLista[2].X, base.pontosLista[2].Y, base.pontosLista[2].Z);    // PtoC
                                                                                                // Face da esquerda
            GL.Color3(Convert.ToByte(cor.CorR), Convert.ToByte(cor.CorG), Convert.ToByte(cor.CorB));
            GL.Normal3(-1, 0, 0);
            GL.Vertex3(base.pontosLista[0].X, base.pontosLista[0].Y, base.pontosLista[0].Z);    // PtoA
            GL.Vertex3(base.pontosLista[3].X, base.pontosLista[3].Y, base.pontosLista[3].Z);    // PtoD
            GL.Vertex3(base.pontosLista[7].X, base.pontosLista[7].Y, base.pontosLista[7].Z);    // PtoH
            GL.Vertex3(base.pontosLista[4].X, base.pontosLista[4].Y, base.pontosLista[4].Z);    // PtoE
            GL.End();

            // if (exibeVetorNormal) //TODO: acho que não precisa.
            //   ajudaExibirVetorNormal(); //TODO: acho que não precisa.
        }

        public void mirar(List<Ponto4D> centros)
        {
            if (mirou == false)
            {


                double distMaisProx = Double.MaxValue;
                int indiceMaisProx = 0;

                int i = 0;

                foreach (Ponto4D pto in centros)
                {
                    //X2 - X1 
                    double distanciax = this.BBox.centro.X - pto.X;
                    //Ao quadrado
                    distanciax = distanciax * distanciax;

                    //Y2 - Y1
                    double distanciay = this.BBox.centro.X - pto.Y;
                    //Ao quadrado
                    distanciay = distanciay * distanciay;

                    //Somam-se as distâncias
                    double distancia = distanciax + distanciay;


                    if (distancia < distMaisProx)
                    {
                        //Este é o mais proximo
                        distMaisProx = distancia;

                        //Guarda-se o indice do mais proximo para depois
                        indiceMaisProx = i;
                    }
                    i++;

                }


                //gira pra esquerda
                if (centros[indiceMaisProx].X < this.BBox.centro.X)
                {
                    base.ObjetosLista[0].RotacaoZBBox(Math.Abs(centros[indiceMaisProx].X - this.BBox.centro.X) * 1.5, 'y');
                }
                else
                { //gira pra direita
                    base.ObjetosLista[0].RotacaoZBBox(-Math.Abs(centros[indiceMaisProx].X - this.BBox.centro.X) * 1.5, 'y');
                }

                mirou = true;
            }
        }

        //TODO: melhorar para exibir não só a lista de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Cubo: " + base.rotulo + "\n";
            for (var i = 0; i < pontosLista.Count; i++)
            {
                retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
            }
            return (retorno);
        }

    }
}