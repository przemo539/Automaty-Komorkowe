using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatyElementarne
{    
    class MapaSasiadow
    {
        List<int> color_id = new List<int>();
        List<int> color_number = new List<int>();
        bool dodawac = false;
        bool dyskokowac = true;
        int energy = 0;
        int size = 0;
        Random random = new Random();
        public void addDyslok(double sasiad, double komorka)
        {
            if(sasiad >= komorka)
            {
                dyskokowac = false;
            }
        }
        public void Add(int id)
        {
            bool dodac = true;
            for(int i = 0; i< color_id.Count; i++)
            {
                if(color_id[i] == id)
                {
                    dodac = false;
                    ++color_number[i];
                    break;
                }
                else if(color_id[i] == -1)
                {
                    color_id[i] = id;
                    dodac = false;
                    ++size;
                    dodawac = true;
                    ++color_number[i];
                    break;
                }
            }
            if (dodac)
            {
                color_id.Add(id);
                color_number.Add(1);
                ++size;
                dodawac = true;
            }
        }
        public void Add(int id_color, int color_cell)
        {
            bool dodac = true;
            for (int i = 0; i < color_id.Count; i++)
            {
                if (color_id[i] == id_color)
                {
                    dodac = false;
                    ++color_number[i];
                    break;
                }
                else if (color_id[i] == -1)
                {
                    color_id[i] = id_color;
                    dodac = false;
                    ++size;
                    dodawac = true;
                    ++color_number[i];
                    break;
                }
            }
            if(id_color != color_cell)
            {
                energy++;
            }

            if (dodac)
            {
                color_id.Add(id_color);
                color_number.Add(1);
                ++size;
                dodawac = true;
            }
        }
       
        public int GetRandomColor()
        {
            return (color_id.Count == 0)?0:color_id[random.Next(0, size)];
        }

        public int GetEnergy()
        {
            return energy;
        }

        public bool WhetherAdd()
        {
            return dodawac;
        }

        public bool WhetherDyslok()
        {
            return dyskokowac;
        }
        public int GetMostFrequentColor()
        {
            int temp_id = color_id[0];
            int temp_liczba = color_number[0];
            for (int i = 1; i < size; i++)
            {
                if(temp_liczba < color_number[i])
                {
                    temp_liczba = color_number[i];
                    temp_id = color_id[i];
                }
            }
            return temp_id;
        }
        public void Clear()
        {
            for (int i = 0; i < color_id.Count; i++)
            {
                color_id[i] = -1;
                color_number[i] = 0;
            }

            dodawac = false;
            energy = 0;
            size = 0;
            dyskokowac = true;
        }
    }
}
