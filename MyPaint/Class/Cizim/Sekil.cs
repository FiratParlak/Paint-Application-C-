using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPaint
{
    class Sekil : Arac
    {
        

        private Rectangle DiktortgenGetir(Point p1, Point p2)
        {// Dikdörtgenin matematiksel kontrolleri
            Rectangle rect = new Rectangle();
            if (p1.X < p2.X)
            {
                rect.X = p1.X;
                rect.Width = p2.X - p1.X;
            }
            else
            {
                rect.X = p2.X;
                rect.Width = p1.X - p2.X;
            }
            if (p1.Y < p2.Y)
            {
                rect.Y = p1.Y;
                rect.Height = p2.Y - p1.Y;
            }
            else
            {
                rect.Y = p2.Y;
                rect.Height = p1.Y - p2.Y;
            }

            return rect;
        }

        private void SekilCiz(CalismaAlani w)
        {
            if (w.SeciliSekil == (int)Sekiller.Dikdortgen)
            {
                if (w.SekilDoldur)
                    w.grafik.FillRectangle(w.firca, DiktortgenGetir(TiklananNokta, MouseKonumu));
                else
                    w.grafik.DrawRectangle(w.kalem, DiktortgenGetir(TiklananNokta, MouseKonumu));
            }
        }

        private void CizgiCiz(CalismaAlani w, Point start, Point end)
        {
            w.grafik.Clear(Color.Transparent);
            w.grafik.DrawImage(w.temp, 0, 0);
            SekilCiz(w);
        }

        public override void OnMouseUp(MouseEventArgs e, CalismaAlani w)
        {
            base.OnMouseUp(e, w);
            SekilCiz(w);
        }

        public override void OnMouseMove(MouseEventArgs e, CalismaAlani w)
        {
            base.OnMouseMove(e, w);
            if (CizimVarMi)
            {
                CizgiCiz(w, TiklananNokta, MouseKonumu);
            }
        }

    }
}
