using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPaint
{
    public partial class MyPaint : Form
    {
        public CalismaAlani calismaAlani;
        public Bitmap ana_resim;
        Graphics g;
        List<Arac> araclar = new List<Arac>();

        public MyPaint()
        {
            InitializeComponent();

           
            lstKatman_SelectedIndexChanged(null, null); // katman eklemek
            calismaAlani.kalem = new Pen(Color.Black); // kalem olustur

            RenkAyarla(Color.Black); //rengi ayarlanır

            // araclari ekledik
            araclar.Insert((int)BoyamaTipi.Kalem, new Kalem());
            araclar.Insert((int)BoyamaTipi.Cizgi, new Cizgi());
            araclar.Insert((int)BoyamaTipi.Sekil, new Sekil());


           // araclar.Insert((int)BoyamaTipi.Silgi, new Silgi());
           
        }
        // calismaAlani.firca = new SolidBrush(Color.Black); // firca olustur
        public void CalismaAlaniOlustur(int width, int height)
        {
            if (lstKatman.Items.Count > 0)
                TumKatmanlariSil();

            ana_resim = new Bitmap(width, height);//Ana resim boyutu ayarlıyoruz...
            calismaAlani.temp = new Bitmap(width, height);//Ana resim boyutu Temp için alıyoruz...
            lstKatman.Clear();

            calismaAlani.Height = height;
            calismaAlani.Width = width;

            // Katman oluştur
            ListViewItem li = new ListViewItem("Katman 1");
            li.Checked = true;
            li.Selected = true;
            Bitmap bmp = new Bitmap(width, height);//Katman için resim oluşturulur...
            calismaAlani.grafik = Graphics.FromImage(bmp);//KAtmandaki resmi çalışma alanına yükle...
            calismaAlani.grafik.Clear(Color.White);
            li.Tag = bmp;//etiketine resim atıyor...
            lstKatman.Items.Add(li);//Bildigimiz gibi ekleme yapılıyor...

            g = Graphics.FromImage(ana_resim);//Grafimize ana resim ekleniyor...
            pbTuval.Image = ana_resim;//Tuvalimizin resmi ana resim oluyor...

            TekrarCiz();
            pbTuval.Show();

            btnKalem_Click(null, null);//Başlangıçta kalem seçili olarak gelir...
        }

        #region Boyama Araçları
        private void Dortgen_Click(object sender, EventArgs e)//içi boş  dikdorrtgen cizdiriri...
        {
            SilgiAyarla(false);

            calismaAlani.SeciliArac = (int)BoyamaTipi.Sekil;
            calismaAlani.SeciliSekil = (int)Sekiller.Dikdortgen;
            calismaAlani.SekilDoldur = false;
        }

        private void btnDoluDikdortgen_Click_1(object sender, EventArgs e)
        {
            SilgiAyarla(false);

            calismaAlani.SeciliArac = (int)BoyamaTipi.Sekil;
            calismaAlani.SeciliSekil = (int)Sekiller.Dikdortgen;
            calismaAlani.SekilDoldur = true;
        }

        private void btnKalem_Click(object sender, EventArgs e)
        {
            SilgiAyarla(false);

            calismaAlani.SeciliArac = (int)BoyamaTipi.Kalem;//Boyama tipi(enum) araçı olarak kalemi seçtik...
        }

        private void btnFirca_Click(object sender, EventArgs e)
        {
            SilgiAyarla(false);

            calismaAlani.SeciliArac = (int)BoyamaTipi.Cizgi;//Boyama tipi(enum) araçı olarak çizgiyi seçtik...
        }

        private void KalemKalinligi_ValueChanged(object sender, EventArgs e)
        {
            calismaAlani.kalem.Width = (float)KalemKalinligi.Value;//Kalem kalınlını ayarlıyoruz...
        }


        #endregion

        #region Tuval Olayları
        private void pbTuval_MouseDown(object sender, MouseEventArgs e)//Tıklandıgın da yapılacak olan şeyler...
        {
            calismaAlani.grafik = Graphics.FromImage(lstKatman.SelectedItems[0].Tag as Bitmap);//Seçili katmandaki resmi grafiye atıyoruz...

            if (calismaAlani.SeciliArac == (int)BoyamaTipi.Cizgi || calismaAlani.SeciliArac == (int)BoyamaTipi.Sekil)
            {
                calismaAlani.temp = new Bitmap(lstKatman.SelectedItems[0].Tag as Bitmap);//İkincisi cizdirdimizde ekrandan kaybolmasın diye temp atıyoruz...
            }

            araclar[calismaAlani.SeciliArac].OnMouseDown(e, calismaAlani);// Seçili araçlardan hangisi varsa onu mouseDown çağır()... 
            TekrarCiz();
        }

        private void pbTuval_MouseMove(object sender, MouseEventArgs e)//Mousen tıklıyken hareket halinde olacak şeyler..
        {
            araclar[calismaAlani.SeciliArac].OnMouseMove(e, calismaAlani);// Seçili araçlardan hangisi varsa onu mouseMouve çağır... 
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                TekrarCiz();
        }

        private void pbTuval_MouseUp(object sender, MouseEventArgs e)//Mouse dan elimizi kaldırdımızda yapılacaklar..
        {
            if (calismaAlani.SeciliArac == (int)BoyamaTipi.Cizgi || calismaAlani.SeciliArac == (int)BoyamaTipi.Sekil)
            {
                calismaAlani.temp.Dispose();//Elimizi mouseden kaldırdımızda temp sil..
            }

            araclar[calismaAlani.SeciliArac].OnMouseUp(e, calismaAlani);// Seçili araçlardan hangisi varsa onu mouseMouve çağır... 
            TekrarCiz();
        }
        #endregion

        #region Katmanlar
        private void lstKatman_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstKatman.SelectedIndices.Count > 0)
            {
               calismaAlani.SeciliKatman = lstKatman.SelectedIndices[0];//seçili katmanı seçer.. :D
            }

            if (lstKatman.Items.Count == 1)//Sadece bir katman varsa butonları pasif yap...
            {
                btnKatmanSil.Enabled = false;
                btnKatmanGizle.Enabled = false;
            }
            else 
            {
                btnKatmanSil.Enabled = true;
                btnKatmanGizle.Enabled = true;
            }
        }

        private void btnKatmanEkle_Click(object sender, EventArgs e)
        {
            ListViewItem li = new ListViewItem(string.Format("Katman {0}", lstKatman.Items.Count + 1));
            li.Checked = true;
            li.Selected = true;
            li.Tag = new Bitmap(calismaAlani.Width, calismaAlani.Height);//Resmin etiketine çalışma alanın yükseklik ve genişlik atanır...
            lstKatman.Items.Insert(0, li);//Katmanı en üstte eklenir..
           // lstKatman.Items.Add(li);//Katmanı en alta ekler...
            TekrarCiz();
        }

        private void btnKatmanSil_Click(object sender, EventArgs e)
        {
            Bitmap bmp = lstKatman.Items[lstKatman.SelectedIndices[0]].Tag as Bitmap;//Seçili katmanın resmini bulur..
            bmp.Dispose();//Seçili katmanı siler...
            
            lstKatman.Items.RemoveAt(lstKatman.SelectedItems[0].Index);//Seçili katmanı siler..

            if (calismaAlani.SeciliKatman != 0)
                lstKatman.Items[calismaAlani.SeciliKatman - 1].Selected = true;//Katman sildigimizde bir Önceki Katman seçili yapar..
            else
                lstKatman.Items[lstKatman.Items.Count - 1].Selected = true;//En son katmanı var olan katmanı yapar..

            TekrarCiz();
        }

        private void btnKatmanGizle_Click(object sender, EventArgs e)
        {
            if (lstKatman.SelectedItems[0].Checked)//Seçili katman varsa gizle..
                lstKatman.SelectedItems[0].Checked = false;
            else
                lstKatman.SelectedItems[0].Checked = true;

            TekrarCiz();//Katmanları tekrar ayarlar...
        }

        public bool KatmanlariBirlestir()
        {
            g.Clear(Color.Transparent);
            for (int x = lstKatman.Items.Count - 1; x >= 0; x--)
            {
                if (lstKatman.Items[x].Checked)
                {
                    g.DrawImage((lstKatman.Items[x].Tag as Bitmap), 0, 0);//Katmanları (0,0) noktasında itabaren birleştiriyor...
                }
            }
            return true;
        }

        public void TekrarCiz()
        {
            if (KatmanlariBirlestir())
            {
                pbTuval.Image = ana_resim;//Birleşen katmanları ana_resimde birleştiriyor...
                pbTuval.Refresh();
            }
        }

        private void TumKatmanlariSil()
        {
            ana_resim.Dispose();
            foreach (ListViewItem item in lstKatman.Items)//Katmaları  Tek Tek Dolaşarak Siler...
            {
                using (Bitmap bmp = item.Tag as Bitmap)
                {
                    bmp.Dispose();
                }
                
            }
        }

        #endregion

        #region Renk Ayarla
        private void btnHazirRenkler_Click(object sender, EventArgs e)
        {
            calismaAlani.renk = (sender as PictureBox).BackColor;//PictureBoxdaki Renkleri dolaşarak seçili renki bulur...
            pbSeciliRenk.BackColor = calismaAlani.renk;
            RenkAyarla(pbSeciliRenk.BackColor);
        }

        private void RenkSec_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();//Renk seçmemizi sağlar...
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                RenkAyarla(colorDialog.Color);//Bunu renkayarla fonkdsiyonumuza gönderiyoruz...
            }
        }

        private void RenkAyarla(Color c)
        {
            pbSeciliRenk.BackColor = c;//Seçili renki ayarlarız
            calismaAlani.renk = c;
           // calismaAlani.firca.Color = c;//Fırcanın renkgini ayarlarız...
            calismaAlani.kalem.Color = c;//kalemin rengini ayarlarız...
        }
        #endregion

        private void MyPaint_Shown(object sender, EventArgs e)
        {
            CalismaAlaniOlustur(pbTuval.Width, pbTuval.Height);//Program Açıldıgında tuali ayarlıyor...
        }

        Color temp = Color.Black;

        private void SilgiAyarla(bool durum)
        {
            if (durum)
            {
                temp = pbSeciliRenk.BackColor;
                RenkAyarla(Color.White);
                KalemKalinligi.Value = 8;
                calismaAlani.SeciliSekil = (int)BoyamaTipi.Kalem;
            }
            else
            {
                RenkAyarla(temp);
            }
        }



        private void btnSilgi_Click(object sender, EventArgs e)
        {
            SilgiAyarla(true);
        }

        private void btnKompleSil_Click(object sender, EventArgs e)
        {
            CalismaAlaniOlustur(pbTuval.Width,pbTuval.Height);
            pbTuval.BackColor = Color.Transparent;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hakkimizda hakkı = new Hakkimizda();
            hakkı.ShowDialog();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog Kaydet = new SaveFileDialog();
            Kaydet.Filter = "(*.jpg)|*.jpg|(*.png)|*.png|(*.ico)|*.ico|(*.bmp)|*.bmp";
            if (Kaydet.ShowDialog()==DialogResult.OK)
            {
                pbTuval.Image.Save(Kaydet.FileName);//kaydetmemizi sağlar...
              
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();//Programdan Çıkmamızı sağlar...
           
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CalismaAlaniOlustur(pbTuval.Width, pbTuval.Height);
            pbTuval.BackColor = Color.Transparent;//Yeni çalışma alanı oluşturur...
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)//Resim eklememizi saglar...
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "(*.jpg)|*.jpg|(*.png(|*.png";
            if (file.ShowDialog()==DialogResult.OK)
            {
                pbTuval.ImageLocation = file.FileName;
            }
        }

        private void MyPaint_Load(object sender, EventArgs e)
        {

        }

         

       

    }
}
