using System;
using System.Collections.Generic;

namespace MT
{
    /// <summary>
    /// ClassVar represents a class variable, it is composed of different values it can take
    /// </summary>
    public class ClassVar
    {
        public List<ClassValue> values;
        public ClassVar(string n)
        {
            values = new List<ClassValue>();
            values.Add(new ClassValue(n));
            //Console.WriteLine("new");
        }

        public void AddVal(string n)
        {
            values.Add(new ClassValue(n));
            //Console.WriteLine(nClass);

        }
        private void swap(int a, int b)
        {
            ClassValue temp1 = values[a];
            ClassValue temp2 = values[b];
            values.RemoveAt(a);
            values.Insert(a, temp2);
            values.RemoveAt(b);
            values.Insert(b, temp1);

        }
        private int getHigher(int a)
        {
            int co = 0;
            int re = 0;
            for (int i = a; i < values.Count; i++)
            {
                if (values[i].GetOcc() > co)
                {
                    co = values[i].GetOcc();
                    re = i;
                }
            }
            return re;
        }
        private void simpleSort(int a)
        {
            int ind = this.getHigher(a);
            if (ind != a)
            {
                swap(a, ind);
            }
            if (a < getClass() - 1)
            {
                simpleSort(a + 1);
            }

        }

        public int getClass()
        {
            return values.Count;
        }
        public void sort()
        {
            if (getClass() == 2)
            {

                if (values[0].boolean() && values[1].boolean())
                {

                    if (!values[0].isFalse())
                    {
                        //Console.WriteLine("SimpleSwap");
                        this.swap(0, 1);
                    }
                }
                else
                {
                    this.simpleSort(0);
                    //Console.WriteLine("simplesort");
                }
            }
            else
            {
                this.simpleSort(0);
                //Console.WriteLine("simplesort");
            }
        }

        public bool isBool()
        {
            
            foreach(ClassValue cv in values)
            {
                if (!cv.boolean())
                {
                    return false;
                }
            }
            return true;
        }
        
    }
}
