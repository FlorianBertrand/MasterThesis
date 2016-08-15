using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT
{
    class Disc : Coverage
    {   
        
        public Disc() : base()
        {

        }
        public Disc(double seed) : base(seed)
        {

        }
        protected override void constraints()
        {
            sort();
            //punishment();
            countDisc();

        }

        
        public override Coverage newCov()
        {
            Disc temp = new Disc();
            return temp;
        }

        public override void print()
        {
            printDisc();
        }

        public override double getFit()
        {
            return size;
        }
    }
}
