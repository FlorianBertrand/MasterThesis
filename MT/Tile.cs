using System;
using System.Collections.Generic;
namespace MT
{
    /// <summary>
    /// Tile is the structure of a Tile, it is constructed with a set of columns
    /// it consists in:
    ///      - a genotype : its set of columns
    ///      - a phenotype : its set of rows associated to its columns
    ///      - a size : the number of booleans set to true within the tile
    /// </summary>
    public class Tile
    {
        bool[] geno;
        bool[] pheno;
        int size;
        int overlap;
        int fp;
        int cols;
        int rows;
        int[] fpRows;
        /// <summary>
        /// This constructor is meant to be used for tiling problems. 
        /// Given a genotype a, the phenotype will be construct to avoir overlapping with Tiles from b array
        /// </summary>
        /// <param name="a">Genotype of the tile</param>
        /// <param name="b">Array of tiles previously built -> problem constraints</param>
        /// <param name="fp">Percentage of falses allowed per rows</param>
        /// <param name="overlap">Bool set to true if tiles may overlap</param>
        public Tile(bool[] a, List<Tile> b, double fp, bool overlap, int nTile)
        {
           
            geno = new bool[Program.col];
            pheno = new bool[Program.rows];
            geno = a;
            fpRows = new int[Program.rows];
         
            //compute(a, b);
            int f;
            int nCol = 0;
            int nRows = 0;
            int accFp = 0;
            int o = 0;
            bool bo;
            for (int j = 0; j < Program.col; j++)
            {
                if (geno[j])
                {
                    nCol++;
                }
            }

            for (int i = 0; i < Program.rows; i++)
            {
                f = 0;
                for (int j = 0; j < Program.col; j++)
                {
                    if (geno[j])
                    {
                        if (!Program.dataset[i, j])
                        {
                            f++;
                        }

                    }

                }

                if (f <= (fp * nCol))
                {
                    pheno[i] = true;
                    accFp += f;
                    fpRows[i] = f;
                    nRows++;
                }
                else
                {
                    fpRows[i] = 0;
                    pheno[i] = false;
                }

            }
            if (overlap)
            {
                for (int j = 0; j < Program.col; j++)
                {
                    if (this.getGene(j))
                    {
                        for (int i = 0; i < Program.rows; i++)
                        {

                            if (pheno[i])
                            {
                                bo = false;
                                foreach (Tile t in b)
                                {
                                    if (t.getGene(j) && t.getPheno(i))
                                    {
                                        bo = true;
                                        break;
                                    }
                                }
                                if (bo && Program.dataset[i, j])
                                {
                                    o++;
                                }
                            }

                        }
                    }
                }
            }
            else
            {
                foreach (Tile t in b)
                {
                    for (int j = 0; j < Program.col; j++)
                    {
                        if (t.getGene(j) && this.getGene(j))
                        {
                            for (int i = 0; i < Program.rows; i++)
                            {
                                if (t.getPheno(i) && pheno[i])
                                {
                                    pheno[i] = false;
                                    fpRows[i] = 0;
                                    nRows--;
                                    for(int k=0; k<Program.col; k++)
                                    {
                                        if (this.getGene(k)&&!Program.dataset[i, k])
                                        {
                                            accFp--;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            this.size = (nRows * nCol) - accFp - o;
            this.rows = nRows;
            this.cols = nCol;
            this.overlap = o;
            this.fp = accFp;
        }

        


        public int getSize()
        {
            return size;
        }

        public bool getGene(int i)
        {
            return geno[i];
        }

        public bool getPheno(int i)
        {
            return pheno[i];
        }
        public bool[] getGeno()
        {
            return geno;
        }
        public void outputSizes()
        {
            Console.WriteLine("Rows=" + rows + " Cols=" + cols+" Fp="+fp+" Overlaps="+overlap);
        }
        private bool[] addCol()
        {
            int ind = Program.rnd.Next(Program.col-cols);
            int j = 0;
            for(int i=0; i<Program.col; i++)
            {
                if (!geno[i])
                {
                    
                    if (j == ind)
                    {
                        break;
                    }
                    j++;
                }

            }
            geno[j] = true;
            return this.geno;
        }

        private bool[] delCol()
        {
            int ind = Program.rnd.Next(cols);
            int j = 0;
            for (int i = 0; i < Program.col; i++)
            {
                if (geno[i])
                {
                    j++;
                    if (j == ind)
                    {
                        break;
                    }
                }

            }
            geno[j] = false;
            return this.geno;
        }

        public bool[] birth(double mu)
        {
            if (Program.rnd.NextDouble() < mu)
            {
                if((Program.rnd.NextDouble() < 0.5 || this.cols<=2) && this.rows>1)
                {
                    return addCol();
                }
                else
                {
                    return delCol();
                }
            }
            else
            {
                return this.geno;
            }

        }

        public void localSearch()
        {
            while (localSearchOneStep());
        }

        public bool localSearchOneStep()
        {
            int bestAdd = 0;
            int bestDel = 0;
            bool add;
            bool Optimum = false;
            int[] change = new int[Program.col];
            int[] holes = new int[Program.rows];
            for(int i=0; i < Program.rows; i++)
            {
                holes[i] = 0;
            }
            for(int i=0; i< Program.col; i++)
            {
                if (!geno[i])
                {
                    add = true;
                    for (int j = 0; j < Program.rows; j++)
                    {
                        if(pheno[j] && !Program.dataset[j, i])
                        {
                            add = false;
                            break;
                        }
                    }
                    if (add)
                    {
                        change[i] = rows;
                        bestAdd += rows;
                    }
                    else
                    {
                        change[i] = 0;
                    }

                }
                else
                {
                    for(int j =0; j<Program.rows; j++)
                    {
                        if(!pheno[j] && !Program.dataset[j, i])
                        {
                            holes[j] += 1;
                        }
                    }
                }
            }

            for(int i=0; i<Program.col; i++)
            {
                if (geno[i])
                {
                    change[i] = -rows;
                    for(int j=0; j<Program.rows; j++)
                    {
                        if (holes[j] == 1)
                        {
                            if (!Program.dataset[j, i])
                            {
                                change[i] += cols - 1;
                            }
                        }
                    }
                    if (change[i] > bestDel)
                    {
                        bestDel = change[i];
                    }
                }
            }
            if(bestDel<=0 && bestAdd <= 0)
            {
                Optimum = true;
            }
            else
            {
                if (bestAdd >= bestDel)
                {
                    for(int i=0; i < Program.col; i++)
                    {
                        if (change[i] == bestAdd)
                        {
                            geno[i] = true;
                            size += change[i];
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Program.col; i++)
                    {
                        if (change[i] == bestDel)
                        {
                            geno[i] = false;
                            size += change[i];
                            for(int j=0; j<Program.rows; j++)
                            {
                                if(holes[j]==1 && !Program.dataset[j, i])
                                {
                                    pheno[j] = true;
                                }

                            }

                        }
                    }
                }
            }
            return Optimum;
        }
    }
}