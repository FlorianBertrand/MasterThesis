using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT
{
    class Block : Coverage
    {
        public Block() : base()
        {

        }

        public Block(double seed) : base(seed)
        {

        }

        protected override void constraints()
        {
            //sort();
            exclusiveCol();
            exclusiveRow();

        }


        public override Coverage newCov()
        {
            Block temp = new Block();

            return temp;
        }

        public override double getFit()
        {
            return size;
        }

    }
}
