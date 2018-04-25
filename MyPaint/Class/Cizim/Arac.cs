using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPaint
{
    class Arac
    {
        public Point MouseKonumu; // şuanki mouse konumu
        public Point TiklananNokta; // ilk tiklanan nokta
        public bool CizimVarMi; // kontroller

        public virtual void Update(MouseEventArgs M) //Mouse koordinatını günceller
        {
            MouseKonumu = M.Location;
        }

        public virtual void OnMouseMove(MouseEventArgs e, CalismaAlani w) //Mousedan elimizi kaldırmadan hareket halideyken yapılan olay...
        {
            Update(e);
        }

        public virtual void OnMouseUp(MouseEventArgs e, CalismaAlani w) //Mouseden elimiz kaldırıldıgında...
        {
            CizimVarMi = false;
        }

        public virtual void OnMouseDown(MouseEventArgs e, CalismaAlani w) // Mouse tıklandıgında...
        {
            TiklananNokta = e.Location; //bir tık koyun
            CizimVarMi = true;
            // çizim tipini ayarla(yumuşak,sert)...
            w.grafik.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                   
            Update(e);
        }
    }
}
