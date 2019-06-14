using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomatyElementarne
{
    class Zarodkowanie
    {
        protected List<List<Cell>> grid = new List<List<Cell>>();
        protected int x = 0;
        protected int y = 0;
        protected int size = 1;
        protected bool border = false;
        protected int kolor = 1;

        public void Start(ref Graphics R_g, int warBrz, int sasiedztwo, int promien)
        {
            /*
            Von Neuman V
            Moore V
            Pentagonalne losowe V
            Heksagonalne Lewe V
            Heksagonalne Prawe V
            Heksagonalne Losowe
            Z promieniem
            */
            Random random = new Random();
            MapaSasiadow mapa = new MapaSasiadow();

            List<List<Cell>> R_grid_kopia = new List<List<Cell>>();
            for (int i = 0; i < x; i++)
            {
                R_grid_kopia.Add(new List<Cell>());
                for (int j = 0; j < y; j++)
                {
                    R_grid_kopia[i].Add(new Cell());
                }
            }

            int pentagonalne_losowe;
            int heksagonalne_losowe = 0;
            bool ischanged = true;

            while (ischanged)
            {

                ischanged = false;
                
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        R_grid_kopia[i][j].Copy(grid[i][j]);
                    }
                }

                for (int j = 0; j < y; j++)
                {
                    for (int i = 0; i < x; i++)
                    {
                        pentagonalne_losowe = random.Next(0, 4);
                        if (sasiedztwo == 5)
                            heksagonalne_losowe = random.Next(1, 3);
                        if (!R_grid_kopia[i][j].Get_state_b())
                        {
                            // z promieniem
                            int X_od=-1, Y_od=-1, X_do=1, Y_do = 1 , srodekX = 0, srodekY = 0;
                            if (sasiedztwo == 6)
                            {
                                srodekX = i * size + 1 + size / 2 + R_grid_kopia[i][j].Get_przesuniecie_x() * size / 4;
                                srodekY = j * size + 1 + size / 2 + R_grid_kopia[i][j].Get_przesuniecie_y() * size / 4;
                                X_od = -x;
                                X_do = x;
                                Y_od = -y;
                                Y_do = y;
                            }


                            for (int ci = X_od; ci <= X_do; ci++)
                                for (int cj = Y_od; cj <= Y_do; cj++)
                                {
                                    if (cj == 0 && ci == 0)
                                        continue;
                                        //Von neuman
                                    else if (sasiedztwo == 0 && (ci == -1 && cj == -1 || ci == 1 && cj == -1 || ci == -1 && cj == 1 || ci == 1 && cj == 1))
                                        continue;
                                    //Pentagonalne ignorujemy prawe
                                    else if (sasiedztwo == 2 && pentagonalne_losowe == 0 && (ci == 1 && cj == -1 || ci == 1 && cj == 0 || ci == 1 && cj == 1))
                                        continue;
                                    //Pentagonalne ignorujemy lewe
                                    else if (sasiedztwo == 2 && pentagonalne_losowe == 1 && (ci == -1 && cj == -1 || ci == -1 && cj == 0 || ci == -1 && cj == 1))
                                        continue;
                                    //Pentagonalne ignorujemy gora
                                    else if (sasiedztwo == 2 && pentagonalne_losowe == 2 && (ci == -1 && cj == -1 || ci == 0 && cj == -1 || ci == 1 && cj == -1))
                                        continue;
                                    //Pentagonalne ignorujemy dol
                                    else if (sasiedztwo == 2 && pentagonalne_losowe == 3 && (ci == -1 && cj == 1 || ci == 0 && cj == 1 || ci == 1 && cj == 1))
                                        continue;
                                    //Heksagonalne lewe
                                    else if ((heksagonalne_losowe == 1 || sasiedztwo == 3 )&& (ci == -1 && cj == -1 || ci == 1 && cj == 1))
                                        continue;
                                    //Heksagonalne prawe
                                    else if ((heksagonalne_losowe == 2 || sasiedztwo == 4) && (ci == 1 && cj == -1 || ci == -1 && cj == 1))
                                        continue;
                                    else
                                    {
                                        double pkt_promien = 0;
                                        double promien_kwadrat = 0; 
                                        if(sasiedztwo == 6)
                                        {
                                            pkt_promien = Math.Pow((srodekX - ((i + ci) * size + 1 + size / 2 + R_grid_kopia[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_przesuniecie_x() * size / 4)), 2.0) + Math.Pow((srodekY - ((j + cj) * size + 1 + size / 2 + R_grid_kopia[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_przesuniecie_y() * size / 4)), 2.0);
                                            promien_kwadrat = Math.Pow(promien, 2.0);
                                        }
                                        switch (warBrz)
                                        {
                                            case 0: //periodyczne                         
                                                if (sasiedztwo != 6 && R_grid_kopia[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_state_b())
                                                {
                                                    int kolorek = R_grid_kopia[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_kolor_id();
                                                    mapa.Add(kolorek);
                                                }
                                                else if(pkt_promien <= promien_kwadrat && R_grid_kopia[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_state_b())
                                                {
                                                    int kolorek = R_grid_kopia[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_kolor_id();
                                                    mapa.Add(kolorek);
                                                }
                                                break;
                                            case 1: // pochlaniajace
                                                if (sasiedztwo != 6 && (i + ci < x && i + ci >= 0 && j + cj < y && j + cj >= 0 && R_grid_kopia[i + ci][j + cj].Get_state_b()))
                                                {
                                                    mapa.Add((R_grid_kopia[i + ci][j + cj].Get_kolor_id()));
                                                }
                                                else if((i + ci < x && i + ci >= 0 && j + cj < y && j + cj >= 0) && pkt_promien <= promien_kwadrat && R_grid_kopia[i + ci][j + cj].Get_state_b())
                                                {
                                                    mapa.Add((R_grid_kopia[i + ci][j + cj].Get_kolor_id()));
                                                }
                                                break;
                                        }
                                    }
                                }
                            
                            if (mapa.WhetherAdd())
                            {
                                grid[i][j].Set_state(true);
                                grid[i][j].Set_color(mapa.GetMostFrequentColor());
                                grid[i][j].Draw(R_g, i * size + 1, j * size + 1, size - 1, border);
                                ischanged = true;
                            }
                            /*Pen blackPen = new Pen(Color.Black, 3);
                            R_g.DrawEllipse(blackPen, (i * size + 1 + size / 2 + grid[i][j].get_przesuniecie_x() * size / 4), (j * size + +1 + size / 2 + grid[i][j].get_przesuniecie_y() * size / 4), 2, 2);*/

                        }
                        mapa.Clear();
                    }

                }
            }

        }
        public void Inicjuj(int R_x, int R_y, int R_size, bool R_border, ref Graphics R_g)
        {
            Wyczysc();
            this.x = R_x;
            this.y = R_y;
            kolor = 1;
            this.size = R_size;
            border = R_border;
            Console.WriteLine(grid.Count);
            for (int i = 0; i < this.x; i++)
            {
                if(grid.Count <= i)
                    grid.Add(new List<Cell>());
                for (int j = grid[i].Count; j < this.y; j++)
                    grid[i].Add(new Cell());
            }
            Draw(ref R_g);
        }
       public void Zaznacz_losowo(int ilosc, ref Graphics R_g)
        {
            int i = 10000;
            Random random = new Random();
                while (i > 0 && ilosc > 0)
                {
                    int randomX = random.Next(0, x);
                    int randomY = random.Next(0, y);
                    if (grid.Count() != 0 && !grid[randomX][randomY].Get_state_b())
                    {
                        grid[randomX][randomY].Set_state(true);
                        grid[randomX][randomY].Set_color(kolor);
                        grid[randomX][randomY].Draw(R_g, randomX * size + 1, randomY * size + 1, size - 1, border);
                        --ilosc;
                        kolor++;
                    }

                    --i;
                }
            }
        public void Zaznacz_z_promieniem(int promien, int ilosc, ref Graphics R_g)
        {
            Console.WriteLine(size);
            int i = 10000;
            Random random = new Random();
                while (i > 0 && ilosc > 0)
                {
                    int randomX = random.Next(0, x);
                    int randomY = random.Next(0, y);
                    if (grid.Count() != 0 && !grid[randomX][randomY].Get_state_b())
                    {
                        bool ok = true;
                        int X_od = (randomX - promien / size - 1) < 0 ? 0: (randomX - promien / size - 1);
                        int X_do = (randomX + promien / size + 2) > x ? x : (randomX + promien / size + 2);
                        int Y_od = (randomY - promien / size - 1) < 0 ? 0 : (randomY - promien / size - 1);
                        int Y_do = (randomY + promien / size + 2) > x ? y : (randomY + promien / size + 2);
                        int randomSrodekX = randomX * size + 1 + size / 2;
                        int randomSrodekY = randomY * size + 1 + size / 2;
                        for (int k = X_od; k < X_do; k++)
                        {
                            for (int j = Y_od; j < Y_do; j++)
                            {
                                double lewa_g = Math.Pow((randomSrodekX - (k * size + 1)), 2.0) + Math.Pow((randomSrodekY - (j * size + 1)), 2.0);
                                double lewa_d = Math.Pow((randomSrodekX - (k * size + 1)), 2.0) + Math.Pow((randomSrodekY - (j * size + 1 + size)), 2.0);
                                double prawa_g = Math.Pow((randomSrodekX - (k * size + 1 + size)), 2.0) + Math.Pow((randomSrodekY - (j * size + 1)), 2.0);
                                double prawa_d = Math.Pow((randomSrodekX - (k * size + 1)+ size), 2.0) + Math.Pow((randomSrodekY - (j * size + 1 + size)), 2.0);
                                if (lewa_g <= Math.Pow(promien, 2.0) || lewa_d <= Math.Pow(promien, 2.0) || prawa_g <= Math.Pow(promien, 2.0) || prawa_d <= Math.Pow(promien, 2.0))
                                {
                                   
                                    if (grid[k][j].Get_state_b())
                                    {
                                        ok = false;
                                        break;
                                    }
                                }
                            }
                        }
                                //(x−a)^2 + (y−b)^2 = r^2
                        if (ok)
                        {

                            grid[randomX][randomY].Set_state(true);
                            grid[randomX][randomY].Set_color(kolor);
                            grid[randomX][randomY].Draw(R_g, randomX * size + 1, randomY * size + 1, size - 1, border);
                            /*RYSOWANIE OKREGÓW TESTOWYCH 
                            Pen blackPen = new Pen(Color.Black, 3);
                            R_g.DrawEllipse(blackPen, randomSrodekX - promien, randomSrodekY - promien, promien*2, promien*2);*/
                            --ilosc;
                            kolor++;
                        }
                    }

                    --i;
                
            }
            if( ilosc > 0 && i<0)
                MessageBox.Show("Nieda się stworzyć tylu punktow", "Error");
        }
        public void Jednorodnie(int wiersz, int kolumna, ref Graphics R_g)
        {
            int odstep_x =  x/wiersz;
            int odstep_y = y/kolumna;
            int srodek_x = x / 2;
            int srodek_y = y / 2;
            if (kolumna > y || wiersz > x)
            {
                MessageBox.Show("Wybrano zbyt dużą ilość komorek, wieksza od siatki", "Error");
            }
            else
            {
                int i_poczatek = 0;
                int i_koniec = 0;
                int j_poczatek = 0;
                int j_koniec = 0;
                if (wiersz % 2 == 1 )
                {
                    i_poczatek = srodek_x - odstep_x * (wiersz - 1) / 2;
                    i_koniec = srodek_x + odstep_x * (wiersz - 1) / 2;
                }
                else //wiersz % 2 == 0 
                {
                    i_poczatek = srodek_x - odstep_x / 2 - odstep_x * (wiersz - 2) / 2;
                    i_poczatek = (srodek_x < wiersz) ? i_poczatek - 1 : i_poczatek;
                    i_koniec = srodek_x + odstep_x / 2 + odstep_x * (wiersz - 2) / 2;
                }

                 if (kolumna % 2 == 0)
                {
                    j_poczatek = srodek_y - odstep_y / 2 - odstep_y * (kolumna - 2) / 2;
                    j_poczatek = (srodek_x < kolumna) ? j_poczatek - 1 : j_poczatek;
                    j_koniec = srodek_y + odstep_y / 2 + odstep_y * (kolumna - 2) / 2;
                }
                else //kolumna % 2 == 1
                {
                    j_poczatek = srodek_y - odstep_y * (kolumna - 1) / 2;
                    j_koniec = srodek_y + odstep_y * (kolumna - 1) / 2;
                }

                for (int i = i_poczatek; i <= i_koniec; i += odstep_x)
                    for (int j = j_poczatek; j <= j_koniec; j += odstep_y)
                    {
                        grid[i][j].Set_state(true);
                        grid[i][j].Set_color(kolor);
                        grid[i][j].Draw(R_g, i * size + 1, j * size + 1, size, border);
                        kolor++;
                    }
            }
        }


        public bool Klik(int R_x, int R_y, ref Graphics R_g)
        {
            if (size > 0)
            {
                R_x = R_x / size;
                R_y = R_y / size;
                if (x > R_x && y > R_y && grid.Count() != 0)
                {
                    if (grid[R_x][R_y].Get_state_b())
                    {
                        grid[R_x][R_y].Set_state(false);
                        grid[R_x][R_y].Set_color(0);
                        --kolor;
                    }
                    else
                    {
                        grid[R_x][R_y].Set_state(true);
                        grid[R_x][R_y].Set_color(kolor);
                        kolor++;
                    }

                    
                    
                    grid[R_x][R_y].Draw(R_g, R_x * size + 1,R_y * size + 1, size, border);///////////////////////////
                    
                    return true;
                }
            }
            return false;
        }
        public void Wyczysc()
        {
            for (int i = 0; i < grid.Count; i++)
                for (int j = 0; j < grid[i].Count; j++)
                    grid[i][j].Wyczysc();
        }
        protected void  Draw(ref Graphics R_g)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    grid[i][j].Draw(R_g, i * size + 1, j * size + 1, size, border);
                }
            }
        }
        protected int Modulo(int liczba, int size)
        {
            int modulo = liczba % size;
            while (modulo < 0)
                modulo  += size;


                return modulo;
        }
    }
}
