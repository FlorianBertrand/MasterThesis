using System;
using System.Collections.Generic;
namespace MT
{
    public class Coverage
    {
        static bool[,] geno;
        static bool[,] pheno;
        static int[,] rec;
        static int size;
        static int nTiles;
        static int overlap;
        static int fp;
        double fpAllowed;
        static int[] cols;
        static int[] rows;
        static int[] fpRows;
        /// <summary>
        /// Instanciate a coverage with a bit probability of seed
        /// </summary>
        /// <param name="n">Number of tiles</param>
        /// <param name="seed">Probability of setting a gene to 1</param>
        /// <param name="f">number of false positives/row allowed</param>
        /// <param name="o">number of overlaps allowed </param>
        public Coverage(int n, double seed, double f, int o)
        {
            size = 0;
            nTiles = n;
            Random rnd = new Random();
            cols = new int[nTiles];
            rows = new int[nTiles];
            fpRows = new int[Program.rows];
            geno = new bool[nTiles,Program.col];
            pheno = new bool[nTiles,Program.rows];
            rec = new int[Program.rows, Program.col];
            overlap = o;
            fp = 0;
            fpAllowed = f;
            arraysInit();
            for (int k = 0; k < nTiles; k++)
            {
                for (int j = 0; j < Program.col; j++)
                {
                    if (rnd.NextDouble() < seed)
                    {
                        geno[k, j] = true;
                        cols[k]++;
                    }
                    else
                    {
                        geno[k, j] = false;
                    }
                }
            }
            infer();


        }
        public void print()
        {
            for(int j=0; j<Program.col; j++)
            {
                Console.Write(geno[0, j]);
            }
            Console.WriteLine("");
        }
        /// <summary>
        /// Initialize a new individual with an array
        /// </summary>
        /// <param name="n">Number of tiles</param>
        /// <param name="g">Genotype of the individual</param>
        /// <param name="f">number of false positives/row allowed</param>
        /// <param name="o">number of overlaps allowed</param>
        public Coverage(int n, bool[,] g, double f, int o)
        {
            size = 0;
            nTiles = n;
            Random rnd = new Random();
            cols = new int[nTiles];
            rows = new int[nTiles];
            fpRows = new int[Program.rows];
            geno = g;
            rec = new int[Program.rows, Program.col];
            pheno = new bool[nTiles, Program.rows];
            overlap = o;
            fp = 0;
            fpAllowed = f;
            arraysInit();
            infer();

        }
        /// <summary>
        /// Infer rows from the columns: a row is selected if there are less false positives on the row that what is allowed (regarding columns of the genotype)
        /// </summary>
        private void infer()
        {   
            
            int f;
            for(int i=0; i<Program.rows; i++)
            {
                for(int j=0; j<Program.col; j++)
                {
                    rec[i, j] = 0;
                }
            }
            for (int k = 0; k < nTiles; k++)
            {
                cols[k] = 0;
                rows[k] = 0;
                for (int j = 0; j < Program.col; j++)
                {
                    if (geno[k, j])
                    {
                        cols[k]++;
                    }
                }
                if (cols[k] != 0)
                {
                    for (int i = 0; i < Program.rows; i++)
                    {
                        f = 0;
                        for (int j = 0; j < Program.col; j++)
                        {
                            if (geno[k, j])
                            {
                                if (!Program.dataset[i, j])
                                {
                                    f++;
                                }
                            }
                        }
                        if ((f / cols[k]) < fpAllowed)
                        {
                            pheno[k, i] = true;
                            for(int j=0; j<Program.col; j++)
                            {
                                if(geno[k, j])
                                {
                                    rec[i, j]++;
                                }
                            }
                            rows[k]++;
                        }
                        else
                        {
                            pheno[k, i] = false;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// remove rows and/or columns to lose ones as less as possible while meeting the overlaps constraint
        /// </summary>
        private void repair()
        {
            int[,] oCols = new int[nTiles,Program.col];
            int[,] oRows = new int[nTiles,Program.rows];
            for(int k=0; k< nTiles; k++)
            {
                for(int j=0; j<Program.col; j++)
                {
                    oCols[k, j] = 0;
                    if (geno[k, j])
                    {
                        for(int i=0; i<Program.rows; i++)
                        {
                            if (pheno[k, i] && rec[i,j] > 1)
                            {
                                oCols[k, j]++;
                            }
                        }
                    }

                }

                for(int i=0; i<Program.rows; i++)
                {
                    oRows[k, i] = 0;
                    if (pheno[k, i])
                    {
                        for(int j=0; j<Program.col; j++)
                        {
                            if(geno[k,i] && rec[i, j] > 1)
                            {
                                oRows[k, i]++;
                            }
                        }
                    }

                }
            }

            for (int k = 0; k < nTiles; k++)
            {
                for (int j = 0; j < Program.col; j++)
                {
    

                }
                for (int i = 0; i < Program.rows; i++)
                {
 

                }

            }
        }
        private void arraysInit()
        {
            
            for(int k=0; k< nTiles; k++)
            {
                rows[k] = 0;
                cols[k] = 0;
            }
            
        }
        /// <summary>
        /// Return the gene of a tile
        /// </summary>
        /// <param name="k">Indice of the tile</param>
        /// <param name="j">Indice of the gene</param>
        /// <returns>Gene j of tile k</returns>
        public bool getGene(int k, int j)
        {
            return geno[k, j];
        }
        public void del(int Tile, int indice)
        {
            geno[Tile, indice] = false;
            infer();
        }
        public void add(int Tile, int indice)
        {
            geno[Tile, indice] = true;
            infer();
        }
        /*
        private int bestFlip()
        {

        }
        */
        public void localSearch()
        {

        }
        /// <summary>
        /// return fitness of the tile
        /// </summary>
        /// <returns>fitness</returns>
        public int getFit()
        {
            return size;
        }

    }
}
