using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Drawing;

namespace AutomatyElementarne
{
    class Elementarne
    {
        BitArray regulaBin;
        List<Cell> grid = new List<Cell>();
        protected int dlugoscWektora = 0;
        protected int l_iteracji = 0;
        protected int size = 0;
        public Elementarne(int dlugosc, int iteracje, int regula,int siz)
        {
            RegulaDecToBin(regula);
            dlugoscWektora = dlugosc;
            l_iteracji = iteracje;
            size = siz;
            for (int i = 0; i < dlugoscWektora; i++)
                grid.Add(new Cell());

            grid[(int)Math.Round(dlugoscWektora / 2.0)].Set_state(true);
        }

        public void Click(int warunek_brz, ref Graphics g)
        {
            Draw(ref g, 0);

            for (int iter = 1; iter <= l_iteracji; iter++)
            {
                List<Cell> gridkopia = new List<Cell>();
                gridkopia.AddRange(grid.Select(s => new Cell(s)));
                for (int i = 0; i < dlugoscWektora; i++)
                {
                    int bufor = 0;
                    switch (warunek_brz)
                    {
                        case 0:
                            bufor = gridkopia[Modulo(i - 1, dlugoscWektora)].Get_state() * 4 + gridkopia[Modulo(i, dlugoscWektora)].Get_state() * 2 + gridkopia[Modulo(i + 1, dlugoscWektora)].Get_state() * 1;
                            break;
                        case 1:
                            bufor = gridkopia[Odbijajace(i - 1, dlugoscWektora)].Get_state() * 4 + gridkopia[i].Get_state() * 2 + gridkopia[Odbijajace(i + 1, dlugoscWektora)].Get_state() * 1;
                            break;
                        case 2:
                            bufor = Pochlaniajace1d(gridkopia, i - 1, 4) + gridkopia[i].Get_state() * 2 + Pochlaniajace1d(gridkopia, i + 1, 1);
                            break;
                    }
                    if (bufor < regulaBin.Length && regulaBin[bufor])
                    {
                        grid[i].Set_state(true);
                        grid[i].Draw(g, i * size + 1, iter * size + 1, size, false);
                    }
                    else
                        grid[i].Set_state(false);

                }

            }

        }
        void Draw(ref Graphics g, int y)
        {
            for (int i = 0; i < dlugoscWektora; i++)
            {
                grid[i].Draw(g, i * size + 1, y * size + 1, size, false);
            }
        }

        void RegulaDecToBin(int reg)
        {
            string binaryString = Convert.ToString(reg, 2);
            regulaBin = new BitArray(binaryString.Length);
            for (int i = 0; i < regulaBin.Length; i++)
            {
                regulaBin[i] = (binaryString[Math.Abs(regulaBin.Length - i - 1)] == '1');
            }
        }

        int Modulo(int liczba, int size)
        {
            int modulo = liczba % size;
            if (modulo < 0)
                return size + liczba;
            else
                return modulo;
        }

        int Odbijajace(int liczba, int size)
        {
            if (liczba < 0)
                return liczba + 1;
            else if (liczba >= size)
                return liczba - 1;
            else
                return liczba;
        }
        int Pochlaniajace1d(List<Cell> gridkopia, int pozycja, int wartosc)
        {
            if (pozycja >= 0 && pozycja < dlugoscWektora)
                return gridkopia[pozycja].Get_state() * wartosc;
            else
                return 0;
        }
    }
}
