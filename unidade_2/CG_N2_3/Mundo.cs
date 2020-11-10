/**
  Autor: Dalton Solano dos Reis
**/

#define CG_Gizmo
// #define CG_Privado

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK.Input;
using CG_Biblioteca;
using CG_N2;

namespace gcgcg
{
    class Mundo : GameWindow
    {
        private static Mundo instanciaMundo = null;

        private Mundo(int width, int height) : base(width, height) { }

        public static Mundo GetInstance(int width, int height)
        {
            if (instanciaMundo == null)
                instanciaMundo = new Mundo(width, height);
            return instanciaMundo;
        }

        private CameraOrtho camera = new CameraOrtho();
        Camera cameraTeclado;
        protected List<Objeto> objetosLista = new List<Objeto>();
        private ObjetoGeometria objetoSelecionado = null;
        private bool bBoxDesenhar = false;
        int mouseX, mouseY;   //TODO: achar método MouseDown para não ter variável Global
        private bool mouseMoverPto = false;
        private Retangulo obj_Retangulo;
#if CG_Privado
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;
#endif

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            cameraTeclado = new Camera(camera);
            camera.xmin = -300; camera.xmax = 300; camera.ymin = -300; camera.ymax = 300;

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");

            //Círculo 1
            Circulo circulo1 = new Circulo("Circulo1", null, new Ponto4D(0.0, 100.0, 0.0, 1.0), 100);

            //Cor Círculo 1 (preto)
            circulo1.ObjetoCor.CorR = 0; circulo1.ObjetoCor.CorG = 0; circulo1.ObjetoCor.CorB = 0;

            //Adiciona objeto na lista de objetos a desenhar
            objetosLista.Add(circulo1);

            Circulo circulo2 = new Circulo("Circulo2", null, new Ponto4D(-100.0, -100.0, 0.0, 1.0), 100);
            circulo2.ObjetoCor.CorR = 0; circulo2.ObjetoCor.CorG = 0; circulo2.ObjetoCor.CorB = 0;
            objetosLista.Add(circulo2);

            Circulo circulo3 = new Circulo("Circulo3", null, new Ponto4D(100.0, -100.0, 0.0, 1.0), 100);
            circulo3.ObjetoCor.CorR = 0; circulo3.ObjetoCor.CorG = 0; circulo3.ObjetoCor.CorB = 0;
            objetosLista.Add(circulo3);

            //Segmentos de reta com pontos nos mesmos lugares que o centro de cada círculo
            SegReta seg1 = new SegReta("Segmento1", null, new Ponto4D(0.0, 100.0, 0.0, 1.0),new Ponto4D(-100.0, -100.0, 0.0, 1.0));

            //Cor ciano
            seg1.ObjetoCor.CorR = 0; seg1.ObjetoCor.CorG = 255; seg1.ObjetoCor.CorB = 255;
            objetosLista.Add(seg1);

            SegReta seg2 = new SegReta("Segmento2", null, new Ponto4D(0.0, 100.0, 0.0, 1.0), new Ponto4D(100.0, -100.0, 0.0, 1.0));
            seg2.ObjetoCor.CorR = 0; seg2.ObjetoCor.CorG = 255; seg2.ObjetoCor.CorB = 255;
            objetosLista.Add(seg2);

            SegReta seg3 = new SegReta("Segmento3", null, new Ponto4D(-100.0, -100.0, 0.0, 1.0), new Ponto4D(100.0, -100.0, 0.0, 1.0));
            seg3.ObjetoCor.CorR = 0; seg3.ObjetoCor.CorG = 255; seg3.ObjetoCor.CorB = 255;
            objetosLista.Add(seg3);


#if CG_Privado
      obj_SegReta = new Privado_SegReta("B", null, new Ponto4D(50, 150), new Ponto4D(150, 250));
      obj_SegReta.ObjetoCor.CorR = 255; obj_SegReta.ObjetoCor.CorG = 0; obj_SegReta.ObjetoCor.CorB = 0;
      objetosLista.Add(obj_SegReta);
      objetoSelecionado = obj_SegReta;

      obj_Circulo = new Privado_Circulo("C", null, new Ponto4D(100, 300), 50);
      obj_Circulo.ObjetoCor.CorR = 0; obj_Circulo.ObjetoCor.CorG = 255; obj_Circulo.ObjetoCor.CorB = 255;
      objetosLista.Add(obj_Circulo);
      objetoSelecionado = obj_Circulo;
#endif
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(camera.xmin, camera.xmax, camera.ymin, camera.ymax, camera.zmin, camera.zmax);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
#if CG_Gizmo
            Sru3D();
#endif

            for (var i = 0; i < objetosLista.Count; i++)
                objetosLista[i].Desenhar();
            if (bBoxDesenhar && (objetoSelecionado != null))
                objetoSelecionado.BBox.Desenhar();
            this.SwapBuffers();
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.H)
                Utilitario.AjudaTeclado();
            else if (e.Key == Key.Escape)
                Exit();
            else if (e.Key == Key.E)
            {
                // Console.WriteLine("--- Objetos / Pontos: ");
                // for (var i = 0; i < objetosLista.Count; i++)
                // {
                //     Console.WriteLine(objetosLista[i]);
                // }
                cameraTeclado.PanEsquerda();
            }
            else if (e.Key == Key.D)
            {
                cameraTeclado.PanDireita();
            }
            else if (e.Key == Key.C)
            {
                cameraTeclado.PanCima();
            }
            else if (e.Key == Key.B)
            {
                cameraTeclado.PanBaixo();
            }
            else if (e.Key == Key.O)
                // bBoxDesenhar = !bBoxDesenhar;
                cameraTeclado.ZoomOut();
            else if (e.Key == Key.I)
                // bBoxDesenhar = !bBoxDesenhar;
                cameraTeclado.ZoomIn();
            else if (e.Key == Key.V)
                mouseMoverPto = !mouseMoverPto;   //TODO: falta atualizar a BBox do objeto
            else
                Console.WriteLine(" __ Tecla não implementada.");
        }

        //TODO: não está considerando o NDC
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            mouseX = e.Position.X; mouseY = 600 - e.Position.Y; // Inverti eixo Y
            if (mouseMoverPto && (objetoSelecionado != null))
            {
                objetoSelecionado.PontosUltimo().X = mouseX;
                objetoSelecionado.PontosUltimo().Y = mouseY;
            }
        }

#if CG_Gizmo
        private void Sru3D()
        {
            GL.LineWidth(2);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
            GL.Vertex3(0, 0, 0); GL.Vertex3(200, 0, 0);
            GL.Color3(Convert.ToByte(0), Convert.ToByte(255), Convert.ToByte(0));
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 200, 0);
            GL.End();
        }
#endif

    }



    class Program
    {
        static void Main(string[] args)
        {
            Mundo window = Mundo.GetInstance(600, 600);
            window.Title = "CG_N2_EX_03";
            window.Run(1.0 / 60.0);
        }
    }
}
