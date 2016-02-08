using System;
using System.Collections.Generic;
namespace MT
{
    /// <summary>
    /// Tiling is a structure composed of multiple tile
    /// size is the coverage of this tiling
    /// </summary>
    public class Tiling
    {
        public List<Tile> geno;
        public int size;
        public Tiling()
        {
            size = 0;
            geno = new List<Tile>();
            /*
            if (Fixed != null)
            {
                geno = Fixed.geno;
            }
            */
        }

        public void addTile(bool[] a, double fp, bool overlap)
        {
            Tile temp = new Tile(a, geno, fp, overlap);
            geno.Add(temp);
            this.size += temp.getSize();
        }
        public Tiling(double seed, int nTiles, double fp, bool overlap)
        {

            geno = new List<Tile>();
            /*
            if (Fixed != null)
            {
                geno = Fixed.geno;
            }
            */
            //Console.WriteLine("instanciate tiling");
            Random rnd = new Random();
            this.size = 0;
            bool[] temp;
            for (int i = 0; i < nTiles; i++)
            {
                temp = new bool[Program.col];
                for (int j = 0; j < Program.col; j++)
                {
                    if (rnd.NextDouble() < seed)
                    {
                        temp[j] = true;
                    }
                    else
                    {
                        temp[j] = false;
                    }
                }

                geno.Add(new Tile(temp, geno, fp, overlap));
                this.size += geno[i].getSize();
            }
            //qsort(0, nTiles);
            //Console.WriteLine("end instanciate tiling");
        }
        /*
        static public void swapTiles(int a, int b)
        {
            Tile temp = geno[a];
            geno[a] = geno[b];
            geno[b] = temp;
        }
        */
        public int getSize()
        {
            return size;
        }
        /*
        /// <summary>
        /// part function sort elements from the array offspring from min to max, around a "pivot"
        /// </summary>
        /// <param name="min">1st element of the partition</param>
        /// <param name="max">last element of the partition</param>
        /// <returns>it returns the pivot element</returns>
        static int part(int min, int max)
        {
            int pivot = geno[max].getSize();
            int it = min;
            for (int i = min; i < max; i++)
            {
                if (geno[i].getSize() > pivot)
                {
                    swapTiles(i, it);
                    it++;
                }
            }
            swapTiles(max, it);
            return it;
        }
        /// <summary>
        /// qsort function is a quicksort algorithm implement to sort the offspring array
        /// </summary>
        /// <param name="min">1st element of the partition to sort</param>
        /// <param name="max">last element of the partition to sort</param>
        static void qsort(int min, int max)
        {
            int p;
            if (min < max)
            {
                p = part(min, max);
                qsort(min, p - 1);
                qsort(p + 1, max);

            }

        }
        */
    }
}