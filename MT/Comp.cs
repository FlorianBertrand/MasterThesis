using System;
using System.Collections.Generic;

namespace MT
{
    /// <summary>
    /// A component (Comp) corresponds to a set of column and a set of rows
    /// </summary>
    public class Comp
    {
        bool[] geno;
        bool[] pheno;
        int size;
        int cols;
        int rows;
        int fp=0;

        public Comp(double seed, int n)
        {
            rows = 0;
            cols = 0;
            size = 0;
            fp = 0;
            geno = new bool[Program.col];
            pheno = new bool[Program.rows];
            for (int j = 0; j < Program.col; j++)
            {
                if (Program.rnd.NextDouble() < seed)
                {
                    geno[j] = true;
                    cols++;
                }
                else
                {
                    geno[j] = false;
                }

            }
            
            infer(n);
        }

        public Comp(bool[] g, int n)
        {
            rows = 0;
            cols = 0;
            size = 0;
            fp = 0;
            geno = g;
            for(int j=0; j<Program.col; j++)
            {
                if (geno[j])
                {
                    cols++;
                }
            }
            pheno = new bool[Program.rows];
            infer(n);

        }

        private Comp(bool[] g, bool[] p, int s, int c, int r, int f)
        {
            geno = g;
            pheno = p;
            size = s;
            cols = c;
            rows = r;
            fp = f;
        }
        
        private void infer(int p)
        {   
            if(cols <=1)
            {
                addCol();
            }
            
            int f;
            for (int i = 0; i < Program.rows; i++)
            {
                f = p;
                for (int j = 0; j < Program.col; j++)
                {
                    if (geno[j])
                    {
                        if (!Program.dataset[i, j])
                        {
                            f--;
                        }

                    }

                }
                if (f >= 0)
                {
                    pheno[i] = true;
                    rows++;
                    fp+=(p - f);

                }
                else
                {
                    pheno[i] = false;
                }
            }
            size = rows * cols - fp;
            
            if (size == 0)
            {
                //Console.WriteLine("DelCol Infer");
                delCol();
                /*
                int temp = Program.rnd.Next(Program.col);
                for (int j = 0; j < Program.col; j++)
                {
                    if (getGene(j))
                    {
                        temp--;
                        if (temp == 0)
                        {
                            delCol(j);
                            break;
                        }

                    }
                }
                */
            }
            

        }

        public bool getGene(int j)
        {
            return geno[j];
        }

        public bool getPheno(int i)
        {
            return pheno[i];
        }

        public bool[] getAllGenes()
        {
            return geno;
        }

        public int getSize()
        {
            return size;
        }

        public int getRow()
        {
            return rows;
        }

        public int getCol()
        {
            return cols;
        }

        public void printBlock()
        {
            Console.WriteLine("Size:" + size + " nCol:" + cols + " nRows:" + rows+ " fp:"+fp);
        }

        public Comp getCopy()
        {
            bool[] tempG = new bool[Program.col];
            bool[] tempP = new bool[Program.rows];
            for(int j=0; j<Program.col; j++)
            {
                tempG[j] = geno[j];
            }
            for(int i=0; i<Program.rows; i++)
            {
                tempP[i] = pheno[i];
            }
            return new Comp(tempG, tempP, size, cols, rows, 0);
        }
        /*
        public void addCol(int j)
        {
            if (!geno[j])
            {
                geno[j] = true;
                cols++;
                int f = 0;
                for (int i = 0; i < Program.rows; i++)
                {
                    if (pheno[i])
                    {
                        if (!Program.dataset[i, j])
                        {
                            f++;
                        }

                    }
                }
                size += (rows - f);
                fp += f;
            }

        }
        */
        public void addCol(int j, int af)
        {
            if (!geno[j])
            {
                geno[j] = true;
                cols++;
                size += rows;
                if (af == 0)
                {
                    
                    for (int i = 0; i < Program.rows; i++)
                    {
                        if (pheno[i])
                        {
                            //Console.WriteLine(size);
                            if (!Program.dataset[i, j])
                            {
                                //Console.WriteLine(size);
                                pheno[i] = false;
                                size -= cols;
                                rows--;
                            }

                        }
                    }
                }
                
                /*
                else
                {
                    Console.WriteLine("warning");
                    for (int i = 0; i < Program.rows; i++)
                    {
                        if (pheno[i])
                        {
                            int f = 0;
                            for (int j2 = 0; j2 < Program.col; j2++)
                            {
                                if (!Program.dataset[i, j2] && geno[j2])
                                {
                                    f++;
                                }
                            }
                            if (f > af)
                            {
                                pheno[i] = false;
                                size -= cols;
                            }
                            else
                            {
                                if (!Program.dataset[i, j])
                                {
                                    size -= 1;
                                }
                            }

                        }
                    }
                }
                */
            }
            
        }
        public void delCol(int j)
        {
            if (geno[j])
            {
                geno[j] = false;
                //Console.WriteLine(cols);
                
                cols--;
                int f = 0;
                
                /*
                if (fp > 0)
                {
                    Console.WriteLine("Warning");
                    for (int i = 0; i < Program.rows; i++)
                    {
                        if (pheno[i])
                        {
                            if (!Program.dataset[i, j])
                            {
                                f++;
                            }
                        }
                    }
                }
                */
                fp -= f;
                
                size -= (rows - f);
                
            }

        }

        public void delCol(int j, int p)
        {
            if (geno[j])
            {
                geno[j] = false;
                cols--;
                size = 0;
                fp = 0;
                rows = 0;
                infer(p);
            }

        }
        /*
        public void addRow(int i)
        {
            if (!pheno[i])
            {
                pheno[i] = true;
                rows++;
                int f = 0;
                for (int j = 0; j < Program.col; j++)
                {
                    if (geno[i])
                    {
                        if (!Program.dataset[i, j])
                        {
                            f++;
                        }
                    }
                }
                size += (cols - f);
                fp += f;
            }

        }
        */
        
        public void delRow(int i)
        {
            if (pheno[i])
            {
                //Console.WriteLine("print row");
                pheno[i] = false;
                //Console.WriteLine(rows);
                rows--;
                int f = 0;
                /*
                if (fp > 0)
                {
                    Console.WriteLine("Warning");
                    for (int j = 0; j < Program.col; j++)
                    {
                        if (geno[i])
                        {
                            if (!Program.dataset[i, j])
                            {
                                f++;
                            }
                        }
                    }
                }
                */
                //Console.WriteLine(size);
                size -= (cols - f);
                fp -= f;
                //Console.WriteLine(cols + " " + size + " " + rows);
            }
        }
        
        public void mutate(int j)
        {
            
            if (getGene(j))
            {
                delCol(j,0);

            }
            else
            {
                addCol(j, 0);
            }

        
        }

        public void delRow(List<int> l)
        {
            foreach (int i in l)
            {
                delRow(i);
            }
        }

        public void delCol(List<int> l)
        {
            foreach (int j in l)
            {
                delCol(j);
            }
        }

        public void mutate()
        {
            mutate(Program.rnd.Next(Program.col));
            
        }

        public void mutate(double mu)
        {   
            /*
            if (rows == 0)
            {
                delCol();
                return;
            }
            */
            if (Program.rnd.NextDouble() < mu)
            {
                mutate();
            }
        }

        private void delCol()
        {
            int temp = Program.rnd.Next(cols);
            for (int j = 0; j < Program.col; j++)
            {
                if (getGene(j))
                {
                    temp--;
                    if (temp == 1)
                    {
                        delCol(j);
                        //Console.WriteLine("DelCol");
                        break;
                    }

                }
            }
        }

        private void addCol()
        {
            int a;
            do
            {
                a = Program.rnd.Next(Program.col);


            } while (geno[a]);
            geno[a] = true;
            cols++;
        }

    }
}