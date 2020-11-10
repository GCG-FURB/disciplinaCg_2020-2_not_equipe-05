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

        private Camera camera = new Camera();
        protected List<Objeto> objetosLista = new List<Objeto>();
        private ObjetoGeometria objetoSelecionado = null;
        private bool bBoxDesenhar = false;
        int mouseX, mouseY;   //TODO: achar método MouseDown para não ter variável Global
        private bool mouseMoverPto = false;
        private SegReta obj_Reta;
        private SegReta obj_Reta1;
        private SegReta obj_Reta2;
        private SegReta obj_Reta3;
        private Spline obj_Spline;
#if CG_Privado
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;
#endif

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            camera.xmin = -400; camera.xmax = 400; camera.ymin = -400; camera.ymax = 400;

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");
            //criar os quatro pontos de controle
            Ponto4D supDir = new Ponto4D(100, 100, 0);
            Ponto4D infDir = new Ponto4D(100, -100, 0);
            Ponto4D supEsq = new Ponto4D(-100, 100, 0);
            Ponto4D infEsq = new Ponto4D(-100, -100, 0);
            obj_Reta = new SegReta("teste", null, infEsq, supDir, supEsq, infDir);
            obj_Reta.ObjetoCor.CorR = 0; obj_Reta.ObjetoCor.CorG = 255; obj_Reta.ObjetoCor.CorB = 255;

            obj_Spline = new Spline("teste", null, infEsq, supEsq, supDir, infDir);
            obj_Spline.ObjetoCor.CorR = 255; obj_Spline.ObjetoCor.CorG = 255; obj_Spline.ObjetoCor.CorB = 0;


            //add os objetos na lista de objetos *importante*
            objetosLista.Add(obj_Reta);
            objetosLista.Add(obj_Spline);


#if CG_Privado
      obj_SegReta = new SegReta("B", null, new Ponto4D(100, 100), new Ponto4D(-100, 100));
      obj_SegReta.ObjetoCor.CorR = 0; obj_SegReta.ObjetoCor.CorG = 255; obj_SegReta.ObjetoCor.CorB = 255;
      objetosLista.Add(obj_SegReta);
      objetoSelecionado = obj_SegReta;

      obj_SegReta1 = new SegReta("B", null, new Ponto4D(100, -100), new Ponto4D(100, 100));
      obj_SegReta1.ObjetoCor.CorR = 0; obj_SegReta1.ObjetoCor.CorG = 255; obj_SegReta1.ObjetoCor.CorB = 255;
      objetosLista.Add(obj_SegReta1);
      objetoSelecionado = obj_SegReta1;

      obj_SegReta2 = new SegReta("B", null, new Ponto4D(-100, -100), new Ponto4D(-100, 100));
      obj_SegReta2.ObjetoCor.CorR = 0; obj_SegReta2.ObjetoCor.CorG = 255; obj_SegReta2.ObjetoCor.CorB = 255;
      objetosLista.Add(obj_SegReta2);
      objetoSelecionado = obj_SegReta2;

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
            else if (e.Key == Key.O)
                bBoxDesenhar = !bBoxDesenhar;
            else if (e.Key == Key.V)
                mouseMoverPto = !mouseMoverPto;   //TODO: falta atualizar a BBox do objeto

            // aqui setta as teclas para poder mudar o ponto para mexer nos objetos
            else if (e.Key == Key.Number1)
                obj_Reta.mudarAtual(1);
            else if (e.Key == Key.Number2)
                obj_Reta.mudarAtual(2);
            else if (e.Key == Key.Number3)
                obj_Reta.mudarAtual(3);
            else if (e.Key == Key.Number4)
                obj_Reta.mudarAtual(4);
            //aqui setta as teclas para aumentar e diminuir o numeros de pontos na spline
            else if (e.Key == Key.Plus)
                obj_Spline.aumentarPontos();
            else if (e.Key == Key.Minus)
                obj_Spline.diminuirPontos();
            // aqui setta as teclas para poder mover o ponto selecionado
            else if (e.Key == Key.E)
                obj_Reta.esquerda();
            else if (e.Key == Key.B)
                obj_Reta.baixo();
            else if (e.Key == Key.D)
                obj_Reta.direita();
            else if (e.Key == Key.C)
                obj_Reta.cima();
            else if (e.Key == Key.R)
                obj_Reta.resetarPontos();
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
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);
            // GL.Color3(1.0f,0.0f,0.0f);
            GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
            GL.Vertex3(0, 0, 0); GL.Vertex3(200, 0, 0);
            // GL.Color3(0.0f,1.0f,0.0f);
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
            window.Title = "CG_N2";
            window.Run(1.0 / 60.0);
        }
    }
}
