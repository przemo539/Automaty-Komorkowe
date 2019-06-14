using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatyElementarne
{
    class RandomGenerator
    {
        List<string> list = new List<string>();
        Random rnd = new Random();
        int size = 0;
        public void Create(int x, int y)
        {
            size = x * y;
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    list.Add(i + " " + j);
        }
        public void Get_random(ref int x, ref int y)
        {
            int index = rnd.Next(0, size);
            string var = list[index];
            string[] cords = var.Split(' ');
            x = System.Convert.ToInt32(cords[0]);
            y = System.Convert.ToInt32(cords[1]);
            list[index] = list[size - 1];
            list[size - 1] = var;
            size--;
        }
        public void Clear()
        {
            size = 0;
        }
    }
}
