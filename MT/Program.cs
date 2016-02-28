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
        
        
        
        static public bool[,] dataset;
        static public int rows, col;
        static int trues = 0, falses = 0;
        static int oriCol;
        static int it;
        static float sparsity;
        static Tiling Sol;
        static public Random rnd;

        // static int bestfit;
        //static int delta;





        /// <summary>
        /// input function open the file entered by the user (through the console),
        //      - compute the number of columns and rows,
        //      - fill the dataset array with the values from the file
        /// </summary>
        static bool input(string fn) {
            
            string[] temp;
            List<ClassVar> varval = new List<ClassVar>();
            rows = 0;
            col = 0;
            string filename;
            string line;
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
                //
                //  This part of the procedure count the number of columns and rows of the data set
                //  It also fill an array of the different classes of each class variables
                //
                StreamReader fs = new StreamReader (filename);
                line = fs.ReadLine();
                rows += 1;
                temp = line.Split(new Char[] { ',', ' '});
                col = temp.Length;
                oriCol = col;
                int e;
                foreach(string s in temp)
                {
                    varval.Add(new ClassVar(s));

                }

                //Console.WriteLine(line);
                while (!fs.EndOfStream)
                {
                    line = fs.ReadLine();
                    temp = line.Split(new Char[] { ',' ,' '});
                    e = 0;
                    bool eq;
                    foreach (string s in temp)
                    {
                        eq = false;
                        for(int f=0; f<varval.ElementAt (e).getClass();f++) 
                        {
                            if (s == varval.ElementAt (e).values.ElementAt (f).GetVal ())
                            {
                                eq = true;
                                ClassValue t = varval.ElementAt(e).values.ElementAt(f);
                                t.count++;
                                varval.ElementAt(e).values.RemoveAt(f);
                                varval.ElementAt(e).values.Insert(f, t);
                                break;
                            }
                        }
                        if (!eq) {
                            varval.ElementAt(e).AddVal(s);
                        }

                        e++;
                    }
                    rows += 1;


                }

                //
                //  This part of the procedure will create new features to have only binary variables
                //  It also fill the dataset array
                //
                col = 0;
                foreach(ClassVar s in varval)
                {
                    col += (s.getClass());
                    s.sort();
                    
                }
 
                dataset = new bool[rows,col];
                fs.Close();
                fs = new StreamReader(filename);
                Console.WriteLine("There are " + col + " columns and " + rows + " rows");
                int e2;
                for(int i=0; i< rows; i++)
                {
                    line = fs.ReadLine();


                    temp = line.Split(new Char[] { ',', ' ' });
                    
                    e2 = 0;
                        for(int j=0; j<temp.Length; j++)
                        {
                            e = 0;
  
                        while (e<varval.ElementAt(j).values.Count())
                        {
                            if (temp[j] == varval.ElementAt(j).values.ElementAt(e).GetVal())
                            {
                                dataset[i, e2] = false;
                                falses++;
                                
                            }
                            else
                            {
                                dataset[i, e2] = true;
                                trues++;
                                
                            }
                            e++;
                            e2++;
                        }

                    }                  
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





        
        

        

        static void Main(string[] args)
        {
            string[] datasets = { "animals.txt", "flare.data", "tic-tac-toe.data", "nursery.data", "house-votes-84.data", "kr-vs-kp.data", "agaricus-lepiota.data" };
            int[] nTileSizes = { 5, 10, 15, 20, 25 };
            int n;
            //int nt = 10;
            double m, columns, cover;
            int d;
            rnd = new Random();
            it = 0;
            Output o = new Output();
            int pop = 100;
            int off = 100;
            Tile so;
            TimeSpan ts;
            string elapsedTime;
            MaxTiling problem;
            MaxTile p;
            m = 0.02;
            columns = 0.01;
            d = 1;
            string line;
            Stopwatch stopWatch = new Stopwatch();
            //foreach (string s in datasets)
            //{
            
            //string s = null;
                if (input(null))
                {
                Population population = new Population(pop, off, columns, 0, 0, m, d);
                for (int i=0; i<10; i++)
                {
                    Console.WriteLine("Best solution : "+ population.newGeneration());
                }
                Coverage best = population.getBest();
                Console.WriteLine("Best = " + best.getFit());    
                /*
                    foreach (int i in nTileSizes)
                    {
                        n = 0;
                        while (n < 1)
                        {
                            stopWatch.Start();
                            //p = new MaxTile(columns, 0, false, new Tiling(), pop, off);
                            //so = p.solve(d, columns, m);
                            //problem = new MaxTiling(pop, off);
                            //Sol = problem.SolveFix(i, m, d, columns, pop, off, false);
                            //Sol = problem.solve(columns, i, d, m, 0, false);
                            //cover = (double)Sol.getSize() / trues;
                            //it = problem.getGen();
                            stopWatch.Stop();
                            ts = stopWatch.Elapsed;
                            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                            ts.Hours, ts.Minutes, ts.Seconds,
                            ts.Milliseconds / 10);
                            line = n.ToString() +' '+ s +' ' + Sol.getSize().ToString() + ' ' + it.ToString() + ' ' + pop.ToString() + ' ' + off.ToString() + ' ' + d.ToString() + ' ' + columns.ToString() + ' ' + m.ToString() + ' ' + cover.ToString() + ' ' + i.ToString() + ' ' + elapsedTime;
                            Console.WriteLine(line);
                            //o.output(line, "maxktiling.csv");
                            //Console.WriteLine("RunTime " + elapsedTime);
                            //o.outputTiling(Sol, "outputTilingTest.csv");
                            //o.output("Nursery.csv");
                            stopWatch.Reset();
                            n++;

                        }
                    }*/

                }
            //}
            Console.WriteLine("Ended");
            Console.ReadLine();

        }
    }
}
