using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT
{
    public class BMF : Coverage
    {
        public BMF():base()
        {

        }

        public BMF(double seed) : base(seed)
        {

        }

        protected override void constraints()
        {
            overlaps();
        }

        public override Coverage newCov()
        {
            BMF temp = new BMF();

            return temp;
        }


        public override double getFit()
        {
            return (size-overlap);
        }
    }
}
