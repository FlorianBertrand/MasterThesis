using System;

namespace MT
{
    public class Population
    {
        static Coverage[] individual;
        static Coverage[] offspring;
        static int bestSol;
        static int generations;
        static int offsize;
        static int popsize;
        static Random rnd;
        static int overlap;
        static double fp;
        static double mu;
        static int nTiles;
        static Coverage bestIndividual;
        /// <summary>
        /// Initialize the initial population
        /// </summary>
        /// <param name="pop">Size of the population</param>
        /// <param name="off">Size of the offspring population</param>
        /// <param name="seed">probability to initialize a bit to 1</param>
        /// <param name="f">Number of false positives allowed per row</param>
        /// <param name="o">number of overlaps allowed</param>
        /// <param name="m">probability to mutate a bit</param>
        /// <param name="t">Number of tiles</param>
        public Population(int pop, int off, double seed, double f, int o, double m, int t)
        {
            generations = 0;
            offsize = off;
            popsize = pop;
            individual = new Coverage[popsize];
            offspring = new Coverage[offsize];
            rnd = new Random();
            fp = f;
            overlap=o;
            mu = m;
            nTiles = t;
            generations = 0;
            bestSol = 0;
            init(seed);
        }
        /// <summary>
        /// This function initialize the population
        /// </summary>
        static void init(double seed)
        {

            //Console.WriteLine("Init");
            for (int i = 0; i < popsize; i++)
            {
                individual[i] = new Coverage(nTiles, seed, fp, overlap);
                if (individual[i].getFit() > bestSol)
                {
                    bestSol = individual[i].getFit();
                    bestIndividual = individual[i];
                }
                //individual[i].print();
                //Console.WriteLine("inside init");
            }

        }
        /// <summary>
        /// This function select randomly poolSize individuals and returns the fittest as parent
        /// </summary>
        /// <param name="poolSize">number of individuals in the mating pool</param>
        /// <returns>Best of poolSize random individuals</returns>
        static int tournament(int poolSize)
        {
            int best, r, s;
            s = 0;
            best = rnd.Next(popsize);
            while (s < poolSize - 1)
            {
                r = rnd.Next(popsize);
                if (individual[r].getFit() > individual[best].getFit())
                {
                    best = r;
                }
                s++;
            }
            return best;
        }

        /// <summary>
        /// Uniform crossover
        /// </summary>
        /// <param name="p1">Indice of parent1</param>
        /// <param name="p2">Indice of parent2</param>
        /// <param name="i">Indice of first offspring</param>
        /// <param name="p">Probability of taking gene of parent1</param>
        /// <returns>Fitness of best new offspring</returns>
        static int uniCrossover(int p1, int p2, int i, double p)
        {
            bool[,] temp1;
            bool[,] temp2;
            temp1 = new bool[nTiles, Program.col];
            temp2 = new bool[nTiles, Program.col];
            for (int k = 0; k < nTiles; k++)
            {

                for (int j = 0; j < Program.col; j++)
                {
                    if (rnd.NextDouble() >= p)
                    {
                        temp1[k,j] = individual[p1].getGene(k,j);
                        temp2[k,j] = individual[p2].getGene(k, j);
                    }
                    else
                    {
                        temp1[k,j] = individual[p2].getGene(k, j);
                        temp2[k,j] = individual[p1].getGene(k, j);
                    }
                    if (rnd.NextDouble() < mu)
                    {
                        temp1[k,j] = !temp1[k,j];
                    }
                    if (rnd.NextDouble() < mu)
                    {
                        temp2[k,j] = !temp2[k,j];
                    }
                }

            }
            offspring[i] = new Coverage(nTiles, temp1, fp, overlap);
            offspring[i + 1] = new Coverage(nTiles, temp2, fp, overlap);
            if (offspring[i].getFit() > offspring[i + 1].getFit())
            {
                if (offspring[i].getFit() > bestSol)
                {
                    bestSol = offspring[i].getFit();
                    bestIndividual = offspring[i];
                }
                return offspring[i].getFit();
            }
            else
            {
                if (offspring[i+1].getFit() > bestSol)
                {
                    bestSol = offspring[i+1].getFit();
                    bestIndividual = offspring[i+1];
                }
                return offspring[i + 1].getFit();
            }

        }

        /// <summary>
        /// One point crossover
        /// </summary>
        /// <param name="p1">Indice of Parent 1</param>
        /// <param name="p2">Indice of Parent 2</param>
        /// <param name="i">Indice of 1st new offspring</param>
        /// <returns>Fitness of best of new offsprings</returns>
        static int onePointCrossover(int p1, int p2, int i)
        {
            bool[,] temp1;
            bool[,] temp2;
            bool par = true;
            int nextCut = rnd.Next(nTiles * Program.col); 
            temp1 = new bool[nTiles, Program.col];
            temp2 = new bool[nTiles, Program.col];
            for (int k = 0; k < nTiles; k++)
            {

                for (int j = 0; j < Program.col; j++)
                {   
                    if(k*j> nextCut)
                    {
                        par = !par;
                    }
                    if (par)
                    {
                        temp1[k, j] = individual[p1].getGene(k, j);
                        temp2[k, j] = individual[p2].getGene(k, j);
                    }
                    else
                    {
                        temp1[k, j] = individual[p2].getGene(k, j);
                        temp2[k, j] = individual[p1].getGene(k, j);
                    }


                    if (rnd.NextDouble() < mu)
                    {
                        temp1[k, j] = !temp1[k, j];
                    }
                    if (rnd.NextDouble() < mu)
                    {
                        temp2[k, j] = !temp2[k, j];
                    }
                }

            }
            offspring[i] = new Coverage(nTiles, temp1, fp, overlap);
            offspring[i + 1] = new Coverage(nTiles, temp2, fp, overlap);

            if(offspring[i].getFit()>offspring[i+1].getFit())
            {   
                if(offspring[i].getFit()> bestSol)
                {
                    bestSol = offspring[i].getFit();
                    bestIndividual = offspring[i];
                }
                return offspring[i].getFit();
            }
            else
            {
                if (offspring[i+1].getFit() > bestSol)
                {
                    bestSol = offspring[i+1].getFit();
                    bestIndividual = offspring[i+1];
                }
                return offspring[i + 1].getFit();
            }

        }
        /// <summary>
        /// Create offsprings
        /// </summary>
        /// <param name="poolSize">Size of the mate pool</param>
        /// <returns>Fitness of best offspring of the new population</returns>
        private int offspringCreation(int poolSize)
        {
            int p1, p2;
            int best = 0;
            int temp;

            for(int i=0; i+1<popsize; i++)
            {
                p1 = tournament(poolSize);
                p2 = tournament(poolSize);
                temp  = uniCrossover(p1, p2, i, 0.5);
                //temp = onePointCrossover(p1, p2, i);
                if (temp > best)
                {
                    best = temp;
                }
            }



            return best;

        }
        /// <summary>
        /// Return best individual of the population
        /// </summary>
        /// <returns>Best individual of the population</returns>
        public Coverage getBest()
        {
            return bestIndividual;
        }
        /// <summary>
        /// Survivor replace individuals by offsprings
        /// </summary>
        private void survivor()
        {
            individual = offspring;
        }
        /// <summary>
        /// Replace the population by a new one
        /// </summary>
        /// <returns>Return fitness of best offspring of the new population</returns>
        public int newGeneration()
        {   
            
            int best;
            best = offspringCreation(2);
            generations++;
            survivor();
            return best;
        }
    }
}
