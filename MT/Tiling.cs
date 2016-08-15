using System;
using System.Collections.Generic;
namespace MT
{
    /// <summary>
    /// Tiling is a structure composed of multiple tile
    /// size is the coverage of this tiling
    /// </summary>
    public class Tiling : Coverage
    {
        
        public Tiling() : base()
        {
            
        }


        public Tiling(double seed) : base(seed)
        {
            
        }

        protected override void constraints()
        {
            
            noOverlap();
            recomputeFit();
            
        }
        

        public override Coverage newCov()
        {   
            Tiling temp = new Tiling();

            return temp;
        }


        public override double getFit()
        {
            return size;
        }
        public override bool comparefitness(int a, int b)
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
        public override bool beterthan(Coverage c)
        {
            if (this.getFit() > c.getFit())
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}