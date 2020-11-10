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
        private Camera cameraTeclado;

        private Circulo circuloMenor;
        private Circulo circuloMaior;

        private Retangulo retangulo;
        private Ponto pto;
        private Ponto4D centro1;
        private Ponto4D centro2;

        protected List<Objeto> objetosLista = new List<Objeto>();
        private ObjetoGeometria objetoSelecionado = null;
        private bool bBoxDesenhar = false;
        int mouseX, mouseY;   //TODO: achar método MouseDown para não ter variável Global
        private bool mouseMoverPto = false;

        private bool trancar = false;
        private bool clickMouse = false;
        private Retangulo obj_Retangulo;
#if CG_Privado
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;
#endif

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            cameraTeclado = new Camera(camera);
            camera.xmin = 0; camera.xmax = 600; camera.ymin = 0; camera.ymax = 600;

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");

            //Dois centros são criados
            centro1 = new Ponto4D(300, 300, 0, 0);
            centro2 = new Ponto4D(300, 300, 0, 0);

            //Circulo menor com centro 1
            circuloMenor = new Circulo("cmenor", null, centro1, 50);
            circuloMenor.ObjetoCor.CorR = 0; circuloMenor.ObjetoCor.CorG = 0; circuloMenor.ObjetoCor.CorB = 0;
            objetosLista.Add(circuloMenor);

            //Circulo maior com centro 2
            circuloMaior = new Circulo("cmaior", null, centro2, 200);
            circuloMaior.ObjetoCor.CorR = 0; circuloMaior.ObjetoCor.CorG = 0; circuloMaior.ObjetoCor.CorB = 0;
            objetosLista.Add(circuloMaior);


            //Descobre-se o ponto referente ao grau 225° do circulo (na posição de referencia 0,0)
            Ponto4D temp1 = Matematica.GerarPtosCirculo(225, 200);

            //É somado a posicao atual do centro
            temp1.X += centro2.X;
            temp1.Y += centro2.Y;

            //Idem, porém com o grau 45
            Ponto4D temp2 = Matematica.GerarPtosCirculo(45, 200);
            temp2.X += centro2.X;
            temp2.Y += centro2.Y;

            //Deste jeito é criado um retangulo com o pontoInfEsq sendo o ponto referente ao angulo 225 do circulo maior
            //E o pontoSupDir sendo o ponto referente ao angulo 45 do circulo maior
            retangulo = new Retangulo("r1", null, temp1, temp2);
            retangulo.ObjetoCor.CorR = 255; retangulo.ObjetoCor.CorG = 0; retangulo.ObjetoCor.CorB = 255;
            objetosLista.Add(retangulo);


            //Debug
            // Console.WriteLine(circuloMaior.BBox.obterCentro);
            // Console.WriteLine(circuloMaior.BBox.obterMenorX);
            // Console.WriteLine(circuloMaior.BBox.obterMaiorX);
            
            //Cria-se um novo objeto da classe ponto com o mesmo ponto do centro do círculo menor
            //Desta maneira, ao movimentar este ponto, o circulo menor tbm é movimentado
            pto = new Ponto("pto1", null, centro1, 10);
            pto.ObjetoCor.CorR = 0; pto.ObjetoCor.CorG = 0; pto.ObjetoCor.CorB = 0;
            objetosLista.Add(pto);

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
            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
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

            //Parte principal onde as distancias são calculadas
            //Se o centro do círculo está abaixo do menor y,
            //acima do maior y,
            //à esquerda do menor x,
            //à direita do maior x,
            //Então ele está fora do retângulo
            if (centro1.X < retangulo.BBox.obterMenorX ||
                centro1.Y < retangulo.BBox.obterMenorY ||
                centro1.X > retangulo.BBox.obterMaiorX ||
                centro1.Y > retangulo.BBox.obterMaiorY){

                    //Muda cor do retangulo para amarelo
                    retangulo.ObjetoCor.CorR = 255; retangulo.ObjetoCor.CorG = 255; retangulo.ObjetoCor.CorB = 0;

                    //É realizado o calculo de distancia euclidiana

                    //X2 - X1 
                    double distanciax = centro1.X - centro2.X;
                    //Ao quadrado
                    distanciax = distanciax * distanciax;
                    
                    //Y2 - Y1
                    double distanciay = centro1.Y - centro2.Y;
                    //Ao quadrado
                    distanciay = distanciay * distanciay;

                    //Somam-se as distâncias
                    double distancia = distanciax + distanciay;
                    
                    //Se a distância for maior que o quadrado do raio do circulo maior
                    //Está fora do circulo
                    if(distancia > 200*200){
                        
                        //Cor do retangulo é mudada para ciano
                        retangulo.ObjetoCor.CorR = 0; retangulo.ObjetoCor.CorG = 255; retangulo.ObjetoCor.CorB = 255;

                        //Variável que controla se o circulo está fora, e consequentemente é trancado seu movimento
                        //Até soltar o botão do mouse, onde então, o circulo volta à posição inicial
                        trancar = true;
                    }
                }else{
                    //Caso não estiver fora do retangulo, a cor é mudada para magenta (pois o circulo apesar de fora pode ainda voltar para dentro)
                    retangulo.ObjetoCor.CorR = 255; retangulo.ObjetoCor.CorG = 0; retangulo.ObjetoCor.CorB = 255;
                }



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

       
        //Na movimentação do mouse (método de evento)
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            //Verifica se o mouse está pressionado e checa se esta dentro da area de proximidade para agarrar
            //A área de proximidade é de 20x20 (para não ser desconfortável tentar acertar o ponto exatamente na posicao de agarrar)
            if (clickMouse && trancar == false &&
            (e.X > (centro1.X - 20)) &&
            (e.X < (centro1.X + 20)) &&
            (600 - e.Y < (centro1.Y + 20)) &&
            (600 - e.Y > (centro1.Y - 20)))
            {
                //Se verdadeiro, o ponto central do circulo menor é ajustado para a mesma posicao do mouse
                centro1.X = e.X;

                //Invertido o Y, pois o evento que gera a pos do mouse possui o Y invertido (comecando no canto superior esquerdo da tela)
                centro1.Y = 600 - e.Y;
            }
            
        }

        //Evento que captura quando o mouse foi pressionado
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            clickMouse = e.IsPressed;//Bool usada para sinalizar pressionamento do mouse
            // Console.WriteLine("X do centro " + centro.X); //Debug

        }

        //Evento que captura quando o mouse foi solto/despressionado
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            //Trancar é a variavel que é ativada quando o circulo passa dos limites
            //Se isso for verdadeiro, o circulo volta à posição inicial
            if(trancar == true){
                trancar = false;
                centro1.X = 300;
                centro1.Y = 300;
            }
            //Atualiza a bool de pressionamento
            clickMouse = e.IsPressed;//Bool usada para sinalizar pressionamento do mouse

        }



#if CG_Gizmo
        private void Sru3D()
        {
            GL.LineWidth(2);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
            GL.Vertex3(20, 20, 0); GL.Vertex3(200, 20, 0);
            GL.Color3(Convert.ToByte(0), Convert.ToByte(255), Convert.ToByte(0));
            GL.Vertex3(20, 20, 0); GL.Vertex3(20, 200, 0);
            GL.End();
        }
#endif

    }



    class Program
    {
        static void Main(string[] args)
        {
            Mundo window = Mundo.GetInstance(600, 600);
            window.Title = "CG_N2_E7";
            window.Run(1.0 / 60.0);
        }
    }
}
