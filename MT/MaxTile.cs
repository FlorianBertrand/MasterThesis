using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT
{
    class MaxTile
    {
        static int popsize;
        //static int generations = 50;
        static int offsize;
        static Tile[] offspring;
        static Tile[] individual;
        static Random rnd;
        static int bind;
        static bool boff;
        static int bestSol;
        static int fp;
        static bool overlap;
        static double seed;
        static protected Tiling Fix;
        static int generations;


        public MaxTile(double s, int f, bool o, Tiling fi, int pop, int off)
        {
            popsize = pop;
            offsize = off;
            Fix = fi;
            seed = s;
            fp = f;
            overlap = o;
            rnd = new Random();
            //init(seed, fp, overlap);

        }

        /// <summary>
        /// Function swap swaps two offspring a and b
        /// </summary>
        /// <param name="a">first element to swap</param>
        /// <param name="b">second element to swap</param>
        static void swap(int a, int b)
        {
            Tile temp;
            temp = offspring[a];
            offspring[a] = offspring[b];
            offspring[b] = temp;
        }
        /// <summary>
        /// part function sort elements from the array offspring from min to max, around a "pivot"
        /// </summary>
        /// <param name="min">1st element of the partition</param>
        /// <param name="max">last element of the partition</param>
        /// <returns>it returns the pivot element</returns>
        static int part(int min, int max)
        {
            int pivot = offspring[max].getSize();
            int it = min;
            for (int i = min; i < max; i++)
            {
                if (offspring[i].getSize() > pivot)
                {
                    swap(i, it);
                    it++;
                }
            }
            swap(max, it);
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
        /// <summary>
        /// init function generate the initial population randomly
        /// </summary>
        /// <param name="seed">seed is the probability parameter</param>
        static void init(double seed, int fp, bool overlap)
        {

            individual = new Tile[popsize];
            offspring = new Tile[offsize + popsize];
            //parent = new Tile[popsize];
            bool[] temp;

            for (int i = 0; i < popsize; i++)
            {
                temp = new bool[Program.col];
                for(int j=0; j < Program.col; j++)
                {
                    if(rnd.NextDouble() < seed)
                    {
                        temp[j] = true;
                    }
                    else
                    {
                        temp[j] = false;
                    }
                
                }
                individual[i] = new Tile(temp, Fix.geno, fp, overlap);
            }

        }
        /// <summary>
        /// This function select randomly two individuals and returns the fittest as parent
        /// </summary>
        /// <returns>Best of two random individuals</returns>
        static int tournament()
        {
            int r1, r2;

            r1 = rnd.Next(popsize);
            r2 = rnd.Next(popsize);
            if (individual[r1].getSize() > individual[r2].getSize())
            {
                return r1;
            }
            else
            {
                return r2;
            }

        }
        /// <summary>
        /// uniCrossover function perform a uniform crossover with parents p1 and p2 and probability p, to create i and i+1 offsprings
        /// </summary>
        /// <param name="p1">parent 1</param>
        /// <param name="p2">parent 2</param>
        /// <param name="i">offspring 1</param>
        /// <param name="p">probability</param>
        static void uniCrossover(int p1, int p2, int i, double p, double mu)
        {
            bool[] temp1 = new bool[Program.col];
            bool[] temp2 = new bool[Program.col];

            for(int j=0; j< Program.col; j++)
            {
                if(rnd.NextDouble() > p)
                {
                    temp1[j] = individual[p1].getGene (j);
                    temp2[j] = individual[p2].getGene (j);
                }
                else
                {
                    temp1[j] = individual[p2].getGene(j);
                    temp2[j] = individual[p1].getGene(j);
                }
                if (rnd.NextDouble() < mu)
                {
                    if (rnd.NextDouble() > 0.5)
                    {
                        temp1[j] = false;
                    }
                    else
                    {
                        temp1[j] = true;
                    }
                }
                if (rnd.NextDouble() < mu)
                {
                    if (rnd.NextDouble() > 0.5)
                    {
                        temp2[j] = false;
                    }
                    else
                    {
                        temp2[j] = true;
                    }
                }
            }
            offspring[i] = new Tile(temp1, Fix.geno, fp, overlap);
            offspring[i + 1] = new Tile(temp2, Fix.geno, fp, overlap);
        }
        /// <summary>
        /// this function creates a new generation of offsprings with individuals as parents
        /// it also makes offsprings mutate
        /// </summary>
        static void newGeneration(double mut)
        {
            int p1, p2;
            for (int i = 0; i < offsize; i += 2)
            {
                p1 = tournament();
                p2 = tournament();
                uniCrossover(p1, p2, i, 0.5, mut);

            }


        }
        /// <summary>
        /// This function select which offsprings and parents have to be kept for the next generation
        /// It follows a mu + lambda evolutionary strategy
        /// </summary>
        static int survivor()
        {

            for (int i = 0; i < popsize; i++)
            {
                offspring[offsize + i] = individual[i];
            }
            qsort(0, offsize + popsize - 1);

            for (int i = 0; i < popsize; i++)
            {
                individual[i] = offspring[i];
            }
            //Console.WriteLine("Best fitind=" + individual[0].getSize());
            return individual[0].getSize();


        }
        /// <summary>
        /// this procedure search the biggest tile on the dataset array dataset
        /// it stops after d generations without improvement
        /// </summary>
        /// <param name="d">number of generations without improvement</param>
        public Tile solve(int d, double pg, double mut)
        {
            int delta = 0;
            int best, last;
            best = 0;
            init(pg, fp, overlap);
            int gen = 0;

            do
            {
                last = best;
                newGeneration(mut);
                best = survivor();
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
            generations = gen;
            return individual[0];
            //Console.ReadLine();
        }

        public int getGen()
        {
            return generations;
        }

    }
}
