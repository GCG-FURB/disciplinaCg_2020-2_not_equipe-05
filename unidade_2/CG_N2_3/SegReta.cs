/**
  Autor: Dalton Solano dos Reis
**/

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
  internal class SegReta : ObjetoGeometria

  {
    //Reta precisa de dois pontos
    public SegReta(string rotulo, Objeto paiRef, Ponto4D pto1, Ponto4D pto2) : base(rotulo, paiRef)
    {
      //Adiciona os dois pontos passados para a lista
      base.PontosAdicionar(pto1);
      base.PontosAdicionar(pto2);
    }

    protected override void DesenharObjeto()
    {
      //Largura 5
      GL.LineWidth(5);

      //Primitiva Lines
      GL.Begin(PrimitiveType.Lines);

      //Para cada ponto na lista, adiciona um vértice da reta
      foreach (Ponto4D pto in pontosLista)
      {
        GL.Vertex2(pto.X, pto.Y);
      }

      GL.End();
    }

  }
}