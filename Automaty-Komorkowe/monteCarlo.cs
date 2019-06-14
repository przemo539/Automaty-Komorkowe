using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatyElementarne
{
    class MonteCarlo :Zarodkowanie
    {
        protected Random random = new Random();
        protected List<List<int>> energyGrid = new List<List<int>>();
        System.Drawing.SolidBrush[] color_brush = {new System.Drawing.SolidBrush(Color.FromArgb(255, 255, 255)), new System.Drawing.SolidBrush(Color.FromArgb(250, 250, 250)), new System.Drawing.SolidBrush(Color.FromArgb(240, 240, 240)),
             new System.Drawing.SolidBrush(Color.FromArgb(230, 230, 230)), new System.Drawing.SolidBrush(Color.FromArgb(220, 220, 220)),  new System.Drawing.SolidBrush(Color.FromArgb(210, 210, 210)),
             new System.Drawing.SolidBrush(Color.FromArgb(200, 200, 200)), new System.Drawing.SolidBrush(Color.FromArgb(190, 190, 190)), new System.Drawing.SolidBrush(Color.FromArgb(180, 180, 180)),
             new System.Drawing.SolidBrush(Color.FromArgb(170, 170, 170)),  new System.Drawing.SolidBrush(Color.FromArgb(160, 160, 160)), new System.Drawing.SolidBrush(Color.FromArgb(150, 150, 150)),
             new System.Drawing.SolidBrush(Color.FromArgb(140, 140, 140)),  new System.Drawing.SolidBrush(Color.FromArgb(130, 130, 130)), new System.Drawing.SolidBrush(Color.FromArgb(120, 120, 120)),
             new System.Drawing.SolidBrush(Color.FromArgb(110, 110, 110)), new System.Drawing.SolidBrush(Color.FromArgb(100, 100, 100)), new System.Drawing.SolidBrush(Color.FromArgb(90, 90, 90)),
             new System.Drawing.SolidBrush(Color.FromArgb(80, 80, 80)), new System.Drawing.SolidBrush(Color.FromArgb(70, 70, 70)), new System.Drawing.SolidBrush(Color.FromArgb(60, 60, 60)),
             new System.Drawing.SolidBrush(Color.FromArgb(50, 50, 50)), new System.Drawing.SolidBrush(Color.FromArgb(40, 40, 40)), new System.Drawing.SolidBrush(Color.FromArgb(30, 30, 30)),
             new System.Drawing.SolidBrush(Color.FromArgb(20, 20, 20)), new System.Drawing.SolidBrush(Color.FromArgb(10, 10, 10)), new System.Drawing.SolidBrush(Color.FromArgb(00, 00, 00))};
        public void Monte_start(ref Graphics R_g, double kT, int warBrz, int sasiedztwo, int promien, int iter, int wyswietlanie)
        {
            int M_size = x * y;           
            int M_x = 0;
            int M_y = 0;
            int kolor_id = 0;

            double randomNumber;
            double delta_energy;
            double Prawd;
            int random_color;

            MapaSasiadow mapa_oryg = new MapaSasiadow();
            MapaSasiadow mapa_temp = new MapaSasiadow();
            RandomGenerator randomCords = new RandomGenerator();
            List<List<Cell>> R_grid_kopia = new List<List<Cell>>();
            for (int k = 0; k < x; k++)
            {
                R_grid_kopia.Add(new List<Cell>());
                for (int j = 0; j < y; j++)
                {
                    R_grid_kopia[k].Add(new Cell());
                }
            }


            for (int iter_start = 0; iter_start < iter; iter_start++)
            {
                randomCords.Create(x, y);
                for (int k = 0; k < x; k++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        R_grid_kopia[k][j].Copy(grid[k][j]);
                    }
                }
                for (int i = 0; i < M_size; i++)
                {


                    randomCords.Get_random(ref M_x, ref M_y);
                    kolor_id = grid[M_x][M_y].Get_kolor_id();

                    // z promieniem
                    Monte_cacl(ref R_grid_kopia, ref M_x, ref M_y, ref sasiedztwo, ref promien, ref warBrz, ref kolor_id, ref mapa_oryg);

                    random_color = mapa_oryg.GetRandomColor();
                    Monte_cacl(ref R_grid_kopia, ref M_x, ref M_y, ref sasiedztwo, ref promien, ref warBrz, ref random_color, ref mapa_temp);

                    randomNumber = random.NextDouble();
                    delta_energy = mapa_temp.GetEnergy() - mapa_oryg.GetEnergy();
                    Prawd = Math.Exp(-delta_energy / kT);

                    if (delta_energy <= 0 || (delta_energy > 0 && Prawd <= randomNumber))
                    {
                        grid[M_x][M_y].Set_state(true);
                        grid[M_x][M_y].Set_color(random_color);
                        if(wyswietlanie == 0)
                            grid[M_x][M_y].Draw(R_g, M_x * size + 1, M_y * size + 1, size - 1, border);

                        energyGrid[M_x][M_y] = mapa_temp.GetEnergy();
                    }
                    else
                    {
                        energyGrid[M_x][M_y] = mapa_oryg.GetEnergy();
                    }


                    mapa_oryg.Clear();
                    mapa_temp.Clear();
                   
                }
                randomCords.Clear();
                if (wyswietlanie == 1)
                    Draw_energy(ref R_g);
            }
        }
 
 
        public void Wyrysuj(ref Graphics R_g, int wyswietlanie){
            if(wyswietlanie == 0)
            {
                Draw(ref R_g);
            }else
            {
                Draw_energy(ref R_g);
            }
        }

        public new void Wyczysc()
        {
            for (int i = 0; i < grid.Count; i++)
                for (int j = 0; j < grid[i].Count; j++)
                {
                    grid[i][j].Wyczysc();
                    energyGrid[i][j] = 0;
                }

        }

        public new void Inicjuj(int R_x, int R_y, int R_size, bool R_border, ref Graphics R_g)
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
                if (grid.Count <= i)
                {
                    grid.Add(new List<Cell>());
                    energyGrid.Add(new List<int>());
                }
                for (int j = grid[i].Count; j < this.y; j++)
                {
                    grid[i].Add(new Cell());
                    energyGrid[i].Add(new int());
                }
            }
            Draw(ref R_g);
        }
        protected void Draw_energy(ref Graphics R_g)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    R_g.FillRectangle(color_brush[energyGrid[i][j] % 25], i * size, j * size, size, size);
                }
            }
        }

        protected void Monte_cacl(ref List<List<Cell>> R_grid_kopia, ref int M_x, ref int M_y, ref int sasiedztwo, ref int promien, ref int warBrz, ref int kolor_id, ref MapaSasiadow mapa)
        {
            int pentagonalne_losowe = random.Next(0, 4);
            int heksagonalne_losowe = 0;
            if (sasiedztwo == 5)
                heksagonalne_losowe = random.Next(1, 3);

            // z promieniem
            int X_od = -1, Y_od = -1, X_do = 1, Y_do = 1, srodekX = 0, srodekY = 0;
            if (sasiedztwo == 6)
            {
                srodekX = M_x * size + 1 + size / 2 + R_grid_kopia[M_x][M_y].Get_przesuniecie_x() * size / 4;
                srodekY = M_y * size + 1 + size / 2 + R_grid_kopia[M_x][M_y].Get_przesuniecie_y() * size / 4;
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
                    else if ((heksagonalne_losowe == 1 || sasiedztwo == 3) && (ci == -1 && cj == -1 || ci == 1 && cj == 1))
                        continue;
                    //Heksagonalne prawe
                    else if ((heksagonalne_losowe == 2 || sasiedztwo == 4) && (ci == 1 && cj == -1 || ci == -1 && cj == 1))
                        continue;
                    else
                    {
                        double pkt_promien = 0;
                        double promien_kwadrat = 0;
                        if (sasiedztwo == 6)
                        {
                            pkt_promien = Math.Pow((srodekX - ((M_x + ci) * size + 1 + size / 2 + R_grid_kopia[Modulo(M_x + ci, x)][Modulo(M_y + cj, y)].Get_przesuniecie_x() * size / 4)), 2.0) + Math.Pow((srodekY - ((M_y + cj) * size + 1 + size / 2 + R_grid_kopia[Modulo(M_x + ci, x)][Modulo(M_y + cj, y)].Get_przesuniecie_y() * size / 4)), 2.0);
                            promien_kwadrat = Math.Pow(promien, 2.0);
                        }
                        switch (warBrz)
                        {
                            case 0: //periodyczne                         
                                if (sasiedztwo != 6 && R_grid_kopia[Modulo(M_x + ci, x)][Modulo(M_y + cj, y)].Get_state_b())
                                {
                                    int kolorek = R_grid_kopia[Modulo(M_x + ci, x)][Modulo(M_y + cj, y)].Get_kolor_id();
                                    mapa.Add(kolorek, kolor_id);
                                }
                                else if (pkt_promien <= promien_kwadrat && R_grid_kopia[Modulo(M_x + ci, x)][Modulo(M_y + cj, y)].Get_state_b())
                                {
                                    int kolorek = R_grid_kopia[Modulo(M_x + ci, x)][Modulo(M_y + cj, y)].Get_kolor_id();
                                    mapa.Add(kolorek, kolor_id);
                                }
                                break;
                            case 1: // pochlaniajace
                                if (sasiedztwo != 6 && (M_x + ci < x && M_x + ci >= 0 && M_y + cj < y && M_y + cj >= 0 && R_grid_kopia[M_x + ci][M_y + cj].Get_state_b()))
                                {
                                    mapa.Add((R_grid_kopia[M_x + ci][M_y + cj].Get_kolor_id()), kolor_id);
                                }
                                else if ((M_x + ci < x && M_x + ci >= 0 && M_y + cj < y && M_y + cj >= 0) && pkt_promien <= promien_kwadrat && R_grid_kopia[M_x + ci][M_y + cj].Get_state_b())
                                {
                                    mapa.Add((R_grid_kopia[M_x + ci][M_y + cj].Get_kolor_id()), kolor_id);
                                }
                                break;
                        }
                    }
                }
        }
    }
}
