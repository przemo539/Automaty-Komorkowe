using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatyElementarne
{
    class Drx:MonteCarlo
    {
        int kolor_zarodki = 1270;

        public void start(ref Graphics DRX_g, double A, double B, double timestep, double timend, double consts, int wysw, int warBrz, int sasiedztwo, int promien) {
            double krytyczna_dyslokacja = 4215840142323.42;
            WriteRhoToFile((A / B + (1 - A / B) * Math.Exp(-B * 0)).ToString());
            for (double i = timestep; i <= timend; i+= timestep)
            {
                DzialoDyslokacji(ref A,ref B, i - timestep, ref i, ref consts);
                Zarodkowanie(ref krytyczna_dyslokacja, i);
                Rozrost(i - timestep, timestep, sasiedztwo, promien, warBrz);
                NowaEnergia(sasiedztwo, promien, warBrz);
                switch (wysw)
                {
                    case 0:
                        Draw(ref DRX_g);
                        break;
                    case 1:
                        Draw_energy(ref DRX_g);
                        break;
                    case 2:
                        Draw_dysloc(ref DRX_g);
                        break;
                }

            }
        }

        public new void Wyrysuj(ref Graphics R_g, int wyswietlanie)
        {
            if (wyswietlanie == 0)
            {
                Draw(ref R_g);
            }
            else if(wyswietlanie == 1)
            {
                Draw_energy(ref R_g);
            }
            else
            {
                Draw_dysloc(ref R_g);
            }
        }

        void DzialoDyslokacji(ref double A, ref double B, double t0,ref double t1, ref double procent_srednia_dys)
        {
            double ro0 = A / B + (1 - A / B) * Math.Exp(-B * t0);
            double ro1 = A / B + (1 - A / B) * Math.Exp(-B * t1);
            WriteRhoToFile(ro1.ToString());
            double deltaRo = ro1 - ro0;
            double sredniaDyslokacja = deltaRo / (x * y);
            double pozostalo = deltaRo;
            for (int i=0; i < x; i++)
            {
                for(int j=0; j<y; j++) {
                    grid[i][j].Add_dysklokacje(procent_srednia_dys * sredniaDyslokacja);
                    pozostalo -= procent_srednia_dys * sredniaDyslokacja;
                }
            }

            double przyznano = 0;
            double procent_mala_pacz = 0;
            while (przyznano <= pozostalo)
            {
                int prawd = random.Next(0, 100);
                int randX;
                int randY;
                procent_mala_pacz = random.NextDouble();
                bool ok = true;

                do
                {
                    randX = random.Next(0, x);
                    randY = random.Next(0, y);
                    if (prawd <= 20 && energyGrid[randX][randY] == 0 || prawd > 20 && energyGrid[randX][randY] > 0)
                    {
                        ok = false;
                    }
                } while (ok);
                grid[randX][randY].Add_dysklokacje(procent_mala_pacz * pozostalo);
                przyznano += 0.001 * pozostalo;
            }
        }

        void Rozrost(double timestepbefore,double timestep, int sasiedztwo, int promien, int  warBrz)
        {
            MapaSasiadow mapa = new MapaSasiadow();
            
            int pentagonalne_losowe;
            int heksagonalne_losowe;
            int X_od = -1, Y_od = -1, X_do = 1, Y_do = 1, srodekX = 0, srodekY = 0;
            double dyslok_komorki = 0;
            List<List<Cell>> R_grid_kopia = new List<List<Cell>>();
            for (int k = 0; k < x; k++)
            {
                R_grid_kopia.Add(new List<Cell>());
                for (int j = 0; j < y; j++)
                {
                    R_grid_kopia[k].Add(new Cell());
                    R_grid_kopia[k][j].Copy(grid[k][j]);
                }
            }

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    dyslok_komorki = grid[i][j].Get_dyslokacje();
                     pentagonalne_losowe = random.Next(0, 4);
                    heksagonalne_losowe = 0;
                    if (sasiedztwo == 5)
                        heksagonalne_losowe = random.Next(1, 3);

                    // z promieniem
                    X_od = -1; Y_od = -1; X_do = 1; Y_do = 1; srodekX = 0; srodekY = 0;
                    if (sasiedztwo == 6)
                    {
                        srodekX = i * size + 1 + size / 2 + R_grid_kopia[i][j].Get_przesuniecie_x() * size / 4;
                        srodekY = i * size + 1 + size / 2 + R_grid_kopia[i][j].Get_przesuniecie_y() * size / 4;
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
                                    pkt_promien = Math.Pow((srodekX - ((i + ci) * size + 1 + size / 2 + R_grid_kopia[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_przesuniecie_x() * size / 4)), 2.0) + Math.Pow((srodekY - ((j + cj) * size + 1 + size / 2 + R_grid_kopia[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_przesuniecie_y() * size / 4)), 2.0);
                                    promien_kwadrat = Math.Pow(promien, 2.0);
                                }
                                switch (warBrz)
                                {
                                    case 0: //periodyczne                         
                                        if (sasiedztwo != 6 && R_grid_kopia[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_state_b())
                                        {
                                            mapa.addDyslok(R_grid_kopia[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_dyslokacje(), dyslok_komorki);
                                            if (R_grid_kopia[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_rekrystalizowany_timestep() == timestepbefore)
                                            {
                                                mapa.Add(R_grid_kopia[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_kolor_id());
                                            }
                                        }
                                        else if (pkt_promien <= promien_kwadrat && R_grid_kopia[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_state_b())
                                        {
                                                mapa.addDyslok(R_grid_kopia[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_dyslokacje(), dyslok_komorki);
                                                if (R_grid_kopia[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_rekrystalizowany_timestep() == timestepbefore)
                                                {
                                                    mapa.Add(R_grid_kopia[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_kolor_id());
                                                }
                                            }
                                        break;
                                    case 1: // pochlaniajace
                                        if (sasiedztwo != 6 && (i + ci < x && i + ci >= 0 && j + cj < y && j + cj >= 0 && R_grid_kopia[i + ci][j + cj].Get_state_b()))
                                        {
                                                mapa.addDyslok(R_grid_kopia[i + ci][j + cj].Get_dyslokacje(), dyslok_komorki);
                                                if (R_grid_kopia[i + ci][j + cj].Get_rekrystalizowany_timestep() == timestepbefore)
                                                {
                                                    mapa.Add(R_grid_kopia[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_kolor_id());
                                                }
                                        }
                                        else if ((i + ci < x && i + ci >= 0 && j + cj < y && j + cj >= 0) && pkt_promien <= promien_kwadrat && R_grid_kopia[i + ci][j + cj].Get_state_b())
                                        {
                                            mapa.addDyslok(R_grid_kopia[i + ci][j + cj].Get_dyslokacje(), dyslok_komorki);
                                            if (R_grid_kopia[i + ci][j + cj].Get_rekrystalizowany_timestep() == timestepbefore)
                                            {
                                                mapa.Add(R_grid_kopia[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_kolor_id());
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    if(mapa.WhetherAdd() && mapa.WhetherDyslok())
                    {
                        grid[i][j].Zarodkuj(mapa.GetMostFrequentColor(), timestepbefore + timestep);
                    }

                    mapa.Clear();
                }
            }
        }

        void Zarodkowanie(ref double krytyczna_dyslokacja, double timestep)
        {
   
            for(int i=0; i < x; i++)
            {
                for(int j=0; j < y; j++)
                {
                    if(grid[i][j].Get_dyslokacje() > krytyczna_dyslokacja && energyGrid[i][j] > 0)
                    {
                        grid[i][j].Zarodkuj(kolor_zarodki--, timestep);
                    }
                }
            }
        }

        void WriteRhoToFile(string text)
        {
            using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(@"Rho.txt", true))
            {
                file.WriteLine(text);
            }
        }
         void Draw_dysloc(ref Graphics DRX_g)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (grid[i][j].Get_rekrystalizowany())
                    {
                        grid[i][j].Draw(DRX_g, i * size + 1, j * size + 1, size - 1, border);
                    }
                }
            }
        }
        void NowaEnergia(int sasiedztwo, int promien, int warBrz)
        {

            MapaSasiadow mapa = new MapaSasiadow();

            int pentagonalne_losowe;
            int heksagonalne_losowe;
            int X_od = -1, Y_od = -1, X_do = 1, Y_do = 1, srodekX = 0, srodekY = 0;
            double dyslok_komorki = 0;

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    dyslok_komorki = grid[i][j].Get_dyslokacje();
                    pentagonalne_losowe = random.Next(0, 4);
                    heksagonalne_losowe = 0;
                    if (sasiedztwo == 5)
                        heksagonalne_losowe = random.Next(1, 3);

                    // z promieniem
                    X_od = -1; Y_od = -1; X_do = 1; Y_do = 1; srodekX = 0; srodekY = 0;
                    if (sasiedztwo == 6)
                    {
                        srodekX = i * size + 1 + size / 2 + grid[i][j].Get_przesuniecie_x() * size / 4;
                        srodekY = i * size + 1 + size / 2 + grid[i][j].Get_przesuniecie_y() * size / 4;
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
                                    pkt_promien = Math.Pow((srodekX - ((i + ci) * size + 1 + size / 2 + grid[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_przesuniecie_x() * size / 4)), 2.0) + Math.Pow((srodekY - ((j + cj) * size + 1 + size / 2 + grid[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_przesuniecie_y() * size / 4)), 2.0);
                                    promien_kwadrat = Math.Pow(promien, 2.0);
                                }
                                switch (warBrz)
                                {
                                    case 0: //periodyczne                         
                                        if (sasiedztwo != 6 && grid[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_state_b())
                                        {
                                            mapa.Add(grid[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_kolor_id(), grid[i][j].Get_kolor_id());                                            
                                        }
                                        else if (pkt_promien <= promien_kwadrat && grid[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_state_b())
                                        {
                                            mapa.Add(grid[Modulo(i + ci, x)][Modulo(j + cj, y)].Get_kolor_id(), grid[i][j].Get_kolor_id());
                                        }
                                        break;
                                    case 1: // pochlaniajace
                                        if (sasiedztwo != 6 && (i + ci < x && i + ci >= 0 && j + cj < y && j + cj >= 0 && grid[i + ci][j + cj].Get_state_b()))
                                        {
                                            mapa.Add(grid[i + ci][j + cj].Get_kolor_id(), grid[i][j].Get_kolor_id());
                                        }
                                        else if ((i + ci < x && i + ci >= 0 && j + cj < y && j + cj >= 0) && pkt_promien <= promien_kwadrat && grid[i + ci][j + cj].Get_state_b())
                                        {
                                            mapa.Add(grid[i + ci][j + cj].Get_kolor_id(), grid[i][j].Get_kolor_id());
                                        }
                                        break;
                                }
                            }
                        }
                    energyGrid[i][j] = mapa.GetEnergy();

                    mapa.Clear();
                }
            }
        }
    }
}
