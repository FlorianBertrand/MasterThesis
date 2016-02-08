using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT
{
    class MaxTiling
    {

        public MaxTiling(int pop, int off)
        {
            rnd = new Random();
            popsize = pop;
            offsize = off;
            generations = 0;
            
        }
        static int popsize;
        //static int generations = 50;
        static int offsize;
        static Tiling[] offspringTiling;
        static Tiling[] individualTiling;
        static Random rnd;
        static int bind;
        static bool boff;
        static int bestSol;
        static int generations;
        /// <summary>
        /// Function swap swaps two offspring a and b
        /// </summary>
        /// <param name="a">first element to swap</param>
        /// <param name="b">second element to swap</param>
        static void swapT(int a, int b)
        {
            Tiling temp;
            temp = offspringTiling[a];
            offspringTiling[a] = offspringTiling[b];
            offspringTiling[b] = temp;
        }
        /// <summary>
        /// part function sort elements from the array offspring from min to max, around a "pivot"
        /// </summary>
        /// <param name="min">1st element of the partition</param>
        /// <param name="max">last element of the partition</param>
        /// <returns>it returns the pivot element</returns>
        static int partT(int min, int max)
        {
            int pivot = offspringTiling[max].getSize();
            int it = min;
            for (int i = min; i < max; i++)
            {
                if (offspringTiling[i].getSize() > pivot)
                {
                    swapT(i, it);
                    it++;
                }
            }
            swapT(max, it);
            return it;
        }
        /// <summary>
        /// qsort function is a quicksort algorithm implement to sort the offspring array
        /// </summary>
        /// <param name="min">1st element of the partition to sort</param>
        /// <param name="max">last element of the partition to sort</param>
        static void qsortT(int min, int max)
        {
            int p;
            if (min < max)
            {
                p = partT(min, max);
                qsortT(min, p - 1);
                qsortT(p + 1, max);

            }

        }


        /// <summary>
        /// init function generate the initial population randomly
        /// </summary>
        /// <param name="seed">seed is the probability parameter</param>


        static void initTiling(double seed, int nTiles, double fp, bool overlap)
        {
            individualTiling = new Tiling[popsize];
            offspringTiling = new Tiling[offsize + popsize];
            
            for (int i = 0; i < popsize; i++)
            {
                individualTiling[i] = new Tiling(seed, nTiles, fp, overlap);
            }
            
        }



        /// <summary>
        /// This function select randomly two individuals and returns the fittest as parent
        /// </summary>
        /// <returns>Best of two random individuals</returns>
        static int tournamentT(int poolSize)
        {
            int best, r, s;
            s = 0;
            best = rnd.Next(popsize);
            while (s < poolSize - 1)
            {
                r = rnd.Next(popsize);
                if (individualTiling[r].getSize() > individualTiling[best].getSize())
                {
                    best = r;
                }
                s++;
            }
            return best;
        }
        /// <summary>
        /// This function select randomly two individuals and returns the fittest as parent
        /// </summary>
        /// <returns>Best of two random individuals</returns>
        static int tournamentT()
        {
            int r1, r2;

            r1 = rnd.Next(popsize);
            r2 = rnd.Next(popsize);
            if (individualTiling[r1].getSize() > individualTiling[r2].getSize())
            {
                return r1;
            }
            else
            {
                return r2;
            }

        }


        static void onePointTilesCrossoverT(int p1, int p2, int i, double p, int t, double mu, double fp, bool overlap)
        {   

            offspringTiling[i] = new Tiling();
            offspringTiling[i + 1] = new Tiling();
            double cross= rnd.Next(t-1)+1;
            for (int k = 0; k < t; k++)
            {
                if (k <= cross)
                {
                    offspringTiling[i].addTile(individualTiling[p1].geno[k].birth(mu), fp, overlap);
                    offspringTiling[i + 1].addTile(individualTiling[p2].geno[k].birth(mu), fp, overlap);
                }
                else
                {
                    offspringTiling[i].addTile(individualTiling[p2].geno[k].birth(mu), fp, overlap);
                    offspringTiling[i+1].addTile(individualTiling[p1].geno[k].birth(mu), fp, overlap);
                }
            }

        }



        static void uniCrossoverT(int p1, int p2, int i, double p, int t, double mu, double fp, bool overlap)
        {
            bool[] temp1;
            bool[] temp2;


            offspringTiling[i] = new Tiling();
            offspringTiling[i + 1] = new Tiling();


            for (int k = 0; k < t; k++)
            {
                temp1 = new bool[Program.col];
                temp2 = new bool[Program.col];
                for (int j = 0; j < Program.col; j++)
                {
                    if (rnd.NextDouble() >= p)
                    {
                        temp1[j] = individualTiling[p1].geno[k].getGene(j);
                        temp2[j] = individualTiling[p2].geno[k].getGene(j);
                    }
                    else
                    {
                        temp1[j] = individualTiling[p2].geno[k].getGene(j);
                        temp2[j] = individualTiling[p1].geno[k].getGene(j);
                    }
                    if (rnd.NextDouble() < mu)
                    {
                        temp1[j] = !temp1[j];
                    }
                    if (rnd.NextDouble() < mu)
                    {
                        temp2[j] = !temp2[j];
                    }
                }

                offspringTiling[i].addTile(temp1, fp, overlap);
                offspringTiling[i + 1].addTile(temp2, fp, overlap);


            }


        }




        static void newGenerationT(double mut, int t, double fp, bool overlap)
        {
            int p1, p2;
            int a, b;
            //offspringTiling = new Tiling[offsize + popsize];
            for (int i = 0; i < offsize; i += 2)
            {
                p1 = tournamentT();
                /*
                do
                {
                    p2 = tournamentT();
                } while (individualTiling[p1].getSize() == individualTiling[p2].getSize());
                */
                p2 = tournamentT();
                /*
                if (rnd.NextDouble() < 0.1)
                {
                    a = rnd.Next(t);
                    do
                    {
                        b = rnd.Next(t);
                    } while (a == b);
                    individualTiling[p1].swapTiles(a, b);
                }
                if (rnd.NextDouble() < 0.1)
                {
                    a = rnd.Next(t);
                    do
                    {
                        b = rnd.Next(t);
                    } while (a == b);
                    individualTiling[p1].swapTiles(a, b);
                }
                */
                //uniCrossoverT(p1, p2, i, 0.5, t, mut, fp, overlap);
                onePointTilesCrossoverT(p1, p2, i, 0.5, t, mut, fp, overlap);

                //offspringTiling[i].mutate(mut);
                //offspringTiling[i + 1].mutate(mut);
                //offspringTiling[i].size=offspringTiling[i].comSize();
                //offspringTiling[i+1].size=offspringTiling[i + 1].comSize();
            }


        }


        static void bestTile()
        {
            int best = 0;
            int ind = 0;
            for (int i = 0; i < offsize; i++)
            {
                if (offspringTiling[i].getSize() > best)
                {
                    ind = i;
                    best = offspringTiling[i].getSize();
                    boff = true;
                }
            }
            for (int i = 0; i < popsize; i++)
            {
                if (individualTiling[i].getSize() > best)
                {
                    ind = i;
                    best = individualTiling[i].getSize();
                    boff = false;
                }
            }
            bind = ind;
            bestSol = best;
        }


        static int survivorT()
        {

            for (int i = 0; i < popsize; i++)
            {
                offspringTiling[offsize + i] = individualTiling[i];
            }
            qsortT(0, offsize + popsize - 1);

            for (int i = 0; i < popsize; i++)
            {
                individualTiling[i] = offspringTiling[i];
            }
            //Console.WriteLine("Best fitind=" + individualTiling[0].getSize());
            return individualTiling[0].getSize();

        }

        static int survivorBattle(double replacementRate, int poolSize, bool rambo)
        {
            int best = 0;
            int b, ind;
            int temp;
            int c;
            if (rambo)
            {
                bestTile();
                c = 1;
                if (boff)
                {
                    individualTiling[0] = offspringTiling[bind];
                }
                else
                {
                    individualTiling[0] = individualTiling[bind];
                }
            }
            else
            {
                c = 0;
            }
            for (int i = c; i < popsize; i++)
            {
                b = 0;
                for (int a = 0; a < poolSize; a++)
                {
                    if (rnd.NextDouble() < replacementRate)
                    {
                        ind = rnd.Next(offsize);
                        temp = offspringTiling[ind].getSize();
                        if (temp > b)
                        {
                            individualTiling[i] = offspringTiling[ind];
                            b = temp;
                        }

                    }
                    else
                    {
                        ind = rnd.Next(popsize);
                        temp = individualTiling[ind].getSize();
                        if (temp > b)
                        {
                            individualTiling[i] = individualTiling[ind];
                            b = temp;
                        }
                    }
                }
                if (b > best)
                {
                    best = b;
                }
            }
            if (best > bestSol)
            {

                return best;
            }
            else
            {
                return bestSol;
            }
        }

        public Tiling solve(double seed, int nTiles, int d, double mut, double fp, bool overlap)
        {

            int delta = 0;
            int best, last;
            best = 0;
            initTiling(seed, nTiles, fp, overlap);
            int gen = 0;
            do
            {
                last = best;
                //Console.WriteLine("begin new gen");
                newGenerationT(mut, nTiles, fp, overlap);
                //Console.WriteLine("end new gen");
                //best = survivorBattle(0.9, 5, false);
                best = survivorT();
                //Console.WriteLine("end survivor");
                if (last == best)
                {
                    delta++;
                }
                else
                {
                    delta = 0;
                }
                gen++;
                //Console.WriteLine(best);
            } while (delta < d);
            bestSol = best;
            //it = gen;
            foreach(Tile t in individualTiling[0].geno)
            {
                t.outputSizes();
            }
            return individualTiling[0];

        }
        public Tiling SolveFix(int nTiles, double colseed, int d, double m, int pop, int off, bool o)
        {
            Tiling Fixed = new Tiling();
            MaxTile p;
            Tile t;
            for (int i = 0; i < nTiles; i++)
            {
                p = new MaxTile(colseed, 0, o, Fixed, pop, off);
                t = p.solve(d, m, colseed);
                t.outputSizes();
                generations += p.getGen();
                Fixed.addTile(t.getGeno(), colseed, o);
                Console.WriteLine(t.getSize().ToString()+" "+Fixed.getSize().ToString());
            }
            return Fixed;
        }
        public int getGen()
        {
            return generations;
        }

    }
}
