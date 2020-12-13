
/**
  Autor: Dalton Solano dos Reis
**/

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;
using System;

namespace gcgcg
{
    internal class Reticula : ObjetoGeometria
    {

        public Reticula(char rotulo, Objeto paiRef, Ponto4D centro) : base(rotulo, paiRef)
        {
            double x = centro.X;
            double y = 0;
            double z = centro.Z;

            base.PontosAdicionar(new Ponto4D(x - 0.3, y - 0.1, z + 1.5)); // PtoA listaPto[0]
            base.PontosAdicionar(new Ponto4D(x + 0.3, y - 0.1, z + 1.5)); // PtoB listaPto[1]
            base.PontosAdicionar(new Ponto4D(x + 0.3, y + 0.1, z + 1.5)); // PtoC listaPto[2]
            base.PontosAdicionar(new Ponto4D(x - 0.3, y + 0.1, z + 1.5)); // PtoD listaPto[3]
            base.PontosAdicionar(new Ponto4D(x - 0.3, y - 0.1, z - 1.5)); // PtoE listaPto[4]
            base.PontosAdicionar(new Ponto4D(x + 0.3, y - 0.1, z - 1.5)); // PtoF listaPto[5]
            base.PontosAdicionar(new Ponto4D(x + 0.3, y + 0.1, z - 1.5)); // PtoG listaPto[6]
            base.PontosAdicionar(new Ponto4D(x - 0.3, y + 0.1, z - 1.5)); // PtoH listaPto[7]

            base.FilhoAdicionar(new Reticula(rotulo, paiRef, centro, 1));
        }

        public Reticula(char rotulo, Objeto paiRef, Ponto4D centro, int parte) : base(rotulo, paiRef)
        {
            double x = centro.X;
            double y = 0;
            double z = centro.Z;

            base.PontosAdicionar(new Ponto4D(x - 1.5, y - 0.1, z + 0.3)); // PtoA listaPto[0]
            base.PontosAdicionar(new Ponto4D(x + 1.5, y - 0.1, z + 0.3)); // PtoB listaPto[1]
            base.PontosAdicionar(new Ponto4D(x + 1.5, y + 0.1, z + 0.3)); // PtoC listaPto[2]
            base.PontosAdicionar(new Ponto4D(x - 1.5, y + 0.1, z + 0.3)); // PtoD listaPto[3]
            base.PontosAdicionar(new Ponto4D(x - 1.5, y - 0.1, z - 0.3)); // PtoE listaPto[4]
            base.PontosAdicionar(new Ponto4D(x + 1.5, y - 0.1, z - 0.3)); // PtoF listaPto[5]
            base.PontosAdicionar(new Ponto4D(x + 1.5, y + 0.1, z - 0.3)); // PtoG listaPto[6]
            base.PontosAdicionar(new Ponto4D(x - 1.5, y + 0.1, z - 0.3)); // PtoH listaPto[7]
        }

        protected override void DesenharObjeto()
        {       // Sentido anti-horário
            GL.Begin(PrimitiveType.Quads);
            // Face da frente
            GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
            GL.Normal3(0, 0, 1);
            GL.Vertex3(base.pontosLista[0].X, base.pontosLista[0].Y, base.pontosLista[0].Z);    // PtoA
            GL.Vertex3(base.pontosLista[1].X, base.pontosLista[1].Y, base.pontosLista[1].Z);    // PtoB
            GL.Vertex3(base.pontosLista[2].X, base.pontosLista[2].Y, base.pontosLista[2].Z);    // PtoC
            GL.Vertex3(base.pontosLista[3].X, base.pontosLista[3].Y, base.pontosLista[3].Z);    // PtoD
                                                                                                // Face do fundo
            GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
            GL.Normal3(0, 0, -1);
            GL.Vertex3(base.pontosLista[4].X, base.pontosLista[4].Y, base.pontosLista[4].Z);    // PtoE
            GL.Vertex3(base.pontosLista[7].X, base.pontosLista[7].Y, base.pontosLista[7].Z);    // PtoH
            GL.Vertex3(base.pontosLista[6].X, base.pontosLista[6].Y, base.pontosLista[6].Z);    // PtoG
            GL.Vertex3(base.pontosLista[5].X, base.pontosLista[5].Y, base.pontosLista[5].Z);    // PtoF
                                                                                                // Face de cima
            GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
            GL.Normal3(0, 1, 0);
            GL.Vertex3(base.pontosLista[3].X, base.pontosLista[3].Y, base.pontosLista[3].Z);    // PtoD
            GL.Vertex3(base.pontosLista[2].X, base.pontosLista[2].Y, base.pontosLista[2].Z);    // PtoC
            GL.Vertex3(base.pontosLista[6].X, base.pontosLista[6].Y, base.pontosLista[6].Z);    // PtoG
            GL.Vertex3(base.pontosLista[7].X, base.pontosLista[7].Y, base.pontosLista[7].Z);    // PtoH
                                                                                                // Face de baixo
            GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
            GL.Color4(0, 0, 255, 2);
            GL.Normal3(0, -1, 0);
            GL.Vertex3(base.pontosLista[0].X, base.pontosLista[0].Y, base.pontosLista[0].Z);    // PtoA
            GL.Vertex3(base.pontosLista[4].X, base.pontosLista[4].Y, base.pontosLista[4].Z);    // PtoE
            GL.Vertex3(base.pontosLista[5].X, base.pontosLista[5].Y, base.pontosLista[5].Z);    // PtoF
            GL.Vertex3(base.pontosLista[1].X, base.pontosLista[1].Y, base.pontosLista[1].Z);    // PtoB
                                                                                                // Face da direita
            GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
            GL.Normal3(1, 0, 0);
            GL.Vertex3(base.pontosLista[1].X, base.pontosLista[1].Y, base.pontosLista[1].Z);    // PtoB
            GL.Vertex3(base.pontosLista[5].X, base.pontosLista[5].Y, base.pontosLista[5].Z);    // PtoF
            GL.Vertex3(base.pontosLista[6].X, base.pontosLista[6].Y, base.pontosLista[6].Z);    // PtoG
            GL.Vertex3(base.pontosLista[2].X, base.pontosLista[2].Y, base.pontosLista[2].Z);    // PtoC
                                                                                                // Face da esquerda
            GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
            GL.Normal3(-1, 0, 0);
            GL.Vertex3(base.pontosLista[0].X, base.pontosLista[0].Y, base.pontosLista[0].Z);    // PtoA
            GL.Vertex3(base.pontosLista[3].X, base.pontosLista[3].Y, base.pontosLista[3].Z);    // PtoD
            GL.Vertex3(base.pontosLista[7].X, base.pontosLista[7].Y, base.pontosLista[7].Z);    // PtoH
            GL.Vertex3(base.pontosLista[4].X, base.pontosLista[4].Y, base.pontosLista[4].Z);    // PtoE
            GL.End();

            // if (exibeVetorNormal) //TODO: acho que não precisa.
            //   ajudaExibirVetorNormal(); //TODO: acho que não precisa.
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