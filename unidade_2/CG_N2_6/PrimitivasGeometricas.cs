/**
  Autor: Dalton Solano dos Reis
**/

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
  internal class PrimitivasGeometricas : ObjetoGeometria
  {
    private int primitiva;
  private Cor[] cores;
    public void mudarPrimitva(){
      if(primitiva == 9){
        primitiva = 0;
      }else{
        primitiva++;
      }
    }
    public PrimitivasGeometricas(string rotulo, Objeto paiRef, Ponto4D ptoSupEsq, Ponto4D ptoSupDir, Ponto4D ptoInfEsq, Ponto4D ptoInfDir) : base(rotulo, paiRef)
    {
      base.PontosAdicionar(ptoSupEsq);
      base.PontosAdicionar(ptoSupDir);
      base.PontosAdicionar(ptoInfEsq);
      base.PontosAdicionar(ptoInfDir);
      
      cores = new[] { new Cor(0, 255, 255, 0), new Cor(255, 0, 255, 255), new Cor(255, 255, 0, 255), new Cor(0, 0, 0, 0) };
      //base.PontosAdicionar(new Ponto4D(ptoSupDir.X, ptoInfEsq.Y));
      //base.PontosAdicionar(ptoSupDir);
      //base.PontosAdicionar(new Ponto4D(ptoInfEsq.X, ptoSupDir.Y));
    }
    
    protected override void DesenharObjeto()
    {
      GL.LineWidth(4);
      GL.PointSize(4);
      GL.Begin((PrimitiveType)primitiva);
      var cor = 0;
      foreach (Ponto4D pto in pontosLista)
      {
        GL.Color3(cores[cor].CorR, cores[cor].CorG, cores[cor].CorB);
        GL.Vertex2(pto.X, pto.Y);
        cor++;
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

  }
}