using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPaint
{
    class Kalem : Arac // Kalıtım
    {

        GraphicsPath gp = new GraphicsPath();
        private Point SonNokta;

        public void CizimYap(Pen pen, Graphics g)
        {
            gp.Widen(pen); 
            gp.Flatten();
            g.DrawPath(pen, gp);
            gp.Reset();
        }

        public override void OnMouseDown(MouseEventArgs e, CalismaAlani w)
        {
            base.OnMouseDown(e, w);//Araçtaki MouseDown cagırır(Kalıtım)...
            SonNokta = MouseKonumu;
        }

        public override void OnMouseMove(MouseEventArgs e, CalismaAlani w)
        {
            base.OnMouseMove(e, w);
            if (CizimVarMi)
            {
                if (SonNokta != MouseKonumu)
                {
                    gp.AddLine(MouseKonumu, SonNokta);
                    SonNokta = MouseKonumu; //Son noktayı güncelle
                    CizimYap(w.kalem, w.grafik);
                }
            }
        }
    }
}
