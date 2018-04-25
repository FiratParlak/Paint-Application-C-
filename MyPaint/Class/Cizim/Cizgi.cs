using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPaint
{
    class Cizgi : Arac
    {
        private void CizgiCiz(CalismaAlani w, Point basla, Point son)
        {
            w.grafik.Clear(Color.Transparent);
            w.grafik.DrawImage(w.temp, 0, 0);
            w.grafik.DrawLine(w.kalem, basla, son);
        }

        public override void OnMouseUp(MouseEventArgs e, CalismaAlani w)
        {
            base.OnMouseUp(e, w);//Araçtaki çizimi false yapıyor(Kalıtım)...
            w.grafik.DrawLine(w.kalem, TiklananNokta, MouseKonumu);//Baştan çizim gerçekleştiriyor...
        }

        public override void OnMouseMove(MouseEventArgs e, CalismaAlani w)
        {
            base.OnMouseMove(e, w);//Araçtaki çizimi false yapıyor(Kalıtım)...
            if (CizimVarMi)
            {
                CizgiCiz(w, TiklananNokta, MouseKonumu);
            }
        }
    }
}
