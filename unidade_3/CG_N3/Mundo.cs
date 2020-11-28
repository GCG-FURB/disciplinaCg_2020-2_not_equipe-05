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

        private CameraOrtho camera = new CameraOrtho();
        protected List<Objeto> objetosLista = new List<Objeto>();
        private ObjetoGeometria objetoSelecionado = null;
        private char objetoId = '@';
        private bool bBoxDesenhar = true;
        int mouseX, mouseY;   //TODO: achar método MouseDown para não ter variável 

        //Bool para controlar se está selecionando o vertice
        private bool selecionandoVertice = false;

        //Indice do vertice selecionado na lista de pontos do objeto
        private int verticeSelecionado = 0; 

        //Objeto que está sendo criado (barra de espaço)
        private Poligono objetoNovo = null;
#if CG_Privado
    private Retangulo obj_Retangulo;
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;
#endif

        //Algoritmo ScanLine
        private bool scanLine(Objeto o)
        {
            // Console.WriteLine("Dentro da bbox");
            ObjetoGeometria og = (ObjetoGeometria)o;
            int nInt = 0;
            int nTi = 0;
            for (int i = 0; i <= og.PontosLista.Count - 2; i++)
            {
                double yI = mouseY;
                double y1 = og.PontosLista[i].Y;
                double y2 = og.PontosLista[i + 1].Y;
                double ti = (yI - y1) / (y2 - y1);

                // Console.WriteLine(ti);
                if (ti >= 0 && ti <= 1)
                {
                    nTi++;

                    double x1 = og.PontosLista[i].X;
                    double x2 = og.PontosLista[i + 1].X;
                    double xi = x1 + ((x2 - x1) * ti);
                    if (xi > mouseX)
                    {

                        nInt++;
                    }
                }
            }
            double yIUltimo = mouseY;
            double y1Ultimo = og.PontosLista[og.PontosLista.Count - 1].Y;
            double y2Ultimo = og.PontosLista[0].Y;
            double tiUltimo = ((yIUltimo - y1Ultimo) / (y2Ultimo - y1Ultimo));
            // Console.WriteLine(tiUltimo);
            if (tiUltimo >= 0 && tiUltimo <= 1)
            {
                nTi++;
                double x1 = og.PontosLista[og.PontosLista.Count - 1].X;
                double x2 = og.PontosLista[0].X;
                double xi = x1 + ((x2 - x1) * tiUltimo);
                if (xi > mouseX)
                {
                    nInt++;
                }
            }
            // Console.WriteLine(nTi);


            // Console.WriteLine(nInt);
            if (nInt % 2 != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Checa se selecionou algum dos filhos (auxiliar da scanline para funcionar com grafo de cena)
        private Objeto procuraFilhos(Objeto o)
        {
            if (mouseX >= o.BBox.obterMenorX &&
                mouseX <= o.BBox.obterMaiorX &&
                mouseY >= o.BBox.obterMenorY &&
                mouseY <= o.BBox.obterMaiorY) //Dentro da bbox
            {
                if (scanLine(o))
                {
                    return o;
                }
            }
            foreach (Objeto filho in o.ObjetosLista)
            {
                Objeto possivelFilho = procuraFilhos(filho);
                if (possivelFilho != null)
                {
                    return possivelFilho;
                }
            }
            return null;
        }

        //Lista os filhos (tecla E)
        private void listaFilhos(Objeto o)
        {

            if (o.ObjetosLista.Count != 0)
            {
                Console.WriteLine("Filhos de " + o.Rotulo);
                foreach (Objeto filho in o.ObjetosLista)
                {
                    Console.WriteLine(filho);
                }
                foreach (Objeto filho in o.ObjetosLista)
                {
                    listaFilhos(filho);
                }


            }

        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            camera.xmin = 0; camera.xmax = 600; camera.ymin = 0; camera.ymax = 600;

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");

            // objetoId = Utilitario.charProximo(objetoId);
            // objetoNovo = new Poligono(objetoId, null);
            // objetosLista.Add(objetoNovo);
            // objetoNovo.PontosAdicionar(new Ponto4D(50, 350));
            // objetoNovo.PontosAdicionar(new Ponto4D(150, 350));  // N3-Exe6: "troque" para deixar o rastro
            // objetoNovo.PontosAdicionar(new Ponto4D(100, 450));
            // objetoSelecionado = objetoNovo;
            // objetoNovo = null;

#if CG_Privado
      obj_Retangulo = new Retangulo(objetoId + 1, null, new Ponto4D(50, 50, 0), new Ponto4D(150, 150, 0));
      obj_Retangulo.ObjetoCor.CorR = 255; obj_Retangulo.ObjetoCor.CorG = 0; obj_Retangulo.ObjetoCor.CorB = 255;
      objetosLista.Add(obj_Retangulo);
      objetoSelecionado = obj_Retangulo;

      obj_SegReta = new Privado_SegReta(objetoId + 1, null, new Ponto4D(50, 150), new Ponto4D(150, 250));
      obj_SegReta.ObjetoCor.CorR = 255; obj_SegReta.ObjetoCor.CorG = 99; obj_SegReta.ObjetoCor.CorB = 71;
      objetosLista.Add(obj_SegReta);
      objetoSelecionado = obj_SegReta;

      obj_Circulo = new Privado_Circulo(objetoId + 1, null, new Ponto4D(100, 300), 50);
      obj_Circulo.ObjetoCor.CorR = 177; obj_Circulo.ObjetoCor.CorG = 166; obj_Circulo.ObjetoCor.CorB = 136;
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

            //Lista polígonos e seus respectivos filhos
            else if (e.Key == Key.E)
            {
                Console.WriteLine("--- Objetos / Pontos: ");
                for (var i = 0; i < objetosLista.Count; i++)
                {
                    Console.WriteLine(objetosLista[i]);
                    listaFilhos(objetosLista[i]);
                }
            }
            //Transforma o poligono selecionado em aberto/fechado
            else if (e.Key == Key.A)
            {
                if (objetoSelecionado != null)
                {
                    Poligono p = (Poligono)objetoSelecionado;
                    p.mudaPrimitiva();//Método que muda para aberto/fechado
                }

            }
            //Selecionar polígono
            else if (e.Key == Key.S)
            {
                //Limpa o selecionado
                objetoSelecionado = null;

                //Para cada objeto no mundo
                foreach (Objeto o in objetosLista)
                {
                    //Procura nele e nos filhos dele se foi selecionado
                    Objeto possivelSelecionado = procuraFilhos(o);

                    //Procura filhos retorna null caso não há filho selecionado
                    //Se não for null... achou um selecionado
                    if (possivelSelecionado != null)
                    {                        
                        objetoSelecionado = (ObjetoGeometria)possivelSelecionado;
                        //Como achou um selecionado, sai do loop
                        break;
                    }
                }

            }
            //Desenha bbox do objeto selecionado
            else if (e.Key == Key.O)
                bBoxDesenhar = !bBoxDesenhar;
            //Termina a criação do polígono
            else if (e.Key == Key.Enter)
            {
                //Tem que estar criando um polígono (ter usado a barra de espaço antes)
                if (objetoNovo != null)
                {
                    //Remove o último ponto (pois este é o rastro)
                    objetoNovo.PontosRemoverUltimo();   // N3-Exe6: "truque" para deixar o rastro

                    //Se tiver um objeto já selecionado, este será o pai do objeto novo
                    if (objetoSelecionado != null)
                    {
                        //Adiciona o filho
                        objetoSelecionado.FilhoAdicionar(objetoNovo);
                        //Diz que o selecionado é o pai do novo
                        objetoNovo.Pai = objetoSelecionado;

                        //Remove o novo da lista do mundo, pois ele pertence à lista de objetos do objeto pai
                        objetosLista.RemoveAt(objetosLista.Count - 1);
                    }
                    //Novo é o novo selecionado
                    objetoSelecionado = objetoNovo;
                    //Não está mais criando um novo, logo novo = null
                    objetoNovo = null;
                }
            }
            //Inicia a criação de um polígono
            else if (e.Key == Key.Space)
            {   
                //Se não há objeto sendo adicionado
                if (objetoNovo == null)
                {
                    //Cria-se um novo objeto
                    objetoId = Utilitario.charProximo(objetoId);
                    objetoNovo = new Poligono(objetoId, null);

                    //Adiciona-o na lista do mundo
                    objetosLista.Add(objetoNovo);

                    //Cria um ponto onde o mouse está apontando
                    objetoNovo.PontosAdicionar(new Ponto4D(mouseX, mouseY));

                    //Cria outro ponto para ser o "rastro", este ponto estara à todo momento
                    //Sendo atualizado para a posição atual do mouse até que outro ponto seja adicionado
                    objetoNovo.PontosAdicionar(new Ponto4D(mouseX, mouseY));  // N3-Exe6: "troque" para deixar o rastro
                }
                else
                    //Caso já esteja no processo de criação, adiciona outro ponto
                    objetoNovo.PontosAdicionar(new Ponto4D(mouseX, mouseY));
            }
            //Objeto selecionado não pode estar null para as próximas teclas e funções
            else if (objetoSelecionado != null)
            {
                //Mostra a matriz de transformação atual do objeto selecionado
                if (e.Key == Key.M)
                    Console.WriteLine(objetoSelecionado.Matriz);
                
                //Remove o vértice que está sendo movimentado
                else if (e.Key == Key.D)
                {
                    //Têm que estar selecionando o vértice
                    if (selecionandoVertice)
                    {
                        //Transforma em polígono para poder remover o ponto
                        Poligono p = (Poligono)objetoSelecionado;

                        //Remove o ponto na lista de pontos do objeto, localizado no indice do vertice selecionado
                        p.removerPonto(verticeSelecionado);

                        //Se só existe um ponto restante no polígono, ele é removido
                        if (p.getPontos().Count == 1)
                        {
                            p.removerPonto(0);
                            objetosLista.Remove(objetoSelecionado);
                            objetoSelecionado = null;
                        }
                        //Sinaliza que não está mais selecionando o vertice
                        selecionandoVertice = !selecionandoVertice;
                    }

                }
                //Muda para vermelho a cor do objeto selecionado
                else if (e.Key == Key.R)
                {
                    objetoSelecionado.ObjetoCor.CorR = 255;
                    objetoSelecionado.ObjetoCor.CorG = 0;
                    objetoSelecionado.ObjetoCor.CorB = 0;
                }
                //Muda para verde a cor do objeto selecionado
                else if (e.Key == Key.G)
                {
                    objetoSelecionado.ObjetoCor.CorR = 0;
                    objetoSelecionado.ObjetoCor.CorG = 255;
                    objetoSelecionado.ObjetoCor.CorB = 0;
                }
                //Muda para azul a cor do objeto selecionado
                else if (e.Key == Key.B)
                {
                    objetoSelecionado.ObjetoCor.CorR = 0;
                    objetoSelecionado.ObjetoCor.CorG = 0;
                    objetoSelecionado.ObjetoCor.CorB = 255;
                }
                //To string do objeto selecionado
                else if (e.Key == Key.P)
                    Console.WriteLine(objetoSelecionado);
                //Atribui identidade à matriz de transformação do objeto selecionado
                //Efetivamente zerando suas tansformações, resetando-o para o ponto original
                else if (e.Key == Key.I)
                    objetoSelecionado.AtribuirIdentidade();
                //Deletar polígono selecionado
                else if (e.Key == Key.Delete)
                {

                    //Se tiver filhos
                    if (objetoSelecionado.ObjetosLista.Count != 0)
                    {
                        //Remove-os também
                        objetoSelecionado.FilhoRemoverTodos();

                    }

                    //Se tiver pai, remove o filho da lista do pai
                    if (objetoSelecionado.Pai != null)
                    {
                        objetoSelecionado.Pai.FilhoRemover(objetoSelecionado);
                    }

                    //Remove o objeto selecionado
                    objetosLista.Remove(objetoSelecionado);
                    objetoSelecionado = null;
                }
                //TODO: não está atualizando a BBox com as transformações geométricas
                else if (e.Key == Key.Left)
                    objetoSelecionado.TranslacaoXYZ(-10, 0, 0);
                else if (e.Key == Key.Right)
                    objetoSelecionado.TranslacaoXYZ(10, 0, 0);
                else if (e.Key == Key.Up)
                    objetoSelecionado.TranslacaoXYZ(0, 10, 0);
                else if (e.Key == Key.Down)
                    objetoSelecionado.TranslacaoXYZ(0, -10, 0);
                else if (e.Key == Key.PageUp)
                    objetoSelecionado.EscalaXYZ(2, 2, 2);
                else if (e.Key == Key.PageDown)
                    objetoSelecionado.EscalaXYZ(0.5, 0.5, 0.5);
                else if (e.Key == Key.Home)
                    objetoSelecionado.EscalaXYZBBox(0.5, 0.5, 0.5);
                else if (e.Key == Key.End)
                    objetoSelecionado.EscalaXYZBBox(2, 2, 2);
                else if (e.Key == Key.Number1)
                    objetoSelecionado.Rotacao(10);
                else if (e.Key == Key.Number2)
                    objetoSelecionado.Rotacao(-10);
                else if (e.Key == Key.Number3)
                    objetoSelecionado.RotacaoZBBox(10);
                else if (e.Key == Key.Number4)
                    objetoSelecionado.RotacaoZBBox(-10);
                else if (e.Key == Key.Number9)
                    objetoSelecionado = null;                     // desmacar objeto selecionado
                else
                    Console.WriteLine(" __ Tecla não implementada.");
            }
            else
                Console.WriteLine(" __ Tecla não implementada.");
        }

        //TODO: não está considerando o NDC
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            mouseX = e.Position.X; mouseY = 600 - e.Position.Y; // Inverti eixo Y

            //Se estiver adicionando um objeto novo
            if (objetoNovo != null)
            {
                //O ultimo ponto deste objeto é o rastro
                objetoNovo.PontosUltimo().X = mouseX;
                objetoNovo.PontosUltimo().Y = mouseY;
            }
            //Se houver um objeto selecionado e estiver selecionando o vértice
            if (objetoSelecionado != null && selecionandoVertice)
            {
                Poligono p = (Poligono)objetoSelecionado;

                //O ponto da lista de pontos localizado no indice "verticeSelecionado" é setado para seguir o mouse
                p.getPontos()[verticeSelecionado].X = mouseX;
                p.getPontos()[verticeSelecionado].Y = mouseY;
            }
        }

        //No click do mouse
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            mouseX = e.Position.X; mouseY = 600 - e.Position.Y;

            //Se houver objeto selecionado e não estiver selecionando um vertice
            if (objetoSelecionado != null && !selecionandoVertice)
            {
                //Sinaliza que está selecionando um vertice
                selecionandoVertice = !selecionandoVertice;

                Poligono p = (Poligono)objetoSelecionado;

                //Calcula o vertice mais proximo do mouse por distancia euclidiana

                double distMaisProx = Double.MaxValue; //Distancia do mais próximo é máxima, sendo assim o primeiro vertice 
                //será o mais proximo

                int indiceMaisProx = 0; //Guarda o indice do vertice mais proximo

                //Variavel de controle do indice
                int i = 0;
                //Para cada ponto do poligono
                foreach (Ponto4D pto in p.getPontos())
                {
                    //X2 - X1 
                    double distanciax = mouseX - pto.X;
                    //Ao quadrado
                    distanciax = distanciax * distanciax;

                    //Y2 - Y1
                    double distanciay = mouseY - pto.Y;
                    //Ao quadrado
                    distanciay = distanciay * distanciay;

                    //Somam-se as distâncias
                    double distancia = distanciax + distanciay;
                    
                    //Se a sua distancia for mais proxima do mouse que o ponto mais proximo
                    if (distancia < distMaisProx)
                    {
                        //Este é o mais proximo
                        distMaisProx = distancia;

                        //Guarda-se o indice do mais proximo para depois
                        indiceMaisProx = i;
                    }
                    i++;

                }
                //Seta a variavel global do vertice selecionado
                verticeSelecionado = indiceMaisProx;


            }
            //Se houver um vertice selecionado e estiver selecionando o vertice
            //Quer dizer que está sendo atualizado a posicao do vertice (clicando para "fixá-lo")
            else if (objetoSelecionado != null && selecionandoVertice)
            {
                //Sinaliza que não está mais selecionando o vertice
                selecionandoVertice = !selecionandoVertice;
                Poligono p = (Poligono)objetoSelecionado;

                //Atualiza sua bbox pois um ponto mudou de posicao
                p.atualizaBBox();
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
            // GL.Color3(0.0f,0.0f,1.0f);
            GL.Color3(Convert.ToByte(0), Convert.ToByte(0), Convert.ToByte(255));
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 0, 200);
            GL.End();
        }
#endif
    }
    class Program
    {
        static void Main(string[] args)
        {
            Mundo window = Mundo.GetInstance(600, 600);
            window.Title = "CG_N3";
            window.Run(1.0 / 60.0);
        }
    }
}
