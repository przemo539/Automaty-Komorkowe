using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AutomatyElementarne
{
    public partial class Form1 : Form
    {
        private Graphics DRX_g;
        Drx drx;
        //!!!!!!!!!!!!!!!! MONTE CARLO !!!!!!!!!!!!!!!!!!!!!!!!!!
        private Graphics M_g;
        MonteCarlo monte;
        //!!!!!!!!!!!!!!!! ROZROST ZIAREN 1/2 !!!!!!!!!!!!!!!!!!!!!!!!!!
        private Graphics R_g;
        Zarodkowanie zarodk;
        // !!!!!!!!!!!!!!!! AUTOMATY ELEMENTARNE !!!!!!!!!!!!!!!!!!!!!!!!!!
        private Graphics g;

        public Form1()
        {
            InitializeComponent();
            DRX_zarodkowanie.SelectedIndex = 0;
            DRX_warBrz.SelectedIndex = 0;
            DRX_sasiedztwo.SelectedIndex = 0;
            DRX_wyswietlanie.SelectedIndex = 0;
            DRX_g = pictureBox4.CreateGraphics();

            M_zarodkowanie.SelectedIndex = 0;
            M_warBrz.SelectedIndex = 0;
            M_sasiedztwo.SelectedIndex = 0;
            M_wyswietlanie.SelectedIndex = 0;
            M_g = pictureBox3.CreateGraphics();

            R_g = pictureBox2.CreateGraphics();
            R_zarodkowanie.SelectedIndex = 0;
            R_warBrz.SelectedIndex = 0;
            R_sasiedztwo.SelectedIndex = 0;

            g = pictureBox1.CreateGraphics();
            Input_warunekBrzegowy.SelectedIndex = 0;
            drx = new Drx();
            zarodk = new Zarodkowanie();
            monte = new MonteCarlo();
        }
        //!!!!!!!!!!!!!!!! MONTE CARLO !!!!!!!!!!!!!!!!!!!!!!!!!!
        private void M_monte_carlo(object sender, EventArgs e)
        {
            double kT = Convert.ToDouble(M_kT.Value);
            monte.Monte_start(ref M_g, kT, M_warBrz.SelectedIndex, M_sasiedztwo.SelectedIndex, Convert.ToInt32(M_promien_sasiedztwa.Value), Convert.ToInt32(M_iteracje.Value), M_wyswietlanie.SelectedIndex);
        }
        private void M_R_init_clicked(object sender, EventArgs e)
        {
            pictureBox3.Refresh();
            int R_M_x = Convert.ToInt32(M_szerokosc.Value);
            int R_M_y = Convert.ToInt32(M_wysokosc.Value);
            int R_M_size = 0;
            bool R_M_border = false;
            pictureBox1.Width = this.Size.Width - 50;
            pictureBox1.Height = this.Size.Height - 200;
            if ((pictureBox1.Width / R_M_x) < (pictureBox1.Height / R_M_y))
                R_M_size = pictureBox1.Width / R_M_x;
            else
                R_M_size = pictureBox1.Height / R_M_y;
            if (R_M_size < 5)
            {
                R_M_size = 5;
               // R_M_border = false;
            }
            else
            {
               // R_M_border = true;
            }
            monte.Inicjuj(R_M_x, R_M_y, R_M_size, R_M_border, ref M_g);


            switch (M_zarodkowanie.SelectedIndex)
            {

                case 0:
                    //Jednorodne
                    monte.Jednorodnie(Convert.ToInt32(M_jednorodne_X.Value), Convert.ToInt32(M_jednorodne_Y.Value), ref M_g);
                    break;
                case 1:
                    //Z promieniem
                    monte.Zaznacz_z_promieniem(Convert.ToInt32(M_promien.Value), Convert.ToInt32(M_promien_ilosc.Value), ref M_g);
                    break;
                case 2:
                    monte.Zaznacz_losowo(Convert.ToInt32(M_promien_ilosc.Value), ref M_g);
                    break;
            }
        }

        private void laduj_typ_wyswietlania(object sender, EventArgs e)
        {
            if (monte != null)
            {
                pictureBox3.Refresh();
                monte.Wyrysuj(ref M_g, M_wyswietlanie.SelectedIndex);
            }
        }

        private void M_reset_clicked(object sender, EventArgs e)
        {
            pictureBox3.Refresh();
            monte.Wyczysc();
        }

        private void M_rozrost_started(object sender, EventArgs e)
        {
            monte.Start(ref M_g, M_warBrz.SelectedIndex, M_sasiedztwo.SelectedIndex, Convert.ToInt32(M_promien_sasiedztwa.Value));
        }

        private void M_picturebox_clicked(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;
            int x = coordinates.X;
            int y = coordinates.Y;

            if (monte.Klik(x, y, ref M_g))
                M_zarodkowanie.SelectedIndex = 3;
        }
        private void laduj_M_pola_zarodkowanie(object sender, EventArgs e)
        {
            switch (M_zarodkowanie.SelectedIndex)
            {
                case 0:
                    l_M_jednorodne1.Show();
                    l_M_jednorodne2.Show();
                    M_jednorodne_X.Show();
                    M_jednorodne_Y.Show();

                    l_M_prom1.Hide();
                    l_M_prom2.Hide();
                    M_promien.Hide();
                    M_promien_ilosc.Hide();
                    break;
                case 1:
                    l_M_prom1.Show();
                    l_M_prom2.Show();
                    M_promien.Show();
                    M_promien_ilosc.Show();

                    l_M_jednorodne1.Hide();
                    l_M_jednorodne2.Hide();
                    M_jednorodne_X.Hide();
                    M_jednorodne_Y.Hide();
                    break;
                case 2:
                    l_M_prom1.Hide();
                    l_M_prom2.Show();
                    M_promien.Hide();
                    M_promien_ilosc.Show();

                    l_M_jednorodne1.Hide();
                    l_M_jednorodne2.Hide();
                    M_jednorodne_X.Hide();
                    M_jednorodne_Y.Hide();
                    break;
                case 3:
                    l_M_prom1.Hide();
                    l_M_prom2.Hide();
                    M_promien.Hide();
                    M_promien_ilosc.Hide();

                    l_M_jednorodne1.Hide();
                    l_M_jednorodne2.Hide();
                    M_jednorodne_X.Hide();
                    M_jednorodne_Y.Hide();
                    break;
            }
        }

        //!!!!!!!!!!!!!!!! ROZROST ZIAREN 1/2 !!!!!!!!!!!!!!!!!!!!!!!!!!
        private void start_rozrost_clicked(object sender, EventArgs e)
        {
            zarodk.Start(ref R_g, R_warBrz.SelectedIndex, R_sasiedztwo.SelectedIndex, Convert.ToInt32(R_promien_sasiedztwa.Value));
        }
        private void R_init_clicked(object sender, EventArgs e)
        {
            pictureBox2.Refresh();
            int R_x = Convert.ToInt32(R_szerokosc.Value);
            int R_y = Convert.ToInt32(R_wysokosc.Value);
            int R_size = 0;
            bool R_border = false;
            pictureBox1.Width = this.Size.Width - 50;
            pictureBox1.Height = this.Size.Height - 200;
            if ((pictureBox1.Width / R_x) < (pictureBox1.Height / R_y))
                R_size = pictureBox1.Width / R_x;
            else
                R_size = pictureBox1.Height / R_y;
            if (R_size < 5)
            {
                R_size = 5;
               // R_border = false;
            }
            else
            {
               // R_border = true;
            }
            zarodk.Inicjuj(R_x, R_y, R_size, R_border, ref R_g);


            switch (R_zarodkowanie.SelectedIndex)
            {

                case 0:
                    //Jednorodne
                    zarodk.Jednorodnie(Convert.ToInt32(N_jednorodne_X.Value), Convert.ToInt32(N_jednorodne_Y.Value), ref R_g);
                    break;
                case 1:
                    //Z promieniem
                    zarodk.Zaznacz_z_promieniem(Convert.ToInt32(Z_promien.Value), Convert.ToInt32(Z_promien_ilosc.Value), ref R_g);
                    break;
                case 2:
                    zarodk.Zaznacz_losowo(Convert.ToInt32(Z_promien_ilosc.Value), ref R_g);
                    break;
            }
        }


        private void R_reset_clicked(object sender, EventArgs e)
        {
            pictureBox2.Refresh();
            zarodk.Wyczysc();
        }

        private void picturebox_clicked(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;
            int x = coordinates.X;
            int y = coordinates.Y;

            if (zarodk.Klik(x, y, ref R_g))
                R_zarodkowanie.SelectedIndex = 3;
        }

        private void laduj_pola_zarodkowanie(object sender, EventArgs e)
        {
            switch (R_zarodkowanie.SelectedIndex)
            {
                case 0:
                    l_jednorodne1.Show();
                    l_jednorodne2.Show();
                    N_jednorodne_X.Show();
                    N_jednorodne_Y.Show();

                    l_z_prom1.Hide();
                    l_z_prom2.Hide();
                    Z_promien.Hide();
                    Z_promien_ilosc.Hide();
                    break;
                case 1:
                    l_z_prom1.Show();
                    l_z_prom2.Show();
                    Z_promien.Show();
                    Z_promien_ilosc.Show();

                    l_jednorodne1.Hide();
                    l_jednorodne2.Hide();
                    N_jednorodne_X.Hide();
                    N_jednorodne_Y.Hide();
                    break;
                case 2:
                    l_z_prom1.Hide();
                    l_z_prom2.Show();
                    Z_promien.Hide();
                    Z_promien_ilosc.Show();

                    l_jednorodne1.Hide();
                    l_jednorodne2.Hide();
                    N_jednorodne_X.Hide();
                    N_jednorodne_Y.Hide();
                    break;
                case 3:
                    l_z_prom1.Hide();
                    l_z_prom2.Hide();
                    Z_promien.Hide();
                    Z_promien_ilosc.Hide();

                    l_jednorodne1.Hide();
                    l_jednorodne2.Hide();
                    N_jednorodne_X.Hide();
                    N_jednorodne_Y.Hide();
                    break;
            }
        }


        // !!!!!!!!!!!!!!!! AUTOMATY ELEMENTARNE !!!!!!!!!!!!!!!!!!!!!!!!!!
        private void Button_start_Elementarne_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
            int l_iteracji = Convert.ToInt32(Input_liczbaIteracji.Value);
            int dlugoscWektora = Convert.ToInt32(Input_dlugoscWektora.Value);

            pictureBox1.Width = this.Size.Width - 100;
            pictureBox1.Height = this.Size.Height - 200;

            int size;
            if ((pictureBox1.Width / dlugoscWektora) < (pictureBox1.Height / l_iteracji))
                size = pictureBox1.Width / dlugoscWektora;
            else
                size = pictureBox1.Height / l_iteracji;
            if (size == 0)
                size = 1;
            Elementarne elem = new Elementarne(dlugoscWektora, l_iteracji, Convert.ToInt32(Input_regulaDec.Value), size);
            elem.Click(Input_warunekBrzegowy.SelectedIndex, ref g);

        }
        // !!!!!!!!!!!!!!!! DRX !!!!!!!!!!!!!!!!!!!!!!!!!!
        private void DRX_init(object sender, EventArgs e)
        {
            pictureBox4.Refresh();
            int R_M_x = Convert.ToInt32(DRX_szerokosc.Value);
            int R_M_y = Convert.ToInt32(DRX_wysokosc.Value);
            int R_M_size = 0;
            bool R_M_border = false;
            pictureBox1.Width = this.Size.Width - 50;
            pictureBox1.Height = this.Size.Height - 200;
            if ((pictureBox1.Width / R_M_x) < (pictureBox1.Height / R_M_y))
                R_M_size = pictureBox1.Width / R_M_x;
            else
                R_M_size = pictureBox1.Height / R_M_y;
            if (R_M_size < 5)
            {
                R_M_size = 5;
               // R_M_border = false;
            }
            else
            {
               // R_M_border = true;
            }
            drx.Inicjuj(R_M_x, R_M_y, R_M_size, R_M_border, ref DRX_g);


            switch (DRX_zarodkowanie.SelectedIndex)
            {

                case 0:
                    //Jednorodne
                    drx.Jednorodnie(Convert.ToInt32(DRX_jednorodne_X.Value), Convert.ToInt32(DRX_jednorodne_Y.Value), ref DRX_g);
                    break;
                case 1:
                    //Z promieniem
                    drx.Zaznacz_z_promieniem(Convert.ToInt32(DRX_promien.Value), Convert.ToInt32(DRX_promien_ilosc.Value), ref DRX_g);
                    break;
                case 2:
                    drx.Zaznacz_losowo(Convert.ToInt32(DRX_promien_ilosc.Value), ref DRX_g);
                    break;
            }
        }

        private void DRX_roz_start(object sender, EventArgs e)
        {
            drx.Start(ref DRX_g, DRX_warBrz.SelectedIndex, DRX_sasiedztwo.SelectedIndex, Convert.ToInt32(DRX_promien_sasiedztwa.Value));
        }
        private void DRX_reset_clicked(object sender, EventArgs e)
        {
            pictureBox4.Refresh();
            drx.Wyczysc();
        }
        private void DRX_monte_carlo(object sender, EventArgs e)
        {
            double kT = Convert.ToDouble(DRX_kT.Value);
            drx.Monte_start(ref DRX_g, kT, DRX_warBrz.SelectedIndex, DRX_sasiedztwo.SelectedIndex, Convert.ToInt32(DRX_promien_sasiedztwa.Value), Convert.ToInt32(DRX_iteracje.Value), DRX_wyswietlanie.SelectedIndex);
        }
        private void DRX_laduj_pola_zarodkowanie(object sender, EventArgs e)
        {
            switch (DRX_zarodkowanie.SelectedIndex)
            {
                case 0:
                    l_DRX_jednorodne1.Show();
                    l_DRX_jednorodne2.Show();
                    DRX_jednorodne_X.Show();
                    DRX_jednorodne_Y.Show();

                    l_DRX_prom1.Hide();
                    l_DRX_prom2.Hide();
                    DRX_promien.Hide();
                    DRX_promien_ilosc.Hide();
                    break;
                case 1:
                    l_DRX_prom1.Show();
                    l_DRX_prom2.Show();
                    DRX_promien.Show();
                    DRX_promien_ilosc.Show();

                    l_DRX_jednorodne1.Hide();
                    l_DRX_jednorodne2.Hide();
                    DRX_jednorodne_X.Hide();
                    DRX_jednorodne_Y.Hide();
                    break;
                case 2:
                    l_DRX_prom1.Hide();
                    l_DRX_prom2.Show();
                    DRX_promien.Hide();
                    DRX_promien_ilosc.Show();

                    l_DRX_jednorodne1.Hide();
                    l_DRX_jednorodne2.Hide();
                    DRX_jednorodne_X.Hide();
                    DRX_jednorodne_Y.Hide();
                    break;
                case 3:
                    l_DRX_prom1.Hide();
                    l_DRX_prom2.Hide();
                    DRX_promien.Hide();
                    DRX_promien_ilosc.Hide();

                    l_DRX_jednorodne1.Hide();
                    l_DRX_jednorodne2.Hide();
                    DRX_jednorodne_X.Hide();
                    DRX_jednorodne_Y.Hide();
                    break;
            }
        }

        private void DRX_pictureBox4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;
            int x = coordinates.X;
            int y = coordinates.Y;

            if (drx.Klik(x, y, ref M_g))
                DRX_zarodkowanie.SelectedIndex = 3;
        }

        private void DRX_laduj_typ_wysw(object sender, EventArgs e)
        {
            if (drx != null)
            {
                pictureBox4.Refresh();
                drx.Wyrysuj(ref DRX_g, DRX_wyswietlanie.SelectedIndex);
            }
        }

        private void DRX_start(object sender, EventArgs e)
        {
            drx.start(ref DRX_g, Convert.ToDouble(DRX_A.Value), Convert.ToDouble(DRX_B.Value), Convert.ToDouble(DRX_timestep.Value), Convert.ToDouble(DRX_timend.Value),
                Convert.ToDouble(DRX_const.Value), DRX_wyswietlanie.SelectedIndex, DRX_warBrz.SelectedIndex, DRX_sasiedztwo.SelectedIndex, Convert.ToInt32(DRX_promien_sasiedztwa.Value));
        }
    }

}