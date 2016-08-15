using System;
using System.Collections.Generic;
namespace MT
{   

    public abstract class Coverage
    {
        Comp[] cov;
        int[] nGenes;
        int[] nPheno;
        protected double size;
        protected int overlap;
        int fp;
        int over;
        double mutation;
        //Overlaps overlaps;
        int kT;
        double alpha=1;
        int pos, neg;


        public Coverage()
        {
            init();
        }
        protected void init()
        {
            over = 0;
            overlap = 0;
            fp = 0;
            kT = 0;
            size = 0;
            cov = new Comp[Program.nTiles];
            nGenes = new int[Program.col];
            nPheno = new int[Program.rows];
            for (int j = 0; j < Program.col; j++)
            {
                nGenes[j] = 0;
            }
            for (int i = 0; i < Program.rows; i++)
            {
                nPheno[i] = 0;
            }
        }
        protected virtual void constraints()
        {
            
        }
        public Coverage(double seed)
        {
            init();
            initSeed(seed);
            checkConstraints();
        }
        protected void initSeed(double seed)
        {
            for (int k = 0; k < Program.nTiles; k++)
            {
                kT++;
                cov[k] = new Comp(seed, 0);
                for (int j = 0; j < Program.col; j++)
                {
                    if (cov[k].getGene(j))
                    {
                        nGenes[j]++;
                    }
                }
                for (int i = 0; i < Program.rows; i++)
                {
                    if (cov[k].getPheno(i))
                    {
                        nPheno[i]++;
                    }
                }
                //size += cov[k].getSize();

            }
        }
        public void checkConstraints()
        {   
                for(int k=0; k<Program.nTiles; k++)
            {
                for (int j = 0; j < Program.col; j++)
                {
                    if (cov[k].getGene(j))
                    {
                        nGenes[j]++;
                    }
                }
                for (int i = 0; i < Program.rows; i++)
                {
                    if (cov[k].getPheno(i))
                    {
                        nPheno[i]++;
                    }
                }
            }
                constraints();
                recomputeFit();
        }

        public virtual Coverage newCov()
        {
            return null;
        }

        public void addComp(Comp c)
        {   
            if (kT != Program.nTiles)
            {
                cov[kT] = c;

                //size += c.getSize();
                kT++;

            }
            else
            {
                throw new ArrayOutOfBoundException();
            }
            
        }

        protected void recomputeFit()
        {
            size = 0;
            for(int k=0; k<Program.nTiles; k++)
            {
                size += cov[k].getSize();
            }
        }
        protected void exclusiveCol() {
            int max;
            int maxk = 0;
            for (int j = 0; j < Program.col; j++)
            {
                    max = 0;
                    for (int k = 0; k < Program.nTiles; k++)
                    {
                        if (cov[k].getGene(j) && cov[k].getRow() > max)
                        {
                            max = cov[k].getRow();
                            maxk = k;
                        }
                    }
                    for (int k = 0; k < Program.nTiles; k++)
                    {
                        if (cov[k].getGene(j) && k != maxk)
                        {
                            cov[k].delCol(j);
                        }
                    }
            }
        }
        protected void exclusiveRow()
        {
            int max;
            int maxk = 0;
            for (int i = 0; i < Program.rows; i++)
            {
                    max = 0;
                    for (int k = 0; k < Program.nTiles; k++)
                    {
                        if (cov[k].getPheno(i) && cov[k].getCol() > max)
                        {
                            max = cov[k].getCol();
                            maxk = k;
                        }
                    }
                    for (int k = 0; k < Program.nTiles; k++)
                    {
                        if (cov[k].getPheno(i) && k != maxk)
                        {
                            cov[k].delRow(i);
                        }
                    }
            }

        }




        protected int nBlock(int i, int j)
        {
            int temp = 0;
            for(int k=0; k<Program.nTiles; k++)
            {
                if(getGene(k, j) && getPheno(k, i))
                {
                    temp++;
                }
            }
            return temp;

        }

        protected int nOverlap(int i, int j)
        {
            int temp = nBlock(i, j);
            if (temp <= 1)
            {
                return 0;
            }
            else
            {
                //overlap += (temp - 1);
                return temp - 1;
            }


        }

        protected void overlaps()
        {
            overlap = 0;
            for (int j = 0; j < Program.col; j++)
            {
                if (nGenes[j] > 1)
                {
                    for (int i = 0; i < Program.rows; i++)
                    {
                        if (nPheno[i] > 1)
                        {
                            if (Program.dataset[i, j])
                            {
                                overlap += nOverlap(i, j);
                            }
                        }
                    }
                }
            }
        }
 
        /// <summary>
        /// remove rows and/or columns to lose ones as less as possible while meeting the overlaps constraint
        /// </summary>
        protected void noOverlap()
        {   bool o =false;
            int temp;
            for (int j = 0; j < Program.col; j++)
            {
                if (nGenes[j] > 1)
                {
                    for (int i = 0; i < Program.rows; i++)
                    {
                        if (nPheno[i] > 1)
                        {
                            if (Program.dataset[i, j])
                            {
                                temp = nBlock(i, j);
                                if (temp > 1)
                                {   
                                    for(int k1=0; k1 < Program.nTiles - 1; k1++)
                                    {
                                        for(int k2=k1+1; k2<Program.nTiles; k2++)
                                        {
                                            repair(k1, k2);
                                        }
                                    }
                                    o = true;
                                }
                            }
                        }
                    }
                }
            }
            if (o) {
                size = 0;
                for (int k = 0; k < Program.nTiles; k++)
                {
                    size += cov[k].getSize();
                }
            }
        }
        

        private void repair(int k1, int k2)
        {
            int oCol = 0;
            int oRow = 0;
            bool col=true;
            int t;
            int best;
            List<int> cols = new List<int>();
            List<int> rows = new List<int>();
            for(int i=0; i<Program.rows; i++)
            {
                if(cov[k1].getPheno(i) && cov[k2].getPheno(i))
                {
                    oRow++;
                    rows.Add(i);
                }
            }
            for(int j=0; j<Program.col; j++)
            {
                if(cov[k1].getGene(j) && cov[k2].getGene(j))
                {
                    oCol++;
                    cols.Add(j);
                }
            }
            if(cov[k1].getCol() > cov[k2].getCol())
            {
                t = k2;
                best = cov[k2].getCol() - oCol;
            }
            else
            {
                t = k1;
                best = cov[k2].getCol() - oCol;
            }
            if (cov[k1].getRow() - oRow < best)
            {
                t = k1;
                col = false;
                best = cov[k1].getRow() - oRow;
            }
            if (cov[k2].getRow() - oRow < best)
            {
                t = k2;
                col = false;
                best = cov[k2].getRow() - oRow;
            }
            if (col)
            {
                cov[t].delCol(cols);
            }
            else
            {
                cov[t].delRow(rows);
            }

        }


        /// <summary>
        /// Initialize arrays
        /// </summary>
        private void arraysInit()
        {
            
            for(int j = 0; j < Program.col; j++)
            {
                nGenes[j] = 0;
            }
            for(int i=0; i<Program.rows; i++)
            {
                nPheno[i] = 0;
            }

        }

        public virtual void print()
        {
            printTiling();
        }

        /// <summary>
        /// Print tiling
        /// </summary>
        protected void printTiling()
        {
            Console.WriteLine("Best = " + getFit() + " overlaps:"+ getOverlap());
            for (int k = 0; k < Program.nTiles; k++)
            {
                cov[k].printBlock();
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
            return cov[k].getGene(j);
        }
        
        public bool getPheno(int k, int i)
        {
            return cov[k].getPheno(i);
        }
  
        /*
        public double getMutation()
        {
            return mutation;
        }
        */

        /*
        /// <summary>
        /// Return this
        /// </summary>
        /// <returns>This</returns>
        public Coverage getCopy()
        {
            Coverage temp = newCov();
            for(int k=0; k<Program.nTiles; k++)
            {
                temp.addComp(this.getComp(k));
            }
            temp.create();
            return temp;
        }
        */
        /// <summary>
        /// Get fitness of the coverage
        /// </summary>
        /// <returns>fitness</returns>
        public virtual double getFit()
        {
            return 0;
        }

     
        public virtual bool comparefitness(int a, int b)
        {
            if (a > b)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool beterthan(Coverage c)
        {
                return false;
        }
        public void swapTiles(int a, int b)
        {
            Comp temp = cov[a];
            cov[a] = cov[b];
            cov[b] = temp;
        }
          /// <summary>
          /// part function sort elements from the array offspring from min to max, around a "pivot"
          /// </summary>
          /// <param name="min">1st element of the partition</param>
          /// <param name="max">last element of the partition</param>
          /// <returns>it returns the pivot element</returns>
          private int part(int min, int max)
          {
              int pivot = cov[max].getSize();
              int it = min;
              for (int i = min; i < max; i++)
              {
                  if (cov[i].getSize() > pivot)
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
          private void qsort(int min, int max)
          {
              int p;
              if (min < max)
              {
                  p = part(min, max);
                  qsort(min, p - 1);
                  qsort(p + 1, max);

              }

          }
        public void sort()
        {
            qsort(0, (Program.nTiles-1));
        }

        public Comp getComp(int k)
        {
            return cov[k];
        }

        public int getOverlap()
        {
            return overlap;
        }

        protected void countDisc()
        {
            //size = 0;
            pos = 0;
            neg = 0;
            for(int i=0; i<Program.rows; i++)
            {
                
                if (nPheno[i]>0)
                {
                    
                    if (Program.targetVar[i])
                    {
                        pos++;
                    }
                    else
                    {
                        neg++;
                    }
                    
                }

            }
            size = (((double)pos / Program.plus) - (alpha * ((double) neg / Program.minus)));
        }

        protected void printDisc()
        {
            Console.WriteLine("Covered - transactions : "+neg+" ("+ ((double) neg / Program.minus)+"%)");
            Console.WriteLine("Covered + transactions : " + pos + " (" + ((double)pos / Program.plus) + "%)");
            Console.WriteLine("Difference +/-         : " + (pos - neg));
        }

        public string getDisc()
        {
            string temp;
            temp = neg + " " + ((double)neg / Program.minus) + " " + pos + " " + ((double)pos / Program.plus);
            return temp;
        }
        protected int nFp()
        {
            int f = 0;
            for (int i = 0; i < Program.rows; i++)
            {
                for (int j = 0; j < Program.col; j++)
                {
                    if (!Program.dataset[i, j] && nBlock(i, j) > 0)
                    {
                        f++;
                    }
                }
            }
            return f;
        }

        public virtual int getFp()
        {
            return 0;
        }
    }






    

}
