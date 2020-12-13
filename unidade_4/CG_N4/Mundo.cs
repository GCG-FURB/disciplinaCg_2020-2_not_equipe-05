#define CG_Gizmo
#define CG_Privado

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK.Input;
using CG_Biblioteca;
using System.Timers;

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



        private Nave nave;
        protected List<Objeto> objetosLista = new List<Objeto>();
        protected List<Objeto> tanques = new List<Objeto>();
        System.Timers.Timer poder;
        System.Timers.Timer movedorT;
        protected List<Nave> naves = new List<Nave>();
        private ObjetoGeometria objetoSelecionado = null;
        private char objetoId = '@';
        private String menuSelecao = "";
        private char menuEixoSelecao = 'z';
        private short deslocamento = 0;

        private List<Ponto4D> centros = new List<Ponto4D>();

        protected List<int> posicoes = new List<int>();
        private List<Nave> raios = new List<Nave>();
        int qtdTanques = 0;

        private bool bBoxDesenhar = false;
        int mouseX, mouseY;   //TODO: achar método MouseDown para não ter variável Global
        private Poligono objetoNovo = null;

#if CG_Privado
        // private Retangulo obj_Retangulo;
        // private Privado_SegReta obj_SegReta;
        // private Privado_Circulo obj_Circulo;
        // private Cilindro obj_Cilindro;
        // private Esfera obj_Esfera;
        // private Cone obj_Cone;
#endif

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");

            System.Timers.Timer geradorT = new System.Timers.Timer();
            geradorT.Elapsed += new ElapsedEventHandler(gerarT);
            geradorT.Interval = 1500;
            geradorT.Enabled = true;

            movedorT = new System.Timers.Timer();
            movedorT.Elapsed += new ElapsedEventHandler(moverT);
            movedorT.Interval = 150;
            movedorT.Enabled = true;



            poder = new System.Timers.Timer();
            poder.Elapsed += new ElapsedEventHandler(ativaPoder);
            poder.Interval = 2000;

            System.Timers.Timer limpaRaios = new System.Timers.Timer();
            limpaRaios.Elapsed += new ElapsedEventHandler(limpaR);
            limpaRaios.Interval = 1000;
            limpaRaios.Enabled = true;


#if CG_Privado
            // objetoId = Utilitario.charProximo(objetoId);
            // obj_Retangulo = new Retangulo(objetoId, null, new Ponto4D(50, 50, 0), new Ponto4D(150, 150, 0));
            // obj_Retangulo.ObjetoCor.CorR = 255; obj_Retangulo.ObjetoCor.CorG = 0; obj_Retangulo.ObjetoCor.CorB = 255;
            // objetosLista.Add(obj_Retangulo);
            // objetoSelecionado = obj_Retangulo;

            // objetoId = Utilitario.charProximo(objetoId);
            // obj_SegReta = new Privado_SegReta(objetoId, null, new Ponto4D(50, 150), new Ponto4D(150, 250));
            // obj_SegReta.ObjetoCor.CorR = 255; obj_SegReta.ObjetoCor.CorG = 99; obj_SegReta.ObjetoCor.CorB = 71;
            // objetosLista.Add(obj_SegReta);
            // objetoSelecionado = obj_SegReta;

            // objetoId = Utilitario.charProximo(objetoId);
            // obj_Circulo = new Privado_Circulo(objetoId, null, new Ponto4D(100, 300), 50);
            // obj_Circulo.ObjetoCor.CorR = 177; obj_Circulo.ObjetoCor.CorG = 166; obj_Circulo.ObjetoCor.CorB = 136;
            // objetosLista.Add(obj_Circulo);
            // objetoSelecionado = obj_Circulo;
            // obj_Cilindro = new Cilindro(objetoId, null);
            // obj_Cilindro.ObjetoCor.CorR = 177; obj_Cilindro.ObjetoCor.CorG = 166; obj_Cilindro.ObjetoCor.CorB = 136;
            // objetosLista.Add(obj_Cilindro);
            // obj_Cilindro.EscalaXYZ(50, 50, 50);
            // obj_Cilindro.TranslacaoXYZ(150, 0, 0);

            // objetoId = Utilitario.charProximo(objetoId);
            // obj_Esfera = new Esfera(objetoId, null);
            // obj_Esfera.ObjetoCor.CorR = 177; obj_Esfera.ObjetoCor.CorG = 166; obj_Esfera.ObjetoCor.CorB = 136;
            // objetosLista.Add(obj_Esfera);
            // obj_Esfera.EscalaXYZ(50, 50, 50);
            // obj_Esfera.TranslacaoXYZ(200, 0, 0);

            // objetoId = Utilitario.charProximo(objetoId);
            // obj_Cone = new Cone(objetoId, null);
            // obj_Cone.ObjetoCor.CorR = 177; obj_Cone.ObjetoCor.CorG = 166; obj_Cone.ObjetoCor.CorB = 136;
            // objetosLista.Add(obj_Cone);
            // obj_Cone.EscalaXYZ(50, 50, 50);
            // obj_Cone.TranslacaoXYZ(250,0,0);
#endif

            // objetoId = Utilitario.charProximo(objetoId);
            // TQ obj_TQ = new TQ(objetoId, null, new Ponto4D(0, 0, 0, 1));
            // objetosLista.Add(obj_TQ);
            // objetoSelecionado = obj_TQ;

            // objetoId = Utilitario.charProximo(objetoId);
            // TQ obj_TQ2 = new TQ('a', null, new Ponto4D(10.0f, 0.0f, 10.0f));
            // objetosLista.Add(obj_TQ2);
            // objetoSelecionado = obj_TQ2;

            objetoId = Utilitario.charProximo(objetoId);
            Nave obj_e = new Nave('a', null, 50, 3, 0.5, new Cor(128, 128, 128), new Ponto4D(20, 50, 10));
            objetosLista.Add(obj_e);
            nave = obj_e;

            objetoId = Utilitario.charProximo(objetoId);
            Nave mae = new Nave('a', null, 50, 20, 4, new Cor(128, 128, 128), new Ponto4D(90, 0, -25));
            centros.Add(new Ponto4D(90, 0, -25));
            mae.RotacaoZBBox(20, 'z');
            mae.RotacaoZBBox(30, 'x');
            objetosLista.Add(mae);
            ((Nave)mae.ObjetosLista[1]).Cor.CorR = 0;
            ((Nave)mae.ObjetosLista[1]).Cor.CorG = 255;
            ((Nave)mae.ObjetosLista[1]).Cor.CorB = 15;
            naves.Add(mae);

            objetoId = Utilitario.charProximo(objetoId);
            Nave mae2 = new Nave('a', null, 50, 10, 3, new Cor(100, 100, 100), new Ponto4D(40, -0.5, -20));
            centros.Add(new Ponto4D(40, -0.5, -20));
            mae2.RotacaoZBBox(-10, 'z');
            mae2.RotacaoZBBox(-5, 'x');
            objetosLista.Add(mae2);
            naves.Add(mae2);

            objetoId = Utilitario.charProximo(objetoId);
            Nave mae3 = new Nave('a', null, 50, 10, 3, new Cor(100, 100, 100), new Ponto4D(150, -0.5, -20));
            centros.Add(new Ponto4D(150, -0.5, -20));
            mae3.RotacaoZBBox(15, 'z');
            mae3.RotacaoZBBox(-10, 'x');
            objetosLista.Add(mae3);
            naves.Add(mae3);






            objetoId = Utilitario.charProximo(objetoId);
            Chao chao = new Chao('a', null, new Cor(255, 192, 74));
            objetosLista.Add(chao);



            // objetoId = Utilitario.charProximo(objetoId);
            // Reticula obj_e = new Reticula('a', null, new Ponto4D(10,10,10));
            // objetosLista.Add(obj_e);
            // objetoSelecionado = obj_e;



            // obj_Cubo.EscalaXYZ(7, 1, 5);
            // obj_Cubo.TranslacaoXYZ(3,-1,2);

            // // Objeto Parede 1
            // objetoId = Utilitario.charProximo(objetoId);
            // obj_Cubo = new Cubo(objetoId, null);
            // objetosLista.Add(obj_Cubo);
            // obj_Cubo.TranslacaoXYZ(0,0,0);

            // // Objeto Parede 2
            // objetoId = Utilitario.charProximo(objetoId);
            // obj_Cubo = new Cubo(objetoId, null);
            // objetosLista.Add(obj_Cubo);
            // obj_Cubo.TranslacaoXYZ(0,0,1);

            // // Objeto Personagem
            // objetoId = Utilitario.charProximo(objetoId);
            // obj_Cubo = new Cubo(objetoId, null);
            // objetosLista.Add(obj_Cubo);
            // obj_Cubo.EscalaXYZ(0.7, 0.7, 0.7);
            // obj_Cubo.TranslacaoXYZ(1,0,3);

            // objetoSelecionado = obj_Cubo;

            // camera.Eye = new Vector3(3.5f, -1, 3);
            // camera.Eye = new Vector3(35.0f,150.0f, 80.0f);
            // camera.At = new Vector3(40.0f, 0.0f, 30.0f);

            //Camera de cima
            camera.Eye = new Vector3(50.0f, 110.0f, 80.0f);
            camera.At = new Vector3(50.0f, 0.0f, 20.0f);

            //Camera debug
            // camera.Eye = new Vector3(30.0f, 30.0f, 30.0f);
            // camera.At = new Vector3(10.0f, 10.0f, 10.0f);


            // camera.At = new Vector3(3.5f, -0.5f, 2.5f);
            // camera.Near = 0.1f;
            // camera.Far = 600.0f;

            GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(camera.Fovy, Width / (float)Height, camera.Near, camera.Far);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Matrix4 modelview = Matrix4.LookAt(camera.Eye, camera.At, camera.Up);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);


#if CG_Gizmo
            // Sru3D();
#endif





            for (var i = 0; i < objetosLista.Count; i++)
            {
                objetosLista[i].Desenhar();
                // objetosLista[i].BBox.Desenhar();
            }



            this.SwapBuffers();
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            // Console.WriteLine("__ " + menuSelecao);
            if (e.Key == Key.H) Utilitario.AjudaTeclado();
            else if (e.Key == Key.Escape) Exit();

            else if (e.Key == Key.X) menuEixoSelecao = 'x';
            else if (e.Key == Key.Y) menuEixoSelecao = 'y';
            else if (e.Key == Key.Z) menuEixoSelecao = 'z';
            else if (e.Key == Key.Minus) deslocamento--;
            else if (e.Key == Key.Plus) deslocamento++;
            else if (e.Key == Key.C) menuSelecao = "[menu] C: Câmera";
            else if (e.Key == Key.O) menuSelecao = "[menu] O: Objeto";

            // Menu: seleção
            else if (menuSelecao.Equals("[menu] C: Câmera")) camera.MenuTecla(e.Key, menuEixoSelecao, deslocamento);
            // else if (menuSelecao.Equals("[menu] O: Objeto")) //FIXME: terminar igual a camera
            // {
            //   if (objetoSelecionado != null) objetoSelecionado.MenuTecla(e.Key, menuEixoSelecao, deslocamento);
            // }

            else if (e.Key == Key.E)
            {
                Console.WriteLine("--- Objetos / Pontos: ");
                for (var i = 0; i < objetosLista.Count; i++)
                {
                    Console.WriteLine(objetosLista[i]);
                }
            }
            else if (e.Key == Key.O)
                bBoxDesenhar = !bBoxDesenhar;
            else if (e.Key == Key.Enter)
            {
                if (objetoNovo != null)
                {
                    objetoNovo.PontosRemoverUltimo();   // N3-Exe6: "truque" para deixar o rastro
                    objetoSelecionado = objetoNovo;
                    objetoNovo = null;
                }
            }
            else if (e.Key == Key.Space)
            {
                Ponto4D xz = nave.atirar();
                if (xz != null)
                {

                    TQ tanqueRemover = null;
                    foreach (TQ tanque in tanques)
                    {
                        if (xz.X >= tanque.BBox.obterMenorX &&
                            xz.X <= tanque.BBox.obterMaiorX &&
                            xz.Z >= tanque.BBox.obterMenorZ &&
                            xz.Z <= tanque.BBox.obterMaiorZ)
                        {
                            tanqueRemover = tanque;
                            break;
                        }
                    }
                    if (tanqueRemover != null)
                    {
                        if (tanqueRemover.Tqespecial)
                        {
                            tanqueRemover.Vida--;
                            if (tanqueRemover.Vida == 0)
                            {
                                objetosLista.Remove(tanqueRemover);
                                tanques.Remove(tanqueRemover);
                                posicoes.Remove(tanqueRemover.Posicao);
                            }
                        }
                        else
                        {
                            objetosLista.Remove(tanqueRemover);
                            tanques.Remove(tanqueRemover);
                            posicoes.Remove(tanqueRemover.Posicao);
                        }

                    }

                }


            }
            else if (e.Key == Key.R)
            {
                naves[1].ativarPiscar();
                poder.Enabled = true;
            }
            else if (e.Key == Key.A)
            {
                camera.deslocarEsquerda();
            }
            else if (e.Key == Key.D)
            {
                camera.deslocarDireita();
            }
            else if (e.Key == Key.W)
            {
                camera.deslocarFrente();
            }
            else if (e.Key == Key.S)
            {
                camera.deslocarTras();
            }
            else if (e.Key == Key.Left)
            {
                if (nave.BBox.centro.X - 2 > 0)
                {
                    nave.TranslacaoXYZ(-2, 0, 0);
                    nave.atualizaBBoxTransformacao();
                }
            }
            else if (e.Key == Key.Right)
            {
                if (nave.BBox.centro.X + 2 < 150)
                {
                    nave.TranslacaoXYZ(2, 0, 0);
                    nave.atualizaBBoxTransformacao();
                }
            }
            else if (e.Key == Key.Up)
            {
                nave.TranslacaoXYZ(0, 0, -2);
                nave.atualizaBBoxTransformacao();
            }
            else if (e.Key == Key.Down)
            {
                nave.TranslacaoXYZ(0, 0, 2);
                nave.atualizaBBoxTransformacao();
            }
            else if (objetoSelecionado != null)
            {
                if (e.Key == Key.M)
                    Console.WriteLine(objetoSelecionado.Matriz);
                else if (e.Key == Key.P)
                    Console.WriteLine(objetoSelecionado);
                else if (e.Key == Key.I)
                    objetoSelecionado.AtribuirIdentidade();


                else if (e.Key == Key.Number8)
                    objetoSelecionado.TranslacaoXYZ(0, 1, 0);
                else if (e.Key == Key.Number9)
                    objetoSelecionado.TranslacaoXYZ(0, -1, 0);
                else if (e.Key == Key.PageUp)
                    objetoSelecionado.EscalaXYZ(2, 2, 2);
                else if (e.Key == Key.PageDown)
                    objetoSelecionado.EscalaXYZ(0.5, 0.5, 0.5);
                else if (e.Key == Key.Home)
                    objetoSelecionado.EscalaXYZBBox(0.5, 0.5, 0.5);
                else if (e.Key == Key.End)
                    objetoSelecionado.EscalaXYZBBox(2, 2, 2);
                else if (e.Key == Key.Number1)
                    objetoSelecionado.Rotacao(10, menuEixoSelecao);
                else if (e.Key == Key.Number2)
                    objetoSelecionado.Rotacao(-10, menuEixoSelecao);
                else if (e.Key == Key.Number3)
                    objetoSelecionado.RotacaoZBBox(10, menuEixoSelecao);
                else if (e.Key == Key.Number4)
                    objetoSelecionado.RotacaoZBBox(-10, menuEixoSelecao);
                else if (e.Key == Key.Number9)
                    objetoSelecionado = null;                     // desmacar objeto selecionado
                else
                    Console.WriteLine(" __ Tecla não implementada.");
            }
            else
                Console.WriteLine(" __ Tecla não implementada.");
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
            // GL.Color3(0.0f,0.0f,1.0f);
            GL.Color3(Convert.ToByte(0), Convert.ToByte(0), Convert.ToByte(255));
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 0, 200);
            GL.End();
        }
#endif

        private void gerarT(object source, ElapsedEventArgs e)
        {
            if (tanques.Count < 30)
            {
                float novaPosicao = 0.0f;
                int posicao = 0;

                do
                {
                    Random r = new Random();
                    posicao = r.Next(1, 50);
                    novaPosicao = (posicao * 3f) - 1.2f;
                } while (posicoes.Contains(posicao) && posicoes.Count < 30);
                posicoes.Add(posicao);

                Random rd = new Random();
                if (rd.NextDouble() < 0.2)
                {
                    objetoId = Utilitario.charProximo(objetoId);
                    TQ obj_TQ = new TQ('a', null, new Ponto4D(novaPosicao, 0.0f, 100.0f), true);
                    obj_TQ.Posicao = posicao;
                    objetosLista.Add(obj_TQ);
                    tanques.Add(obj_TQ);
                    obj_TQ.RotacaoZBBox(180, 'y');
                }
                else
                {
                    objetoId = Utilitario.charProximo(objetoId);
                    TQ obj_TQ = new TQ('a', null, new Ponto4D(novaPosicao, 0.0f, 100.0f), false);
                    obj_TQ.Posicao = posicao;
                    objetosLista.Add(obj_TQ);
                    tanques.Add(obj_TQ);
                    obj_TQ.RotacaoZBBox(180, 'y');
                }

            }


        }



        private void moverT(object source, ElapsedEventArgs e)
        {
            foreach (TQ tanque in tanques)
            {
                if (!(tanque.BBox.obterCentro.Z < 0))
                {
                    tanque.TranslacaoXYZ(0, 0, -0.5);
                    ObjetoGeometria tq = (ObjetoGeometria)tanque;
                    tq.atualizaBBoxTransformacao();

                }
                else
                {
                    tanque.mirar(centros);

                    if (((Nave)naves[0].ObjetosLista[1]).Cor.CorR < 255)
                    {
                        ((Nave)naves[0].ObjetosLista[1]).Cor.CorR += 1;
                    }
                    else if (((Nave)naves[0].ObjetosLista[1]).Cor.CorG != 0)
                    {
                        ((Nave)naves[0].ObjetosLista[1]).Cor.CorG -= 1;
                    }
                    else
                    {
                        foreach (Nave nave in naves)
                        {
                            nave.Cor.CorR = 33;
                            nave.Cor.CorG = 33;
                            nave.Cor.CorB = 33;
                            ((Nave)nave.ObjetosLista[0]).Cor.CorR = 33;
                            ((Nave)nave.ObjetosLista[0]).Cor.CorG = 33;
                            ((Nave)nave.ObjetosLista[0]).Cor.CorB = 33;
                            ((Nave)nave.ObjetosLista[1]).Cor.CorR = 33;
                            ((Nave)nave.ObjetosLista[1]).Cor.CorG = 33;
                            ((Nave)nave.ObjetosLista[1]).Cor.CorB = 33;
                        }


                        // objetosLista.Remove(naves[0]);
                        // naves.RemoveAt(0);
                        movedorT.Enabled = false;
                    }

                }
            }
        }

        private void ativaPoder(object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < tanques.Count; i++)
            {
                Ponto4D centro = tanques[i].BBox.centro;
                if (centro.Z < 3)
                {
                    centro.Y = 101;
                    Nave raio = new Nave('a', null, 50, 2, -100, new Cor(128, 0, 255), centro, 3);
                    raios.Add(raio);
                    objetosLista.Add(raio);
                    objetosLista.Remove((Objeto)tanques[i]);
                    posicoes.Remove(((TQ)tanques[i]).Posicao);
                    tanques.Remove(tanques[i]);

                }
            }

            this.poder.Enabled = false;
        }

        private void limpaR(object source, ElapsedEventArgs e)
        {
            foreach (Nave raio in raios)
            {
                objetosLista.Remove(raio);
            }
            raios.Clear();
        }
    }




    class Program
    {
        static void Main(string[] args)
        {
            Mundo window = Mundo.GetInstance(600, 600);
            window.Title = "CG_N4";
            window.Run(1.0 / 60.0);
        }
    }
}
