using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatyElementarne
{
    class Cell
    {
        bool state;
        private Pen black = new Pen(Color.Black, 3);
        private Pen grey = new Pen(Color.LightGray, 3);
        private System.Drawing.SolidBrush Black = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        private System.Drawing.SolidBrush color_brush;
        private System.Drawing.SolidBrush White = new System.Drawing.SolidBrush(System.Drawing.Color.White);
        int przesuniecie_x = 0;
        int przesuniecie_y = 0;
        int kolor_id;
        double gestosc_dyslokacji;
        bool rekrystalizowany;
        double rekrystalizowany_timestep;
        static Random random = new Random();
        public void Set_state(bool state)
        {
            this.state = state;
        }
        public void Set_state()
        {
            this.state = !state;
        }
        public int Get_state()
        {
            return state ? 1 : 0;
        }
        public bool Get_state_b()
        {
            return state;
        }

        public int Get_kolor_id()
        {
            return kolor_id;
        }
        public int Get_przesuniecie_x()
        {
            return przesuniecie_x;
        }
        public int Get_przesuniecie_y()
        {
            return przesuniecie_y;
        }
        public void Add_dysklokacje(double x)
        {
            gestosc_dyslokacji += x;
        }
        public double Get_dyslokacje()
        {
            return gestosc_dyslokacji;
        }

        public double Get_rekrystalizowany_timestep()
        {
            return rekrystalizowany_timestep; ;
        }
        public bool Get_rekrystalizowany()
        {
            return rekrystalizowany;
        }
      
        public void Zarodkuj(int kolor, double timestep)
        {
            Set_color(kolor);
            gestosc_dyslokacji = 0;
            rekrystalizowany = true;
            rekrystalizowany_timestep = timestep;
        }
        public void Draw(Graphics g, int x, int y, int size, bool border)
        {
            if (state)
            {
                g.FillRectangle(color_brush, x, y, size, size);

            }
            else
            {
                g.FillRectangle(White, x, y, size, size);
            }
           // if (border)
               // g.DrawRectangle(grey, x, y, size, size);


        }
        public void Set_color(int kolor_id)
        {
            this.kolor_id = kolor_id;
            int red = 0;
            int blue = 0;
            int green = 0;
            if (kolor_id > 765) {
                kolor_id -= 509;
                if (kolor_id > 510)
                {
                 red = kolor_id - 510;
                 kolor_id -= 510;
                }
                if (kolor_id > 255)
                {
                    blue = kolor_id - 255;
                    kolor_id -= 255;
                }

                green = kolor_id;
            }
            else if(kolor_id >=0 && kolor_id < 256)
            {
                red = kolor_id;
            }else if(kolor_id >=256 && kolor_id < 511)
            {
                green = kolor_id - 255;
            }
            else
            {
                blue = kolor_id - 510;
            }



            color_brush = new System.Drawing.SolidBrush(Color.FromArgb(red, green, blue));
        }
        public void Wyczysc()
        {
            przesuniecie_x = random.Next(-1, 2);
            przesuniecie_y = random.Next(-1, 2);
            state = false;
            color_brush = Black;
            kolor_id = -1;
            rekrystalizowany = false;
            gestosc_dyslokacji = 0;
            rekrystalizowany_timestep = 0;
        }

        public Cell()
        {
            przesuniecie_x = random.Next(-1, 2);
            przesuniecie_y = random.Next(-1, 2);
            state = false;
            color_brush = Black;
            kolor_id = -1;
            gestosc_dyslokacji = 0;

        }
        public Cell(Cell copyFrom)
        {
            this.state = copyFrom.state;
        }
        public void Copy(Cell x)
        {
            state = x.state;
            color_brush = x.color_brush;
            kolor_id = x.kolor_id;
            przesuniecie_x = x.przesuniecie_x;
            przesuniecie_y = x.przesuniecie_y;
            rekrystalizowany = x.rekrystalizowany;
            gestosc_dyslokacji = x.gestosc_dyslokacji;
        }

    }
}
