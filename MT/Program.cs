using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;


namespace MT
{
    class Program
    {


        static public Stopwatch globWatch;
        static public Stopwatch globWatch2;
        static public bool[,] dataset;
        static public bool[] targetVar;
        static public int rows, col, nTiles;
        static public int trues = 0, falses = 0;
        static int oriCol;
        static public int plus = 0, minus = 0;
        static string problem = "BMF";
        //static int it;
        static float sparsity;

        //static public int minSize=2;

        static public float fp=0;
        //static Tiling Sol;
        static public Random rnd;
        //static public int overlap=0;
        static List<ClassVar> varval; 

        // static int bestfit;
        //static int delta;

            static private void fillVarval(string filename)
        {   
            varval = new List<ClassVar>();
            string[] temp;
            string line;
            rows = 1;
            //col = 0;
            //
            //  This part of the procedure count the number of columns and rows of the data set
            //  It also fill an array of the different classes of each class variables
            //
            StreamReader fs = new StreamReader(filename);
            line = fs.ReadLine();
            //rows += 1;
            temp = line.Split(new Char[] { ',', ' ' });
            col = temp.Length;
            oriCol = col;
            int e;
            foreach (string s in temp)
            {
                varval.Add(new ClassVar(s));

            }

            //Console.WriteLine(line);
            while (!fs.EndOfStream)
            {
                line = fs.ReadLine();
                temp = line.Split(new Char[] { ',', ' ' });
                e = 0;
                bool eq;
                foreach (string s in temp)
                {
                    eq = false;
                    for (int f = 0; f < varval.ElementAt(e).getClass(); f++)
                    {
                        if (s == varval.ElementAt(e).values.ElementAt(f).GetVal())
                        {
                            eq = true;
                            varval.ElementAt(e).values.ElementAt(f).count++;
                            break;
                        }
                    }
                    if (!eq)
                    {
                        varval.ElementAt(e).AddVal(s);
                    }

                    e++;
                }
                rows++;


            }
            fs.Close();
        }

        static private void fillBinary(string[] temp, int i)
        {
            for (int j = 0; j < (temp.Length); j++)
            {
                if (temp[j] == varval.ElementAt(j).values.ElementAt(0).GetVal())
                {
                    dataset[i,j] = !varval.ElementAt(j).values.ElementAt(0).isFalse();

                }
                else
                {
                    dataset[i, j] = varval.ElementAt(j).values.ElementAt(0).isFalse();

                }
                if(dataset[i, j])
                {
                    trues++;
                }
                else
                {
                    falses++;
                }
            }
            
        }

        static private void fillOneHotTarget(string[] temp, int i)
        {
                if (temp[temp.Length - 1] == varval.ElementAt(temp.Length - 1).values.ElementAt(0).GetVal())
                {
                    targetVar[i] = !varval.ElementAt(temp.Length - 1).values.ElementAt(0).isFalse();

                }
                else
                {
                    targetVar[i] = varval.ElementAt(temp.Length - 1).values.ElementAt(0).isFalse();

                }
                if (targetVar[i])
                {
                    plus++;
                }
                else
                {
                    minus++;
                }
                fillOneHot(temp, i, 1);
            
        }

        static private void fillOneHot(string[] temp, int i, int a)
        {
            int e;
            int e2 = 0;
            for (int j = 0; j < (temp.Length - a); j++)
            {
                e = 0;

                while (e < varval.ElementAt(j).values.Count())
                {
                    if (temp[j] == varval.ElementAt(j).values.ElementAt(e).GetVal())
                    {
                        dataset[i, e2] = true;
                        trues++;
                    }
                    else
                    {
                        dataset[i, e2] = false;
                        falses++;
                    }
                    e++;
                    e2++;
                }

            }
        }


        /// <summary>
        /// input function open the file entered by the user (through the console),
        //      - compute the number of columns and rows,
        //      - fill the dataset array with the values from the file
        /// </summary>
        static private bool input(string fn, string rep) {

            col = 0;
            rows = 0;
            string[] temp;
            string line;
            string filename;
            StreamReader fs;
            if (fn == null)
            {
                Console.WriteLine("Give the dataset name :");
                filename = Console.ReadLine();
            }
            else
            {
                filename = fn;
            }


            if (File.Exists(filename))
            {

                fillVarval(filename);

                //
                //  This part of the procedure will create new features to have only binary variables
                //  It also fill the dataset array
                //
                if (rep != "binary")
                {
                    col = 0;
                    foreach (ClassVar s in varval)
                    {
                        col += (s.getClass());
                        //s.sort();

                    }

                    if (rep == "target")
                    {
                        if (!varval.Last().isBool())
                        {
                            Console.WriteLine("Last column isn't boolean");
                            return false;
                        }
                        else
                        {
                            targetVar = new bool[rows];
                            col -= 2;
                        }
                    }
                }
                else
                {
                    foreach (ClassVar s in varval)
                    {
                        if (!s.isBool())
                        {
                            Console.WriteLine("At least one column isn't boolean");
                            return false;
                        }

                    }
                }

                dataset = new bool[rows,col];
                
                fs = new StreamReader(filename);
                Console.WriteLine("There are " + col + " columns and " + rows + " rows");
                trues = 0;
                falses = 0;

                char[] sep = new char[] { ',', ' ' };
                for (int i = 0; i < rows; i++)
                {
                    line = fs.ReadLine();
                    temp = line.Split(sep);
                    switch (rep)
                    {
                        case "oneHot": fillOneHot(temp, i, 0);
                            break;
                        case "binary": fillBinary(temp, i);
                            break;
                        case "target": fillOneHotTarget(temp, i);
                            break;
                    }
   
                }
                if (rep == "target")
                {
                    Console.WriteLine("plus : " + plus + "   minus : " + minus);
                }
                fs.Close();
                sparsity = (float)trues / (trues + falses);

                //Console.ReadLine();
                return true;
            }
            else
            {
                Console.WriteLine("File doesn't exist");
                return false;

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string[] datasets = { /*"animals.txt" , "flare.data"/*, "house-votes-84.data" ,*/ "tic-tac-toe.data"/*, "nursery.data"/*, "kr-vs-kp.data", "agaricus-lepiota.data"*/};
            int[] nTileSizes = {20};
            double m, columns, cover;
            rnd = new Random();
            //int round=0;
            Output o = new Output();
            string outputFile = "tictacBMF.csv";
            
            //o.output("sep=,", outputFile);
            //string[] prob = { "Disc", "Tiling", "Block", "BMF" };
            string rep;

            int pop = 200;
            int off = pop;
            //string temp;
            TimeSpan ts;
            string elapsedTime;
            globWatch = new Stopwatch();
            globWatch2 = new Stopwatch();
            //columns = 0.02;
            //int delta = 0;
            //double CurrSol;
            Coverage best;

            string line;
            Stopwatch stopWatch = new Stopwatch();
            nTiles =5;
            
            //string s = null;
            switch (problem)
            {
                case "Disc": rep = "target";
                    break;
                case "Block": rep = "binary";
                    break;
                default: rep = "oneHot";
                    break;
            }
            foreach (string s in datasets)
            {
                if (input(s, rep))
                {
                    //o.output("flarehot.data");
                    foreach (int b in nTileSizes)
                {
                    for (int c = 1; c <= 20; c++)
                    {
                        nTiles = c;
                        for (int a = 0; a < 5; a++)
                        {
                            columns = (double)2.5 / col;
                            m = 0.3*Math.Pow(0.5,((double)nTiles/5));//1.5 / nTiles;
                            stopWatch.Start();
                            Population population = new Population(pop, off, columns, 0, m, problem);
                            //Population population2 = new Population(pop, off, columns, 0, m, problem);


                            //bestSol = 0;
                            //delta = 0;
                            while (population.getGenWOImp() < 15 )//|| population2.getGenWOImp() < 15)
                            {
                                population.newGeneration();
                                //population2.newGeneration();
                                /*
                                for (int q = 0; q < 1; q++)
                                {
                                    population.setRandom(population2.getBest());
                                    population2.setRandom(population.getBest());
                                }
                                */
                            }
                            /*
                            if(population.getBest().getFit() > population.getBest().getFit())
                            {
                                best = population.getBest();
                            }
                            else
                            {
                                best = population2.getBest();
                            }
                            */
                            stopWatch.Stop();
                            ts = stopWatch.Elapsed;
                            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                            ts.Hours, ts.Minutes, ts.Seconds,
                            ts.Milliseconds / 10);
                            best = population.getBest();
                            //best.print();
                            //best.getDisc();
                            cover = (double)best.getFit() / trues;
                            line = s + ' ' + best.getFit().ToString() + ' ' + nTiles.ToString() + ' ' + pop.ToString() + /*' ' + off.ToString() +*/ ' ' + best.getOverlap() + ' ' + cover.ToString() + ' ' + population.getIt().ToString() + ' ' + elapsedTime + ' ' + m.ToString();
                            Console.WriteLine(line);
                            o.output(line, outputFile);
                            
                            //o.outputTiling(best, outputFile);

                            stopWatch.Reset();
                        }
                    }
                }
                }
            }
            

            Console.WriteLine("Ended");
            Console.ReadLine();

        }
    }
}
