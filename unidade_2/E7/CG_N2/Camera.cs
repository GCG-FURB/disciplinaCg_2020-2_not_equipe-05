using System;
using CG_Biblioteca;
using OpenTK.Input;

namespace CG_N2
{
    public class Camera
    {
        private CameraOrtho cameraOrtho;

        public Camera(CameraOrtho c)
        {
            this.cameraOrtho = c;
        }

        public void ZoomIn()
        {
            cameraOrtho.ZoomIn();
        }

        public void ZoomOut()
        {
            cameraOrtho.ZoomOut();
        }

        public void PanEsquerda()
        {
            cameraOrtho.PanEsquerda();

        }

        public void PanDireita()
        {
            cameraOrtho.PanDireita();

        }

        public void PanCima()
        {
            cameraOrtho.PanCima();

        }

        public void PanBaixo()
        {
            cameraOrtho.PanBaixo();

        }


    }
}
