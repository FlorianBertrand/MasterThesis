using System;
using System.Diagnostics;


namespace MT
{
    public class Population
    {
        Coverage[] individual;
        Coverage[] offspring;
        double bestSol;
        int generations;
        int offsize;
        int popsize;
        string problem;
        double delta;
        int genWOImp;
        double decay;
        //Random rnd;
        int overlap;
        //double fp;
        double colgen;
        double mu;
        private Coverage bestIndividual;
        public int test2, test1=0;
        double sigma = 1;
        int it;

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
        public Population(int pop, int off, double seed, int o, double m, string p)
        {

            problem = p;
            generations = 0;
            offsize = off;
            popsize = pop;
            individual = new Coverage[popsize];
            offspring = new Coverage[offsize];
            //rnd = new Random();
            //fp = f;
            colgen = seed;
            overlap=o;
            mu = m;
            genWOImp = 0;
            delta = 1;
            generations = 0;
            decay = 1;
            it = 0;
            bestSol = 0;
            switch (problem)
            {
                case "Tiling":
                    initTiling(seed);
                    break;
                case "BMF":
                    initBMF(seed);
                    break;
                case "Disc":
                    initDisc(seed);
                    break;
                case "Block":
                    initBlock(seed);
                    break;
                default:
                    Console.WriteLine("Wrong problem name");
                    throw new WrongProblemException();
     
            }


        }

        private bool checkBestSol(int l)
        {
            if (individual[l].getFit() > bestSol)
            {
                bestSol = individual[l].getFit();
                bestIndividual = individual[l];
                return true;
            }
            else
            {
                return false;
            }
        }

        private void setDecay(double dec)
        {
            decay = dec * delta + (1 - dec) * decay;
        }

        private double sigmoid(int k, double mu, double lambda)
        {
            return (1 / (1 + Math.Pow(2.71828, (lambda * (k - mu)))));
        }

        /// <summary>
        /// This function initialize the population
        /// </summary>
        private void initTiling(double seed)
        {

            for (int i = 0; i < popsize; i++)
            {
                individual[i] = new Tiling(seed);
                checkBestSol(i);
            }

        }
        /// <summary>
        /// This function initialize the population
        /// </summary>
        private void initBMF(double seed)
        {

            for (int i = 0; i < popsize; i++)
            {
                individual[i] = new BMF(seed);
                checkBestSol(i);
            }

        }
        /// <summary>
        /// This function initialize the population
        /// </summary>
        private void initDisc(double seed)
        {

            for (int i = 0; i < popsize; i++)
            {
                individual[i] = new Disc(seed);
                checkBestSol(i);
            }

        }
        /// <summary>
        /// This function initialize the population
        /// </summary>
        private void initBlock(double seed)
        {

            for (int i = 0; i < popsize; i++)
            {
                individual[i] = new Block(seed);
                checkBestSol(i);
            }

        }


        /// <summary>
        /// This function select randomly poolSize individuals and returns the fittest as parent
        /// </summary>
        /// <param name="poolSize">number of individuals in the mating pool</param>
        /// <returns>Best of poolSize random individuals</returns>
        private int tournament(int poolSize)
        {
            int best, r, s;
            s = 0;
            best = Program.rnd.Next(popsize);
            while (s < poolSize - 1)
            {
                r = Program.rnd.Next(popsize);
                if (individual[r].getFit() > individual[best].getFit())
                {
                    best = r;
                }
                s++;
            }
            return best;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="k"></param>
        /// <param name="m"></param>
        /// <param name="sig"></param>
        /// <returns></returns>
        private double gauss(double k, double m, double sig)
        {
            return ((1 / (sig * Math.Sqrt((2 * 3.1416)))) * Math.Pow(2.71828,(-0.5 * Math.Pow(((k - m) / sig),2))));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="k"></param>
        /// <param name="i"></param>
        /// <param name="b"></param>
        private void macrossover(int p1, int p2, int k, int i, bool b)
        {
            //test2++;
            if (b)
            {
                offspring[i].addComp(individual[p1].getComp(k).getCopy());
                offspring[i + 1].addComp(individual[p2].getComp(k).getCopy());
            }
            else
            {
                offspring[i].addComp(individual[p2].getComp(k).getCopy());
                offspring[i + 1].addComp(individual[p1].getComp(k).getCopy());
            }
            
            offspring[i].getComp(k).mutate(mu*1.1);
            offspring[i + 1].getComp(k).mutate(mu*1.1);
            

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="k"></param>
        /// <param name="i"></param>
        private void microssover(int p1, int p2, int k, int i)
        {
            //test1++;
            if (false)//individual[p1].getComp(k).getCol() == individual[p2].getComp(k).getCol() && individual[p1].getComp(k).getRow() == individual[p2].getComp(k).getRow())
            {
                macrossover(p1, p2, k, i, (Program.rnd.NextDouble() < 0.5));

            }
            else
            {
                bool[] temp1 = new bool[Program.col];
                bool[] temp2 = new bool[Program.col];

                for (int j = 0; j < Program.col; j++)
                {
                    if (Program.rnd.NextDouble() >= 0.5)
                    {
                        temp1[j] = individual[p1].getComp(k).getGene(j);
                        temp2[j] = individual[p2].getComp(k).getGene(j);
                    }
                    else
                    {
                        temp1[j] = individual[p2].getComp(k).getGene(j);
                        temp2[j] = individual[p1].getComp(k).getGene(j);
                    }
                    if (mu / Program.col > Program.rnd.NextDouble())
                    {
                        temp1[j] = !temp1[j];
                    }
                    if (mu / Program.col > Program.rnd.NextDouble())
                    {
                        temp2[j] = !temp2[j];
                    }


                }
                Comp tempComp1 = new Comp(temp1, offspring[i].getFp());
                Comp tempComp2 = new Comp(temp2, offspring[i+1].getFp());
                offspring[i].addComp(tempComp1);
                offspring[i + 1].addComp(tempComp2);
            }

        }



        /// <summary>
        /// Uniform Tile crossover
        /// </summary>
        /// <param name="p1">Indice of parent1 [0..Popsize-1]</param>
        /// <param name="p2">Indice of parent2 [0..Popsize-1]</param>
        /// <param name="i">Indice of first offspring [0..Offsize-2]</param>
        /// <param name="p">Probability of taking gene of parent1 [0..1]</param>
        /// <returns>Fitness of best new offspring [0..Inf]</returns>
        private void uniCrossover(int p1, int p2, int i, double p)
        {

            offspring[i] = individual[p1].newCov();
            offspring[i + 1] = individual[p2].newCov();

            for (int k = 0; k < Program.nTiles; k++)
            {

                if (Program.rnd.NextDouble()/* * gauss(0, 0, sigma) */> gauss(k, ((int)(decay * (Program.nTiles - 1))), sigma))
                {
                    macrossover(p1, p2, k, i, (Program.rnd.NextDouble() < 0.5));

                }
                else
                {

                    microssover(p1, p2, k, i);
                }
            }
            offspring[i].checkConstraints();
            offspring[i + 1].checkConstraints();


        }

        /// <summary>
        /// One point crossover
        /// </summary>
        /// <param name="p1">Indice of Parent 1</param>
        /// <param name="p2">Indice of Parent 2</param>
        /// <param name="i">Indice of 1st new offspring</param>
        /// <returns>Fitness of best of new offsprings</returns>
        private void onePointCrossover(int p1, int p2, int i)
        {
            offspring[i] = individual[p1].newCov();
            offspring[i + 1] = individual[p2].newCov();
            int nextCut = Program.rnd.Next(Program.nTiles); 

            for (int k = 0; k < Program.nTiles; k++)
            {
                if (Program.rnd.NextDouble()/**gauss(0,0,sigma)*/ > gauss(k, ((int)(decay) * (Program.nTiles-1)), sigma))
                {

                    macrossover(p1, p2, k, i, (k > nextCut));

                }
                else
                {
                    microssover(p1, p2, k, i);
                }

            }
            offspring[i].checkConstraints();
            offspring[i + 1].checkConstraints();
        }
        
        /// <summary>
        /// Create offsprings
        /// </summary>
        /// <param name="poolSize">Size of the mate pool</param>
        /// <returns>Fitness of best offspring of the new population</returns>
        private void offspringCreation(int poolSize)
        {
            int p1, p2;


            for(int i=0; i<(popsize-1); i+=2)
            {
                p1 = tournament(poolSize);
                p2 = tournament(poolSize);

                //uniCrossover(p1, p2, i, 0.5);
                onePointCrossover(p1, p2, i);
            }

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
        private int survivor()
        {
            double best=0;
            int bestI=0;
            individual = new Coverage[popsize];
            individual = offspring;
            offspring = new Coverage[offsize];
            for(int l = 0; l<popsize; l++)
            {
                if (individual[l].getFit() > best)
                {
                    best = individual[l].getFit();
                    bestI = l;
                }
            }
            
            return bestI;
        }

        /// <summary>
        /// Return a random individual
        /// </summary>
        /// <returns></returns>
        public Coverage getRandom()
        {
            return individual[Program.rnd.Next(popsize)];
        }
        /// <summary>
        /// Set a random individual to c
        /// </summary>
        /// <param name="c">Individual to integrate randomly in the population</param>
        public void setRandom(Coverage c)
        {
            individual[Program.rnd.Next(popsize)] = c;
        }
        public int getGenWOImp()
        {
            return genWOImp;
        }

        public int getIt()
        {
            return it;
        }

        /// <summary>
        /// Replace the population by a new one
        /// </summary>
        /// <returns>Return fitness of best offspring of the new population</returns>
        public double newGeneration()
        {


            it++;            
            offspringCreation(10);
            generations++;
            int l = survivor();
            
            if (checkBestSol(l))
            {
                genWOImp = 0;
                delta = ((double)individual[l].getFit() / bestSol)-1;
            }
            else
            {
                genWOImp++;
                delta = 0;
            }
            setDecay(0.05);
            //Console.WriteLine((1 - decay) * Program.nTiles + " best = " +bestSol+" bestGen = "+ individual[l].getFit() +" genWOImp = " +genWOImp);
            //Console.WriteLine((1 - decay) * Program.nTiles);
            //Console.WriteLine(test1 + " " + test2);
            /*
            if (decay < 0.1)
            {
                bestIndividual.print();
            }
            */
            //Console.WriteLine("mutation rate:" + bestIndividual.getMutation());
            return individual[l].getFit();
        }


    }
}
